using Moq;
using System;
using System.Data.Entity;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.Common;
using Xunit;
using System.Threading.Tasks;

namespace WorkforceManagementAPI.Test
{
    public class ValidationServiceTest : ServicesTestBase
    {
        [Fact]
        public void EnsureTeamExist_Must_Throw_Exception_When_Team_Null()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<EntityNotFoundException>(() => validation.EnsureTeamExist(null));
        }

        [Fact]
        public void CheckTeamName_Must_Throw_Exception_When_Team_ExistAsync()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var validation = new ValidationService(mockContext);

            Assert.Throws<NameExistException>(() => validation.CheckTeamName(regularTeam.Title));
        }
    }
}
