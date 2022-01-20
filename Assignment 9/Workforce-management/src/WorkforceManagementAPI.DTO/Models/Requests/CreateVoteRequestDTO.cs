using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
