﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.Common;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;

namespace WorkforceManagementAPI.BLL.Services
{
    public class ValidationService : IValidationService
    {
        private readonly DatabaseContext _context;
        private readonly IIdentityUserManager _userManager;

        public ValidationService(DatabaseContext context, IIdentityUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void EnsureUserExist(User user)
        {
            if (user == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "User"));
            }
        }

        public void EnsureLenghtIsValid(string forCheck, int length, string message)
        {
            if (forCheck.Length <= length)
            {
                throw new InvalidLengthException(String.Format(Constants.InvalidLength, message, length));
            }
        }

        public void EnsureEmailIsValid(string email)
        {
            if (!new EmailAddressAttribute().IsValid(email) || email.Length <= 4)
            {
                throw new InvalidEmailException(Constants.InvalidEmail);
            }
        }

        public async Task EnsureEmailIsUniqueAsync(string email)
        {
            if (await _userManager.VerifyEmail(email) == false)
            {
                throw new EmailAlreadyInUseException(Constants.EmailAreadyInUse);
            }
        }

        public void EnsureTeamExist(Team team)
        {
            if (team == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "Team"));
            }
        }

        public void EnsureTimeOffExist(TimeOff timeOff)
        {
            if (timeOff == null)
            {
                throw new EntityNotFoundException(String.Format(Constants.NotFound, "TimeOff"));
            }
        }

        public void CheckTeamName(string title)
        {
            if (_context.Teams.Any(p => p.Title == title))
            {
                throw new NameExistException(String.Format(Constants.NameAlreadyInUse, "Team name"));
            }
        }

        public async Task EnsureUpdateEmailIsUniqueAsync(string email,User user)
        {
            if (await _userManager.VerifyEmail(email) == false && user.Email != email)
            {
                throw new EmailAlreadyInUseException(Constants.EmailAreadyInUse);
            }
        }

        public void CheckIfUserIsMember(Team team, string userId)
        {
            if (team.Users.Any(u => u.Id == userId))
            {
                throw new Exception("User is already a member.");
            }
        }

        public void CheckIfUserToUnassignIsTeamLeader(Team team, string userId)
        {
            if (team.TeamLeaderId == userId)
            {
                throw new Exception("Can't unassign team leader from the team.");
            }
        }

        public void CheckIfUserToAssignIsTeamLeader(Team team, string userId)
        {
            if (team.TeamLeaderId == userId)
            {
                throw new Exception("User is already the assigned team leader.");
            }
        }

        public void CheckIfUserToAssignIsMember(Team team, string userId)
        {
            if (!team.Users.Any(u => u.Id == userId))
            {
                throw new Exception("Can't assign user as a leader in a team where they are not a member of.");
            }
        }
    }
}
