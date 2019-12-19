using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Resolvers;
using Day5V2;
using Microsoft.VisualBasic;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            //  var csvString = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day7/day7part2.txt".ParseCsvString();

            // Test1();


            var code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day7/Part2.txt".ParseCsvString();

            var max2 = GetMaxPhaseSettings(code2);
        }

        private static void Test1()
        {
            var code1 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day7/day7Part1test.txt".ParseCsvString();


            var max = GetMaxPhaseSettings(code1);
        }

        private static BigInteger GetMaxPhaseSettings(List<int> code)
        {
            BigInteger max = 0;

            var phase = new List<int>();
            var count = 0;


            foreach (var permutation in Enumerable.Range(0, 1))
            {
                var phaseList = new List<int> {9, 8, 7, 6, 5};

                int previous = 0;
                while (new Comp(code, previous, 5).IsHalted)
                    foreach (var p in phaseList.Skip(1))
                    {
                        Comp comp = new Comp(code, 0, p);
                        var output = comp.Run();
                        if (comp.IsHalted)
                        {
                            previous = new Comp(code, output, 9).Run();
                        }
                    }


                if (previous != null && max < previous)
                {
                    max = (int) previous;
                    phase = phaseList;
                }

                count++;
            }

            return 1;
        }

        static IEnumerable<IEnumerable<T>>
            GetPermutations<T>(List<T> list, int length)
        {
            if (length == 1)
                return list.Select(t => new[]
                {
                    t
                });

            var enumerable = list.ToList();
            return GetPermutations(enumerable, length - 1)
                .SelectMany(t => enumerable.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new[]
                    {
                        t2
                    }));
        }


        public class Comp
        {
            public List<int> Code;
            public int Input;
            public int Phase;
            public bool IsHalted { get; set; }

            public Comp(List<int> code, int input, int phase)
            {
                Code = new List<int>(code);
                Input = input;
                Phase = phase;
            }

            public int Run()
            {
                int output;
                int idx = 0;
                int inputCounter = 0;

                while (idx < Code.Count)
                {
                    output = OpCode(ref idx, Input, ref inputCounter, Phase);

                    if (IsHalted)
                    {
                        return output;
                    }
                }

                throw new NotImplementedException();
            }


            private static int GetValueForModeAndParamMode(ModeAndParam modeAndParam, List<int> code)
            {
                return modeAndParam.Mode == 0 ? code[modeAndParam.Parameter] : modeAndParam.Parameter;
            }

            public int OpCode(
                ref int currentIndex,
                int input,
                ref int inputCounter,
                int phase)
            {
                int value;

                List<int> line;
                int outputCount = 0;

                List<(int, bool)> prrinted = new List<(int, bool)>();
                while (!IsHalted)
                {
                    var opCodeAndParamModes = Code[currentIndex];
                    int opCode = opCodeAndParamModes % 100;

                    if (opCode == 99)
                    {
                        IsHalted = true;
                    }

                    int mode1 = opCodeAndParamModes / 100 % 10;
                    int mode2 = opCodeAndParamModes / 1000 % 10;
                    int mode3 = opCodeAndParamModes / 10000 % 10;

                    List<ModeAndParam> modeAndParams = CreateInstruction(Code.Skip(currentIndex).ToList(), opCode, mode1, mode2, mode3).Mode
                        .Zip(CreateInstruction(Code.Skip(currentIndex).ToList(), opCode, mode1, mode2, mode3).Parameters)
                        .Select(x => new ModeAndParam(x.Second, x.First)).ToList();

                    line = Code.Skip(currentIndex).ToList();


                    switch (opCode)
                    {
                        case 99:
                            IsHalted = true;
                            break;
                        case 1:
                            value = GetValueForModeAndParamMode(modeAndParams[0], Code) + GetValueForModeAndParamMode(modeAndParams[1], Code);
                            Code[line[3]] = value;
                            currentIndex += 4;
                            break;
                        case 2:
                            value = GetValueForModeAndParamMode(modeAndParams[0], Code) * GetValueForModeAndParamMode(modeAndParams[1], Code);
                            Code[line[3]] = value;
                            currentIndex += 4;
                            break;
                        case 3:
                            inputCounter++;
                            if (inputCounter == 1)
                            {
                                Code[modeAndParams[0].Parameter] = phase;
                            }
                            else
                            {
                                Code[modeAndParams[0].Parameter] = input;
                            }

                            currentIndex += 2;

                            break;
                        case 4:
                            value = Code[modeAndParams[0].Parameter];
                            currentIndex += 2;
                            prrinted.Add((value, true));
                            IsHalted = true;
                            break;
                        case 5:
                            if (GetValueForModeAndParamMode(modeAndParams[0], Code) != 0)
                            {
                                currentIndex = GetValueForModeAndParamMode(modeAndParams[1], Code);
                            }
                            else
                            {
                                currentIndex += 3;
                            }

                            break;
                        case 6:

                            if (GetValueForModeAndParamMode(modeAndParams[0], Code) == 0)
                            {
                                currentIndex = GetValueForModeAndParamMode(modeAndParams[1], Code);
                            }
                            else
                            {
                                currentIndex += 3;
                            }

                            break;
                        case 7:
                            if (GetValueForModeAndParamMode(modeAndParams[0], Code) < GetValueForModeAndParamMode(modeAndParams[1], Code))
                            {
                                Code[modeAndParams[2].Parameter] = 1;
                            }
                            else
                            {
                                Code[modeAndParams[2].Parameter] = 0;
                            }

                            currentIndex += 4;

                            break;


                        case 8:
                            if (GetValueForModeAndParamMode(modeAndParams[0], Code) == GetValueForModeAndParamMode(modeAndParams[1], Code))
                            {
                                Code[modeAndParams[2].Parameter] = 1;
                            }
                            else
                            {
                                Code[modeAndParams[2].Parameter] = 0;
                            }

                            currentIndex += 4;
                            break;
                        default:
                            throw new NotImplementedException("opcode " + opCode + " line " + line + "index " + currentIndex);
                    }
                }

                return prrinted.Single(x => x.Item2).Item1;
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

            public static Instruction CreateInstruction(List<int> parameters, int opCode, int mode1, int mode2, int mode3)
            {
                if (opCode == 3 || opCode == 4 || parameters.Count == 2)
                {
                    return new Instruction(new List<int> {mode1}, new List<int> {parameters[1]}, opCode);
                }

                return new Instruction(new List<int>
                {
                    mode1, mode2, mode3
                }, new List<int>
                {
                    parameters[1], parameters[2], parameters[3]
                }, opCode);
            }
        }
    }
}