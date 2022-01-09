using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.BLL.Services;
using Xunit;

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
