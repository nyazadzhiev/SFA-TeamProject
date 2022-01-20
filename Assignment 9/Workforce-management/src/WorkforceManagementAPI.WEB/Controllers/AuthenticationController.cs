using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IUserService userService;

        public AuthenticationController(IIdentityUserManager userManager)
        {
            this.userService = userManager;
        }

        [HttpPost, Route("Login")]
        public async Task<string> Login(AuthenticationLoginRequestDTO loginModel)
        {
            
            var client = new HttpClient();

            var url = "https://localhost:5001/connect/token";

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            nvc.Add(new KeyValuePair<string, string>("username", loginModel.Username));
            nvc.Add(new KeyValuePair<string, string>("password", loginModel.Password));
            nvc.Add(new KeyValuePair<string, string>("client_id", "WorkforceManagementAPI"));
            nvc.Add(new KeyValuePair<string, string>("client_secret", "seasharp_BareM1n1mum"));
            nvc.Add(new KeyValuePair<string, string>("scope", "users offline_access WorkforceManagementAPI"));
            
            using (client)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(nvc)).Result;
                return response.Content.ReadAsStringAsync().Result;
            }

        }

    }
}
