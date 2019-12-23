using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extenions
{
    public static class Extensions
    {
        public static List<int> ParseCsvString(this string str)
        {
            var text = File.ReadAllText(str);
            return text.Split(",").Select(int.Parse).ToList();
        }



        public static  T Convert<T>(this string str, Func<object, T> converter)
        {
            return converter(str);
        }
    }
}