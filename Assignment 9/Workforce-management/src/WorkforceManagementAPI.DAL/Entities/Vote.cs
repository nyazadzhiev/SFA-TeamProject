using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities.Enums;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class Vote
    {
        public Guid Id { get; set; }

        public Status Response { get; set; }

        public string TeamLeaderId { get; set; }

        public virtual User TeamLeader { get; set; }

        public Guid TimeOffId { get; set; }

        public virtual TimeOff TimeOff { get; set; }
    }
}
