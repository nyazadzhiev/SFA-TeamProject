using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityUserManager _userManager;
        private readonly IValidationService _validationService;

        public UserService(IIdentityUserManager userManager, IValidationService validationService)
        {
            _userManager = userManager;
            _validationService = validationService;
        }

        public async Task<bool> CreateUser(string email, string password, string firstName, string lastName)
        {
            _validationService.EnsureLenghtIsValid(password, 7, nameof(password));
            _validationService.EnsureLenghtIsValid(firstName, 2, nameof(firstName));
            _validationService.EnsureLenghtIsValid(lastName, 2, nameof(lastName));

            _validationService.EnsureEmailIsValid(email);
            await _validationService.EnsureEmailIsUniqueAsync(email);

            User user = new User()
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
            };
            await _userManager.CreateUserAsync(user, password);
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
            await _validationService.EnsureEmailIsUniqueAsync(newEmail);

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


    }
}
