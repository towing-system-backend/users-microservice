using Application.Core;
using Moq;
using User.Application;
using User.Domain;
using Xunit;

namespace User.Test
{
    public class UpdateUserCommandHandlerTest
    {
        private readonly Mock<IMessageBrokerService> _messageBrokerServiceMock;
        private readonly Mock<IEventStore> _eventStoreMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;

        public UpdateUserCommandHandlerTest()
        {
            _messageBrokerServiceMock = new Mock<IMessageBrokerService>();
            _eventStoreMock = new Mock<IEventStore>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _updateUserCommandHandler = new UpdateUserCommandHandler(_messageBrokerServiceMock.Object, _eventStoreMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Not_Update_User_When_User_Not_Found()
        {
            // Arrange
            var command = new UpdateUserCommand(
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

            _userRepositoryMock.Setup(repo => repo.FindById(command.Id))
                .ReturnsAsync(Optional<Domain.User>.Empty());

            // Act
            var result = await _updateUserCommandHandler.Execute(command);

            // Assert
            Assert.True(result.IsError);
            var exception = Assert.Throws<UserNotFoundError>(() => result.Unwrap());
            Assert.Equal("User not found.", exception.Message);

            _userRepositoryMock.Verify(repo => repo.Save(It.IsAny<Domain.User>()), Times.Never);
            _eventStoreMock.Verify(store => store.AppendEvents(It.IsAny<List<DomainEvent>>()), Times.Never);
            _messageBrokerServiceMock.Verify(service => service.Publish(It.IsAny<List<DomainEvent>>()), Times.Never);
        }

        [Fact]
        public async Task Should_Not_Update_User_When_No_Properties_Are_Updated()
        {
            // Arrange
            var command = new UpdateUserCommand(
                "036b4f28-8e02-4c85-b613-66f4809137a4",
                null, null, null, null, null, null, null, null
            );

            var user = Domain.User.Create(
                new UserId("036b4f28-8e02-4c85-b613-66f4809137a4"),
                new SupplierCompanyId("c85f3f68-c8f5-4c50-b37c-2f3863f84377"),
                new UserName("Julio Gonzales"),
                new UserImage("https://t3.ftcdn.net/jpg/02/99/04/20/360_F_299042079_vGBD7wIlSeNl7vOevWHiL93G4koMM967.jpg"),
                new UserEmail("julio_gonza@hotmail.com"),
                new UserRole("Admin"),
                new UserStatus("Active"),
                new UserPhoneNumber("04145244854"),
                new UserIdentificationNumber(20014515),
                true
            );

            _userRepositoryMock.Setup(repo => repo.FindById(command.Id))
                .ReturnsAsync(Optional<Domain.User>.Of(user));

            // Act
            var result = await _updateUserCommandHandler.Execute(command);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(command.Id, result.Unwrap().UserId);

            _userRepositoryMock.Verify(repo => repo.Save(It.Is<Domain.User>(savedUser =>
                savedUser.Equals(user)
            )), Times.Once);

            _eventStoreMock.Verify(store => store.AppendEvents(It.Is<List<DomainEvent>>(events => 
            events.Count == 0)), Times.Once);

            _messageBrokerServiceMock.Verify(service => service.Publish(It.Is<List<DomainEvent>>(events => 
            events.Count == 0)), Times.Once);
        }

        [Fact]
        public async Task Should_Update_User_Name()
        {
            // Arrange
            var command = new UpdateUserCommand(
                "036b4f28-8e02-4c85-b613-66f4809137a4",
                null, "Julio A. Gonzales", null, null, null, null, null, null
            );

            var user = Domain.User.Create(
                new UserId("036b4f28-8e02-4c85-b613-66f4809137a4"),
                new SupplierCompanyId("c85f3f68-c8f5-4c50-b37c-2f3863f84377"),
                new UserName("Julio Gonzales"),
                new UserImage("https://t3.ftcdn.net/jpg/02/99/04/20/360_F_299042079_vGBD7wIlSeNl7vOevWHiL93G4koMM967.jpg"),
                new UserEmail("julio_gonza@hotmail.com"),
                new UserRole("Admin"),
                new UserStatus("Active"),
                new UserPhoneNumber("04145244854"),
                new UserIdentificationNumber(20014515),
                true
            );

            _userRepositoryMock.Setup(repo => repo.FindById(command.Id))
                .ReturnsAsync(Optional<Domain.User>.Of(user));

            // Act
            var result = await _updateUserCommandHandler.Execute(command);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(command.Id, result.Unwrap().UserId);

            _userRepositoryMock.Verify(repo => repo.Save(It.Is<Domain.User>(savedUser =>
                savedUser.GetUserName().GetValue() == command.Name
            )), Times.Once);

            _eventStoreMock.Verify(store => store.AppendEvents(It.Is<List<DomainEvent>>(events =>
                events.Count == 1 && events[0] is UserNameUpdatedEvent
            )), Times.Once);

            _messageBrokerServiceMock.Verify(service => service.Publish(It.Is<List<DomainEvent>>(events =>
                events.Count == 1 && events[0] is UserNameUpdatedEvent
            )), Times.Once);
        }

        [Fact]
        public async Task Should_Update_User_Name_And_Email()
        {
            // Arrange
            var command = new UpdateUserCommand(
                "036b4f28-8e02-4c85-b613-66f4809137a4",
                null, "Julio", null, "julio@gmail.com", null, null, null, null
            );

            var user = Domain.User.Create(
                new UserId("036b4f28-8e02-4c85-b613-66f4809137a4"),
                new SupplierCompanyId("c85f3f68-c8f5-4c50-b37c-2f3863f84377"),
                new UserName("Julio Gonzales"),
                new UserImage("https://t3.ftcdn.net/jpg/02/99/04/20/360_F_299042079_vGBD7wIlSeNl7vOevWHiL93G4koMM967.jpg"),
                new UserEmail("julio_gonza@hotmail.com"),
                new UserRole("Admin"),
                new UserStatus("Active"),
                new UserPhoneNumber("04145244854"),
                new UserIdentificationNumber(20014515),
                true
            );

            _userRepositoryMock.Setup(repo => repo.FindById(command.Id))
                .ReturnsAsync(Optional<Domain.User>.Of(user));

            // Act
            var result = await _updateUserCommandHandler.Execute(command);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(command.Id, result.Unwrap().UserId);

            _userRepositoryMock.Verify(repo => repo.Save(It.Is<Domain.User>(savedUser =>
                savedUser.GetUserName().GetValue() == command.Name &&
                savedUser.GetUserEmail().GetValue() == command.Email
            )), Times.Once);

            _eventStoreMock.Verify(store => store.AppendEvents(It.Is<List<DomainEvent>>(events =>
                events.Count == 2 &&
                events.Any(e => e is UserNameUpdatedEvent) &&
                events.Any(e => e is UserEmailUpdatedEvent)
            )), Times.Once);

            _messageBrokerServiceMock.Verify(service => service.Publish(It.Is<List<DomainEvent>>(events =>
                events.Count == 2 &&
                events.Any(e => e is UserNameUpdatedEvent) &&
                events.Any(e => e is UserEmailUpdatedEvent)
            )), Times.Once);
        }


        [Fact]
        public async Task Should_Update_All_User_Properties()
        {
            // Arrange
            var command = new UpdateUserCommand(
                "036b4f28-8e02-4c85-b613-66f4809137a4",
                "b4071a1b-7e46-4271-b5c6-b78456c0c0c6",
                "New Name",
                "new_image_url",
                "new_email@example.com",
                "Employee",
                "Inactive",
                "04145240000",
                12345678
            );

            var user = Domain.User.Create(
                new UserId("036b4f28-8e02-4c85-b613-66f4809137a4"),
                new SupplierCompanyId("c85f3f68-c8f5-4c50-b37c-2f3863f84377"),
                new UserName("Julio Gonzales"),
                new UserImage("https://t3.ftcdn.net/jpg/02/99/04/20/360_F_299042079_vGBD7wIlSeNl7vOevWHiL93G4koMM967.jpg"),
                new UserEmail("julio_gonza@hotmail.com"),
                new UserRole("Admin"),
                new UserStatus("Active"),
                new UserPhoneNumber("04145244854"),
                new UserIdentificationNumber(20014515),
                true
            );

            _userRepositoryMock.Setup(repo => repo.FindById(command.Id))
                .ReturnsAsync(Optional<Domain.User>.Of(user));

            // Act
            var result = await _updateUserCommandHandler.Execute(command);

            // Assert
            Assert.False(result.IsError);
            Assert.Equal(command.Id, result.Unwrap().UserId);

            _userRepositoryMock.Verify(repo => repo.Save(It.Is<Domain.User>(savedUser =>
                savedUser.GetSupplierCompanyId().GetValue() == command.SupplierCompanyId &&
                savedUser.GetUserName().GetValue() == command.Name &&
                savedUser.GetUserImage().GetValue() == command.Image &&
                savedUser.GetUserEmail().GetValue() == command.Email &&
                savedUser.GetUserRole().GetValue() == command.Role &&
                savedUser.GetStatus().GetValue() == command.Status &&
                savedUser.GetUserPhoneNumber().GetValue() == command.PhoneNumber &&
                savedUser.GetUserIdentificationNumber().GetValue() == command.IdentificationNumber
            )), Times.Once);

            _eventStoreMock.Verify(store => store.AppendEvents(It.Is<List<DomainEvent>>(events =>
                events.Count == 8 &&
                events.Any(e => e is SupplierCompanyIdUpdatedEvent) &&
                events.Any(e => e is UserNameUpdatedEvent) &&
                events.Any(e => e is UserImageUpdatedEvent) &&
                events.Any(e => e is UserEmailUpdatedEvent) &&
                events.Any(e => e is UserRoleUpdatedEvent) &&
                events.Any(e => e is UserStatusUpdatedEvent) &&
                events.Any(e => e is UserPhoneNumberUpdatedEvent) &&
                events.Any(e => e is UserIdentificationNumberUpdatedEvent)
            )), Times.Once);

            _messageBrokerServiceMock.Verify(service => service.Publish(It.Is<List<DomainEvent>>(events =>
                events.Count == 8 &&
                events.Any(e => e is SupplierCompanyIdUpdatedEvent) &&
                events.Any(e => e is UserNameUpdatedEvent) &&
                events.Any(e => e is UserImageUpdatedEvent) &&
                events.Any(e => e is UserEmailUpdatedEvent) &&
                events.Any(e => e is UserRoleUpdatedEvent) &&
                events.Any(e => e is UserStatusUpdatedEvent) &&
                events.Any(e => e is UserPhoneNumberUpdatedEvent) &&
                events.Any(e => e is UserIdentificationNumberUpdatedEvent)
            )), Times.Once);
        }
    }
}