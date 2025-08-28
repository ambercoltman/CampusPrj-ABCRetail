using ABCRetail.Models;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Services
{
    public class TableStorage
    {
        //keeping track of tables going to connect to
        private readonly TableClient _customerTableClient; //for the customer table
        private readonly TableClient _productTableClient; //for the product table
        private readonly TableClient _orderTableClient; //for the order table
        //will take in the connection string, connecting table to the connection string
        public TableStorage(string connectionString)
        {
            _customerTableClient = new TableClient(connectionString, "Customer"); 
            _productTableClient = new TableClient(connectionString, "Product");
            _orderTableClient = new TableClient(connectionString, "Order");
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


        //public async Task<IActionResult>DetailCustomer(Customer customer, string partitionKey, string rowKey)
        //{
        //    var e = _customerTableClient.GetEntity<Customer>(partitionKey, rowKey);
        //    Console.WriteLine($"Name: {e.Value.custName}, Surname: {e.Value.surname}, Email: {e.Value.email}, Phone Number: {e.Value.phoneNumber}, Address: {e.Value.address}");
            
        //}

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

        //public async Task AddOrderAsync(Order order)
        //{
        //    if (string.IsNullOrEmpty(order.PartitionKey) || string.IsNullOrEmpty(order.RowKey))
        //    {
        //        throw new ArgumentException("PartitionKey and RowKey must be set.");
        //    }
        //    try
        //    {
        //        await _orderTableClient.AddEntityAsync(order); 
        //    }
        //    catch (RequestFailedException ex)
        //    {
        //        throw new InvalidOperationException("Error adding entity to Table Storage.", ex); 
        //    }
        //}
        //public async Task DeleteOrderAsync(string partitionKey, string rowKey)
        //{
        //    await _orderTableClient.DeleteEntityAsync(partitionKey, rowKey);
        //}

        //gets all of the products
        public async Task<List<Product>> GetAllProductAsync()
        {
            var prod = new List<Product>();
            await foreach (var products in _productTableClient.QueryAsync<Product>())
            {
                prod.Add(products);
            }
            return prod;
        }

        //adds the products
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

        public async Task AddOrderSummaryAsync(Order summary)
        {
            if (string.IsNullOrEmpty(summary.PartitionKey) || string.IsNullOrEmpty(summary.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set.");
            }
            try
            {
                await _orderTableClient.AddEntityAsync(summary);
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException("Error adding entity to Table Storage.", ex);
            }
        }

        public async Task<List<Order>> GetAllOrderSummaryAsync()
        {
            var order = new List<Order>();
            await foreach (var summary in _orderTableClient.QueryAsync<Order>())
            {
                order.Add(summary);
            }
            return order;
        }
    }
}

////IIEVC School of Computer Science, 2025. CLDV6212. [video online]. Available at: <https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7&si=W9pUZezCTxlrsDl2> [Accessed 27 August 2025].