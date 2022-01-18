using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Repositories;
using WorkforceManagementAPI.DTO.Models.Requests;

namespace WorkforceManagementAPI.BLL.Service
{
    public class TeamService : ITeamService
    {
        private readonly DatabaseContext _context;
        private readonly IValidationService _validationService;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamService(DatabaseContext context, IValidationService validationService, ITeamRepository teamRepository, IMapper mapper)
        {
            _validationService = validationService;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _teamRepository.GetAllTeamsAsync();
        }

        public async Task<List<Team>> GetMyTeamsAsync(string userId)
        {
            return await _teamRepository.GetMyTeamsAsync(userId);
        }

        public async Task<Team> GetTeamByIdAsync(Guid teamId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            return team;
        }

        public async Task<bool> CreateTeamAsync(TeamRequestDTO teamRequest, string creatorId)
        {
            _validationService.CheckTeamName(teamRequest.Title);

            var now = DateTime.Now;
            var team = _mapper.Map<Team>(teamRequest);
            team.CreatorId = creatorId;
            team.ModifierId = creatorId;
            team.CreatedAt = now;
            team.ModifiedAt = now;

            await _teamRepository.AddTeamAsync(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditTeamAsync(Guid teamId, string modifierId, TeamRequestDTO editTeamRequest)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);
            _validationService.CheckTeamNameForEdit(editTeamRequest.Title, team.Title);

            team.Title = editTeamRequest.Title;
            team.Description = editTeamRequest.Description;
            team.ModifierId = modifierId;
            team.ModifiedAt = DateTime.Now;

            _teamRepository.UpdateTeam(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTeamAsync(Guid teamId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            _teamRepository.RemoveTeam(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignUserToTeamAsync(Guid teamId, string userId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            _validationService.EnsureUserExist(user);

            _validationService.CheckIfUserIsMember(team, userId);

            if (team.Users.Count == 0)
            {
                team.TeamLeaderId = userId;
                _teamRepository.UpdateTeam(team);
            }

            _teamRepository.AddTeamUser(team, user);

            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnassignUserFromTeamAsync(Guid teamId, string userId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            _validationService.EnsureUserExist(user);

            _validationService.CheckIfUserToUnassignIsTeamLeader(team, userId);

            _teamRepository.RemoveTeamUser(team, user);

            await _teamRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignTeamLeaderAsync(Guid teamId, string userId)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            _validationService.EnsureTeamExist(team);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            _validationService.EnsureUserExist(user);

            _validationService.CheckIfUserToAssignIsTeamLeader(team, userId);
            _validationService.CheckIfUserToAssignIsMember(team, userId);

            team.TeamLeaderId = userId;
            _teamRepository.UpdateTeam(team);
            await _teamRepository.SaveChangesAsync();

            return true;
        }
    }
}
