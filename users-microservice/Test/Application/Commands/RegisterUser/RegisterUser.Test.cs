using Application.Core;
using Moq;
using User.Application;
using User.Domain;
using Xunit;

namespace User.Test
{
    public class RegisterUserCommandHandlerTest
    {
        private readonly Mock<IMessageBrokerService> _messageBrokerServiceMock;
        private readonly Mock<IEventStore> _eventStoreMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly RegisterUserCommandHandler _registerUserCommandHandler;

        public RegisterUserCommandHandlerTest()
        {
            _messageBrokerServiceMock = new Mock<IMessageBrokerService>();
            _eventStoreMock = new Mock<IEventStore>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _registerUserCommandHandler = new RegisterUserCommandHandler(_messageBrokerServiceMock.Object, _eventStoreMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Not_Register_User_When_User_Already_Exits()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "036b4f28-8e02-4c85-b613-66f4809137a4",
                "c85f3f68-c8f5-4c50-b37c-2f3863f84377",
                "Julio Gonzales",
                "https://t3.ftcdn.net/jpg/02/99/04/20/360_F_299042079_vGBD7wIlSeNl7vOevWHiL93G4koMM967.jpg",
                "julio_gonza@hotmail.com",
                "Admin",
                "Active",
                "04145244854",
                20014515
            );

            _userRepositoryMock.Setup(repo => repo.FindByEmail(command.Email))
                .ReturnsAsync(
                    Optional<Domain.User>.Of(
                        Domain.User.Create(
                            new UserId("036b4f28-8e02-4c85-b613-66f4809137a4"),
                            new SupplierCompanyId("c85f3f68-c8f5-4c50-b37c-2f3863f84377"),
                            new UserName("Julio Gonzales"),
                            new UserImage("https://t3.ftcdn.net/jpg/"),
                            new UserEmail("julio_gonza@hotmail.com"),
                            new UserRole("Admin"),
                            new UserStatus("Active"),
                            new UserPhoneNumber("04145244854"),
                            new UserIdentificationNumber(20014515),
                            true
                        )
                    )
                );

            // Act
            var result = await _registerUserCommandHandler.Execute(command);

            // Assert
            Assert.True(result.IsError);
            var exception = Assert.Throws<UserAlreadyExistsError>(() => result.Unwrap());
            Assert.Equal("User already exists.", exception.Message);

            _userRepositoryMock.Verify(repo => repo.Save(It.IsAny<Domain.User>()), Times.Never);
            _eventStoreMock.Verify(store => store.AppendEvents(It.IsAny<List<DomainEvent>>()), Times.Never);
            _messageBrokerServiceMock.Verify(service => service.Publish((List<DomainEvent>)It.IsAny<IEnumerable<DomainEvent>>()), Times.Never);
        }

        [Fact]
        public async Task Should_Register_User()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "036b4f28-8e02-4c85-b613-66f4809137a4",
                "c85f3f68-c8f5-4c50-b37c-2f3863f84377",
                "Julio Gonzales",
                "https://t3.ftcdn.net/jpg/02/99/04/20/360_F_299042079_vGBD7wIlSeNl7vOevWHiL93G4koMM967.jpg",
                "julio_gonza@hotmail.com",
                "Admin",
                "Active",
                "04145244854",
                20014515
            );

            _userRepositoryMock.Setup(repo => repo.FindByEmail(command.Email))
                .ReturnsAsync(Optional<Domain.User>.Empty());

            // Act
            var result = await _registerUserCommandHandler.Execute(command);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(command.Id, result.Unwrap().UserId);

            _userRepositoryMock.Verify(repo => repo.Save(It.Is<Domain.User>(user =>
                    user.GetUserId().GetValue()== command.Id &&
                    user.GetSupplierCompanyId().GetValue() == command.SupplierCompanyId &&
                    user.GetUserName().GetValue() == command.Name &&
                    user.GetUserImage().GetValue() == command.Image &&
                    user.GetUserEmail().GetValue() == command.Email &&
                    user.GetUserRole().GetValue() == command.Role &&
                    user.GetStatus().GetValue() == command.Status &&
                    user.GetUserPhoneNumber().GetValue() == command.PhoneNumber &&
                    user.GetUserIdentificationNumber().GetValue() == command.IdentificationNumber
                )
            ), Times.Once);

            _eventStoreMock.Verify(store => store.AppendEvents(It.Is<List<DomainEvent>>(events => 
                    events.Count == 1 &&
                    events[0] is UserCreatedEvent
                )
            ), Times.Once);

            _messageBrokerServiceMock.Verify(service => service.Publish(It.Is<List<DomainEvent>>(events =>
                    events.Count == 1 &&
                    events[0] is UserCreatedEvent
                )
            ), Times.Once);
        }
    }
}