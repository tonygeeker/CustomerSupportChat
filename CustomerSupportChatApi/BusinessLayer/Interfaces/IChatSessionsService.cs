using CustomerSupportChatApi.BusinessLayer.Dtos;
using CustomerSupportChatApi.DataLayer.Models;

namespace CustomerSupportChatApi.BusinessLayer.Interfaces
{
    public interface IChatSessionsService: IService<ChatSession>
    {
        Task<bool> CreateChatSession(CreateChatSessionRequestDto request);
        void AssignChatSessionToAgent(object? sender, EventArgs e);
    }
}
