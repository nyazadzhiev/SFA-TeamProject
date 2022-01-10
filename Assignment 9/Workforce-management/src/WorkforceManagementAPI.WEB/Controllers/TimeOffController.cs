using Microsoft.AspNetCore.Mvc;
using System;
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
            List<TimeOffResponseDTO> requests = new List<TimeOffResponseDTO>();

            foreach(TimeOff timeOff in await _timeOffService.GetAllAsync())
            {
                requests.Add(new TimeOffResponseDTO()
                {
                    Reason = timeOff.Reason,
                    Type = timeOff.Type,
                    Status = timeOff.Status,
                    startDate = timeOff.StartDate,
                    endDate = timeOff.EndDate,
                    CreatorName = timeOff.Creator.FirstName + " " + timeOff.Creator.LastName,
                    ModifierName = timeOff.Modifier.FirstName + " " + timeOff.Modifier.LastName
                });
            }

            return Ok(requests);
        }

        [HttpGet("MyRequests")]
        public async Task<ActionResult<TimeOffResponseDTO>> GetMyRequests()
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            List<TimeOffResponseDTO> requests = new List<TimeOffResponseDTO>();

            foreach (TimeOff timeOff in await _timeOffService.GetMyTimeOffs(currentUser.Id))
            {
                requests.Add(new TimeOffResponseDTO()
                {
                    Reason = timeOff.Reason,
                    Type = timeOff.Type,
                    Status = timeOff.Status,
                    startDate = timeOff.StartDate,
                    endDate = timeOff.EndDate,
                    CreatorName = timeOff.Creator.FirstName + " " + timeOff.Creator.LastName,
                    ModifierName = timeOff.Modifier.FirstName + " " + timeOff.Modifier.LastName
                });
            }

            return Ok(requests);
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

            if(isCreated && ModelState.IsValid)
            {
                return CreatedAtAction("Post", String.Format(Constants.Created, "TimeOff request"));
            }
            else
            {
                return BadRequest(Constants.OperationFailed);
            }
        }

        [HttpPut("{timeoffId}")]
        public async Task<ActionResult<TimeOffResponseDTO>> Edit(Guid timeOffId, TimeOffRequestDTO model)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            if(await _timeOffService.EditTimeOffAsync(timeOffId, model.Reason, model.StartDate, model.EndDate, model.Type, model.Status))
            {
                TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
                _validationService.EnsureTimeOffExist(timeOff);

                return new TimeOffResponseDTO()
                {
                    Reason = timeOff.Reason,
                    Type = timeOff.Type,
                    Status = timeOff.Status,
                    startDate = timeOff.StartDate,
                    endDate = timeOff.EndDate,
                    CreatorName = timeOff.Creator.FirstName + " " + timeOff.Creator.LastName,
                    ModifierName = timeOff.Modifier.FirstName + " " + timeOff.Modifier.LastName
                };
            }
            else
            {
                return BadRequest(Constants.OperationFailed);
            }
        }

        [HttpDelete("{timeOffId}")]
        public async Task<ActionResult> Delete(Guid timeOffId)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            if (await _timeOffService.DeleteTimeOffAsync(timeOffId))
            {
                return Ok(String.Format(Constants.Deleted, "TimeOff request"));
            }
            else
            {
                return BadRequest(Constants.OperationFailed);
            }
        }
    }
}

