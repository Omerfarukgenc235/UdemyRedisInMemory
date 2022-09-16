using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashkey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if(db.KeyExists(hashkey))
            {
                //db.HashGet(hashkey, "pen");
                db.HashGetAll(hashkey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name,string val)
        {
            db.HashSet(hashkey, name, val);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            db.HashDelete(hashkey, name);
            return RedirectToAction("Index");
        }
    }
}
