using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.UI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
