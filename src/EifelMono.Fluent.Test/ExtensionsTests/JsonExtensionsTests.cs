using System;
using System.Collections.Generic;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class JsonExtensionsTests : XunitCore
    {
        public JsonExtensionsTests(ITestOutputHelper output) : base(output) { }

        class PropertyClass
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public double Weight { get; set; }

            public DayOfWeek DayOfWeek { get; set; }

            public List<string> ListOfString { get; set; } = new List<string>();
        }

        class FieldClass
        {
            public string Name;
            public int Age;
            public double Weight;

            public DayOfWeek DayOfWeek;

            public List<string> ListOfString = new List<string>();
        }
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
        [InlineData(true, true, true)]
        public void SerializeDeserializeTests(bool indented, bool defaults, bool enumAsString)
        {

            {
                var test = (int)1;
                var json = test.ToJson(indented, defaults, enumAsString);
                Assert.Equal(test, json.FromJson<int>());
                Assert.Equal(test, json.FromJson(typeof(int)));
                Assert.Equal(test.GetType().Name, json.FromJson(typeof(int)).GetType().Name);
            }

            {
                var test = (double)1.23;
                var json = test.ToJson(indented, defaults, enumAsString);
                Assert.Equal(test, json.FromJson<double>());
                Assert.Equal(test, json.FromJson(typeof(double)));
                Assert.Equal(test.GetType().Name, json.FromJson(typeof(double)).GetType().Name);
            }

            {
                var test = "hallo";
                var json = test.ToJson(indented, defaults, enumAsString);
                Assert.Equal(test, json.FromJson<string>());
                Assert.Equal(test, json.FromJson(typeof(string)));
                Assert.Equal(test.GetType().Name, json.FromJson(typeof(string)).GetType().Name);
            }

            {
                var test = new PropertyClass
                {
                    Name = "hello",
                    Age = 1,
                    Weight = 47.11,
                    DayOfWeek = DayOfWeek.Monday,
                    ListOfString = new List<string> { "A", "B", "C" }
                };
                var json = test.ToJson(indented, defaults, enumAsString);
                if (enumAsString)
                    Assert.Contains(test.DayOfWeek.ToString().ToLower(), json);
                else
                    Assert.DoesNotContain(test.DayOfWeek.ToString().ToLower(), json);
                {
                    var result = json.FromJson<PropertyClass>();
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                    Assert.Equal(test.DayOfWeek, result.DayOfWeek);
                    Assert.Equal(3, result.ListOfString.Count);
                    Assert.Equal("A", result.ListOfString[0]);
                    Assert.Equal("B", result.ListOfString[1]);
                    Assert.Equal("C", result.ListOfString[2]);
                }
                {
                    var result = (PropertyClass)json.FromJson(typeof(PropertyClass));
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                    Assert.Equal(test.DayOfWeek, result.DayOfWeek);
                    Assert.Equal(3, result.ListOfString.Count);
                    Assert.Equal("A", result.ListOfString[0]);
                    Assert.Equal("B", result.ListOfString[1]);
                    Assert.Equal("C", result.ListOfString[2]);
                }
            }

            {
                var test = new PropertyClass
                {
                    Name = "hello",
                    Age = 1,
                    Weight = 47.11,
                    DayOfWeek = DayOfWeek.Friday,
                    ListOfString = new List<string> { "A", "B", "C" }
                };
                var json = test.ToJsonEnvelope(indented, defaults, enumAsString);
                if (enumAsString)
                    Assert.Contains(test.DayOfWeek.ToString().ToLower(), json);
                else
                    Assert.DoesNotContain(test.DayOfWeek.ToString().ToLower(), json);
                {
                    var result = json.FormJsonEnvelope<PropertyClass>();
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                    Assert.Equal(test.DayOfWeek, result.DayOfWeek);
                    Assert.Equal(3, result.ListOfString.Count);
                    Assert.Equal("A", result.ListOfString[0]);
                    Assert.Equal("B", result.ListOfString[1]);
                    Assert.Equal("C", result.ListOfString[2]);

                }

                {
                    var resultRaw = json.FormJsonEnvelope(typeof(PropertyClass));
                    Assert.Equal(typeof(PropertyClass), resultRaw.GetType());
                    var result = (PropertyClass)resultRaw;
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                    Assert.Equal(test.DayOfWeek, result.DayOfWeek);
                    Assert.Equal(3, result.ListOfString.Count);
                    Assert.Equal("A", result.ListOfString[0]);
                    Assert.Equal("B", result.ListOfString[1]);
                    Assert.Equal("C", result.ListOfString[2]);
                }

                {
                    var (Name, Data) = json.FormJsonEnvelopeAsEnvelop<PropertyClass>();
                    Assert.Equal(Name, typeof(PropertyClass).Name);
                    Assert.Equal(typeof(PropertyClass).Name, Data.GetType().Name);
                    Assert.Equal(test.Name, Data.Name);
                    Assert.Equal(test.Age, Data.Age);
                    Assert.Equal(test.Weight, Data.Weight);
                    Assert.Equal(3, Data.ListOfString.Count);
                    Assert.Equal("A", Data.ListOfString[0]);
                    Assert.Equal("B", Data.ListOfString[1]);
                    Assert.Equal("C", Data.ListOfString[2]);
                }
            }
        }


        [Theory]
        [InlineData((long)1)]
        [InlineData((double)1.1)]
        [InlineData("1234")]
        [InlineData(true)]
        public void JsonClone_Json_ValueTypeTests(object testObject)
        {
            var result = testObject.JsonClone();
            Assert.Equal(testObject.GetType(), result.GetType());
            Assert.Equal(testObject, result);
        }



        [Fact]
        public void JsonClonePropertyClassTests()
        {
            var test = new PropertyClass
            {
                Name = "hello",
                Age = 1,
                Weight = 47.11,
                DayOfWeek = DayOfWeek.Monday,
                ListOfString = new List<string> { "A", "B", "C" }
            };
            var result = test.JsonClone();
            Assert.Equal(typeof(PropertyClass), result.GetType());
            Assert.Equal(test.Name, result.Name);
            Assert.Equal(test.Age, result.Age);
            Assert.Equal(test.Weight, result.Weight);
            Assert.Equal(test.DayOfWeek, result.DayOfWeek);
            Assert.Equal(test.ListOfString.Count, result.ListOfString.Count);
            Assert.Equal(test.ListOfString[0], result.ListOfString[0]);
            Assert.Equal(test.ListOfString[1], result.ListOfString[1]);
            Assert.Equal(test.ListOfString[2], result.ListOfString[2]);
            test.Name = "";
            Assert.NotEqual(test.Name, result.Name);
            test.Age = 0;
            Assert.NotEqual(test.Age, result.Age);
            test.Weight = 0;
            Assert.NotEqual(test.Weight, result.Weight);
            test.DayOfWeek = DayOfWeek.Friday;
            Assert.NotEqual(test.DayOfWeek, result.DayOfWeek);
            test.ListOfString.Add("D");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(3, result.ListOfString.Count);
            result.ListOfString.Add("D");
            result.ListOfString.Add("E");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(5, result.ListOfString.Count);
        }

        [Fact]
        public void JsonClonFieldClassTests()
        {
            var test = new FieldClass
            {
                Name = "hello",
                Age = 1,
                Weight = 47.11,
                DayOfWeek = DayOfWeek.Monday,
                ListOfString = new List<string> { "A", "B", "C" }
            };
            var result = test.JsonClone();
            Assert.Equal(typeof(FieldClass), result.GetType());
            Assert.Equal(test.Name, result.Name);
            Assert.Equal(test.Age, result.Age);
            Assert.Equal(test.Weight, result.Weight);
            Assert.Equal(test.DayOfWeek, result.DayOfWeek);
            Assert.Equal(test.ListOfString.Count, result.ListOfString.Count);
            Assert.Equal(test.ListOfString[0], result.ListOfString[0]);
            Assert.Equal(test.ListOfString[1], result.ListOfString[1]);
            Assert.Equal(test.ListOfString[2], result.ListOfString[2]);
            test.Name = "";
            Assert.NotEqual(test.Name, result.Name);
            test.Age = 0;
            Assert.NotEqual(test.Age, result.Age);
            test.Weight = 0;
            Assert.NotEqual(test.Weight, result.Weight);
            test.DayOfWeek = DayOfWeek.Friday;
            Assert.NotEqual(test.DayOfWeek, result.DayOfWeek);
            test.ListOfString.Add("D");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(3, result.ListOfString.Count);
            result.ListOfString.Add("D");
            result.ListOfString.Add("E");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(5, result.ListOfString.Count);
        }

        [Fact]
        public void JsonClonePropertyToFieldTests()
        {
            var test = new PropertyClass
            {
                Name = "hello",
                Age = 1,
                Weight = 47.11,
                DayOfWeek = DayOfWeek.Monday,
                ListOfString = new List<string> { "A", "B", "C" }
            };
            var result = test.JsonClone<PropertyClass, FieldClass>();
            Assert.Equal(typeof(FieldClass), result.GetType());
            Assert.Equal(test.Name, result.Name);
            Assert.Equal(test.Age, result.Age);
            Assert.Equal(test.Weight, result.Weight);
            Assert.Equal(test.DayOfWeek, result.DayOfWeek);
            Assert.Equal(test.ListOfString.Count, result.ListOfString.Count);
            Assert.Equal(test.ListOfString[0], result.ListOfString[0]);
            Assert.Equal(test.ListOfString[1], result.ListOfString[1]);
            Assert.Equal(test.ListOfString[2], result.ListOfString[2]);
            test.Name = "";
            Assert.NotEqual(test.Name, result.Name);
            test.Age = 0;
            Assert.NotEqual(test.Age, result.Age);
            test.Weight = 0;
            Assert.NotEqual(test.Weight, result.Weight);
            test.DayOfWeek = DayOfWeek.Friday;
            Assert.NotEqual(test.DayOfWeek, result.DayOfWeek);
            test.ListOfString.Add("D");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(3, result.ListOfString.Count);
            result.ListOfString.Add("D");
            result.ListOfString.Add("E");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(5, result.ListOfString.Count);
        }

        [Fact]
        public void JsonCloneFieldToProeprtyTests()
        {
            var test = new FieldClass
            {
                Name = "hello",
                Age = 1,
                Weight = 47.11,
                DayOfWeek = DayOfWeek.Monday,
                ListOfString = new List<string> { "A", "B", "C" }
            };
            var result = test.JsonClone<FieldClass, PropertyClass>();
            Assert.Equal(typeof(PropertyClass), result.GetType());
            Assert.Equal(test.Name, result.Name);
            Assert.Equal(test.Age, result.Age);
            Assert.Equal(test.Weight, result.Weight);
            Assert.Equal(test.DayOfWeek, result.DayOfWeek);
            Assert.Equal(test.ListOfString.Count, result.ListOfString.Count);
            Assert.Equal(test.ListOfString[0], result.ListOfString[0]);
            Assert.Equal(test.ListOfString[1], result.ListOfString[1]);
            Assert.Equal(test.ListOfString[2], result.ListOfString[2]);
            test.Name = "";
            Assert.NotEqual(test.Name, result.Name);
            test.Age = 0;
            Assert.NotEqual(test.Age, result.Age);
            test.Weight = 0;
            Assert.NotEqual(test.Weight, result.Weight);
            test.DayOfWeek = DayOfWeek.Friday;
            Assert.NotEqual(test.DayOfWeek, result.DayOfWeek);
            test.ListOfString.Add("D");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(3, result.ListOfString.Count);
            result.ListOfString.Add("D");
            result.ListOfString.Add("E");
            Assert.Equal(4, test.ListOfString.Count);
            Assert.Equal(5, result.ListOfString.Count);
        }
    }
}
