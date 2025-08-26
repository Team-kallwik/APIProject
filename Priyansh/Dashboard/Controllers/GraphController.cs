using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class GraphsController : Controller
    {
        // GET: /Graphs/
        public IActionResult Index()
        {
            return View();
        }
    }
}