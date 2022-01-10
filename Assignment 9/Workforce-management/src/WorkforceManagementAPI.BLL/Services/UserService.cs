using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Services
{
    public class UserService
    {
        private readonly IIdentityUserManager _userManager;

        public UserService(IIdentityUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateUser(string email, string password, string firstName, string lastName)
        {
            if (password.Length <= 7)
            {
                throw new ArgumentException("The field Password must have a minimum length of '8");
            }
            if (firstName.Length < 2)
            {
                throw new ArgumentException("The field FirstName must have a minimum length of '2");
            }
            if (lastName.Length < 2)
            {
                throw new ArgumentException("The field LastName must have a minimum length of '2");
            }
            if (!new EmailAddressAttribute().IsValid(email) || email.Length <= 4)
            {
                throw new ArgumentException("The email must be valid");
            }
            if (await _userManager.VerifyEmail(email) == false)
            {
                throw new Exception("The email already exists");
            }
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


    }
}
