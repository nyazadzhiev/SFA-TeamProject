using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class Team
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string TeamLeaderId { get; set; }

        public virtual User TeamLeader { get; set; }

        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public string ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public List<User> Users { get; set; }

        public Team()
        {
            this.Users = new List<User>();
        }
    }
}
