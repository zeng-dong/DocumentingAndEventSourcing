using FluentAssertions;
using Marten;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weasel.Postgresql;
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

            //_theStore = DocumentStore.For(ConnectionSource.ConnectionString);
            _theStore = DocumentStore.For(x =>
            {
                x.Connection(ConnectionSource.ConnectionString);

                // in prouction : x.AutoCreateSchemaObjects = AutoCreate.None;
                x.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            });

            //_theStore.Advanced.Clean.CompletelyRemoveAll();
        }

        [Fact]
        public void clean_it_off()
        {
            _theStore.Advanced.Clean.CompletelyRemoveAll();
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
                },
                Address = new Address
                {
                    City = "Houston",
                    Country = "US",
                    StateOrProvince = "TX"
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

        [Fact]
        public async Task save_and_load_order_async()
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
                await session.SaveChangesAsync();
            }

            using (var session = _theStore.QuerySession())
            {
                var order2 = await session.LoadAsync<Order>(order.Id);

                order2.Should().NotBeNull();
                order2.Should().NotBeSameAs(order);

                order2.CustomerId.Should().Be(order.CustomerId);

                _output.WriteLine(JsonConvert.SerializeObject(order2));
            }
        }
    }
}