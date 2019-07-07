using System.Collections.Generic;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class GenericExtensionsTests : XunitCore
    {
        public GenericExtensionsTests(ITestOutputHelper output) : base(output) { }

        [Theory]
        [InlineData(1, new object[] { 1, 2, 3 }, true)]
        [InlineData(1, new object[] { 2, 3 }, false)]
        [InlineData("Hello", new object[] { "Hello", "World" }, true)]
        [InlineData("hello", new object[] { "Hello", "World" }, false)]
        [InlineData(TriState.Yes, new object[] { TriState.Unkown, TriState.No }, false)]
        [InlineData(TriState.Yes, new object[] { TriState.Yes }, true)]
        public void IsTests(object value, object[] values, bool result)
        {
            Assert.Equal(result, value.Is(values));
            Assert.Equal(result, value.Is(values as IEnumerable<object>));
        }


        [Fact]
        public void Backing_TestString()
        {
            string newValue = "Hallo";
            string newerValue = "Hallo1";
            string backingField = default;

            Assert.Equal(default, backingField);
            var x = this.Backing(ref backingField, () => newValue);
            Assert.Equal(newValue, x);
            Assert.Equal(newValue, backingField);
            var y = this.Backing(ref backingField, () => newerValue);
            Assert.Equal(newValue, y);
            Assert.Equal(newValue, backingField);
        }

        [Fact]
        public void Backing_TestInt()
        {
            int newValue = 1;
            int newerValue = 2;
            int backingField = default;

            Assert.Equal(default, backingField);
            var x = this.Backing(ref backingField, () => newValue);
            Assert.Equal(newValue, x);
            Assert.Equal(newValue, backingField);
            var y = this.Backing(ref backingField, () => newerValue);
            Assert.Equal(newValue, y);
            Assert.Equal(newValue, backingField);
        }

        public class TestClass
        {
            public string Name { get; set; }
        }

        [Fact]
        public void Backing_TestClass()
        {
            TestClass newValue = new TestClass { Name = "Name" };
            TestClass newerValue = new TestClass { Name = "Name1" };
            TestClass backingField = default;

            Assert.Equal(default, backingField);
            var x = this.Backing(ref backingField, () => newValue);
            Assert.Equal(newValue, x);
            Assert.Equal(newValue.Name, x.Name);
            Assert.Equal(newValue, backingField);
            Assert.Equal(newValue.Name, backingField.Name);
            var y = this.Backing(ref backingField, () => newerValue);
            Assert.Equal(newValue, y);
            Assert.Equal(newValue.Name, y.Name);
            Assert.Equal(newValue, backingField);
            Assert.Equal(newValue.Name, backingField.Name);
        }
    }
}
