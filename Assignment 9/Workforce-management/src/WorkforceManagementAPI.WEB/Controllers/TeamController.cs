﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Service;
using WorkforceManagementAPI.BLL.Services.IdentityServices;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private static IdentityUserManager _userService;
        private static TeamService _teamService;
        private User currentUser;

        public TeamController(IdentityUserManager userService, TeamService teamService) : base()
        {
            _userService = userService;
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetAllTeamsAsync()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            {
                return (from team in teams
                        select new TeamResponseDTO()
                        {
                            Id = team.Id,
                            Title = team.Title,
                            Description = team.Description,
                            CreatorId = team.CreatorId,
                            ModifierId = team.ModifierId,
                            CreatedAt = team.CreatedAt,
                            ModifiedAt = team.ModifiedAt
                        }).ToList();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamResponseDTO>> GetTeamByIdAsync(Guid id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);

            return MapTeam(team);
        }

        [HttpGet("My/")]
        public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetMyTeamsAsync()
        {
            currentUser = await _userService.GetUserAsync(User);

            var teams = await _teamService.GetAllTeamsAsync();
            {
                return (from team in teams
                        select new TeamResponseDTO()
                        {
                            Id = team.Id,
                            Title = team.Title,
                            Description = team.Description,
                            CreatorId = team.CreatorId,
                            ModifierId = team.ModifierId,
                            CreatedAt = team.CreatedAt,
                            ModifiedAt = team.ModifiedAt
                        }).ToList();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateTeamAsync(TeamRequestDTO team)
        {
            currentUser = await _userService.GetUserAsync(User);

            bool isCreated = await _teamService.CreateTeamAsync(team.Title, team.Description, currentUser.Id);
            if (isCreated && ModelState.IsValid)
            {
                return Ok("Team created successfully.");
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTeamAsync(Guid id, TeamRequestDTO teamEdit)
        {
            currentUser = await _userService.GetUserAsync(User);

            bool isEdited = await _teamService.EditTeamAsync(id, currentUser.Id, teamEdit.Title, teamEdit.Description);
            if (isEdited)
            {
                return Ok("Team edited successfully.");
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamAsync(Guid id)
        {
            currentUser = await _userService.GetUserAsync(User);

            bool isDeleted = await _teamService.DeleteTeamAsync(id);
            if (isDeleted)
            {
                return Ok("Team deleted successfully.");
            }

            return BadRequest();
        }

        private TeamResponseDTO MapTeam(Team teamEntity)
        {
            var team = new TeamResponseDTO()
            {
                Id = teamEntity.Id,
                Title = teamEntity.Title,
                Description = teamEntity.Description,
                CreatorId = teamEntity.CreatorId,
                ModifierId = teamEntity.ModifierId,
                CreatedAt = teamEntity.CreatedAt,
                ModifiedAt = teamEntity.ModifiedAt
            };

            return team;
        }
    }
}
