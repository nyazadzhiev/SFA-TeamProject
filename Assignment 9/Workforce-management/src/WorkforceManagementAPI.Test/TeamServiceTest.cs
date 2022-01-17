using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Service;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DTO.Models.Requests;
using Xunit;

namespace WorkforceManagementAPI.Test
{
    public class TeamServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task CreateTeam_Should_Return_True_When_Team_Does_Not_Exist()
        {
            var teamService = SetupMockedDefaultTeamService();
            var createRequest = new TeamRequestDTO
            {
                Title = "Test",
                Description = "Test Description"
            };
            
            var result = await teamService.CreateTeamAsync(createRequest.Title, createRequest.Description, Guid.NewGuid().ToString("D"));

            Assert.True(result);
        }
    }
}
