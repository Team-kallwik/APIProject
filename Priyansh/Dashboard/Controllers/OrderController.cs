using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.Controllers
{
    public class OrderController : Controller
    {
        // Static list to simulate a database.
        private static  List<dynamic> orders = new List<dynamic>
        {
            new { OrderId = "#98765", Customer = "Vikas Tiwari", Status = "Completed", Total = 72000.00m, Date = "2023-10-25" },
            new { OrderId = "#98764", Customer = "Paras Vanve", Status = "Pending", Total = 7500.50m, Date = "2023-10-24" },
            new { OrderId = "#98763", Customer = "Abhishek Sharma", Status = "Cancelled", Total = 450.00m, Date = "2023-10-23" },
            new { OrderId = "#98762", Customer = "Shivam Gour", Status = "Return", Total = 9900.99m, Date = "2023-10-22" },
        };

        // GET: Order
        public IActionResult Index()
        {
            ViewBag.Orders = orders;
            return View();
        }

        // Action to add a new order
        [HttpPost]
        public IActionResult AddOrder([FromBody] dynamic newOrder)
        {
            if (newOrder != null)
            {
                orders.Add(newOrder);
                return Ok();
            }
            return BadRequest();
        }

        // Action to get the orders table HTML
        [HttpGet]
        public IActionResult GetOrdersTable()
        {
            return PartialView("_OrderTable", orders);
        }
    }
}
