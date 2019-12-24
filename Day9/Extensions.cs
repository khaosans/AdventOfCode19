using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day9
{
    public static class Extensions
    {
        public static List<BigInteger> ParseCsvStringBigInteger(this string str)
        {
            var text = File.ReadAllText(str);
            return text.Split(",").Select(BigInteger.Parse).ToList();
        }



        public static  T Convert<T>(this string str, Func<object, T> converter)
        {
            return converter(str);
        }
    }
}