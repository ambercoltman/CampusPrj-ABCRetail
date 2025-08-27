using Azure.Storage.Queues;

namespace ABCRetail.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(string connectionString, string queue)
        {
            _queueClient = new QueueClient(connectionString, queue);
        }

        public async Task SendSummaryMessage(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }
    }
}
