using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public class TimeOffService : ITimeOffService
    {
        private readonly IValidationService _validationService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ITimeOffRepository timeOffRepository;

        public TimeOffService(IValidationService validationService, IUserService userService, INotificationService notificationService, IMapper mapper, ITimeOffRepository timeOffRepository)
        {
            _validationService = validationService;
            _userService = userService;
            _notificationService = notificationService;
            _mapper = mapper;
            this.timeOffRepository = timeOffRepository;
        }

        public async Task<bool> CreateTimeOffAsync(TimeOffRequestDTO timoffRequest, string creatorId)
        {
            _validationService.EnsureInputFitsBoundaries(((int)timoffRequest.Type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.EnsureDateRangeIsValid(timoffRequest.StartDate, timoffRequest.EndDate);

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

            await timeOffRepository.CreateTimeOffAsync(timeOff);

            if (timeOff.Type == RequestType.SickLeave)
            {
                message = string.Format(Constants.SickMessage, user.FirstName, user.LastName, timeOff.StartDate, timeOff.EndDate, timeOff.Reason);

                timeOff.Status = Status.Approved;

                await timeOffRepository.SaveChangesAsync();
            }
            else
            {
                message = string.Format(Constants.RequestMessage, user.FirstName, user.LastName, timeOff.StartDate.Date, timeOff.EndDate.Date, timeOff.Type, timeOff.Reason);

                user.Teams.ForEach(t => t.TeamLeader.UnderReviewRequests.Add(timeOff));
                await timeOffRepository.SaveChangesAsync();
            }

            await _notificationService.Send(timeOff.Reviewers, subject, message);

            return true;
        }

        public async Task<List<TimeOff>> GetAllAsync()
        {
            return await timeOffRepository.GetAllAsync();
        }

        public async Task<List<TimeOff>> GetMyTimeOffs(string userId)
        {
            var user = await _userService.GetUserById(userId);
            _validationService.EnsureUserExist(user);

            return await timeOffRepository.GetMyTimeOffsAsync(userId);
        }

        public async Task<TimeOff> GetTimeOffAsync(Guid id)
        {
            return await timeOffRepository.GetTimeOffAsync(id);
        }

        public async Task<bool> DeleteTimeOffAsync(Guid id)
        {
            TimeOff timeOff = await GetTimeOffAsync(id);
                    
            _validationService.EnsureTimeOffExist(timeOff);

            timeOffRepository.DeleteTimeOffAsync(timeOff);
            await timeOffRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTimeOffAsync(Guid id, TimeOffRequestDTO timoffRequest)
        {
            _validationService.EnsureInputFitsBoundaries(((int)timoffRequest.Type), 0, Enum.GetNames(typeof(RequestType)).Length - 1);
            _validationService.EnsureDateRangeIsValid(timoffRequest.StartDate, timoffRequest.EndDate);

            var timeOff = await GetTimeOffAsync(id);
            _validationService.EnsureTimeOffExist(timeOff);

            timeOff.Reason = timoffRequest.Reason;
            timeOff.Type = timoffRequest.Type;
            timeOff.StartDate = timoffRequest.StartDate;
            timeOff.EndDate = timoffRequest.EndDate;
            timeOff.ModifiedAt = DateTime.Now;

            await timeOffRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SubmitFeedbackForTimeOffRequestAsync(User user, Guid timeOffId, Status status)
        {
            var timeOff = await GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);
            _validationService.EnsureNoReviewersLeft(timeOff);
            _validationService.EnsureUserIsReviewer(timeOff, user);
            _validationService.EnsureResponseIsValid(status);

            timeOff.Reviewers.Remove(user);
            user.UnderReviewRequests.Remove(timeOff);

            await timeOffRepository.SaveChangesAsync();

            var message = UpdateRequestStatus(status, timeOff);

            bool allReviersGaveFeedback = timeOff.Reviewers.Count == 0;
            if (allReviersGaveFeedback)
            {
                await FinalizeRequestFeedback(timeOff, message);
            }

            return true;
        }

        private string UpdateRequestStatus(Status status, TimeOff timeOff)
        {
            if (status == Status.Rejected)
            {
                timeOff.Status = Status.Rejected;
                timeOff.Reviewers.Clear();
                return "Your time off request has been rejected.";
            }

            timeOff.Status = Status.Awaiting;

            return string.Empty;
        }

        private async Task FinalizeRequestFeedback(TimeOff timeOff, string message)
        {
            if (timeOff.Status != Status.Rejected)
            {
                message = "Your time off request has been approved.";
                timeOff.Status = Status.Approved;
            }

            await _notificationService.Send(new List<User>() { timeOff.Creator }, "response", message);
        }
    }
}
