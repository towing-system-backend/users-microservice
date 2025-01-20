using Application.Core;
using User.Application;
using Microsoft.AspNetCore.Mvc;
using User.Domain;
using Microsoft.AspNetCore.Authorization;

namespace User.Infrastructure
{
    [ApiController]
    [Route("api/user")]
    public class UserController(
        IdService<string> idService,
        Logger logger,
        IMessageBrokerService messageBrokerService,
        IEventStore eventStore,
        IUserRepository userRepository,
        IPerformanceLogsRepository performanceLogsRepository
    ) : ControllerBase
    {
        private readonly IdService<string> _idService = idService;
        private readonly Logger _logger = logger;
        private readonly IMessageBrokerService _messageBrokerService = messageBrokerService;
        private readonly IEventStore _eventStore = eventStore;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPerformanceLogsRepository _performanceLogsRepository = performanceLogsRepository;

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("create")]
        public async Task<ObjectResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = new RegisterUserCommand(
                createUserDto.Id,
                createUserDto.SupplierCompanyId,
                createUserDto.Name,
                createUserDto.Image,
                createUserDto.Email,
                createUserDto.Role,
                createUserDto.Status,
                createUserDto.PhoneNumber,
                createUserDto.IdentificationNumber
            );

            var handler =
                new ExceptionCatcher<RegisterUserCommand, RegisterUserResponse>(
                    new PerfomanceMonitor<RegisterUserCommand, RegisterUserResponse>(
                        new LoggingAspect<RegisterUserCommand, RegisterUserResponse>(
                            new RegisterUserCommandHandler(_messageBrokerService, _eventStore, _userRepository), _logger
                        ), _logger, _performanceLogsRepository, nameof(RegisterUserCommandHandler), "Write"
                    ), ExceptionParser.Parse
                );
            var res = await handler.Execute(command);

            return Ok(res.Unwrap());
        }
        [Authorize(Roles = "Admin, Provider")]
        [HttpPatch("update")]
        public async Task<ObjectResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var command = new UpdateUserCommand(
                updateUserDto.Id,
                updateUserDto.SupplierCompanyId,
                updateUserDto.Name,
                updateUserDto.Image,
                updateUserDto.Email,
                updateUserDto.Role,
                updateUserDto.Status,
                updateUserDto.PhoneNumber,
                updateUserDto.IdentificationNumber
            );

            var handler =
                new ExceptionCatcher<UpdateUserCommand, UpdateUserResponse>(
                    new PerfomanceMonitor<UpdateUserCommand, UpdateUserResponse>(
                        new LoggingAspect<UpdateUserCommand, UpdateUserResponse>(
                            new UpdateUserCommandHandler(_messageBrokerService, _eventStore, _userRepository), _logger
                        ), _logger, _performanceLogsRepository, nameof(UpdateUserCommandHandler), "Write"
                    ), ExceptionParser.Parse
                );
            var res = await handler.Execute(command);

            return Ok(res.Unwrap());
        }

        [HttpGet("find/{id}")]
        [Authorize(Roles = "Admin, Provider")]
        public async Task<ObjectResult> FindUserById(string id)
        {
            var query = new FindUserByIdQuery();
            var res = await query.Execute(id);
            return Ok(res.Unwrap());
        }

        [HttpGet("find/all")]
        [Authorize(Roles = "Admin, Provider")]
        public async Task<ObjectResult> FindAllUsers()
        {
            var query = new FindAllUsersQuery();
            var res = await query.Execute("");
            return Ok(res.Unwrap());
        }

        [HttpGet("find/names/users")]
        [Authorize(Roles = "Admin, Provider")]
        public async Task<ObjectResult> FindUsersName()
        {
            var query = new FindUsersNameQuery();
            var res = await query.Execute("");
            return Ok(res.Unwrap());
        }
        
        [HttpGet("find/role/{role}")]
        [Authorize(Roles = "Admin, Provider")]
        public async Task<ObjectResult> FindUsersByRole(string role)
        {
            var query = new FindUsersByRoleQuery();
            var res = await query.Execute(role);
            return Ok(res.Unwrap());
        }
    }
}