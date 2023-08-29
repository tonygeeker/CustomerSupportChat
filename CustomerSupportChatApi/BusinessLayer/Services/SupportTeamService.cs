using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Models;

namespace CustomerSupportChatApi.BusinessLayer.Services
{
    public class SupportTeamService : BaseService<SupportTeam>, ISupportTeamService
    {
        IRepository<SupportTeam> _repository;
        IService<Employee> _employeeService;
        IConfiguration _config;

        public SupportTeamService(IRepository<SupportTeam> repository,
                                  IConfiguration config,
                                  IService<Employee> employeeService)
            : base(repository)
        {
            _employeeService = employeeService;
            _config = config;
        }

        public async Task<SupportTeam> GetCurrentTeamOnShift()
        {
            return (await GetQueryable(x => x.IsOnDuty, "QueuedSessions, Agents")).FirstOrDefault();
        }

        public async Task<SupportTeam> AddOverflowAgentToTeam(SupportTeam onDutyTeam, ChatSession chatSession)
        {
            var potentialOverflowTeamMember = (await _employeeService.GetQueryable(x => !x.IsAgent && x.IsAvailable)).FirstOrDefault();

            var agent = new Agent
            {
                Name = potentialOverflowTeamMember.Name,
                Seniority = DataLayer.Enumerations.AgentSeniority.Junior,
                IsAvailable = potentialOverflowTeamMember.IsAvailable,
                IsOverflowAgent = true
            };

            onDutyTeam.Agents.Add(agent);
            await Update(onDutyTeam);

            // remove worker from available employee list
            potentialOverflowTeamMember.IsAvailable = false;
            await _employeeService.Update(potentialOverflowTeamMember);
            _employeeService.SaveChanges();
            
            SaveChanges();
            return onDutyTeam;
        }

        public async Task<bool> IsOverflowAvailable()
        {
            var potentialOverflow = (await _employeeService.GetQueryable(x => !x.IsAgent || x.IsAvailable)).FirstOrDefault();
            if (potentialOverflow == null)
                return false;
            else 
                return true;
        }

        public bool IsItDuringOfficeHours(SupportTeam onDutyTeam)
        {
            return onDutyTeam.ShiftStartTime >= DateTime.Now && onDutyTeam.ShiftEndTime <= DateTime.Now;
        }

        public int GetTeamCapacity(SupportTeam onDutyTeam)
        {
            var teamCapacity = 0;
            decimal multiplier = 0;
            foreach (var agent in onDutyTeam.Agents)
            {
                if (agent.IsAvailable == false)
                    continue;

                switch (agent.Seniority)
                {
                    case DataLayer.Enumerations.AgentSeniority.Junior:
                        multiplier = onDutyTeam.Agents.Count * decimal.Parse(_config["SeniorityMultipliers:Junior"]);
                        teamCapacity += (int)Math.Floor(multiplier);
                        break;

                    case DataLayer.Enumerations.AgentSeniority.MidLevel:
                        multiplier = onDutyTeam.Agents.Count * decimal.Parse(_config["SeniorityMultipliers:Mid-Level"]);
                        teamCapacity += (int)Math.Floor(multiplier);
                        break;

                    case DataLayer.Enumerations.AgentSeniority.Senior:
                        multiplier = onDutyTeam.Agents.Count * decimal.Parse(_config["SeniorityMultipliers:Senior"]);
                        teamCapacity += (int)Math.Floor(multiplier);
                        break;

                    case DataLayer.Enumerations.AgentSeniority.TeamLead:
                        multiplier = onDutyTeam.Agents.Count * decimal.Parse(_config["SeniorityMultipliers:TeamLead"]);
                        teamCapacity += (int)Math.Floor(multiplier);
                        break;

                    default:
                        break;
                }
            }

            return teamCapacity;
        }
    }
}
