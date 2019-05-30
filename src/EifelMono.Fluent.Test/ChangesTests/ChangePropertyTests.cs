using System;
using System.Linq;
using EifelMono.Fluent.Changes;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Tests.ChangesTests
{
    public class ChangePropertyTests : XunitCore
    {
        public ChangePropertyTests(ITestOutputHelper output) : base(output) { }

        public class NumericClass : ChangeClass
        {
            public ChangeProperty<int> IntProperty { get; set; } = new ChangeProperty<int>();
            public ChangeProperty<double> DoubleProperty { get; set; } = new ChangeProperty<double>();
        }

        public class RootClass : ChangeClass
        {
            public ChangeProperty<bool> BoolProperty { get; set; } = new ChangeProperty<bool>();
            public ChangeProperty<string> StringProperty { get; set; } = new ChangeProperty<string>();
            public NumericClass NumericClass { get; set; } = new NumericClass();
        }
        [Fact]
        public void Tests()
        {
            var root = new RootClass();
            Assert.Null(root.Name);
            root.Name = "Root";

            var propertyChanged = 0;
            root.OnNotify.Add(p =>
            {
                propertyChanged++;
            });


            Assert.Equal("Root.NumericClass.IntProperty", root.NumericClass.IntProperty.FullName);

            Assert.NotEmpty(root.ChangedProperties());
            Assert.Equal(4, root.ChangedProperties().Count());

            Assert.Empty(root.ChangedProperties().Where(i => i.TimeStamp != DateTime.MinValue));

            root.NumericClass.DoubleProperty.Value = 4711;
            Assert.Equal(1, propertyChanged);

            Assert.Single(root.ChangedProperties().Where(i => i.TimeStamp != DateTime.MinValue));

            foreach(var item in root.ChangedProperties())
            {
                WriteLine(item.ToChangeString(false));
                WriteLine(item.ToChangeString(true));
            }
        }
    }
}
