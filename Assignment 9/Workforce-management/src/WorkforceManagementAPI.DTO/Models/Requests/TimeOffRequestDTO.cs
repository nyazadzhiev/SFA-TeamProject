using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class TimeOffRequestDTO
    {   
        [Required]
        public string Reason { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType Type { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
