using System;
using System.Text.Json;
using Azure.Data.Tables;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ABCFuctions;

public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly string _storageConnectionString;
    private TableClient _tableClient;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;

        //reads connection string 
        _storageConnectionString = Environment.GetEnvironmentVariable("connection");

        //creates the table client
        var serviceClient = new TableServiceClient(_storageConnectionString);
        _tableClient = serviceClient.GetTableClient("ABCTable");
    }

    [Function(nameof(QueueFunction))]
    public async Task QueueFunction([QueueTrigger("abcretail", Connection = "connection")] QueueMessage message)
    {
        _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

        //creates the table if it does not exist
        await _tableClient.CreateIfNotExistsAsync();

        //manually deserilize JSON string into object
        var abcR = JsonSerializer.Deserialize<Retail>(message.MessageText);

        if (abcR == null)
        {
            _logger.LogError("Failed to deserialize JSON message");
            return;
        }

        //set the partitionkey and rowkey
        abcR.PartitionKey = Guid.NewGuid().ToString();
        abcR.PartitionKey = "ABC";

        _logger.LogInformation($"Saving entity with RowKey: {abcR.RowKey}");

        //manually add the entity
        await _tableClient.AddEntityAsync(abcR);
        _logger.LogInformation("Successfully saved person to table.");
    }
}