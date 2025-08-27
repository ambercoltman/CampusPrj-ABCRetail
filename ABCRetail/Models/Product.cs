using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models
{
    public class Product : ITableEntity
    {
        [Key]
        public int product_ID { get; set; }
        public string? prodName { get; set; }
        public string? Description { get; set; }
        public string? Price { get; set; }
        public string? Availability { get; set; }
        public string? Image { get; set; }

        //ITableEntity implementation
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
