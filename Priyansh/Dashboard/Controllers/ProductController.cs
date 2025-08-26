using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class ProductController : Controller
    {
        // GET: /Product/
        public IActionResult Index()
        {
            // This will look for a view at Views/Product/Index.cshtml
            return View();
        }
    }
}
