using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DTO.Models.Responses
{
    public class BaseResponseDTO
    {
        public int Id { get; set; }

        public int CreatorId { get; set; }

        public int ModifierId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifierAt { get; set; }
    }
}
