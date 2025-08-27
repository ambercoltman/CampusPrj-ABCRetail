using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class OrderSummaryController : Controller
    {
        private readonly TableStorage _tableStorage;
        private readonly QueueService _queueService;

        public OrderSummaryController(TableStorage tableStorage, QueueService queueService)
        {
            _tableStorage = tableStorage;
            _queueService = queueService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var sum = await _tableStorage.GetAllOrderSummaryAsync();
        //    return View(sum);
        //}

        public async Task<IActionResult> Summary()
        {
            var cust = await _tableStorage.GetAllCustomerAsync();
            var ord = await _tableStorage.GetAllOrderAsync();
            var prod = await _tableStorage.GetAllProductAsync();

            if(cust == null || cust.Count == 0)
            {
                //when no customer is found
                ModelState.AddModelError("", "There is no customer with this information found. Please add a customer first");
                return View();
            }

            if (ord == null || ord.Count == 0)
            {
                //when no order is found
                ModelState.AddModelError("", "There is no order with this information found. Please add an order first");
                return View();
            }

            if (prod == null || prod.Count == 0)
            {
                //when no product is found
                ModelState.AddModelError("", "There is no product with this information found. Please add a product first");
                return View();
            }
            ViewData["Customer"] = cust;
            ViewData["Order"] = ord;
            ViewData["Product"] = prod;
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Summary(OrderSummary summary)
        {
            if(ModelState.IsValid)
            {
                //table service
                
                //message queue
            }
            //reloads table lists if the validation fails
            var cust = await _tableStorage.GetAllCustomerAsync();
            var ord = await _tableStorage.GetAllOrderAsync();
            var prod = await _tableStorage.GetAllProductAsync();
            ViewData["Customer"] = cust;
            ViewData["Order"] = ord;
            ViewData["Product"] = prod;
            return View(summary);
        }
    }
}
