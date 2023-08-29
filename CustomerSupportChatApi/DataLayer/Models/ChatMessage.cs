using CustomerSupportChatApi.DataLayer.Enumerations;

namespace CustomerSupportChatApi.DataLayer.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public MessageSender Sender { get; set; }
        public int AgentId { get; set; }
        public string Message { get; set; }
        public DateTime TimeSent { get; set; }
    }
}
