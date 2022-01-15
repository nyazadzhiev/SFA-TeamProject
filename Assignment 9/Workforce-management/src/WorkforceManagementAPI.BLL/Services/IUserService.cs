using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(string email, string password, string firstName, string lastName);

        Task<bool> DeleteUser(string id);

        Task<List<User>> GetAll();

        Task<User> GetCurrentUser(ClaimsPrincipal principal);

        Task<User> GetUserById(string id);

        Task<bool> UpdateUser(string userId, string newPassword, string newEmail, string newFirstName, string newLastName);

        Task<bool> IsUserInRole(string userId, string roleName);

        Task SetAdministrator(string userId);

        Task<bool> Login(string email, string password);


    }
}