using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models
{
    public class Order : ITableEntity
    {
        [Key]
        public int order_ID { get; set; }
        public string? Name { get; set; }
        public string? phoneNumber { get; set; }
        public string? Address { get; set; }
        public int? Orders { get; set; }
        public int? Price { get; set; }
        public string? paymentMethod { get; set; }
        public string? paymentStatus { get; set; }

        //ITableEntity implementation
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
