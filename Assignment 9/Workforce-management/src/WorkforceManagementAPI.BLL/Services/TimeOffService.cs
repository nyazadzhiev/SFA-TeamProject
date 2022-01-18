using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public class TimeOffService : ITimeOffService
    {
        private readonly DatabaseContext _context;
        private readonly IValidationService _validationService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public TimeOffService(DatabaseContext context, IValidationService validationService, IUserService userService, INotificationService notificationService, IMapper mapper)
        {
            _context = context;
            _validationService = validationService;
            _userService = userService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<bool> CreateTimeOffAsync(TimeOffRequestDTO timoffRequest, string creatorId)
        {
            _validationService.EnsureInputFitsBoundaries(((int)timoffRequest.Type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.ValidateDateRange(timoffRequest.StartDate, timoffRequest.EndDate);

            var user = await _userService.GetUserById(creatorId);
            _validationService.EnsureUserExist(user);

            var timeOff = _mapper.Map<TimeOff>(timoffRequest);

            timeOff.CreatedAt = DateTime.Now;
            timeOff.ModifiedAt = DateTime.Now;
            timeOff.CreatorId = creatorId;
            timeOff.Creator = user;
            timeOff.ModifierId = creatorId;
            timeOff.Modifier = user;
        

            string subject = timeOff.Type.ToString() + " Time Off";
            string message;

            timeOff.Reviewers = user.Teams.Select(t => t.TeamLeader).ToList();
            await _context.Requests.AddAsync(timeOff);
            await _context.SaveChangesAsync();

            if (timeOff.Type == RequestType.SickLeave)
            {
                message = String.Format(Constants.SickMessage, user.FirstName, user.LastName, timeOff.StartDate, timeOff.EndDate, timeOff.Reason);

                timeOff.Status = Status.Approved;

                await _context.SaveChangesAsync();
            }
            else
            {
                message = String.Format(Constants.RequestMessage, user.FirstName, user.LastName, timeOff.StartDate.Date, timeOff.EndDate.Date, timeOff.Type, timeOff.Reason);

                user.Teams.ForEach(t => t.TeamLeader.UnderReviewRequests.Add(timeOff));
                await _context.SaveChangesAsync();
            }

            await _notificationService.Send(timeOff.Reviewers, subject, message);

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

        public async Task<bool> EditTimeOffAsync(Guid id, TimeOffRequestDTO timoffRequest)
        {
            _validationService.EnsureInputFitsBoundaries(((int)timoffRequest.Type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.ValidateDateRange(timoffRequest.StartDate, timoffRequest.EndDate);

            var timeOff = await GetTimeOffAsync(id);
            _validationService.EnsureTimeOffExist(timeOff);

            timeOff.Reason = timoffRequest.Reason;
            timeOff.Type = timoffRequest.Type;
            timeOff.StartDate = timoffRequest.StartDate;
            timeOff.EndDate = timoffRequest.EndDate;
            timeOff.ModifiedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
