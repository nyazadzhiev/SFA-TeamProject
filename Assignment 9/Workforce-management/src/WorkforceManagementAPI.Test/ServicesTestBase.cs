using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Contracts;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.DAL.Entities.Enums;
using WorkforceManagementAPI.DTO.Models.Requests;
using WorkforceManagementAPI.WEB.AutoMapperProfiles;

namespace WorkforceManagementAPI.Test
{
    public class ServicesTestBase
    {
        private static IMapper _mapper;

        public DateTime testDate;

        public CreateUserRequestDTO inputUser;
        public User defaultUser { get; set; }

        public User TeamLeader { get; set; }

        public Team regularTeam { get; set; }

        public TimeOff testTimeOff { get; set; }

        public List<TimeOff> TestTimeOffList { get; set; }

        public ServicesTestBase()
        {
            testDate = new DateTime(2022, 2, 1, 16, 5, 7, 123);

            this.TeamLeader = new User();

            this.defaultUser = new User()
            {
                UserName = "test@abv.bg",
                Email = "test@abv.bg",
                FirstName = "test",
                LastName = "tester",
                Id = "7cd150cd-413d-43d1-bdff-73cc5d4f04e3"
            };

            this.regularTeam = new Team()
            {
                Title = "testteam",
                TeamLeader = TeamLeader,
                TeamLeaderId = TeamLeader.Id,
                Users = new List<User>() { TeamLeader }
            };

            this.testTimeOff = new TimeOff()
            {
                Reason = "Test",
                CreatedAt = DateTime.Now,
                CreatorId = defaultUser.Id,
                ModifiedAt = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = testDate,
                Type = RequestType.Paid,
            };

            inputUser = new CreateUserRequestDTO
            {
                Email = "asd@abv.bg",
                FirstName = "Andrey",
                LastName = "Andrey",
                Password = "123456789",
                RepeatPassword = "123456789"
            };

            TestTimeOffList = new List<TimeOff>();
        }

        protected ValidationService SetupMockedDefaultValidationService()
        {
            var dbContextMock = new Mock<DatabaseContext>();
            var mockedManager = new Mock<IIdentityUserManager>();

            var validation = new ValidationService(dbContextMock.Object, mockedManager.Object);

            return validation;
        }

        protected DatabaseContext SetupMockedDBValidationServiceAsync()
        {
            List<Team> teams = new List<Team>();
            teams.Add(regularTeam);

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(m => m.Teams).Returns(() => ToDbSet(teams));


            return mockContext.Object;
        }

        protected DatabaseContext SetupDefaultMockedDBTimeOffServiceAsync()
        {
            List<TimeOff> requests = new List<TimeOff>();

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(m => m.Requests).Returns(() => ToDbSet(requests));

            return mockContext.Object;
        }

        protected DatabaseContext SetupMockedDBTimeOffServiceAsync()
        {
            List<TimeOff> requests = new List<TimeOff>();
            requests.Add(testTimeOff);

            var mockContext = new Mock<DatabaseContext>();
            mockContext.Setup(m => m.Requests).Returns(() => ToDbSet(requests));

            return mockContext.Object;
        }

        private DbSet<Team> ToDbSet(List<Team> sourceList)
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<Team>>();
            mockSet.As<IQueryable<Team>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Team>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Team>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Team>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<Team>())).Callback<Team>(sourceList.Add);

            return mockSet.Object;
        }

        private DbSet<TimeOff> ToDbSet(List<TimeOff> sourceList)
        {
            var queryable = sourceList.AsQueryable();

            var mockSet = new Mock<DbSet<TimeOff>>();
            mockSet.As<IQueryable<TimeOff>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<TimeOff>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<TimeOff>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<TimeOff>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            mockSet.Setup(d => d.Add(It.IsAny<TimeOff>())).Callback<TimeOff>(sourceList.Add);

            return mockSet.Object;
        }

        protected UserService SetupMockedDefaultUserService()
        {
            SetupMockedMapperUser();
            var mockedManager = new Mock<IIdentityUserManager>();
            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }

        private static void SetupMockedMapperUser()
        {
            var myProfile = new UserProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);
        }

        protected UserService SetupMockedDefaultUserServiceWithDefaultUser()
        {
            SetupMockedMapperUser();
            var mockedManager = new Mock<IIdentityUserManager>();
            mockedManager.Setup(userRep => userRep.FindByIdAsync(It.IsAny<String>()))
                    .ReturnsAsync(new User());

            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }

        protected UserService SetupMockedDefaultUserServiceWithDefaultUsers()
        {
            SetupMockedMapperUser();
            var mockedManager = new Mock<IIdentityUserManager>();
            mockedManager.Setup(userRep => userRep.GetAllAsync())
                    .ReturnsAsync(new List<User>());

            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }

        protected Mapper SetupMockedTimeOff()
        {
            var timeOffProfile = new TimeOffProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(timeOffProfile));
            return new Mapper(configuration);

        }

        protected UserService SetupMockedDefaultUserServiceForTimeOffs()
        {
            SetupMockedMapperUser();

            var mockedManager = new Mock<IIdentityUserManager>();
            mockedManager.Setup(userRep => userRep.FindByIdAsync(It.IsAny<String>()))
                    .ReturnsAsync(defaultUser);
            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }

        protected TimeOffService SetupMockedTimeOffService()
        {

            var mockedTimeOffMapper = SetupMockedTimeOff();
            var validationServiceMock = new Mock<IValidationService>();
            var userServiceMock = SetupMockedDefaultUserServiceForTimeOffs();
            var notificationServiceMock = new Mock<INotificationService>();
            var timeOffRepositoryMock = new Mock<ITimeOffRepository>();

            timeOffRepositoryMock.Setup(to => to.GetAllAsync()).ReturnsAsync(TestTimeOffList);
            timeOffRepositoryMock.Setup(tr => tr.GetTimeOffAsync(It.IsAny<Guid>()))
                .ReturnsAsync(testTimeOff);

            var timeOffService = new TimeOffService(validationServiceMock.Object, userServiceMock
                , notificationServiceMock.Object, mockedTimeOffMapper, timeOffRepositoryMock.Object);

            return timeOffService;
        }

        protected IIdentityUserManager SetupMockedUserManager()
        {
            var userManagerMock = new Mock<IIdentityUserManager>();
            return userManagerMock.Object;
        }

        protected TimeOffService SetupMockedTimeOffServiceForMyTimeOffsMethod()
        {

            var mockedTimeOffMapper = SetupMockedTimeOff();
            var validationServiceMock = new Mock<IValidationService>();
            var userServiceMock = SetupMockedDefaultUserServiceForTimeOffs();
            var notificationServiceMock = new Mock<INotificationService>();
            var timeOffRepositoryMock = new Mock<ITimeOffRepository>();

            timeOffRepositoryMock.Setup(tr => tr.GetMyTimeOffsAsync(defaultUser.Id)).ReturnsAsync(TestTimeOffList);

            var timeOffService = new TimeOffService(validationServiceMock.Object, userServiceMock
                , notificationServiceMock.Object, mockedTimeOffMapper, timeOffRepositoryMock.Object);

            return timeOffService;
        }

    }
}
