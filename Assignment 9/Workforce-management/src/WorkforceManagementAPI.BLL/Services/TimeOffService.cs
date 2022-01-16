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

namespace WorkforceManagementAPI.BLL.Services
{
    public class TimeOffService : ITimeOffService
    {
        private readonly DatabaseContext _context;
        private readonly IValidationService _validationService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public TimeOffService(DatabaseContext context, IValidationService validationService, IUserService userService, INotificationService notificationService)
        {
            _context = context;
            _validationService = validationService;
            _userService = userService;
            _notificationService = notificationService;
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

            string subject = timeOff.Type.ToString() + " Time Off";
            string message = String.Format(Constants.RequestMessage, user.FirstName, user.LastName, timeOff.StartDate.Date, timeOff.EndDate.Date, timeOff.Type, timeOff.Reason);

            user.Teams.ForEach(t => t.TeamLeader.UnderReviewRequests.Add(timeOff));
            timeOff.Reviewers = user.Teams.Select(t => t.TeamLeader).ToList();
            await _context.Requests.AddAsync(timeOff);
            await _context.SaveChangesAsync();

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

        public async Task<bool> AnswerRequests(User user, Guid timeOffId, Status status)
        {
            var timeOff = await GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            timeOff.Reviewers.Remove(user);
            user.UnderReviewRequests.Remove(timeOff);

            string message = "";

            if (status == Status.Rejected)
            {
                message = "Rejected.";
                timeOff.Status = Status.Rejected;
                timeOff.Reviewers.Clear();
            }
            else
            {
                timeOff.Status = Status.Awaiting;
            }

            if (timeOff.Reviewers.Count == 0)
            {
                if (timeOff.Status != Status.Rejected)
                {
                    message = "Approved.";
                    timeOff.Status = Status.Approved;
                }

                await _notificationService.Send(new List<User>() { timeOff.Creator }, "response", message);
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
