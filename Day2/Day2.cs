using System;
using System.Collections.Generic;
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
            var newLIne = new List<int>();
            newLIne[0] = line
            switch (operation)
            {
                case OpCodeValue.One:
                    newLIne[3] = array[position1] + array[position2];
                    break;
                case OpCodeValue.Two:
                    newLIne[3] = array[position1] * array[position2];
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

        public static List<int> RunCode(List<int> intList)
        {
            var table = CreateTable(intList);
            var lineNumber = 0;
            var lineLists = table.Select(line => line.Select(x => x).ToList()).ToList();
            foreach (var lineList in lineLists)
            {
                if (lineList.First() == 99)
                {
                    break;
                }
                if (lineList.First() == 1)
                {
                    table[lineNumber] = OpCode(lineList, intList.ToArray(), OpCodeValue.One).ToList();
                }

                if (lineList.First() == 2)
                {
                    table[lineNumber] = OpCode(lineList, intList.ToArray(), OpCodeValue.Two).ToList();
                }

                table[lineNumber] = lineList;

                

                lineNumber++;
            }

            var table1 = table.SelectMany(i => i).ToList();
            
            Console.Out.WriteLine(table1);

            return table1;
        }
    }
}