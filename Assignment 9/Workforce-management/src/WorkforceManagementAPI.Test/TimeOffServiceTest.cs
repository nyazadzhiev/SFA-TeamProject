using System;
using System.Threading.Tasks;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;
using Xunit;

namespace WorkforceManagementAPI.Test
{
    public class TimeOffServiceTest : ServicesTestBase
    {
        [Fact]
      public async Task Create_TimeOff_Successfully_ReturnsTrue()
        {
            
            var timeOffService = await SetupMockedTimeOffService();
            TimeOffRequestDTO newTimeOff = new TimeOffRequestDTO
            {
                Reason = "test",
                Type = RequestType.NonPaid,
                StartDate = DateTime.Now,
                EndDate = base.testDate
            };
            var result = await timeOffService.CreateTimeOffAsync(newTimeOff, defaultUser.Id);
            Assert.True(result);
        }  
    }
}
