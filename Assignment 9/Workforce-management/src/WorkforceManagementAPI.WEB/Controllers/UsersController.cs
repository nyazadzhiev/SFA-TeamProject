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

        [HttpGet]
        [Route("All")]
        public async Task<List<UserResponseDTO>> GetAll()
        {
            var users = await _userService.GetAll();
            return _mapper.Map<List<UserResponseDTO>>(users);
        }

        [HttpGet("{id}")]
        public async Task<UserResponseDTO> GetUserById(Guid id)
        {
            User user = await _userService.GetUserById(id.ToString());
            return _mapper.Map<UserResponseDTO>(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, EditUserRequest user)
        {
            bool isEdited = await _userService.UpdateUser(id.ToString(), user);
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
