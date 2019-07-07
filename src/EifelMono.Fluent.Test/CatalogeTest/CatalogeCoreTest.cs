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
                StringItems = new List<string>
                {
                    "Items1.1",
                    "Items1.2",
                    "Items1.3",
                },
                IntItems = new List<int>
                {
                    1,
                    2,
                    3,
                },
                o2 = new
                {
                    Name = "name.2",
                    Items = new List<string>
                    {
                        "Items2.1",
                        "Items2.2",
                        "Items2.3",
                    },
                }
            };
            {
                var cataloge = o1.ToCataloge();
                WriteLine(cataloge);
            }
            {
                var cataloge = o1.ToCataloge((s)=> s.SetDepth(5));
                WriteLine(cataloge);
            }
        }
    }
}
