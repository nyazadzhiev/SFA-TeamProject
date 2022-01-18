using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Service;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DTO.Models.Requests;
using Xunit;

namespace WorkforceManagementAPI.Test
{
    public class TeamServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task CreateTeam_ReturnsTrue()
        {
            var teamService = SetupMockedDefaultTeamService();
            var createRequest = new TeamRequestDTO
            {
                Title = "Test",
                Description = "Test Description"
            };
            
            var result = await teamService.CreateTeamAsync(createRequest.Title, createRequest.Description, defaultUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task EditTeam_ReturnsTrue()
        {
            var teamService = SetupMockedDefaultTeamService();
            var editRequest = new TeamRequestDTO
            {
                Title = "Edited Test Title",
                Description = "Edited Test Description"
            };

            var result = await teamService.EditTeamAsync(regularTeam.Id, defaultUser.Id, editRequest.Title, editRequest.Description);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTeam_ReturnsTrue()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.DeleteTeamAsync(regularTeam.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task AssignUserToTeam_ReturnsTrue()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.AssignUserToTeamAsync(regularTeam.Id, defaultUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task UnassignUserToTeam_ReturnsTrue()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.UnassignUserFromTeamAsync(regularTeam.Id, defaultUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task AssignTeamLeader_ReturnsTrue()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.AssignTeamLeaderAsync(regularTeam.Id, defaultUser.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task GetAllTeams_ReturnsListOfTeams()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.GetAllTeamsAsync();

            Assert.Equal(typeof(List<Team>), result.GetType());
        }

        [Fact]
        public async Task GetMyTeams_ReturnsListOfTeams()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.GetMyTeamsAsync(defaultUser.Id);

            Assert.Equal(typeof(List<Team>), result.GetType());
        }

        [Fact]
        public async Task GetTeamById_ReturnsTeam()
        {
            var teamService = SetupMockedDefaultTeamService();

            var result = await teamService.GetTeamByIdAsync(regularTeam.Id);

            Assert.Equal(typeof(Team), result.GetType());
        }
    }
}
