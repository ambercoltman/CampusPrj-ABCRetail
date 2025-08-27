using System.ComponentModel.DataAnnotations;
using Azure;

namespace ABCRetail.Models
{
    public class OrderSummary
    {
        [Key]
        public int orderSummary_ID { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        //validation sample
        [Required(ErrorMessage = "Please select a customer.")]
        public int custName {  get; set; } //FK to the Customer who made the order

        [Required(ErrorMessage = "Please select the customer's address.")]
        public string? address { get; set; } //FK to the Customer who made the order

        [Required(ErrorMessage = "Please select the product.")]
        public string? prodName { get; set; } //FK to the Product name

        [Required(ErrorMessage = "Please select the number of orders.")]
        public int? Orders { get; set; } //FK to the number of Orders made

        [Required(ErrorMessage = "Please select the price.")]
        public string? Price { get; set; } //FK to the price of the Product

        [Required(ErrorMessage = "Please select the payment status.")]
        public string? paymentStatus { get; set; } //FK to the payment staus of the Order

        [Required(ErrorMessage = "Please select the availability of the product.")]
        public string? Availability { get; set; } //FK to the availability of the Product



    }
}
