using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Day5V2;
using Microsoft.VisualBasic;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            //  var csvString = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day7/day7part2.txt".ParseCsvString();

            var code = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day7/day7Part1test.txt".ParseCsvString();

            List<int> digits = new List<int> {0, 1, 2, 3, 4, 5};

            var permutations = GetPermutations(digits, 6).ToList();

            double max = 0;
            
            List<int> phase = new List<int>();
            foreach (var permutation in permutations)
            {
                var phaseSetting1 = permutation.ToList();

                var ints0 = Run(code,0);
                var ints1 = Run(ints0, phaseSetting1[0]);
                var ints2 = Run(ints1, phaseSetting1[1]);
                var ints3 = Run(ints2, phaseSetting1[2]);
                var ints4 = Run(ints3, phaseSetting1[3]);
                var ints5 = Run(ints4, phaseSetting1[4]);


                List<string> list1 = ints5.Select(x=> x.ToString()).ToList();
                var list = list1.Select(x => x.ToString()).ToArray();

                string join = String.Join("",list1);


                double output = 0;
                try
                {
                    output = Convert.ToDouble(join);
                }
                catch
                {
                    Console.Out.WriteLine($"j {join}");
                }

                if (max < output)
                {
                    max = output;
                    phase = phaseSetting1;
                }
            }


            Console.Out.WriteLine($"{max}");
            Console.Out.WriteLine($"{phase}");
        }


        static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new[] {t});

            var enumerable = list.ToList();
            return GetPermutations(enumerable, length - 1)
                .SelectMany(t => enumerable.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new[] {t2}));
        }

     
        public static readonly Func<List<int>, int, List<int>> Run = (code, input) =>
        {
            int skip = 0;
            int idx = 0;
            List<int> line;
            
            while (idx < code.Count)
            {
                var opCodeAndParamModes = code[idx];

                int opCode = opCodeAndParamModes % 100;
                int mode1 = opCodeAndParamModes / 100 % 10;
                int mode2 = opCodeAndParamModes / 1000 % 10;
                int mode3 = opCodeAndParamModes / 10000 % 10;

                int valueToInc;

                line = code.Skip(idx + skip).ToList();

                if (opCode == 3 || opCode == 4)
                {
                    valueToInc = 2;
                }
                else
                {
                    valueToInc = 4;
                }

                line = line.Take(valueToInc).ToList();


                if (line.First() == 99)
                {
                    Console.Out.WriteLine("HALT 99");
                    return code;
                }

                Instruction instruction = CreateInstruction(line, opCode, mode1, mode2, mode3);

                List<ModeAndParam> modeAndParams = instruction.Mode.Zip(instruction.Parameters).Select(x => new ModeAndParam(x.Second, x.First)).ToList();


                var current = idx;
                code = OpCode(opCode, modeAndParams, code, line, idx, out idx, input, valueToInc, out valueToInc);
                var newIndex = idx;

                if (current != newIndex)
                {
                    continue;
                    
                }

                idx += valueToInc;
            }

            Console.Out.WriteLine("HALT <");
            return code;
        };


        private static int GetValueForModeAndParamMode(ModeAndParam modeAndParam, List<int> code)
        {
            return modeAndParam.Mode == 0 ? code[modeAndParam.Parameter] : modeAndParam.Parameter;
        }

        (int, int) GetValueForModeAndParamModeAndIndex(ModeAndParam modeAndParam, List<int> code)
        {
            return modeAndParam.Mode == 0 ? (modeAndParam.Parameter, code[modeAndParam.Parameter]) : (2, modeAndParam.Parameter);
        }


        public static List<int> OpCode(int operation, List<ModeAndParam> modeAndParams, List<int> code, List<int> line, in int currentIndex, out int newIndex, int input, in int currentValueToInc
            , out int newValueToInc)
        {
            /*
             * Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
               Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
               Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
               Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
             */
            int value;
            newIndex = currentIndex;
            newValueToInc = currentValueToInc;

            switch (operation, line)
            {
                case (1, _):
                    value = GetValueForModeAndParamMode(modeAndParams[0], code) + GetValueForModeAndParamMode(modeAndParams[1], code);
                    code[line[3]] = value;
                    return code;
                case (2, _):
                    value = GetValueForModeAndParamMode(modeAndParams[0], code) * GetValueForModeAndParamMode(modeAndParams[1], code);
                    code[line[3]] = value;
                    return code;
                case (3, _):
                    code[modeAndParams[0].Parameter] = input;
                    return code;
                case (4, _):
                    value = code[modeAndParams[0].Parameter];
                    Console.Out.WriteLine(value);
                    return code;
                case (5, _):
                    //Opcode 5 is jump-if-true: if the first parameter is non-zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.
                    if (GetValueForModeAndParamMode(modeAndParams[0], code) != 0)
                    {
                        newIndex = GetValueForModeAndParamMode(modeAndParams[1], code);
                    }
                    else
                    {
                        newIndex += 3;
                    }

                    return code;
                case (6, _):
                    //Opcode 6 is jump-if-false: if the first parameter is zero, it sets the instruction pointer to the value from the second parameter. Otherwise, it does nothing.

                    if (GetValueForModeAndParamMode(modeAndParams[0], code) == 0)
                    {
                        newIndex = GetValueForModeAndParamMode(modeAndParams[1], code);
                    }
                    else
                    {
                        newIndex += 3;
                    }

                    return code;
                //  Opcode 7 is less than: if the first parameter is less than the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.

                case (7, _):
                    if (GetValueForModeAndParamMode(modeAndParams[0], code) < GetValueForModeAndParamMode(modeAndParams[1], code))
                    {
                        code[modeAndParams[2].Parameter] = 1;
                    }
                    else
                    {
                        code[modeAndParams[2].Parameter] = 0;
                    }


                    return code;
                //   Opcode 8 is equals: if the first parameter is equal to the second parameter, it stores 1 in the position given by the third parameter. Otherwise, it stores 0.
                case (8, _):
                    if (GetValueForModeAndParamMode(modeAndParams[0], code) == GetValueForModeAndParamMode(modeAndParams[1], code))
                    {
                        code[modeAndParams[2].Parameter] = 1;
                    }
                    else
                    {
                        code[modeAndParams[2].Parameter] = 0;
                    }

                    return code;

                default:
                    throw new NotImplementedException("op ope code " + operation + " line " + line + "index " + newIndex);
            }
        }


        public class ModeAndParam
        {
            public int Mode { get; }
            public int Parameter { get; }

            public ModeAndParam(int param, int mode = 0)
            {
                Mode = mode;
                Parameter = param;
            }
        }

        public class Instruction
        {
            public List<int> Mode { get; set; }
            public int Op { get; set; }
            public List<int> Parameters { get; set; }

            public Instruction(List<int> mode, List<int> parameters, int op)
            {
                Mode = mode;
                Op = op;
                Parameters = parameters;
            }
        }

        public static Instruction CreateInstruction(List<int> line, int opCode, int mode1, int mode2, int mode3)
        {
            if (opCode == 3 || opCode == 4)
            {
                return new Instruction(new List<int> {mode1}, new List<int> {line[1]}, opCode);
            }

            return new Instruction(new List<int>
            {
                mode1, mode2, mode3
            }, new List<int>
            {
                line[1], line[2], line[3]
            }, opCode);
        }
    }
}