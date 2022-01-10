using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Service;
using WorkforceManagementAPI.BLL.Services.IdentityServices;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private static IdentityUserManager _userService;
        private static TeamService _teamService;

        public TeamController(IdentityUserManager userService, TeamService teamService) : base()
        {
            _userService = userService;
            _teamService = teamService;
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
