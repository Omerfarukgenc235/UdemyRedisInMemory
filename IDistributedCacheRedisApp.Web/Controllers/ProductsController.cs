using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            _distributedCache.SetString("name", "Ömer", cacheEntryOptions);
            await _distributedCache.SetStringAsync("surname", "Genç", cacheEntryOptions);
            return View();
        }
        public async Task<IActionResult> Add()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            Product product = new Product { Id = 1, Name = "Denemelerim", Price = 15 };
            string jsonproduct = JsonConvert.SerializeObject(product);
            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            _distributedCache.Set("product:1", byteproduct);
            //await _distributedCache.SetStringAsync("product:2", jsonproduct,cacheEntryOptions);
            return View();
        }
        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonproduct = Encoding.UTF8.GetString(byteProduct);
            //string jsonproduct = _distributedCache.GetString("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);
            ViewBag.product = p;
            //ViewBag.data = _distributedCache.GetString("name");
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/download.jpg");
            byte[] imageByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("resim", imageByte);
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimbyte = _distributedCache.Get("resim");
            return File(resimbyte,"image/jpg");
        }

    }
}