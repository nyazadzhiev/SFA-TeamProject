using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.DAL.Repositories
{
    public  class TimeOffRepository
    {
        private readonly DatabaseContext _context;

        public TimeOffRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateTimeOffAsync(TimeOff timeOff)
        {
            await _context.Requests.AddAsync(timeOff);
        }

        public async Task<List<TimeOff>> GetAllAsync()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<TimeOff>> GetMyTimeOffsAsync(string userId)
        {
            var timeOffs = await _context.Requests
                .Where(ci => ci.CreatorId == userId)
                    .ToListAsync();

            return timeOffs;
        }

        public async Task<TimeOff> GetTimeOffAsync(Guid id)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        }

        public void UpdateTimeOff(TimeOff timeOff)
        {
           _context.Update(timeOff);
        }

        public void DeleteTimeOffAsync(TimeOff timeOff)
        {
            _context.Remove(timeOff);
        }

    }
}
