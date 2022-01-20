using System.ComponentModel.DataAnnotations;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class CreateVoteRequestDTO
    {
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status Status { get; set; }
    }
}
