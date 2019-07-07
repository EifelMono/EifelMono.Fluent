using System;
using System.Collections.Generic;
using EifelMono.Fluent.Cataloge;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.CoreTests
{

    public class CatalogCoreTest : XunitCore
    {
        public CatalogCoreTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void ViewCataloge()
        {
            var o1 = new
            {
                Name = "Name.1",
                Items = new List<string>
                {
                    "Items1.1",
                    "Items1.2",
                    "Items1.3",
                    "Items1.4",
                },
                o2 = new
                {
                    Name = "name.2",
                    Items = new List<string>
                    {
                        "Items2.1",
                        "Items2.2",
                        "Items2.3",
                        "Items2.4",
                    },
                }
            };
            var cataloge = o1.ToCataloge();
            Console.WriteLine(cataloge);
        }
    }
}
