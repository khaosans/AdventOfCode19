using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Day2
{
    public class Day2
    {
        public enum OpCodeValue : int
        {
            One = 1,
            Two = 2
        };

        public static List<int> OpCode(List<int> code)
        {
            var currentIdx = 0;
            while (currentIdx < code.Count + 1)
            {
                List<int> line = code.Skip(currentIdx).Take(4).ToList();

                if (line.First() == 99)
                {
                    return code;
                }

                var position1 = line[1];
                var position2 = line[2];
                var position3 = line[3];
                code[position3] = line.First() switch
                {
                    1 => (code[position1] + code[position2]),
                    2 => (code[position1] * code[position2]),
                    _ => code[position3]
                };
                currentIdx = currentIdx + 4;
            }

            return code;
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