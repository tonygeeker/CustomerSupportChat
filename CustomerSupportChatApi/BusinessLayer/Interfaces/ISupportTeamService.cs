using CustomerSupportChatApi.DataLayer.Models;

namespace CustomerSupportChatApi.BusinessLayer.Interfaces
{
    public interface ISupportTeamService: IService<SupportTeam>
    {
        //
        int GetTeamCapacity(SupportTeam supportTeam);
        Task<bool> IsOverflowAvailable();
        bool IsItDuringOfficeHours(SupportTeam onDutyTeam);
        Task<SupportTeam> AddOverflowAgentToTeam(SupportTeam onDutyTeam, ChatSession chatSession);
        Task<SupportTeam> GetCurrentTeamOnShift();
    }
}
