using Application.Core;
using User.Application;
using Microsoft.AspNetCore.Mvc;
using User.Domain;

namespace User.Infrastructure
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IdService<string> idService, IMessageBrokerService messageBrokerService, IEventStore eventStore, IUserRepository userRepository) : ControllerBase
    {
        private readonly IdService<string> _idService = idService;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;
        private readonly IEventStore _eventStore = eventStore;
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost("create")]
        public async Task<ObjectResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = new RegisterUserCommand(createUserDto.Name, createUserDto.Email, createUserDto.IdentificationNumber);
            var handler = 
                new ExceptionCatcher<RegisterUserCommand, RegisterUserResponse>(
                    new PerfomanceMonitor<RegisterUserCommand, RegisterUserResponse>(
                        new LoggingAspect<RegisterUserCommand, RegisterUserResponse>(
                            new RegisterUserCommandHandler(_idService, _messageBrokerService, _eventStore, _userRepository)
                        )
                    ), ExceptionParser.Parse
                );
            var res = await handler.Execute(command);

            return Ok(res.Unwrap());
        }

        [HttpPatch("update")]
        public async Task<ObjectResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var command = new UpdateUserCommand(updateUserDto.Id, updateUserDto.Name, updateUserDto.Email, updateUserDto.IdentificationNumber);
            var handler =
                new ExceptionCatcher<UpdateUserCommand, UpdateUserResponse>(
                    new PerfomanceMonitor<UpdateUserCommand, UpdateUserResponse>(
                        new LoggingAspect<UpdateUserCommand, UpdateUserResponse>(
                            new UpdateUserCommandHandler(_messageBrokerService, _eventStore, _userRepository)
                        )
                    ), ExceptionParser.Parse
                );
            var res = await handler.Execute(command);

            return Ok(res.Unwrap());
        }
    }
}
