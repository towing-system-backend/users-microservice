using MassTransit;
using RabbitMQ.Contracts;

namespace Application.Core
{
    public class CreateUserConsumer(IServiceProvider serviceProvider) : IConsumer<CreateUser>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public Task Consume(ConsumeContext<CreateUser> @event)
        {
            var message = @event.Message;
            new MessageProcessor(_serviceProvider).ProcessMessage(message);

            return Task.CompletedTask;
        }
    }
}