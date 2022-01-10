using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Service;
using WorkforceManagementAPI.BLL.Services.IdentityServices;

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
    }
}
