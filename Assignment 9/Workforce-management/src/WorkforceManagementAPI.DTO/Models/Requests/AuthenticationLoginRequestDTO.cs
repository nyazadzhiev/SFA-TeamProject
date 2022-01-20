using System.ComponentModel.DataAnnotations;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class AuthenticationLoginRequestDTO //This model will be moved to project WEB/Models in future versions of the API
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
