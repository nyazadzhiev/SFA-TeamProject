using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.Common;
using AutoMapper;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace ProjectManagementApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeOffCOntroller : ControllerBase
    {
        private readonly IUserService _userService;
        private IValidationService _validationService;
        private ITimeOffService _timeOffService;
        private readonly IMapper _mapper;

        public TimeOffCOntroller(IValidationService validationService, IUserService userService, ITimeOffService timeOffService, IMapper mapper) : base()
        {
            _validationService = validationService;
            _userService = userService;
            _timeOffService = timeOffService;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<List<TimeOffResponseDTO>> GetAll()
        {
            var requests = await _timeOffService.GetAllAsync();

            return _mapper.Map<List<TimeOffResponseDTO>>(requests);
        }

        [HttpGet("MyRequests")]
        public async Task<List<TimeOffResponseDTO>> GetMyRequests()
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            var requests = await _timeOffService.GetMyTimeOffs(currentUser.Id);

            return _mapper.Map<List<TimeOffResponseDTO>>(requests);
        }

        [HttpGet("{timeOffId}")]
        public async Task<TimeOffResponseDTO> GetById(Guid timeOffId)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            return _mapper.Map<TimeOffResponseDTO>(timeOff);
        }

            [HttpPost]
        public async Task<ActionResult> CreateTimeOff(TimeOffRequestDTO model)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            _validationService.EnsureUserExist(currentUser);

            bool isCreated = await _timeOffService.CreateTimeOffAsync(model, currentUser.Id);

            if(isCreated && ModelState.IsValid)
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

            bool isEdited = await _timeOffService.EditTimeOffAsync(timeOffId, model);

            if (!isEdited)
            {
                return BadRequest(Constants.OperationFailed);
            }

            TimeOff timeOff = await _timeOffService.GetTimeOffAsync(timeOffId);
            _validationService.EnsureTimeOffExist(timeOff);

            return _mapper.Map<TimeOffResponseDTO>(timeOff);
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
    }
}

