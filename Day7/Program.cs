using System;
using System.Collections.Generic;
using System.Linq;
using Day5V2;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> code2 = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day7/Part2.txt".ParseCsvString();

            var allSeq= CreateComputers(code2);

            var max = FindMaxSignal(allSeq);

            Console.Out.WriteLine($"Max {max}");
        }

        private static int FindMaxSignal(List<List<Computer>> allSeq)
        {
            var max = 0;
            foreach (var computers in allSeq)
            {
                List<Computer> runWithFeedBack = RunWithFeedBack(computers);

                if (runWithFeedBack.Count > 0 && runWithFeedBack.Last().OutputHistory.Last() > max)
                {
                    max = runWithFeedBack.Last().OutputHistory.Last();
                }
            }

            return max;
        }

        private static List<Computer> RunWithFeedBack(List<Computer> list)
        {
            list[0] = list[0].Run(0);

            int skip = 1;

            while (!list.Last().IsHalted)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (skip > 0)
                    {
                        skip--;
                        continue;
                    }

                    if (i == 0)
                    {
                        list[i] = list[i].Run(list.Last().OutputValue);
                    }
                    else
                    {
                        list[i] = list[i].Run(list[i - 1].OutputValue);
                    }
                }
            }

            return list;
        }

        private static List<List<Computer>> CreateComputers(List<int> code)
        {
            var permuatatedComputers = new List<List<Computer>>();
            var permutations = GetPermutations(new List<int> {9, 8, 7, 6, 5}, 5).ToList();
            permutations.Reverse();

            foreach (var permutation in permutations)
            {
                var computers = new List<Computer>();
                var stackOfPhases = new Stack<int>(permutation.ToList());

                computers.Add(new Computer(code, stackOfPhases.Pop()));

                while (stackOfPhases.Count > 0)
                {
                    computers.Add(new Computer(code, stackOfPhases.Pop()));
                }
                permuatatedComputers.Add(computers);
            }

            return permuatatedComputers;
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
                .SelectMany(t => enumerable
                        .Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new[]
                    {
                        t2
                    }));
        }


        public class Computer
        {
            public List<int> Code;
            public bool IsHalted { get; set; }
            public bool IsPaused { get; set; }
            public int InstructionPointer { get; set; }
            public int OutputValue { get; set; }
            public List<int> OutputHistory = new List<int>();
            public List<int> InputHistory = new List<int>();
            public int Phase;
            private Instruction _intstruction;
            private List<int> _line;
            private List<int> inputs = new List<int>();
            private int OutPutCount => OutputHistory.Count;
            private int inputCounter => InputHistory.Count;


            public Computer(List<int> code, int phase)
            {
                IsHalted = false;
                Code = new List<int>(code);
                InstructionPointer = 0;
                IsPaused = false;
                Phase = phase;
            }

            private static int GetValueForModeAndParamMode(ModeAndParam modeAndParam, List<int> code)
            {
                return modeAndParam.Mode == 0 ? code[modeAndParam.Parameter] : modeAndParam.Parameter;
            }


            public Computer Run(int input)
            {
                IsPaused = false;
                inputs.Add(input);
                while (!IsPaused || !IsHalted)
                {
                    var opCodeAndParamModes = Code[InstructionPointer];

                    int opCode = opCodeAndParamModes % 100;
                    int mode1 = opCodeAndParamModes / 100 % 10;
                    int mode2 = opCodeAndParamModes / 1000 % 10;
                    int mode3 = opCodeAndParamModes / 10000 % 10;

                    _intstruction = CreateInstruction(Code.Skip(InstructionPointer).ToList(), opCode, mode1, mode2, mode3);
                    List<ModeAndParam> modeAndParams = _intstruction.Mode
                        .Zip(_intstruction.Parameters)
                        .Select(x => new ModeAndParam(x.Second, x.First)).ToList();

                    _line = Code.Skip(InstructionPointer).ToList();

                    if (_intstruction.Op == 99)
                    {
                        IsHalted = true;
                        return this;
                    }

                    switch (opCode)
                    {
                        case 99:
                            IsHalted = true;
                            return this;
                        case 1:
                            OutputValue = GetValueForModeAndParamMode(modeAndParams[0], Code) + GetValueForModeAndParamMode(modeAndParams[1], Code);
                            Code[_line[3]] = OutputValue;
                            InstructionPointer += 4;
                            break;
                        case 2:
                            OutputValue = GetValueForModeAndParamMode(modeAndParams[0], Code) * GetValueForModeAndParamMode(modeAndParams[1], Code);
                            Code[_line[3]] = OutputValue;
                            InstructionPointer += 4;
                            break;
                        case 3:
                            if (inputCounter == 0)
                            {
                                InputHistory.Add(Phase);
                                Code[modeAndParams[0].Parameter] = Phase;
                            }
                            else
                            {
                                InputHistory.Add(input);
                                Code[modeAndParams[0].Parameter] = input;
                            }

                            InstructionPointer += 2;

                            break;
                        case 4:
                            OutputValue = Code[modeAndParams[0].Parameter];
                            InstructionPointer += 2;
                            OutputHistory.Add(OutputValue);
                            IsPaused = true;
                            return this;
                        case 5:
                            if (GetValueForModeAndParamMode(modeAndParams[0], Code) != 0)
                            {
                                InstructionPointer = GetValueForModeAndParamMode(modeAndParams[1], Code);
                            }
                            else
                            {
                                InstructionPointer += 3;
                            }

                            break;
                        case 6:

                            if (GetValueForModeAndParamMode(modeAndParams[0], Code) == 0)
                            {
                                InstructionPointer = GetValueForModeAndParamMode(modeAndParams[1], Code);
                            }
                            else
                            {
                                InstructionPointer += 3;
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

                            InstructionPointer += 4;

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

                            InstructionPointer += 4;

                            break;
                        default:
                            throw new NotImplementedException(@"opcode  + {opCode} ");
                    }
                }


                throw new NotImplementedException(@"opcode  + {opCode} ");
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


            //todo clean up 

            public static Instruction CreateInstruction(List<int> parameters, int opCode, int mode1, int mode2, int mode3)
            {
                if (opCode == 99)
                {
                    return new Instruction(new List<int>(), new List<int>(), opCode);
                }

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
                    },
                    opCode);
            }
        }
    }
}