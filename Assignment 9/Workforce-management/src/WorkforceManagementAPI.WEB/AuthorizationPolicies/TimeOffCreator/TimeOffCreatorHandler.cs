using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.BLL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.TimeOffCreator
{
    public class TimeOffCreatorHandler : AuthorizationHandler<TimeOffCreatorRequirement>
    {
        private IIdentityUserManager userManager;
        private ITimeOffService timeOffService;
        private IHttpContextAccessor httpContextAccessor;
        private IUserService userService;

        public TimeOffCreatorHandler(IIdentityUserManager userManager, ITimeOffService timeOffService, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.userManager = userManager;
            this.timeOffService = timeOffService;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimeOffCreatorRequirement requirement)
        {

            var loggedUser = await userManager.GetUserAsync(context.User);
            var timeOffCreator = await ValidateLoggedUserIsTimeOffCreator();

            if (loggedUser.Id == timeOffCreator.Id)
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
            else
            {
                context.Fail();
                await Task.CompletedTask;
            }
           
        }

        private async Task<User> ValidateLoggedUserIsTimeOffCreator()
        {

            var timeOffId = httpContextAccessor.HttpContext.GetRouteValue("timeOffId").ToString();
            Guid actualId = new Guid(timeOffId);
            var timeOff = await timeOffService.GetTimeOffAsync(actualId);
            var timeOffCreator = await userService.GetUserById(timeOff.CreatorId);
            return timeOffCreator; 

        }
    }
}
