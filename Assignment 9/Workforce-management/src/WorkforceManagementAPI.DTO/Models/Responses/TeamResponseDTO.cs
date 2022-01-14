using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DTO.Models.Responses
{
    public class TeamResponseDTO : BaseResponseDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string TeamLeaderId { get; set; }
    }
}
