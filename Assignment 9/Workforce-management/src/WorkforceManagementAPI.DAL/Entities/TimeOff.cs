using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class TimeOff
    {
        public int Id { get; set; }

        public string Reason { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public int ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public List<User> Reviewers { get; set; }

        public TimeOff()
        {
            this.Reviewers = new List<User>();
        }
    }
}
