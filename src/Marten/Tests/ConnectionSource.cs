using Marten;
using System;

namespace Tests
{
    public class ConnectionSource : ConnectionFactory
    {
        public static readonly string Default = "Host=localhost;Port=5432;Database=TestingMarten;Username=tester;password=tester";

        public static readonly string ConnectionString = Environment.GetEnvironmentVariable("marten_testing_database") ?? Default;

        public ConnectionSource(Func<string> connectionSource) : base(connectionSource)
        {
        }
    }
}