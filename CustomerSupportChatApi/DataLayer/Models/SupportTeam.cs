using CustomerSupportChatApi.DataLayer.Enumerations;

namespace CustomerSupportChatApi.DataLayer.Models
{
    public class SupportTeam
    {
        public int Id { get; set; }
        public ShiftType Shift { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public bool IsOnDuty { get; set; }
        public List<Agent> Agents { get; set; }
        public int Capacity { get; set; }

    }
}
