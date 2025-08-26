using System.Collections.Generic;

namespace MyDashboard.Models.ViewModels
{
    public class OrdersPageViewModel
    {
        public OrdersStatsViewModel Stats { get; set; }
        public List<OrderViewModel> Orders { get; set; }
    }

    public class OrdersStatsViewModel
    {
        public int TotalOrders { get; set; }
        public int Delivered { get; set; }
        public int Pending { get; set; }
        public int Processing { get; set; }
    }
}
