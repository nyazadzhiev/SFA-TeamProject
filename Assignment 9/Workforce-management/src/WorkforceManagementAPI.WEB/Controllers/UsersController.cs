using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) : base()
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO user)
        {
            User currentUser = await _userService.GetCurrentUser(User);
            bool result = await _userService.CreateUser(user.Email, user.Password, user.FirstName, user.LastName);
            if (result)
            {
                return Ok("User created successfully");
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
