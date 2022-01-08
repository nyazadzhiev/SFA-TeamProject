using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Service
{
    public class TeamService
    {
        private readonly DatabaseContext _context;

        public TeamService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Team> GetTeamByIdAsync(Guid teamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
            {
                throw new Exception($"Invalid input. Team Id doesn't exist.");
            }

            return team;
        }

        public async Task<List<Team>> GetMyTeams(Guid userId)
        {
            var teams = await _context.Teams
                .Where(t => t.Users
                    .Any(u => u.Id == userId))
                .ToListAsync();

            return teams;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<bool> CreateTeamAsync(string title, string description, Guid creatorId)
        {
            if (await _context.Teams.AnyAsync(t => t.Title == title))
            {
                throw new Exception("Name is already in use.");
            }

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

        public async Task<bool> EditTeamAsync(Guid teamId, Guid modifierId, string title, string description)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            if (team == null)
            {
                throw new Exception("Invalid input. Team Id doesn't exist.");
            }

            if (await _context.Teams.AnyAsync(p => p.Title == title) && team.Title != title)
            {
                throw new Exception("Name is already in use.");
            }

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
            if (team == null)
            {
                throw new Exception("Invalid input. Team Id doesn't exist.");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
