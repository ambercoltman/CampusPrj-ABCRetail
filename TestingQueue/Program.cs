using System.Text.Json;
using Azure.Storage.Queues;

namespace TestingQueue
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=abcretailfunction;AccountKey=YIyHhdoNAkUbtRfnuXJw7w4Pfy1uVqCpn88pjtna9JsdbkIoIHlp1xQU9X9m5qvQGuvlD4L5zpaB+ASt+Q+LYg==;EndpointSuffix=core.windows.net";

            var queueClient = new QueueClient(
                connectionString, "abcretail", new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 }  //ensure its base64
            );

            //create queue if it doesn't exist
            await queueClient.CreateIfNotExistsAsync();

            //build test object
            var abcR = new { custName = "Amber", surname = "Coltman", email = "Amber@example.com", phoneNumber = 0774355574, address = "23 Main Road", prodName = "Iron Flame", Description = "Dragons", Price = "R350", Availability = "Yes" };

            //serialize object to JSON
            string json = JsonSerializer.Serialize(abcR);

            //send a plain JSON string
            await queueClient.SendMessageAsync(json);
            Console.WriteLine($"Message sent: {json}");
        }
    }
}
