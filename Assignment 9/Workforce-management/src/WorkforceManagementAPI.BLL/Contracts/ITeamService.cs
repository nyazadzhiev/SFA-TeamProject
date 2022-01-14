using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Contracts
{
    public interface ITeamService
    {
        Task<bool> AssignTeamLeaderAsync(Guid teamId, string userId);
        Task<bool> AssignUserToTeamAsync(Guid teamId, string userId);
        Task<bool> CreateTeamAsync(string title, string description, string creatorId);
        Task<bool> DeleteTeamAsync(Guid teamId);
        Task<bool> EditTeamAsync(Guid teamId, string modifierId, string title, string description);
        Task<List<Team>> GetAllTeamsAsync();
        Task<List<Team>> GetMyTeamsAsync(string userId);
        Task<Team> GetTeamByIdAsync(Guid teamId);
        Task<bool> UnassignUserFromTeamAsync(Guid teamId, string userId);
    }
}