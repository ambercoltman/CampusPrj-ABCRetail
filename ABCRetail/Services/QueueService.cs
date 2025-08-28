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

////IIEVC School of Computer Science, 2025. CLDV6212. [video online]. Available at: <https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7&si=W9pUZezCTxlrsDl2> [Accessed 28 August 2025].