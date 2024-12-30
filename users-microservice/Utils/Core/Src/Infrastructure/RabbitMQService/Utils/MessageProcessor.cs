using RabbitMQ.Contracts;
using System.Reflection;
using User.Infrastructure;

namespace Application.Core
{
    public class MessageProcessor(IServiceProvider serviceProvider)
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Task ProcessMessage(IRabbitMQMessage message)
        {
            var controller = _serviceProvider.GetRequiredService<UserController>();
            var method = controller.GetType().GetMethod($"{message.GetType().Name}", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (method == null) return Task.FromResult(false);

            var dto = DtoCreator<IRabbitMQMessage, IDto>.GetDto(message);

            method.Invoke(controller, new object[] { dto });

            return Task.CompletedTask;
        }
    }
}
