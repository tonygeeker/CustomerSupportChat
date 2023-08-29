namespace CustomerSupportChatApi.DataLayer.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAgent { get; set; }
        public bool IsAvailable { get; set; }
    }
}
