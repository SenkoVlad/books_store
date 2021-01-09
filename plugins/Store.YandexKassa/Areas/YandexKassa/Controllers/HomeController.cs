using Microsoft.AspNetCore.Mvc;
using Store.YandexKassa.Areas.YandexKassa.Models;
using Store.Web.App;

namespace Store.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        public IActionResult Index(int orderId, string returnUri)
        {
            var model = new ExampleModel 
            {
                OrderId = orderId, 
                ReturnUri = returnUri
            };

            return View(model);
        }
        // /YandexKassa/Home/Callback
        public IActionResult Callback(int orderId, string returnUri)
        {
            //HttpContext.Session.RemoveCart();

            var model = new ExampleModel
            {
                OrderId = orderId,
                ReturnUri = returnUri
            };

            return View(model);
        }
    }
}
