using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class OrderController : Controller
    {
        private readonly TableStorage _tableStorage;
        private readonly QueueService _queueService;

        public OrderController(TableStorage tableStorage, QueueService queueService)
        {
            _tableStorage = tableStorage;
            _queueService = queueService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _tableStorage.GetAllOrderSummaryAsync();
            return View(orders);
        }

        public async Task<IActionResult> Summary()
        {
            var cust = await _tableStorage.GetAllCustomerAsync();
            var prod = await _tableStorage.GetAllProductAsync();

            if(cust == null || cust.Count == 0)
            {
                ModelState.AddModelError("", "No customer found. Please add the customer first.");
                return View();
            }
            if (prod == null || prod.Count == 0)
            {
                ModelState.AddModelError("", "No product found. Please add the product first.");
                return View();
            }
            ViewData["Customer"] = cust;
            ViewData["Product"] = prod;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Summary(Order order)
        {
            if(ModelState.IsValid)
            {
                //table service
                order.Order_Date = DateTime.SpecifyKind(order.Order_Date, DateTimeKind.Utc);
                order.PartitionKey = "OrderPartition";
                order.RowKey = Guid.NewGuid().ToString();
                await _tableStorage.AddOrderSummaryAsync(order);

                //message queue
                string message = $"New order by customer {order.cust_ID}" + $" the product {order.product_ID}" + $" at {order.Order_Location} on {order.Order_Date}" + $"status is {order.Order_Status}";
                await _queueService.SendSummaryMessage(message);
                return RedirectToAction("Index");
            }
            //reload the customer and pproduct list if the validation fails
            var cust = await _tableStorage.GetAllCustomerAsync();
            var prod = await _tableStorage.GetAllProductAsync();
            ViewData["Customer"] = cust;
            ViewData["Product"] = prod;
            return View(order);
        }
    }
}
