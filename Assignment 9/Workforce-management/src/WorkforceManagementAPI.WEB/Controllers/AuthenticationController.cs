using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Create a TOKEN for AUTHENTICATION, using existing username and password.
        /// </summary>
        /// <param name="loginModel"></param>
        /// 
        /// <returns></returns>
        /// <response code="200">OK - Request succeeded.</response>
        /// <response code="401">Unauthorized - Please check the provided credentials.</response>
        /// <response code="403">Forbidden - Your credentials don't meet the required authorization level to access the resource. 
        ///Please, contact your administrator to get desired permissions.</response>
        [HttpPost, Route("Login")]
        public string Login(AuthenticationLoginRequestDTO loginModel)
        {
            var client = new HttpClient();

            var url = "https://localhost:5001/connect/token";

            var IdentityServerParameters = new List<KeyValuePair<string, string>>();
            IdentityServerParameters.Add(new KeyValuePair<string, string>("grant_type", "password"));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("username", loginModel.Username));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("password", loginModel.Password));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("client_id", "WorkforceManagementAPI"));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("client_secret", "seasharp_BareM1n1mum"));
            IdentityServerParameters.Add(new KeyValuePair<string, string>("scope", "users offline_access WorkforceManagementAPI"));
            
            using (client)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(IdentityServerParameters)).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
