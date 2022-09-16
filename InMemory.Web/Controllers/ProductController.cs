using InMemory.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {

            if (!_memoryCache.TryGetValue("zaman2", out string zamancache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
                options.SlidingExpiration = TimeSpan.FromSeconds(10);
                options.Priority = CacheItemPriority.High;
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value} => sebep: {reason}");
                });
                _memoryCache.Set<string>("zaman2", DateTime.Now.ToString(),options);
            }

            Product p = new Product {Id = 1,Name="Kalem",Price = 15 };
            _memoryCache.Set<Product>("product:1", p);
            return View();
        }
        public IActionResult Show()
        {
            ViewBag.zaman = _memoryCache.Get<string>("zaman2");
            ViewBag.veri = _memoryCache.Get<Product>("product:1");
            _memoryCache.TryGetValue("callback", out string hatamesaji);
            ViewBag.hata = hatamesaji;
            return View();
        }
    }
}