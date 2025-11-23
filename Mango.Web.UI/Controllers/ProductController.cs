using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.UI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
