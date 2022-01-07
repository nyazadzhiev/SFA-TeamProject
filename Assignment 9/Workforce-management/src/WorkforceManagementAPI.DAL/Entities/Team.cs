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

        public int TeamLeaderId { get; set; }

        public virtual User TeamLeader { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public int ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public virtual List<User> Users { get; set; }

        public Team()
        {
            this.Users = new List<User>();
        }
    }
}
