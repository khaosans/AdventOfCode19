using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    public class Day2
    {
        public enum OpCodeValue : int
        {
            One = 1,
            Two = 2
        };

        public static List<int> OpCode(List<int> line, int[] array, OpCodeValue operation)
        {
            var position1 = line[1];
            var position2 = line[2];

            switch (operation)
            {
                case OpCodeValue.One:
                    line[3] = array[position1] + array[position2];
                    break;
                case OpCodeValue.Two:
                    line[3] = array[position1] * array[position2];
                    break;
            }

            return line;
        }

        public static List<IEnumerable<int>> CreateTable(List<int> seq)
        {
            List<IEnumerable<int>> list = seq.Select((x, i) => new {Value = x, Index = i})
                .GroupBy(x => x.Index / 4)
                .Select(g => g.Select(x => x.Value))
                .ToList();

            return list;
        }
    }
}