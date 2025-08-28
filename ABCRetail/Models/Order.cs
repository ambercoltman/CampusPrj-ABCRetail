using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Models
{
    public class Order : ITableEntity
    {
        [Key]
        public int order_ID { get; set; }
        [Required(ErrorMessage ="Please select a customer.")]
        public int cust_ID { get; set; } //FK to order
        [Required(ErrorMessage = "Please select a product.")]
        public int product_ID { get; set; } //FK to product
        [Required(ErrorMessage = "Please select a date.")]
        public DateTime Order_Date { get; set; } 
        [Required(ErrorMessage = "Please enter the location.")]
        public string? Order_Location { get; set; }
        [Required(ErrorMessage = "Please enter the status.")]
        public string? Order_Status {get; set;}

        //ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
