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

        public virtual List<Request> MyRequests { get; set; }

        public virtual List<Request> PendingRequests { get; set; }

        public User()
        {
            this.Teams = new List<Team>();
            this.MyRequests = new List<Request>();
            this.PendingRequests = new List<Request>();
        }
    }
}
