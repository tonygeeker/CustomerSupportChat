using CustomerSupportChatApi.BusinessLayer.Dtos;
using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Enumerations;
using CustomerSupportChatApi.DataLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Models;

namespace CustomerSupportChatApi.BusinessLayer.Services
{
    public class ChatSessionsService : BaseService<ChatSession>, IChatSessionsService
    {
        IRepository<ChatSession> _chatSessionsRepository;
        IService<Agent> _agentService;
        ISupportTeamService _supportTeamService;
        IObservableQueueWrapper<ChatSession> _chatSessionsQueue;
        public ChatSessionsService(IRepository<ChatSession> chatSessionsRepository,
                                   ISupportTeamService supportTeamService,
                                   IService<Agent> agentService,
                                   IObservableQueueWrapper<ChatSession> chatSessionsQueue)
            : base(chatSessionsRepository) 
        {
            _supportTeamService = supportTeamService;
            _chatSessionsQueue = chatSessionsQueue;
            _chatSessionsQueue.Enqueuedhandler += AssignChatSessionToAgent;
            _agentService = agentService;
        }

        public void AssignChatSessionToAgent(object? sender, EventArgs e)
        {
            var queue = sender as ObservableQueueWrapper<ChatSession>;
            if (queue.Count() < 1)
                return;

            var onDutyTeam = _supportTeamService.GetCurrentTeamOnShift().Result;
            var maxConcurrentChats = _supportTeamService.GetTeamCapacity(onDutyTeam);

            var availableAgents = onDutyTeam.Agents?.Where(x => x.IsAvailable && x.ActiveSessions.Count < maxConcurrentChats);
            AssignWithRoundRobin(queue, availableAgents);
        }

        private void AssignWithRoundRobin(ObservableQueueWrapper<ChatSession>? queue, IEnumerable<Agent>? availableAgents)
        {
            if (availableAgents.Count() < 1)
                return;

            var seniroties = new List<AgentSeniority>()
            {
                AgentSeniority.Junior,
                AgentSeniority.MidLevel,
                AgentSeniority.Senior,
                AgentSeniority.TeamLead,
            };

            foreach (var seniority in seniroties)
            {
                var agent = availableAgents.FirstOrDefault(x => x.Seniority == seniority);
                if (agent != null)
                {
                    var frontItem = queue.Dequeue();
                    agent.ActiveSessions.Add(frontItem);
                    _agentService.Update(agent);
                    break;
                }
            }

            _agentService.SaveChanges();
        }

        public Task<bool> AssignChatSessionToAgent(ChatSession chatSession, bool isOverflowRequired = false)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateChatSession(CreateChatSessionRequestDto request)
        {
            var chatSession = MapRequestToChatSession(request);
            Add(chatSession);
            SaveChanges();

            return await EnqueueChatSession(chatSession);
        }

        public async Task<bool> EnqueueChatSession(ChatSession chatSession)
        {
            // 1st see if queue max length is reached, using: maxQSize = 1.5* teamCapacity
            var onDutyTeam = await _supportTeamService.GetCurrentTeamOnShift();
            onDutyTeam.Capacity = _supportTeamService.GetTeamCapacity(onDutyTeam);

            // refuse chat if queue is full and no overflow available
            if (IsQueueFull(_chatSessionsQueue, onDutyTeam))
            {
                if (await IsOverFlowPossible(onDutyTeam))
                {
                    onDutyTeam = await _supportTeamService.AddOverflowAgentToTeam(onDutyTeam, chatSession);
                    await _supportTeamService.Update(onDutyTeam);
                    _supportTeamService.SaveChanges();
                }
                else
                {
                    return false;
                }
            }

            _chatSessionsQueue.Enqueue(chatSession);
            return true;
        }

        private async Task<bool> IsOverFlowPossible(SupportTeam onDutyTeam)
        {
            return _supportTeamService.IsItDuringOfficeHours(onDutyTeam) && await _supportTeamService.IsOverflowAvailable();
        }

        private bool IsQueueFull(IObservableQueueWrapper<ChatSession> queueWrapper, SupportTeam onDutyTeam)
        {
            return queueWrapper.Count() >= 1.5 * onDutyTeam.Capacity;
        }

        private ChatSession MapRequestToChatSession(CreateChatSessionRequestDto request)
        {
            return new ChatSession
            {
                Customer = new CustomerInfo
                {
                    Email = request.CustomerEmail,
                    Name = request.CustomerName
                },
                ChatMessages = new List<ChatMessage>
                {
                    new ChatMessage
                    {
                        Message = request.Message,
                        Sender = DataLayer.Enumerations.MessageSender.Customer
                    }
                },
                Subject = request.Subject,
                IsAssigned = false
            };
        }
    }
}
