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

        public async Task<bool> EditTeamAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTeamAsync()
        {
            throw new NotImplementedException();
        }
    }
}
