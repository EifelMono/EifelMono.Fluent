using System.Collections.Generic;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test.ExtensionsTests
{
    public class StringExtensionsTests : XunitCore
    {
        public StringExtensionsTests(ITestOutputHelper output) : base(output) { }


        [Theory]
        [InlineData("<xml></xml>", "<xml>", "</xml>", "")]
        [InlineData("<xml>a</xml><xml>b</xml><xml>c</xml>", "<xml>", "</xml>", "a", "b", "c")]
        public void Test_Text_Between_Ok(string text, string startFrame, string endFrame, params string[] values)
        {
            var betweenValues = text.Between(startFrame, endFrame);
            Assert.Equal(betweenValues.Count, values.Length);
            for (int i = 0; i < betweenValues.Count; i++)
                Assert.Equal(betweenValues[i], values[i]);
        }

        [Theory]
        [InlineData("abcdef1", "abc", "def1")]
        [InlineData("abcdef1abcdef2", "abc", "def1abcdef2")]
        [InlineData("abcdef1abcdef2abcdef3", "abc", "def1abcdef2abcdef3")]
        [InlineData("", "", "")]
        public void Test_Text_After_Ok(string text, string start, string after)
        {
            Assert.Equal(after, text.After(start));
        }

        [Theory]
        [InlineData("abcdef1", "abc", "def1")]
        [InlineData("abcdef1abcdef2", "abc", "def2")]
        [InlineData("abcdef1abcdef2abcdef3", "abc", "def3")]
        [InlineData("", "", "")]
        public void Test_Text_LastAfter_Ok(string text, string start, string after)
        {
            Assert.Equal(after, text.LastAfter(start));
        }

        [Theory]
        [InlineData("abcdef1", "def1", "abc")]
        [InlineData("abcdef1abcdef2", "def2", "abcdef1abc")]
        [InlineData("abcdef1abcdef2abcdef3", "def3", "abcdef1abcdef2abc")]
        [InlineData("", "", "")]
        public void Test_Text_Before_Ok(string text, string start, string before)
        {
            Assert.Equal(before, text.Before(start));
        }

        [Theory]
        [InlineData("abcdef1", "def1", "abc")]
        [InlineData("abcdef1abcdef2", "def2", "abcdef1abc")]
        [InlineData("abcdef1abcdef2abcdef3", "def3", "abcdef1abcdef2abc")]
        [InlineData("abcdef1abcdef2", "def1", "abc")]
        [InlineData("abcdef1abcdef2abcdef3", "def2", "abcdef1abc")]
        [InlineData("", "", "")]
        public void Test_Text_LastBefore_Ok(string text, string start, string before)
        {
            Assert.Equal(before, text.LastBefore(start));
        }

    }
}
