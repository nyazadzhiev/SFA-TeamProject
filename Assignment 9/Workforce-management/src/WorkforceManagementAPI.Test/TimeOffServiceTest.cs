using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;
using Xunit;

namespace WorkforceManagementAPI.Test
{
    public class TimeOffServiceTest : ServicesTestBase
    {
        [Fact]
        public async Task CreateTimeOff_Should_Return_True_When_TimeOff_Doesnt_Exist()
        {
            var mockDB = SetupDefaultMockedDBTimeOffServiceAsync();
            var mockValidation = new Mock<IValidationService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotification = new Mock<INotificationService>();
            var service = new TimeOffService(mockDB, mockValidation.Object, mockUserService.Object, mockNotification.Object);

            var result = await service.CreateTimeOffAsync(testTimeOff.Reason, testTimeOff.Type, testTimeOff.StartDate, testTimeOff.EndDate, testTimeOff.CreatorId);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTimeOff_Should_Return_True_When_TimeOff_Exist()
        {
            var mockDB = SetupMockedDBTimeOffServiceAsync();
            var mockValidation = new Mock<IValidationService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotification = new Mock<INotificationService>();
            var service = new TimeOffService(mockDB, mockValidation.Object, mockUserService.Object, mockNotification.Object);

            var result = await service.DeleteTimeOffAsync(testTimeOff.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task EditTimeOff_Should_Return_True_When_TimeOff_Exist()
        {
            var mockDB = SetupMockedDBTimeOffServiceAsync();
            var mockValidation = new Mock<IValidationService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotification = new Mock<INotificationService>();
            var service = new TimeOffService(mockDB, mockValidation.Object, mockUserService.Object, mockNotification.Object);

            var result = await service.EditTimeOffAsync(testTimeOff.Id, testTimeOff.Reason, testTimeOff.StartDate, testTimeOff.EndDate, testTimeOff.Type);

            Assert.True(result);
        }

        [Fact]
        public async Task GetTimeOff_Should_Return_Null_When_TimeOff_Doesnt_Exist()
        {
            var mockDB = SetupDefaultMockedDBTimeOffServiceAsync();
            var mockValidation = new Mock<IValidationService>();
            var mockUserService = new Mock<IUserService>();
            var mockNotification = new Mock<INotificationService>();
            var service = new TimeOffService(mockDB, mockValidation.Object, mockUserService.Object, mockNotification.Object);

            var result = await service.GetTimeOffAsync(testTimeOff.Id);

            Assert.Null(result);
        }
    }
}
