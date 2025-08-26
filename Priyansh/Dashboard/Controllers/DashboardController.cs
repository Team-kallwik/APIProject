using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dashboard.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dashboard/SearchOrders
        // This action handles the search request and returns a partial view with filtered data.
        [HttpGet]
        public IActionResult SearchOrders(string query)
        {
            // For this example, we'll use a static list of orders.
            // In a real application, you would fetch this from a database.
            var allOrders = new List<dynamic>
            {
                new { OrderId = "#98765", Customer = "Vikas Tiwari", Status = "Completed", Total = 72000.00, Date = "2023-10-25" },
                new { OrderId = "#98764", Customer = "Paras Smith", Status = "Pending", Total = 7500.50, Date = "2023-10-24" },
                new { OrderId = "#98763", Customer = "Abhi Jones", Status = "Cancelled", Total = 45.00, Date = "2023-10-23" },
                new { OrderId = "#98762", Customer = "Shivam Doe", Status = "Completed", Total = 9900.99, Date = "2023-10-22" },
                new { OrderId = "#98761", Customer = "Dj Johnson", Status = "Pending", Total = 5500.00, Date = "2023-10-21" },
            };

            var filteredOrders = allOrders.AsEnumerable();

            if (!string.IsNullOrEmpty(query))
            {    
                // Filter the orders based on the query.
                filteredOrders = filteredOrders.Where(o =>
                    o.Customer.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    o.OrderId.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Return a partial view containing just the table rows.
            return PartialView("_OrderTableBody", filteredOrders.ToList());
        }
    }
}
