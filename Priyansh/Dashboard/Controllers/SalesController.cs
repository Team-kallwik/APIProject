using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class SalesController : Controller
    {
        // GET: /Sales/
        public IActionResult Index()
        {
            // This will look for a view at Views/Sales/Index.cshtml
            return View();
        }
    }
}
