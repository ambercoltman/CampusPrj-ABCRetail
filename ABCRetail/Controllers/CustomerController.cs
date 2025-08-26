using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;
using ABCRetail.Models;

namespace ABCRetail.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableStorage _tableStorage;

        public CustomerController(TableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IActionResult> Index()
        {
            var cust = await _tableStorage.GetAllCustomerAsync();
            return View(cust);
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            customer.PartitionKey = "CustPartition";
            customer.RowKey = Guid.NewGuid().ToString();

            await _tableStorage.AddCustomerAsync(customer);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }
    

    }
}
