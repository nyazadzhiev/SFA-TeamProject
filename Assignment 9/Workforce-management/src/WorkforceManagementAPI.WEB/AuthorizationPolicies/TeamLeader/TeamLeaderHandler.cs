using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.WEB.AuthorizationPolicies.TeamLeader
{
    public class TeamLeaderHandler : AuthorizationHandler<TeamLeaderRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TeamLeaderRequirement requirement)
        {
            throw new System.NotImplementedException();
        }
    }
}
