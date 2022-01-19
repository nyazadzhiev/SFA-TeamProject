using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    
    public class TeamController : ControllerBase
    {
        private static IUserService _userService;
        private static ITeamService _teamService;
        private readonly IMapper _mapper;
        private User currentUser;
        
        public TeamController(IUserService userService, ITeamService teamService, IMapper mapper) : base()
        {
            _userService = userService;
            _teamService = teamService;
            _mapper = mapper;
        }

        /// <summary>
        /// List all teams, existing in the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<TeamResponseDTO>> GetAllTeamsAsync()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);
        }
        /// <summary>
        /// Find a team by its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpGet("{id}")]
        public async Task<TeamResponseDTO> GetTeamByIdAsync(Guid id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);

            return _mapper.Map<TeamResponseDTO>(team);
        }
        /// <summary>
        /// List all teams, the logged user is member of.
        /// </summary>
        /// <returns></returns>
        
        [HttpGet("My/")]
        public async Task<IEnumerable<TeamResponseDTO>> GetMyTeamsAsync()
        {
            currentUser = await _userService.GetCurrentUser(User);

            var teams = await _teamService.GetMyTeamsAsync(currentUser.Id);
            return _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);
        }
        /// <summary>
        /// Create a team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        
        [HttpPost]
        public async Task<ActionResult> CreateTeamAsync(TeamRequestDTO team)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isCreated = await _teamService.CreateTeamAsync(team, currentUser.Id);
            if (isCreated && ModelState.IsValid)
            {
                return Ok("Team created successfully.");
            }

            return BadRequest();
        }
        /// <summary>
        /// Edit a team.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teamEdit"></param>
        /// <returns></returns>
       
        [HttpPut("{id}")]
        public async Task<IActionResult> EditTeamAsync(Guid id, TeamRequestDTO teamEdit)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isEdited = await _teamService.EditTeamAsync(id, currentUser.Id, teamEdit);
            if (isEdited)
            {
                return Ok("Team edited successfully.");
            }

            return BadRequest();
        }
        /// <summary>
        /// REMOVE a team.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamAsync(Guid id)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isDeleted = await _teamService.DeleteTeamAsync(id);
            if (isDeleted)
            {
                return Ok("Team deleted successfully.");
            }

            return BadRequest();
        }
        /// <summary>
        /// Assign user to a team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
       
        [HttpPost("{teamId}/Assign/{userId}")]
        public async Task<IActionResult> AssignUserToTeamAsync(Guid teamId, string userId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isAssigned = await _teamService.AssignUserToTeamAsync(teamId, userId);
            if (isAssigned)
            {
                return Ok("User assigned to team successfully.");
            }

            return BadRequest();
        }

        /// <summary>
        /// REMOVE user from a team.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
       
        [HttpDelete("{teamId}/Unassign/{userId}")]
        public async Task<IActionResult> UnassignUserFromTeamAsync(Guid teamId, string userId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isUnassigned = await _teamService.UnassignUserFromTeamAsync(teamId, userId);
            if (isUnassigned)
            {
                return Ok("User unassigned from team successfully.");
            }

            return BadRequest();
        }

        /// <summary>
        /// Assign user as a TEAM LEADER.
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        
        [HttpPut("{teamId}/AssignLeader/{userId}")]
        public async Task<IActionResult> AssignTeamLeaderAsync(Guid teamId, string userId)
        {
            currentUser = await _userService.GetCurrentUser(User);

            bool isAssigned = await _teamService.AssignTeamLeaderAsync(teamId, userId);
            if (isAssigned)
            {
                return Ok("Team leader assigned to team successfully.");
            }

            return BadRequest();
        }


    }
}
