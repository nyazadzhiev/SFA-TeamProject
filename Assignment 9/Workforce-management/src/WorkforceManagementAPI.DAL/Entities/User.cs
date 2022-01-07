using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.DAL.Entities
{
    public class User 
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual List<Team> Teams { get; set; }

        public virtual List<TimeOff> Requests  { get; set; }

        public virtual List<TimeOff> UnderReviewRequests { get; set; }

        public User()
        {
            this.Teams = new List<Team>();
            this.Requests  = new List<TimeOff>();
            this.UnderReviewRequests = new List<TimeOff>();
        }
    }
}
