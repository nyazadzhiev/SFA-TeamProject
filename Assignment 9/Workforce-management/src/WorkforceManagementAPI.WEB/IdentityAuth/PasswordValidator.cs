using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.WEB.IdentityAuth
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        /*private readonly IUserManager userManager;

        public PasswordValidator(IUserManager userManager)
        {
            this.userManager = userManager;
        }*/
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
           // User user = await userManager.FindByNameAsync(context.UserName);

            User user = new User(); // This is a temporary code line. Use the above code line in future versions of the api with user manager implemented.

            if (user != null)
            {
                // bool authResult = await userManager.ValidateUserCredentials(context.UserName, context.Password);

                bool authResult = true; // This is a temporary code line. Use the above code line in future versions of the api with user manager implemented.

                if (authResult)
                {
                    List<string> roles = new List<string>();

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password", claims: claims);
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
                }

                return;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");

        }
    }
}
