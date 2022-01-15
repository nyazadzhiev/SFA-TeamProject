using Moq;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.BLL.Services.IdentityServices;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
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
        public void EnsureUserExist_Must_Throw_Exception_When_User_Null()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<EntityNotFoundException>(() => validation.EnsureUserExist(null));
        }

        [Fact]
        public void EnsureTimeOffExist_Must_Throw_Exception_When_TimeOff_Null()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<EntityNotFoundException>(() => validation.EnsureTimeOffExist(null));
        }

        [Fact]
        public void EnsureLenghtIsValid_Must_Throw_Exception_When_Length_Invalid()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<InvalidLengthException>(() => validation.EnsureLenghtIsValid("1", 2, "1"));
        }

        [Fact]
        public void EnsureMailIsValid_Must_Throw_Exception_When_Mail_Invalid()
        {
            var validation = SetupMockedDefaultValidationService();

            Assert.Throws<InvalidEmailException>(() => validation.EnsureEmailIsValid("mail"));
        }

        [Fact]
        public async Task EnsureEmailIsUniqueAsync_Must_Throw_Exception_When_Mail_Invalid()
        {
            var validation = SetupMockedDefaultValidationService();

            await Assert.ThrowsAsync<EmailAlreadyInUseException>(() => validation.EnsureEmailIsUniqueAsync("mail"));
        }

        [Fact]
        public void CheckTeamName_Must_Throw_Exception_When_Team_ExistAsync()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<NameExistException>(() => validation.CheckTeamName(regularTeam.Title));
        }
    }
}
