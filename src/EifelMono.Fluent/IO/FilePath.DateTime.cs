using System;
using System.Globalization;
using System.IO;
using System.Linq;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.IO
{
    public partial class FilePath : ValuePath, IFluentExists
    {
        public enum DateTimeFormat
        {
            yyyyMMdd,
            HHmmss,
            yyyyMMddHHmmss
        }

        public FilePath ChangeFileNameWithoutExtensionAppend(DateTimeFormat dateTimeFormat)
        {
            var datetime = "";
            switch (dateTimeFormat)
            {
                case DateTimeFormat.yyyyMMdd:
                    datetime = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case DateTimeFormat.HHmmss:
                    datetime = DateTime.Now.ToString("HHmmss");
                    break;
                case DateTimeFormat.yyyyMMddHHmmss:
                    datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    break;
            }
            ChangeFileNameWithoutExtension($"{FileNameWithoutExtension}.{datetime}");
            return this;
        }

        public (bool Ok, DateTime Value) FileWileNameWithoutExtensionLastAsDateTime()
        {
            var split = FileNameWithoutExtension.Split('.');
            if (split.Length <= 1)
                return (false, DateTime.MinValue);
            var last = split.Last();
            if (last.AreDigits())
                foreach (var format in fluent.Enum.Names<DateTimeFormat>())
                    if (last.Length == format.Length)
                        if (DateTime.TryParseExact(last, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                            return (true, dateTime);
            return (false, DateTime.MinValue);
        }
    }
}
