using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class ProductController : Controller
    {
        private readonly TableStorage _tableStorage;

        public ProductController(TableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IActionResult> Index()
        {
            var prod = await _tableStorage.GetAllProductAsync();
            return View(prod);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.PartitionKey = "ProdPartition";
            product.RowKey = Guid.NewGuid().ToString();

            await _tableStorage.AddProductAsync(product);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}
