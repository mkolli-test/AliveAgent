using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace AliveAgent
{
    public static class MessageSubscriberRabbitMQ : Mees
    {
       public static void RecieveRabbitMQMessages()
    {
            var hostName = "localhost";
            var uri = $"amqp://transcode_user:password@{hostName}/video.transcode.vhost";
            string app_url;

           var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(uri)
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine("waiting for messages...");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    //header details
                    var message_id = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["message_id"]);
                    app_url = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["app_url"]);
                    var page_details = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["page_details"]);
                    var content_type = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["content_type"]);

                    Console.WriteLine($"message id: {message_id},{Environment.NewLine} app url: {app_url},{Environment.NewLine} page details: {page_details},{Environment.NewLine} content type: {content_type}{Environment.NewLine}");

                    //read the message
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());

                    Console.WriteLine($"[x] {message}");
                    channel.BasicAck(ea.DeliveryTag, multiple: false);
                };

                var queueName = "videoreceived.queue";
                var consumerTag = channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

                Console.WriteLine("press enter to exit");
                Console.ReadLine();
                channel.BasicCancel(consumerTag);
               // return app_url;
            }
        }
     
    }
}
