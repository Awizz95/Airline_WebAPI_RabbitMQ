using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Airline.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendingMessage<T>(T message)
        {
            //Класс для управления соединением с RabbitMQ
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                VirtualHost = "/"
            };

            //открытие TCP соединения по протоколу AMQP
            var conn = factory.CreateConnection();

            //создание канала
            using var channel = conn.CreateModel();

            //создание очереди
            channel.QueueDeclare("bookings", durable: true, exclusive: false);

            var jsonString = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(jsonString);

            //Оптправка сообщения в очередь через default exchange
            channel.BasicPublish("", "bookings", body: body);
        }
    }
}
