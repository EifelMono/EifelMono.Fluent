using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Extensions
{
    public static class StringBuilderExtensions
    {
        public static int PositionOf(this StringBuilder thisValue, string searchText)
        {
            try
            {
                if (string.IsNullOrEmpty(searchText))
                    return -1;
                for (int i = 0; i <= thisValue.Length - searchText.Length; i++)
                {
                    var equal = 0;
                    for (int j = 0; j < searchText.Length; j++)
                    {
                        if (thisValue[i + j] == searchText[j])
                            equal++;
                        else
                            break;
                    }
                    if (equal == searchText.Length)
                        return i;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return -1;
        }
    }
}
