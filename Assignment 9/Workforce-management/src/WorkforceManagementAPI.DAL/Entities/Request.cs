using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class Request
    {
        public Guid Id { get; set; }

        public string Reason { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        public Guid CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public Guid ModifierId { get; set; }

        public virtual User Modifier { get; set; }

        public virtual List<User> Reviewers { get; set; }

        public Request()
        {
            this.Reviewers = new List<User>();
        }
    }
}
