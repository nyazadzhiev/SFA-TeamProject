using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DTO.Models.Responses;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.Common;

namespace ProjectManagementApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeOffCOntroller : ControllerBase
    {
        private readonly IUserService _userService;
        private IValidationService _validationService;
        private ITimeOffService _timeOffService;

        public TimeOffCOntroller(IValidationService validationService, IUserService userService, ITimeOffService timeOffService) : base()
        {
            _validationService = validationService;
            _userService = userService;
            _timeOffService = timeOffService;
        }

        [HttpGet()]
        public async Task<ActionResult> GetAll()
        {
            var requests = await _timeOffService.GetAllAsync();

            return Ok(requests
                        .Select(request => new TimeOffResponseDTO()
                        {
                            Id = request.Id,
                            Reason = request.Reason,
                            Type = request.Type,
                            Status = request.Status,
                            startDate = request.StartDate,
                            endDate = request.EndDate,
                            CreatorName = request.Creator.FirstName + " " + request.Creator.LastName,
                            ModifierName = request.Modifier.FirstName + " " + request.Modifier.LastName
                        }).ToList());
        }

        [HttpGet("MyRequests")]
        public async Task<ActionResult<TimeOffResponseDTO>> GetMyRequests()
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            var requests = await _timeOffService.GetMyTimeOffs(currentUser.Id);

            return Ok(requests
                        .Select(request => new TimeOffResponseDTO()
                        {
                            Id = request.Id,
                            Reason = request.Reason,
                            Type = request.Type,
                            Status = request.Status,
                            startDate = request.StartDate,
                            endDate = request.EndDate,
                            CreatorName = request.Creator.FirstName + " " + request.Creator.LastName,
                            ModifierName = request.Modifier.FirstName + " " + request.Modifier.LastName
                        }).ToList());
        }

        [HttpGet("{timeOffId}")]
        public async Task<ActionResult<TimeOffResponseDTO>> GetById(Guid timeOffId)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            return new TimeOffResponseDTO()
            {
                Id = timeOff.Id,
                Reason = timeOff.Reason,
                Type = timeOff.Type,
                Status = timeOff.Status,
                startDate = timeOff.StartDate,
                endDate = timeOff.EndDate,
                CreatorName = timeOff.Creator.FirstName + " " + timeOff.Creator.LastName,
                ModifierName = timeOff.Modifier.FirstName + " " + timeOff.Modifier.LastName
            };
        }

        [HttpPost]
        public async Task<ActionResult> CreateTimeOff(TimeOffRequestDTO model)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isCreated = await _timeOffService.CreateTimeOffAsync(model.Reason, model.Type, model.Status, model.StartDate, model.EndDate, currentUser.Id);

            if (isCreated && ModelState.IsValid)
            {
                return Created(nameof(HttpPostAttribute), String.Format(Constants.Created, "TimeOff request"));
            }
            else
            {
                return BadRequest(Constants.OperationFailed);
            }
        }

        [HttpPut("{timeOffId}")]
        public async Task<ActionResult<TimeOffResponseDTO>> Edit(Guid timeOffId, TimeOffRequestDTO model)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isEdited = await _timeOffService.EditTimeOffAsync(timeOffId, model.Reason, model.StartDate, model.EndDate, model.Type, model.Status);

            if (!isEdited)
            {
                return BadRequest(Constants.OperationFailed);
            }

            TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            return new TimeOffResponseDTO()
            {
                Id = timeOff.Id,
                Reason = timeOff.Reason,
                Type = timeOff.Type,
                Status = timeOff.Status,
                startDate = timeOff.StartDate,
                endDate = timeOff.EndDate,
                CreatorName = timeOff.Creator.FirstName + " " + timeOff.Creator.LastName,
                ModifierName = timeOff.Modifier.FirstName + " " + timeOff.Modifier.LastName
            };
        }

        [HttpDelete("{timeOffId}")]
        public async Task<ActionResult> Delete(Guid timeOffId)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isDeleted = await _timeOffService.DeleteTimeOffAsync(timeOffId);
            if (!isDeleted)
            {
                return BadRequest(Constants.OperationFailed);
            }

            return Ok(String.Format(Constants.Deleted, "TimeOff request"));
        }

        [HttpPost("SubmitFeedback/{timeOffId}")]
        public async Task<ActionResult> SubmitFeedbackForTimeOffRequest(Guid timeOffId, CreateVoteRequestDTO vote)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isCompleted = await _timeOffService.SubmitFeedbackForTimeOffRequestAsync(currentUser, timeOffId, vote.Status);
            if (!isCompleted)
            {
                return BadRequest(Constants.OperationFailed);
            }

            return Ok(Constants.AnswerToRequest);
        }
    }
}

