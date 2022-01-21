﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.BLL.Contracts.IdentityContracts;
using WorkforceManagementAPI.WEB.AuthorizationPolicies.Requirements;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.Handlers
{
    public class TeamLeaderHandler : AuthorizationHandler<TeamLeaderTimeOffCreatorRequirement>
    {
        private IIdentityUserManager userManager;
        private ITimeOffService timeOffService;
        private IHttpContextAccessor httpContextAccessor;
        private IUserService userService;

        public TeamLeaderHandler(IIdentityUserManager userManager, ITimeOffService timeOffService, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.userManager = userManager;
            this.timeOffService = timeOffService;
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamLeaderTimeOffCreatorRequirement requirement)
        {
            if (await TeamLeaderValidaition(context))
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }

        private async Task<bool> TeamLeaderValidaition(AuthorizationHandlerContext context)
        {
            var loggedUser = await userManager.GetUserAsync(context.User);

            var timeOffId = httpContextAccessor.HttpContext.GetRouteValue("timeOffId").ToString();
            Guid actualId = new Guid(timeOffId);
            var timeOff = await timeOffService.GetTimeOffAsync(actualId);
            var timeOffCreator = await userService.GetUserById(timeOff.CreatorId);

            var isLoggedUserValidTeamLeader = timeOffCreator.Teams.Any(t => t.TeamLeaderId == loggedUser.Id);

            return isLoggedUserValidTeamLeader;
        }
    }
}
