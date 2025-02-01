using BasketService.MessagingBus.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace ProductService.MessageBus;

public interface IMessageRabbitHelper
{
    Task<IConnection> CreateRabbitMqConnection(string hostName, string userName, string password);
    ValueTask<IConnection> CheckCreateRabbitMqConnection(string hostName, string userName, string password);
    byte[] CreateBody(BaseMessage message);
}

public class RabbitMqMessageBusHelper : IMessageRabbitHelper
{
    private static IConnection _connection;
    public async Task<IConnection> CreateRabbitMqConnection(string hostName, string userName, string password)
    {
        try
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            };

            _connection = connectionFactory.CreateConnection();
            return _connection;
        }
        catch (Exception e)
        {
            Console.WriteLine($"can not Create connection: {e.Message}");
            throw;
        }
    }

    public async ValueTask<IConnection> CheckCreateRabbitMqConnection(string hostName, string userName, string password)
    {
        if (_connection is not null)
            return _connection;

        return await CreateRabbitMqConnection(hostName, userName, password);
    }
    public byte[] CreateBody(BaseMessage message)
    {
        var json = JsonConvert.SerializeObject(message);
        return Encoding.UTF8.GetBytes(json);
    }
}
