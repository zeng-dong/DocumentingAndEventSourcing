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
        public Address Address { get; set; }
    }

    public class OrderDetail
    {
        public string PartNumber { get; set; }
        public int Number { get; set; }
    }

    public class Address
    {
        public  string Address1 { get; set; }
        public  string Address2 { get; set; }
        public  string City { get; set; }
        public  string StateOrProvince { get; set; }
        public  string Country { get; set; }
        public  string PostalCode { get; set; }
        
        public bool Primary { get; set; }
    }
}