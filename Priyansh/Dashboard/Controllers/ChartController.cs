using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class ChartsController : Controller
    {
        // GET: /Charts/
        public IActionResult Index()
        {
            return View();
        }
    }
}