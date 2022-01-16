using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Service;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.DAL.Entities;
using WorkforceManagementAPI.WEB.AutoMapperProfiles;

namespace WorkforceManagementAPI.Test
{
    public class ServicesTestBase
    {
        private static IMapper _mapper;
        public User defaultUser { get; set; }

        public Team regularTeam { get; set; }

        public TimeOff testTimeOff { get; set; }

        public ServicesTestBase()
        {
            this.regularTeam = new Team()
            {
                Title = "testteam"
            };

            this.testTimeOff = new TimeOff()
            {
            };
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
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            var mockedManager = new Mock<IIdentityUserManager>();
            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }

        protected UserService SetupMockedDefaultUserServiceWithDefaultUser()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var mockedManager = new Mock<IIdentityUserManager>();
            mockedManager.Setup(userRep => userRep.FindByIdAsync(It.IsAny<String>()))
                    .ReturnsAsync(new User());

            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }

        protected UserService SetupMockedDefaultUserServiceWithDefaultUsers()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var mockedManager = new Mock<IIdentityUserManager>();
            mockedManager.Setup(userRep => userRep.GetAllAsync())
                    .ReturnsAsync(new List<User>());

            var validationMock = new Mock<IValidationService>();
            var userService = new UserService(mockedManager.Object, validationMock.Object, _mapper);

            return userService;
        }


    }
}
