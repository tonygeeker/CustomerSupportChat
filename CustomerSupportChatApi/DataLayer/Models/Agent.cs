using CustomerSupportChatApi.DataLayer.Enumerations;

namespace CustomerSupportChatApi.DataLayer.Models
{
    public class Agent : Employee
    {
        public AgentSeniority Seniority { get; set; }
        public bool IsOverflowAgent { get; set; }
        public List<ChatSession> ActiveSessions { get; set; }

    }
}
