﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.BLL.Services
{
    public class TimeOffService : ITimeOffService
    {
        private readonly DatabaseContext _context;
        private readonly IValidationService _validationService;
        private readonly IUserService _userService;

        public TimeOffService(DatabaseContext context, IValidationService validationService, IUserService userService)
        {
            _context = context;
            _validationService = validationService;
            _userService = userService;
        }

        public async Task<bool> CreateTimeOffAsync(string reason, RequestType type, Status status, DateTime startDate, DateTime endDate, string creatorId)
        {
            _validationService.EnsureInputFitsBoundaries(((int)type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.EnsureInputFitsBoundaries(((int)status), 0, Enum.GetNames(typeof(Status)).Length - 1);
            _validationService.ValidateDateRange(startDate, endDate);

            var user = await _userService.GetUserById(creatorId);
            _validationService.EnsureUserExist(user);

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
                Creator = user,
                ModifierId = creatorId,
                Modifier = user
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
            var user = await _userService.GetUserById(userId);
            _validationService.EnsureUserExist(user);

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

        public async Task<bool> EditTimeOffAsync(Guid id, string newReason, DateTime newStart, DateTime newEnd, RequestType newType, Status newStatus)
        {
            _validationService.EnsureInputFitsBoundaries(((int)newType), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.EnsureInputFitsBoundaries(((int)newStatus), 0, Enum.GetNames(typeof(Status)).Length - 1);
            _validationService.ValidateDateRange(newStart, newEnd);

            var timeOff = await GetTimeOffAsync(id);
            _validationService.EnsureTimeOffExist(timeOff);

            timeOff.Reason = newReason;
            timeOff.Status = newStatus;
            timeOff.Type = newType;
            timeOff.StartDate = newStart;
            timeOff.EndDate = newEnd;
            timeOff.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
