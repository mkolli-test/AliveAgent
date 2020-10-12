
namespace Alive.Agent.Message.Brokers
{
    internal static class MessageBrokerFactory
    {
        const string brokerConnectionStringRabbitMq = "amqp://alive_user:password@localhost/alive.vhost";
        const string topicExchange = "alive.orchestration.exchange";
        const string queueName = "alive.orchestration.agent.queue";

        public static (
            MessageBrokerPublisherBase messageBrokerPublisher,
            MessageBrokerSubscriberBase messageBrokerSubscriber) Create(MessageBrokerType messageBrokerType)
        {
            switch (messageBrokerType)
            {
                case MessageBrokerType.RabbitMq:
                    return (
                        new MessageBrokerPublisherRabbitMq(brokerConnectionStringRabbitMq, topicExchange),
                        new MessageBrokerSubscriberRabbitMq(brokerConnectionStringRabbitMq, topicExchange, queueName));
               
            }

            throw new MessageBrokerTypeNotSupportedException($"The MessageBrokerType: {messageBrokerType}, is not supported yet");
        }
    }

    internal enum MessageBrokerType { RabbitMq, ServiceBus }
}
