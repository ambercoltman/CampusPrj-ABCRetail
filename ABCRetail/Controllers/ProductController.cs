using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Controllers
{
    public class ProductController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableStorage _tableStorage;

        public ProductController(BlobService blobservice, TableStorage tableStorage)
        {
            _blobService = blobservice;
            _tableStorage = tableStorage;
        }

        public async Task<IActionResult> Index()
        {
            var prod = await _tableStorage.GetAllProductAsync();
            return View(prod);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile file)
        {
            if(file != null)
            {
                using var stream = file.OpenReadStream();
                var image = await _blobService.UploadAsync(stream, file.Name);
                product.Image = image;
            }

            if(ModelState.IsValid)
            {
                product.PartitionKey = "ProdPartition";
                product.RowKey = Guid.NewGuid().ToString();
                await _tableStorage.AddProductAsync(product);
                return RedirectToAction("Index");   
            }
            return View(product);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string partitionKey, string rowKey, Product product)
        {
            if(product != null && !string.IsNullOrEmpty(product.Image))
            {
                await _blobService.DeleteBlobAsync(product.Image);
            }
            await _tableStorage.DeleteProductAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}
