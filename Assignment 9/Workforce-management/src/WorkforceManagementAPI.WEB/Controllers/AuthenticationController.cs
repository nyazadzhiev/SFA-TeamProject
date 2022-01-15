using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private GrantValidationResult result;
        private IUserService userService;
        private IIdentityUserManager userManager;

        public AuthenticationController(IUserService userService, IIdentityUserManager userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login(UserLoginRequestDTO model)
        {

            var authResult = await userService.Login(model.Email, model.Password);
            if (authResult)
            {
                var foundUser = await userManager.FindByNameAsync(model.Email);

                List<Claim> userClaims = new List<Claim>();
                
                List<string> roles = await userManager.GetUserRolesAsync(foundUser);

                userClaims.Add(new Claim(ClaimTypes.Name, foundUser.UserName));

                foreach (var role in roles)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("seasharp_B@reM1nimum"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001/resources",
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials

                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return BadRequest();

        }

    }
}
