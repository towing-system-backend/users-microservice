using Application.Core;

namespace User.Application
{
    using User.Domain;
    public class RegisterUserCommandHandler(IdService<string> idService, IMessageBrokerService messageBrokerService, IEventStore eventStore, IUserRepository userRepository) : IService<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IdService<string> _idService = idService;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;
        private readonly IEventStore _eventStore = eventStore;
        private readonly IUserRepository _userRepository = userRepository;

        async public Task<Result<RegisterUserResponse>> Execute(RegisterUserCommand command)
        {
            var emailRegistered = await _userRepository.FindByEmail(command.Email);
            if (emailRegistered.HasValue()) return Result<RegisterUserResponse>.MakeError(new UserAlreadyExistsError());

            //var id = _idService.GenerateId();
            var user = User.Create(
                new UserId(command.Id),
                new SupplierCompanyId(command.SupplierCompanyId),
                new UserName(command.Name),
                new UserImage(command.Image),
                new UserEmail(command.Email),
                new UserRole(command.Role),
                new UserStatus(command.Status),
                new UserPhoneNumber(command.PhoneNumber),
                new UserIdentificationNumber(command.IdentificationNumber)
            );

            var events = user.PullEvents();
            await _userRepository.Save(user);
            await _eventStore.AppendEvents(events);
            await _messageBrokerService.Publish(events);

            return Result<RegisterUserResponse>.MakeSuccess(new RegisterUserResponse(command.Id));
        }
    }
}