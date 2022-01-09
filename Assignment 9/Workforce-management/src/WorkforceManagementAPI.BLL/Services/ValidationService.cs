using System;
using System.Linq;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Services
{
    public class ValidationService
    {
        private readonly DatabaseContext _context;

        public ValidationService(DatabaseContext context)
        {
            _context = context;
        }

        public void EnsureTeamExist(Team team)
        {
            if (team == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "Team"));
            }
        }

        public void CheckTeamName(string title)
        {
            if (_context.Teams.Any(p => p.Title == title))
            {
                throw new NameExistException(String.Format(Constants.NameAlreadyInUse, "Team name"));
            }
        }
    }
}
