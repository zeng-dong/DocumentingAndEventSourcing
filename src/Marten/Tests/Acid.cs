using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using Xunit;

namespace Tests
{
    public class Acid
    {
        private IDocumentStore _theStore;

        public Acid() {
            _theStore = DocumentStore.For(x =>
            {
                x.Connection(ConnectionSource.ConnectionString);

                x.Schema.For<Target>().Index(d => d.Color);

            });
            
            _theStore.Advanced.Clean.CompletelyRemoveAll();
        }

        [Fact]
        public async Task Proving_that_Marten_is_ACID_compliant() {
            var targets = Target.GenerateRandomData(1000).ToArray();
            var greenCount = targets.Count(x => x.Color == Colors.Green);
            
            
            
        }
    }

    public class Target
    {
        private static readonly Random _random = new Random(67);

        private static readonly string[] _strings =
        {
            "Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Violet", "Pink", "Gray", "Black"
        };
        private static readonly string[] _otherStrings =
        {
            "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"
        };
        
        public static IEnumerable<Target> GenerateRandomData(int number) {
            var i = 0;
            while (i < number)
            {
                yield return Random(true);

                i++;
            }
        }

        public static Target Random(bool deep = false) {
            var target = new Target();
            target.String = _strings[_random.Next(0, 10)];
            target.AnotherString = _otherStrings[_random.Next(0, 10)];
            target.Number = _random.Next();

            target.Flag = _random.Next(0, 10) > 5;
            target.Float = Single.Parse(_random.NextDouble().ToString());
            target.NumberArray = new[] { _random.Next(0, 10), _random.Next(0, 10), _random.Next(0, 10) };
            target.NumberArray = target.NumberArray.Distinct().ToArray();

            switch (_random.Next(0, 2))
            {
                case 0:
                    target.Color = Colors.Blue;
                    break;
                case 1:
                    target.Color = Colors.Green;
                    break;
                case 2:
                    target.Color = Colors.Red;
                    break;
            }

            target.Long = 100 * _random.Next();
            target.Double = _random.NextDouble();
            target.Long = _random.Next() * 10000;
            target.Date = DateTime.Today.AddDays(_random.Next(-10000, 1000000)).ToUniversalTime();

            if (deep)
            {
                target.Inner = Random();
                var number = _random.Next(1, 10);
                

            }
        }

        public int[] NumberArray { get; set; }

        public float Float { get; set; }

        public bool Flag { get; set; }

        public string String { get; set; }
        public string AnotherString { get; set; }
        public string Color { get; set; }
        public int Number { get; set; }
    }

    public class Colors
    {
    }

}