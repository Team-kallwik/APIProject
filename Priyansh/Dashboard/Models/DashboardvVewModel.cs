using System.Collections.Generic;

namespace MyDashboard.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
        public int ActiveProducts { get; set; }

        // Chart Data
        public List<string> Months { get; set; } = new();
        public List<decimal> SalesMonthly { get; set; } = new();
        public List<decimal> RevenueMonthly { get; set; } = new();
    }
}
