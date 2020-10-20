using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using System.Threading;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("routing key:");
        var routingKey = Console.ReadLine();

        Console.WriteLine("message: ");
        var message = Console.ReadLine();

        Console.WriteLine("number of messages: ");
        int numOfMassages;
        while(!int.TryParse(Console.ReadLine(), out numOfMassages))
        {
            Console.WriteLine("invalied input!");
        }

        Console.WriteLine("amount of messages per second: ");
        int messagesPerSecond;
        while (!int.TryParse(Console.ReadLine(), out messagesPerSecond))
        {
            Console.WriteLine("invalied input!");
        }

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

            for (int i = 0; i < numOfMassages; i++)
            {
                if (i % messagesPerSecond == 0)
                    Thread.Sleep(1000);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "topic_logs", routingKey: routingKey, basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
            }
        }
    }
}