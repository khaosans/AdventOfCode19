using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Day5;

namespace Day5V2
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvString = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day5/day5input.txt".ParseCsvString();
            // Run(csvString, 1);
             Run(csvString, 5);

            //Run(new List<int>(){3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9}, 0);
            //Run(new List<int> {3,3,1105,-1,9,1101,0,0,12,4,12,99,1}, 0);
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