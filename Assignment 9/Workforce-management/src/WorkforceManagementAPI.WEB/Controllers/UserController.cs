using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
   
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,IMapper mapper) : base()
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequestDTO user)
        {
            bool result = await _userService.CreateUser(user);
            if (result)
            {
                return Created(nameof(HttpPostAttribute), String.Format(Constants.Created, "User"));
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// List all users, existing in the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _userService.GetAll();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        /// <summary>
        /// Find a user by Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public async Task<UserResponseDTO> GetUserByIdAsync(Guid userId)
        {
            User user = await _userService.GetUserById(userId.ToString());
            return _mapper.Map<UserResponseDTO>(user);
        }

        /// <summary>
        /// Edit a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public async Task<IActionResult> EditUserAsync(Guid userId, EditUserRequest user)
        {
            bool isEdited = await _userService.UpdateUser(userId.ToString(), user);
            if (isEdited && ModelState.IsValid)
            {
                return Ok("User edited successfully");
            }
            return BadRequest();
        }


        /// <summary>
        /// REMOVE a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            bool isDeleted = await _userService.DeleteUser(userId.ToString());
            if (isDeleted)
            {
                return Ok("User deleted successfully");
            }
            return BadRequest();
        }

        /// <summary>
        /// Set user as ADMINISTRATOR.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("SetAdmin/{userId}")]
        public async Task<IActionResult> SetAdministrator(Guid userId)
        {
            await _userService.SetAdministrator(userId.ToString());
            return Ok($"User: {userId} is set as Admin");
        }
    }
}
