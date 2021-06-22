using System;
using AutoFixture;
using FluentAssertions;
using Moq;
using TestApp.CreditProviders;
using TestApp.DataAccess;
using TestApp.Models;
using TestApp.Repositories;
using TestApp.Services;
using TestApp.Validation;
using Xunit;

namespace TestApp.UnitTests
{
    public class UserServiceTest
    {
        private readonly UserService _sut;
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IUserDataAccess> _userDataAccessMock;
        private readonly Mock<IUserCreditService> _userCreditServiceMock;
        private readonly CreditLimitProviderFactory _creditLimitProviderFactory;
        private readonly UserValidator _userValidator;
        private readonly Fixture _fixture;

        public UserServiceTest()
        {
            _creditLimitProviderFactory = new CreditLimitProviderFactory(_userCreditServiceMock.Object);
            _userValidator = new UserValidator(_dateTimeProviderMock.Object);
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _userCreditServiceMock = new Mock<IUserCreditService>();

            _fixture = new Fixture();

            _sut = new UserService(_clientRepositoryMock.Object,
                _creditLimitProviderFactory,
                _userDataAccessMock.Object,
                _userValidator);
        }

        [Fact]
        public void AddUser_ShouldCreateUser_WhenAllParametersAreValid()
        {
            //Arrange

            const int clientId = 1;
            const string email = "test@test.com";
            var client = _fixture.Build<Client>().With(_ => _.Id, clientId).Create();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var dateOfBirth = new DateTime(1992, 1, 1);

            _dateTimeProviderMock.Setup(_ => _.DateTimeUtcNow).Returns(new DateTime(2021, 3, 17));
            _clientRepositoryMock.Setup(_ => _.GetById(It.IsAny<int>())).Returns(client);
            _userCreditServiceMock
                .Setup(_ => _.GetCreditLimit(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(600);

            //Act

            var result = _sut.AddUser(firstName, lastName, email, dateOfBirth, clientId);

            //Assert
            result.Should().BeTrue();

            _userDataAccessMock.Verify(_ => _.AddUser(It.IsAny<User>()), Times.Once);
        }
    }
}
