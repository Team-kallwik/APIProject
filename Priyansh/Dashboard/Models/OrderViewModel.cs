using System;

namespace MyDashboard.Models.ViewModels
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Products { get; set; }
        public int ItemsCount { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
    }
}
