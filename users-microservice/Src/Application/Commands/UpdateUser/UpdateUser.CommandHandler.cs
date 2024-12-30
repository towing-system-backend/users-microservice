using Application.Core;

namespace User.Application
{
    using User.Domain;
    public class UpdateUserCommandHandler(IMessageBrokerService messageBrokerService, IEventStore eventStore, IUserRepository userRepository) : IService<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;
        private readonly IEventStore _eventStore = eventStore;
        private readonly IUserRepository _userRepository = userRepository;

        async public Task<Result<UpdateUserResponse>> Execute(UpdateUserCommand command)
        {
            var userRegistered = await _userRepository.FindById(command.Id);
            if (!userRegistered.HasValue()) return Result<UpdateUserResponse>.MakeError(new UserNotFoundError());

            var user = userRegistered.Unwrap();
            if (command.SupplierCompanyId != null) user.UpdateSupplierCompanyId(new SupplierCompanyId(command.SupplierCompanyId));
            if (command.Name != null) user.UpdateUserName(new UserName(command.Name));
            if (command.Image != null) user.UpdateUserImage(new UserImage(command.Image));
            if (command.Email != null) user.UpdateUserEmail(new UserEmail(command.Email));
            if (command.Role != null) user.UpdateUserRole(new UserRole(command.Role));
            if (command.Status != null) user.UpdateUserStatus(new UserStatus(command.Status));
            if (command.PhoneNumber != null) user.UpdateUserPhoneNumber(new UserPhoneNumber(command.PhoneNumber));
            if (command.IdentificationNumber != null) user.UpdateUserIdentificationNumber(new UserIdentificationNumber((int)command.IdentificationNumber));

            var events = user.PullEvents();
            await _userRepository.Save(user);
            await _eventStore.AppendEvents(events);
            await _messageBrokerService.Publish(events);

            return Result<UpdateUserResponse>.MakeSuccess(new UpdateUserResponse(command.Id));
        }
    }
}