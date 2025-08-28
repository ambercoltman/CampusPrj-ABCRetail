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

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product, IFormFile file)
        {
            if(file != null)
            {
                using var stream = file.OpenReadStream();
                var image = await _blobService.UploadAsync(stream, file.FileName);
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

////IIEVC School of Computer Science, 2025. CLDV6212. [video online]. Available at: <https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7&si=W9pUZezCTxlrsDl2> [Accessed 27 August 2025].