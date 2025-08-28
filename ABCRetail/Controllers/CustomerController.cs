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

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(string partitionKey, string rowKey, Customer cust)
        {
            if (cust != null && !string.IsNullOrEmpty(cust.PartitionKey) || cust != null && !string.IsNullOrEmpty(cust.RowKey))
            {
                await _tableStorage.DeleteCustomerAsync(partitionKey, rowKey);
            }
            await _tableStorage.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}

////IIEVC School of Computer Science, 2025. CLDV6212. [video online]. Available at: <https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7&si=W9pUZezCTxlrsDl2> [Accessed 27 August 2025].