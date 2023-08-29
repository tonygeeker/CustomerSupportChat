namespace CustomerSupportChatApi.DataLayer.Models
{
    public class ChatSession
    {
        public int Id { get; set; }
        public int? AgentId { get; set; }
        public string Subject { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }
        public virtual List<ChatMessage> ChatMessages { get; set; }
        public CustomerInfo Customer { get; set; }
    }
}
