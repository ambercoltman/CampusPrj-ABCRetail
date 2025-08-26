using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class OrderController : Controller
    {
        private readonly TableStorage _tableStorage;

        public OrderController(TableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _tableStorage.GetAllOrderAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            order.PartitionKey = "OrdersPartition";
            order.RowKey = Guid.NewGuid().ToString();

            await _tableStorage.AddOrderAsync(order);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }
    }
}
