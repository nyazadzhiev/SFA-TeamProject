using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.BLL.Services
{
    public class TimeOffService
    {
        private readonly DatabaseContext _context;
        private readonly ValidationService _validationService;

        public TimeOffService(DatabaseContext context, ValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        public async Task<bool> CreateTimeOffAsync(string reason, RequestType type, Status status, DateTime startDate, DateTime endDate, string creatorId)
        {
            var timeOff = new TimeOff()
            {
                Reason = reason,
                Type = type,
                Status = status,
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                CreatorId = creatorId,
                ModifierId = creatorId
            };

            await _context.Requests.AddAsync(timeOff);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TimeOff>> GetAllAsync()
        {
            return await _context.Requests.ToListAsync();
        }

        public async Task<List<TimeOff>> GetMyTimeOffs(string userId)
        {
            return await _context.Requests.Where(r => r.CreatorId.Equals(userId)).ToListAsync();
        }

        public async Task<TimeOff> GetTimeOffAsync(Guid id)
        {
            return await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> DeleteTimeOffAsync(Guid id)
        {
            TimeOff timeOff = await GetTimeOffAsync(id);

            _validationService.EnsureTimeOffExist(timeOff);

            _context.Requests.Remove(timeOff);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
