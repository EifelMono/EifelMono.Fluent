﻿using EifelMono.Fluent.IO;
using EifelMono.Fluent.NuGet;
using EifelMono.Fluent.Test.XunitTests;
using Xunit;
using Xunit.Abstractions;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent.Test.DotNetTests
{

    public class GlobalJsonTest : XunitCore
    {
        public GlobalJsonTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async void GetGlobalJsonTests()
        {
            {
                var result = await dotnet.GlobalJson.GetFilesBackwardsAsync();
                Assert.True(result.Count > 0);
            }

            {
                var result = await dotnet.GlobalJson.GetExistingFilesBackwardsAsync();
                Assert.True(result.Count == 1);
            }
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
