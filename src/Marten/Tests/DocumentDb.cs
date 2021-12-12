using FluentAssertions;
using Marten;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class DocumentDb
    {
        private readonly ITestOutputHelper _output;
        private readonly DocumentStore _theStore;

        public DocumentDb(ITestOutputHelper output)
        {
            _output = output;

            _theStore = DocumentStore.For(ConnectionSource.ConnectionString);

            _theStore.Advanced.Clean.CompletelyRemoveAll();
        }

        [Fact]
        public void clean_it_off()
        {
        }

        [Fact]
        public void save_and_load_order()
        {
            var order = new Order
            {
                Priority = Priority.High,
                CustomerId = "Kroger",
                Details = new List<OrderDetail>()
                {
                    new OrderDetail { PartNumber = "Abc101", Number = 5 },
                    new OrderDetail { PartNumber = "Apple", Number = 15 }
                }
            };

            using (var session = _theStore.LightweightSession())
            {
                session.Store(order);
                session.SaveChanges();
            }

            _output.WriteLine($"The order id is {order.Id}");

            using (var session = _theStore.QuerySession())
            {
                var order2 = session.Load<Order>(order.Id);
                order2.Should().NotBeNull();
                order2.Should().NotBeSameAs(order);

                order2.CustomerId.Should().Be(order.CustomerId);

                _output.WriteLine(JsonConvert.SerializeObject(order2));
            }
        }
    }
}