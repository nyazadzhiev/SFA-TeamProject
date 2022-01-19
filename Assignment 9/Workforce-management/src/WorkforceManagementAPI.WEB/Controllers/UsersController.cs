using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.DTO.Models.Responses;

namespace WorkforceManagementAPI.WEB.Controllers
{
   
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService,IMapper mapper) : base()
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
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO user)
        {
            bool result = await _userService.CreateUser(user);
            if (result)
            {
                return Ok("User created successfully");
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
        [Route("All")]
        public async Task<List<UserResponseDTO>> GetAll()
        {
            var users = await _userService.GetAll();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        /// <summary>
        /// Find a user by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserResponseDTO> GetUserById(Guid id)
        {
            User user = await _userService.GetUserById(id.ToString());
            return _mapper.Map<UserResponseDTO>(user);
        }

        /// <summary>
        /// Edit a user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, EditUserReauestDTO user)
        {
            bool isEdited = await _userService.UpdateUser(id.ToString(), user);
            if (isEdited && ModelState.IsValid)
            {
                return Ok("User edited successfully");
            }
            return BadRequest();
        }


        /// <summary>
        /// REMOVE a user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Set user as ADMINISTRATOR.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("SetAdministrator/{id}")]
        public async Task<IActionResult> SetAdministrator(Guid id)
        {
            await _userService.SetAdministrator(id.ToString());
            return Ok($"User: {id} is set as Admin");
        }


    }
}
