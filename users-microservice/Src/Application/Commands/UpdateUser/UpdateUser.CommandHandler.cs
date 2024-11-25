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
            if (command.Name != null) user.UpdateUserName(new UserName(command.Name));
            if (command.IdentificationNumber != null) user.UpdateUserIdentificationNumber(new UserIdentificationNumber((int)command.IdentificationNumber));
            if (command.Email != null) user.UpdateUserEmail(new UserEmail(command.Email));

            var events = user.PullEvents();
            await _userRepository.Save(user);
            await _eventStore.AppendEvents(events);
            await _messageBrokerService.Publish(events);

            return Result<UpdateUserResponse>.MakeSuccess(new UpdateUserResponse(command.Id));
        }
    }
}