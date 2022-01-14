using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUserManager _userManager;
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;

        public UserService(IIdentityUserManager userManager, IValidationService validationService, IMapper mapper)
        {
            _userManager = userManager;
            _validationService = validationService;
            _mapper = mapper;
        }

        public async Task<bool> CreateUser(CreateUserRequestDTO userRequest)
        {
            _validationService.EnsureLenghtIsValid(userRequest.Password, 7, nameof(userRequest.Password));
            _validationService.EnsureLenghtIsValid(userRequest.FirstName, 2, nameof(userRequest.FirstName));
            _validationService.EnsureLenghtIsValid(userRequest.LastName, 2, nameof(userRequest.LastName));

            _validationService.EnsureEmailIsValid(userRequest.Email);
            await _validationService.EnsureEmailIsUniqueAsync(userRequest.Email);

            var user = _mapper.Map<User>(userRequest);
            user.UserName = userRequest.Email;
            await _userManager.CreateUserAsync(user, userRequest.Password);
           
            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var checkUser = await _userManager.FindByIdAsync(id);
            _validationService.EnsureUserExist(checkUser);

            await _userManager.DeleteUserAsync(checkUser);
            return true;
        }

        public async Task<bool> UpdateUser(string userId, string newPassword, string newEmail, string newFirstName, string newLastName)
        {
            _validationService.EnsureLenghtIsValid(newPassword, 7, nameof(newPassword));
            _validationService.EnsureLenghtIsValid(newFirstName, 2, nameof(newFirstName));
            _validationService.EnsureLenghtIsValid(newLastName, 2, nameof(newLastName));

            User user = await _userManager.FindByIdAsync(userId);
            _validationService.EnsureUserExist(user);

            _validationService.EnsureEmailIsValid(newEmail);
            await _validationService.EnsureUpdateEmailIsUniqueAsync(newEmail,user);

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            user.UserName = newEmail;
            user.FirstName = newFirstName;
            user.LastName = newLastName;
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            user.Email = newEmail;
            await _userManager.UpdateUserDataAsync(user);

            return true;
        }

        public async Task<List<User>> GetAll()
        {
            return await _userManager.GetAllAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            var checkUser = await _userManager.FindByIdAsync(id);
            _validationService.EnsureUserExist(checkUser);

            return checkUser;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task SetAdministrator(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception($"User with id: {userId} not found");
            }
            if (await IsUserInRole(user.Id, "Admin"))
            {
                throw new Exception("User is already an admin");
            }
            await _userManager.AddUserToRoleAsync(user, "Admin");
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            return await _userManager.IsUserInRole(userId, roleName);
        }


    }
}
