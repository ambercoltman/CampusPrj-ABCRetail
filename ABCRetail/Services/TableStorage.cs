using ABCRetail.Models;
using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Services
{
    public class TableStorage
    {
        //keeping track of tables going to connect to
        private readonly TableClient _customerTableClient; //for the customer table
        private readonly TableClient _productTableClient; //for the product table
        private readonly TableClient _orderTableClient; //for the order table
        private readonly TableClient _orderSummaryTableClient; //for the order table

        //will take in the connection string, connecting table to the connection string
        public TableStorage(string connectionString)
        {
            _customerTableClient = new TableClient(connectionString, "Customer"); 
            _productTableClient = new TableClient(connectionString, "Product");
            _orderTableClient = new TableClient(connectionString, "Order");
            _orderSummaryTableClient = new TableClient(connectionString, "OrderSummary");
        }

        //getting all the customer async
        public async Task<List<Customer>> GetAllCustomerAsync() //async task list of customer table and will loop and get list of all the customers
        {
            var cust = new List<Customer>();
            await foreach (var customer in _customerTableClient.QueryAsync<Customer>())
            {
                cust.Add(customer);
            }
            return cust;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.PartitionKey) || string.IsNullOrEmpty(customer.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set.");
            }
            try
            {
                await _customerTableClient.AddEntityAsync(customer); //will add customer to the table if the PartitionKey and RowKey have input
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to Table Storage.", ex); //will send error message if adding does not work
            }
        }

        //public async Task EditCustomerAsync(Customer customer,string partitionKey, string rowKey)
        //{
        //    if (string.IsNullOrEmpty(customer.PartitionKey) || string.IsNullOrEmpty(customer.RowKey))
        //    {
        //        throw new ArgumentException("PartitionKey and RowKey must be set.");
        //    }

        //    try
        //    {
        //        await _customerTableClient.GetEntityAsync<Customer>(partitionKey, rowKey);
        //    }
        //    catch (RequestFailedException e)
        //    {
        //        throw new InvalidOperationException("Data not found", e);
        //    }
        //}
        
         public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
        {
            await _customerTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<List<Order>> GetAllOrderAsync() 
        {
            var orders = new List<Order>();
            await foreach (var order in _orderTableClient.QueryAsync<Order>()) 
            {
                orders.Add(order);
            }
            return orders;
        }

        public async Task AddOrderAsync(Order order)
        {
            if (string.IsNullOrEmpty(order.PartitionKey) || string.IsNullOrEmpty(order.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set.");
            }
            try
            {
                await _orderTableClient.AddEntityAsync(order); 
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to Table Storage.", ex); 
            }
        }
        public async Task DeleteOrderAsync(string partitionKey, string rowKey)
        {
            await _orderTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<List<Product>> GetAllProductAsync()
        {
            var prod = new List<Product>();
            await foreach (var products in _productTableClient.QueryAsync<Product>())
            {
                prod.Add(products);
            }
            return prod;
        }

        public async Task AddProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.PartitionKey) || string.IsNullOrEmpty(product.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set.");
            }
            try
            {
                await _productTableClient.AddEntityAsync(product);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to Table Storage.", ex);
            }
        }

        public async Task DeleteProductAsync(string partitionKey, string rowKey)
        {
            await _productTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        //public async Task AddOrderSummaryAsync(OrderSummary summary)
        //{
        //    if (string.IsNullOrEmpty(summary.PartitionKey) || string.IsNullOrEmpty(summary.RowKey))
        //    {
        //        throw new ArgumentException("PartitionKey and RowKey must be set.");
        //    }
        //    try
        //    {
        //        await _orderSummaryTableClient.AddEntityAsync(summary);
        //    }
        //    catch (RequestFailedException ex)
        //    {
        //        throw new InvalidOperationException("Error adding entity to Table Storage.", ex);
        //    }
        //}

        //public async Task<List<OrderSummary>> GetAllOrderSummaryAsync()
        //{
        //    var sum = new List<OrderSummary>();
        //    await foreach (var summary in _orderSummaryTableClient.QueryAsync<OrderSummary>())
        //    {
        //        sum.Add(summary);
        //    }
        //    return sum;
        //}
    }
}
