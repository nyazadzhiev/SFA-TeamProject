using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DTO.Models.Requests
{
    public class CreateVoteRequestDTO
    {
        [Required]
        public Status Status { get; set; }
    }
}
