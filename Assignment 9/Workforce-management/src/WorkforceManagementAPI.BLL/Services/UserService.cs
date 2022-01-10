using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;

namespace WorkforceManagementAPI.BLL.Services
{
    public class UserService
    {
        private readonly IIdentityUserManager _userManager;

        public UserService(IIdentityUserManager userManager)
        {
            _userManager = userManager;
        }


    }
}
