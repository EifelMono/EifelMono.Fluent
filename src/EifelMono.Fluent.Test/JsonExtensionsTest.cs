using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Test
{
    public class JsonExtensionsTest : XunitCore
    {
        public JsonExtensionsTest(ITestOutputHelper output) : base(output) { }

        class TestObject
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public double Weight { get; set; }
        }
        [Fact]
        public void SerializeDeserialize()
        {

            {
                var test = (int)1;
                var json = test.ToJson();
                Assert.Equal(test, json.FromJson<int>());
                Assert.Equal(test, json.FromJson(typeof(int)));
                Assert.Equal(test.GetType().Name, json.FromJson(typeof(int)).GetType().Name);
            }

            {
                var test = (double)1.23;
                var json = test.ToJson();
                Assert.Equal(test, json.FromJson<double>());
                Assert.Equal(test, json.FromJson(typeof(double)));
                Assert.Equal(test.GetType().Name, json.FromJson(typeof(double)).GetType().Name);
            }

            {
                var test = "hallo";
                var json = test.ToJson();
                Assert.Equal(test, json.FromJson<string>());
                Assert.Equal(test, json.FromJson(typeof(string)));
                Assert.Equal(test.GetType().Name, json.FromJson(typeof(string)).GetType().Name);
            }

            {
                var test = new TestObject { Name = "hello", Age = 1, Weight = 47.11 };
                var json = test.ToJson();
                {
                    var result = json.FromJson<TestObject>();
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                }
                {
                    var result = (TestObject)json.FromJson(typeof(TestObject));
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                }
            }

            {
                var test = new TestObject { Name = "hello", Age = 1, Weight = 47.11 };
                var json = test.ToJsonEnvelope();
                {
                    var result = json.FormJsonEnvelope<TestObject>();
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                }

                {
                    var resultRaw = json.FormJsonEnvelope(typeof(TestObject));
                    Assert.Equal(typeof(TestObject), resultRaw.GetType());
                    var result = (TestObject)resultRaw;
                    Assert.Equal(test.Name, result.Name);
                    Assert.Equal(test.Age, result.Age);
                    Assert.Equal(test.Weight, result.Weight);
                }

                {
                    var (Name, Data) = json.FormJsonEnvelopeAsEnvelop<TestObject>();
                    Assert.Equal(Name, typeof(TestObject).Name);
                    Assert.Equal(typeof(TestObject).Name, Data.GetType().Name);
                    Assert.Equal(test.Name, Data.Name);
                    Assert.Equal(test.Age, Data.Age);
                    Assert.Equal(test.Weight, Data.Weight);
                }
            }

        }
    }
}
