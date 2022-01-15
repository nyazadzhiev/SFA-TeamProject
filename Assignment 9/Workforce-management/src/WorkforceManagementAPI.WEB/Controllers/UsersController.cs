using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpGet]
        [Route("All")]
        public async Task<List<UserResponseDTO>> GetAll()
        {
            var users = await _userService.GetAll();
            return users
                    .Select(user => new UserResponseDTO()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    })
                    .ToList();
        }

        [HttpGet("{id}")]
        public async Task<UserResponseDTO> GetUserById(Guid id)
        {
            User user = await _userService.GetUserById(id.ToString());
            if (user != null)
            {
                return new UserResponseDTO()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
            }
            return new UserResponseDTO();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, EditUserReauestDTO user)
        {
            bool isEdited = await _userService.UpdateUser(id.ToString(), user.NewPassword, user.NewEmail, user.NewFirstName, user.NewLastName);
            if (isEdited && ModelState.IsValid)
            {
                return Ok("User edited successfully");
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            bool isDeleted = await _userService.DeleteUser(id.ToString());
            if (isDeleted)
            {
                return Ok("User deleted successfully");
            }
            return BadRequest();
        }

        [HttpPost("SetAdministrator/{id}")]
        public async Task<IActionResult> SetAdministrator(Guid id)
        {
            await _userService.SetAdministrator(id.ToString());
            return Ok($"User: {id} is set as Admin");
        }


    }
}
