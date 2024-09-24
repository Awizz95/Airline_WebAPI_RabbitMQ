using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Airline.TicketProcessing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the ticketing service!");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password",
                VirtualHost = "/"
            };

            var conn = factory.CreateConnection();

            using var channel = conn.CreateModel();

            channel.QueueDeclare("bookings", durable: true, exclusive: false);

            var consumer = new EventingBasicConsumer(channel);

            //подписка на событие получения сообщения
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"New ticket processing is initiated for - {message}");
            };

            //регистрирует потребителя для получения сообщений
            channel.BasicConsume(queue: "bookings", autoAck: true, consumer);

            Console.ReadKey();
        }

    }
}
