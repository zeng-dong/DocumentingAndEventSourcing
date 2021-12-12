using System;
using System.Collections.Generic;

namespace Tests
{
    public enum Priority
    {
        Low,
        High
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Priority Priority { get; set; }
        public string CustomerId { get; set; }
        public IList<OrderDetail> Details { get; set; } = new List<OrderDetail>();
    }

    public class OrderDetail
    {
        public string PartNumber { get; set; }
        public int Number { get; set; }
    }
}