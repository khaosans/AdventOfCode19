using System;
using System.Collections.Generic;
using System.Linq;

namespace Day5V2
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvString = "/Users/souriyakhaosanga/Documents/AdventOfCode/Day5V2/day5input.txt".ParseCsvString();
            Run(csvString, 1);
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
                    return code;
                }

                Instruction instruction = CreateInstruction(line, opCode, mode1, mode2, mode3);

                List<ModeAndParam> modeAndParams = instruction.Mode.Zip(instruction.Parameters).Select(x => new ModeAndParam(x.Second, x.First)).ToList();
                
                code = OpCode(opCode, modeAndParams, code, line, idx, input);

                idx += valueToInc;
            }

            return code;
        };


        private static int GetValueForModeAndParamMode(ModeAndParam modeAndParam, List<int> code)
        {
            return modeAndParam.Mode == 0 ? code[modeAndParam.Parameter] : modeAndParam.Parameter;
        }

        private static List<int> OpCode(int operation, List<ModeAndParam> modeAndParams, List<int> code, List<int> line, int index, int input)
        {
            int value;
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
                default:
                    throw new NotImplementedException("op ope code " + operation + " line " + line + "index " + index);
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
            return new Instruction(new List<int> {mode1, mode2, mode3}, new List<int> {line[1], line[2], line[3]}, opCode);
        }
    }
}