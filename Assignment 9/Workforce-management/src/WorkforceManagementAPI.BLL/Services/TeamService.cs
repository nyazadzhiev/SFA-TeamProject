using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Service
{
    public class TeamService
    {
        private readonly DatabaseContext _context;
        private readonly IValidationService _validationService;

        public TeamService(DatabaseContext context, IValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task<Team> GetTeamByIdAsync(Guid teamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            _validationService.EnsureTeamExist(team);

            return team;
        }

        public async Task<List<Team>> GetMyTeamsAsync(string userId)
        {
            var teams = await _context.Teams
                .Where(t => t.Users
                    .Any(u => u.Id.Equals(userId)))
                .ToListAsync();

            return teams;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<bool> CreateTeamAsync(string title, string description, string creatorId)
        {
            _validationService.CheckTeamName(title);

            var now = DateTime.Now;
            var team = new Team()
            {
                Title = title,
                Description = description,
                CreatorId = creatorId,
                ModifierId = creatorId,
                CreatedAt = now,
                ModifiedAt = now
            };

            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeamAsync(Guid teamId, string modifierId, string title, string description)
        {
            _validationService.CheckTeamName(title);

            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            _validationService.EnsureTeamExist(team);

            team.Title = title;
            team.Description = description;
            team.ModifierId = modifierId;
            team.ModifiedAt = DateTime.Now;

            _context.Teams.Update(team);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTeamAsync(Guid teamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            _validationService.EnsureTeamExist(team);

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
