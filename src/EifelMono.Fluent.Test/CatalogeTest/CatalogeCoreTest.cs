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
        public void Run_ToCataloge()
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
                var cataloge = o1.ToCataloge((s) => s.SetDepth(5));
                WriteLine(cataloge);
            }

            {
                var cataloge = o1.ToCatalogeToString();
                WriteLine(cataloge);
            }
        }

        class ClassA
        {
            public string Name { get; set; }
            public int Cip { get; set; } = DateTime.Now.Millisecond;
            public DateTime TimeStamp { get; set; } = DateTime.Now;


            public List<string> Items { get; set; } = new List<string>();
            public ClassA A { get; set; }

            public override string ToString()
                => this.ToCatalogeToString();
        }

        [Fact]
        public void Run_ClassA_ToCataloge()
        {
            var o = new ClassA
            {
                Name = "1",
            };
            WriteLine(o.ToCataloge());
            WriteLine(o.ToCataloge(s => s.SetDepth(1)));
            WriteLine(o.ToCataloge(s => s.SetDepth(2)));
            WriteLine(o.ToCataloge(s => s.SetDepth(3)));
            WriteLine(o.ToCataloge(s => s.SetDepth(4)));
            WriteLine();
            var o1 = new ClassA
            {
                Name = "1.1",
                Items= new List<string> { "Item1.1.A", "Item1.1.B", "Item1.1.C" }
            };
            o.A = o1;
            WriteLine(o.ToCataloge());
            WriteLine(o.ToCataloge(s => s.SetDepth(1)));
            WriteLine(o.ToCataloge(s => s.SetDepth(2)));
            WriteLine(o.ToCataloge(s => s.SetDepth(3)));
            WriteLine(o.ToCataloge(s => s.SetDepth(4)));
            WriteLine();
            var o2 = new ClassA
            {
                Name = "1.1.1"
            };
            o1.A = o2;
            WriteLine(o.ToCataloge());
            WriteLine(o.ToCataloge(s => s.SetDepth(1)));
            WriteLine(o.ToCataloge(s => s.SetDepth(2)));
            WriteLine(o.ToCataloge(s => s.SetDepth(3)));
            WriteLine(o.ToCataloge(s => s.SetDepth(4)));
            WriteLine();
            var o3 = new ClassA
            {
                Name = "1.1.1.1"
            };
            o2.A = o3;
            WriteLine(o.ToCataloge());
            WriteLine(o.ToCataloge(s => s.SetDepth(1)));
            WriteLine(o.ToCataloge(s => s.SetDepth(2)));
            WriteLine(o.ToCataloge(s => s.SetDepth(3)));
            WriteLine(o.ToCataloge(s => s.SetDepth(4)));
        }

        [Fact]
        public void Run_ClassA_ToCatalogeAsToString()
        {
            var o = new ClassA
            {
                Name = "1",
            };
            WriteLine(o.ToCatalogeToString());
            WriteLine(o.ToCatalogeToString(1));
            WriteLine(o.ToCatalogeToString(2));
            WriteLine(o.ToCatalogeToString(3));
            WriteLine(o.ToCatalogeToString(4));
            WriteLine();
            var o1 = new ClassA
            {
                Name = "1.1",
                Items = new List<string> { "Item1.1.A", "Item1.1.B", "Item1.1.C" }
            };
            o.A = o1;
            WriteLine(o.ToCatalogeToString());
            WriteLine(o.ToCatalogeToString(1));
            WriteLine(o.ToCatalogeToString(2));
            WriteLine(o.ToCatalogeToString(3));
            WriteLine(o.ToCatalogeToString(4));
            WriteLine();
            var o2 = new ClassA
            {
                Name = "1.1.1"
            };
            o1.A = o2;
            WriteLine(o.ToCatalogeToString());
            WriteLine(o.ToCatalogeToString(1));
            WriteLine(o.ToCatalogeToString(2));
            WriteLine(o.ToCatalogeToString(3));
            WriteLine(o.ToCatalogeToString(4));
            WriteLine();
            var o3 = new ClassA
            {
                Name = "1.1.1.1"
            };
            o2.A = o3;
            WriteLine(o.ToCatalogeToString());
            WriteLine(o.ToCatalogeToString(1));
            WriteLine(o.ToCatalogeToString(2));
            WriteLine(o.ToCatalogeToString(3));
            WriteLine(o.ToCatalogeToString(4));
        }
    }
}
