using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace EifelMono.Fluent.Test
{
    public class XunitCore
    {
        protected readonly ITestOutputHelper Output;

        public XunitCore(ITestOutputHelper output) => Output = output;

        public void WriteLine(string text = "") => Output.WriteLine(text);

        public void Line(int count = 80) => Output.WriteLine(new string('-', count));

        public void DoubleLine(int count = 80) => Output.WriteLine(new string('=', count));

        public void Dump(object dump, string title = default(string))
        {
            if (!string.IsNullOrEmpty(title))
            {
                Line();
                WriteLine(title);
            }
            WriteLine(JsonConvert.SerializeObject(dump, Formatting.Indented));
        }

        public void TryCatch(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.ToString());
                throw new Xunit.Sdk.XunitException(ex.ToString());
            }
        }

        public static void Fail(string message)
           => throw new Xunit.Sdk.XunitException(message);
    }
}
