using System.Collections.Immutable;

namespace CustomerSupportChatApi.DataLayer.Models
{
    public class ChatSessionsQueue<ChatSession>
    {
        public Queue<ChatSession> QueuedSessions { get; set; }
        public int MaxQueueLength { get; set; }

        public int IsOverflow { get; set; }
    }
}
