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

            var timeOffService = SetupMockedTimeOffService();
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

        [Fact]
        public async Task Crate_TimeOff_WithoutProperReason_ReturnsFalse()
        {
            var timeOffService = SetupMockedTimeOffService();
            TimeOffRequestDTO newTimeOff = new TimeOffRequestDTO
            {
                Reason = "",
                Type = RequestType.NonPaid,
                StartDate = DateTime.Now,
                EndDate = base.testDate
            };
            var result = await timeOffService.CreateTimeOffAsync(newTimeOff, defaultUser.Id);
            Assert.True(result);
        }
    }
}
