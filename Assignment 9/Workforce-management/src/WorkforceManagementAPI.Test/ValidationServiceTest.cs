using Moq;
using System;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Exceptions;
using WorkforceManagementAPI.BLL.Services;
using Xunit;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;

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

            Assert.Throws<NameExistException>(() => validation.EnsureTeamNameIsUniquee(regularTeam.Title));
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureInputFitsBoundaries(10, 0, 5));
        }

        [Fact]
        public void EnsureInputFitsBoundaries_Must_Throw_Exception_When_Date_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureInputFitsBoundaries(new DateTime(2022, 1, 14), new DateTime(2022, 1, 15), new DateTime(2023, 1, 1)));
        }

        [Fact]
        public void ValidateDateRange_Must_Throw_Exception_When_Date_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureDateRangeIsValid(new DateTime(2022, 1, 18), new DateTime(2022, 1, 15)));
        }

        [Fact]
        public void CheckAccessToTeam_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UnauthorizedUserException>(() => validation.EnsureUserHasAccessToTeam(regularTeam, defaultUser));
        }

        [Fact]
        public void CheckTeamLeader_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UserAlreadyTeamLeaderException>(() => validation.EnsureUserIsNotAlreadyATeamLeader(regularTeam, TeamLeader));
        }

        [Fact]
        public void CanAddToTeam_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UserAlreadyInTeamException>(() => validation.EnsureUserIsNotAlreadyPartOfTheTeam(regularTeam, TeamLeader));
        }

        [Fact]
        public void EnsureNoReviewersLeft_Must_Throw_Exception_When_TimeOff_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<RequestAlreadyCompletedException>(() => validation.EnsureNoReviewersLeft(noReviewersTimeOff));
        }

        [Fact]
        public void EnsureUserIsReviewer_Must_Throw_Exception_When_User_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<UnauthorizedUserException>(() => validation.EnsureUserIsReviewer(testTimeOff, defaultUser));
        }

        [Fact]
        public void EnsureResponseIsValid_Must_Throw_Exception_When_Input_Invalid()
        {
            var mockContext = SetupMockedDBValidationServiceAsync();

            var mockedManager = new Mock<IIdentityUserManager>();
            var validation = new ValidationService(mockContext, mockedManager.Object);

            Assert.Throws<InputOutOfBoundsException>(() => validation.EnsureResponseIsValid(Status.Awaiting));
        }
    }
}
