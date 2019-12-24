using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Day9
{
    public class Computer
    {
        public List<BigInteger> Code;
        public Dictionary<BigInteger, BigInteger> Memory = new Dictionary<BigInteger, BigInteger>();
        public bool IsHalted { get; set; }
        public bool IsPaused { get; set; }
        public BigInteger InstructionPointer { get; set; }
        public BigInteger OutputValue { get; set; }
        public List<BigInteger> OutputHistory = new List<BigInteger>();
        public List<BigInteger> InputHistory = new List<BigInteger>();
        public int Phase;
        private Instruction _intstruction;
        private List<BigInteger> _line;
        private List<BigInteger> inputs = new List<BigInteger>();
        public BigInteger RelativeBase;

        private int OutPutCount => OutputHistory.Count;
        private int inputCounter => InputHistory.Count;


        public Computer(List<BigInteger> code, int phase = 0)
        {
            IsHalted = false;
            Code = new List<BigInteger>(code);
            for (int i = code.Count + 1; i < 9999; i++)
            {
                Code.Add(0);
            }


            InstructionPointer = 0;
            IsPaused = false;
            Phase = phase;
            RelativeBase = 0;
        }

        private BigInteger GetValueForModeAndParamMode(ModeAndParam modeAndParam)
        {
            if (modeAndParam.Mode == 2) return Code[(int) (InstructionPointer + RelativeBase + 1)];
            return modeAndParam.Mode == 0 ? Code[(int) modeAndParam.Parameter] : modeAndParam.Parameter;
        }


        public Computer Run(BigInteger input = default)
        {
            IsPaused = false;
            inputs.Add(input);
            while (!IsPaused || !IsHalted)
            {
                var opCodeAndParamModes = Code[(int) InstructionPointer];

                int opCode = (int) opCodeAndParamModes % 100;
                int mode1 = (int) opCodeAndParamModes / 100 % 10;
                int mode2 = (int) opCodeAndParamModes / 1000 % 10;
                int mode3 = (int) opCodeAndParamModes / 10000 % 10;

                var bigIntegers = Code.Skip((int) InstructionPointer).ToList();
                _intstruction = CreateInstruction(bigIntegers, opCode, mode1, mode2, mode3);
                List<ModeAndParam> modeAndParams = _intstruction.Mode
                    .Zip(_intstruction.Parameters)
                    .Select(x => new ModeAndParam(x.Second, x.First)).ToList();

                _line = Code.Skip((int) InstructionPointer).ToList();


                switch (opCode)
                {
                    case 99:
                        IsHalted = true;

                        if (input == 0)
                        {

                            var indexOf99 = OutputHistory.FindIndex(x => x == 99);

                            var missingFront = OutputHistory.Take(indexOf99 + 1).ToList();

                            var back = OutputHistory.Skip(indexOf99).ToList();

                            List<BigInteger> realFront = Code.Take(back.Count - 1).ToList();

                            realFront.AddRange(missingFront);

                            OutputHistory = realFront;
                        }


                        return this;
                    case 1:
                        OutputValue = GetValueForModeAndParamMode(modeAndParams[0]) + GetValueForModeAndParamMode(modeAndParams[1]);
                        Code[(int) _line[3]] = OutputValue;
                        InstructionPointer += 4;
                        break;
                    case 2:
                        OutputValue = GetValueForModeAndParamMode(modeAndParams[0]) * GetValueForModeAndParamMode(modeAndParams[1]);
                        Code[(int) _line[3]] = OutputValue;
                        InstructionPointer += 4;
                        break;
                    case 3:
                        InputHistory.Add(input);
                        Code[(int) modeAndParams[0].Parameter] = input;
                        InstructionPointer += 2;
                        break;
                    case 4:

                        OutputValue = GetValueForModeAndParamMode(modeAndParams[0]);
                        InstructionPointer += 2;
                        OutputHistory.Add(OutputValue);
                        //   IsPaused = true;
                        break;
                    case 5:
                        if (GetValueForModeAndParamMode(modeAndParams[0]) != 0)
                        {
                            InstructionPointer = (int) GetValueForModeAndParamMode(modeAndParams[1]);
                        }
                        else
                        {
                            InstructionPointer += 3;
                        }

                        break;
                    case 6:

                        if (GetValueForModeAndParamMode(modeAndParams[0]) == 0)
                        {
                            InstructionPointer = (int) GetValueForModeAndParamMode(modeAndParams[1]);
                        }
                        else
                        {
                            InstructionPointer += 3;
                        }

                        break;
                    case 7:
                        if (GetValueForModeAndParamMode(modeAndParams[0]) < GetValueForModeAndParamMode(modeAndParams[1]))
                        {
                            Code[(int) modeAndParams[2].Parameter] = 1;
                        }
                        else
                        {
                            Code[(int) modeAndParams[2].Parameter] = 0;
                        }

                        InstructionPointer += 4;

                        break;

                    case 8:
                        if (GetValueForModeAndParamMode(modeAndParams[0]) == GetValueForModeAndParamMode(modeAndParams[1]))
                        {
                            Code[(int) modeAndParams[2].Parameter] = 1;
                        }
                        else
                        {
                            Code[(int) modeAndParams[2].Parameter] = 0;
                        }

                        InstructionPointer += 4;

                        break;
                    case 9:
                        var valueForModeAndParamMode = GetValueForModeAndParamMode(modeAndParams[0]);
                        RelativeBase += valueForModeAndParamMode;

                        InstructionPointer += 2;

                        break;
                    default:
                        throw new NotImplementedException(@"opcode  + {opCode} ");
                }
            }


            throw new NotImplementedException(@"opcode  + {opCode} ");
        }


        public class ModeAndParam
        {
            public BigInteger Mode;
            public BigInteger Parameter { get; }

            public ModeAndParam(BigInteger param, BigInteger mode)
            {
                Mode = mode;
                Parameter = param;
            }
        }

        public class Instruction
        {
            public List<BigInteger> Mode { get; set; }
            public BigInteger Op { get; set; }
            public List<BigInteger> Parameters { get; set; }

            public Instruction(List<BigInteger> mode, List<BigInteger> parameters, BigInteger op)
            {
                Mode = mode;
                Op = op;
                Parameters = parameters;
            }
        }


        //todo clean up 

        public static Instruction CreateInstruction(List<BigInteger> parameters, BigInteger opCode, BigInteger mode1, BigInteger mode2, BigInteger mode3)
        {
            if (opCode == 99)
            {
                return new Instruction(new List<BigInteger>(), new List<BigInteger>(), opCode);
            }

            if (opCode == 3 || opCode == 4 || parameters.Count == 2)
            {
                return new Instruction(new List<BigInteger> {mode1}, new List<BigInteger> {parameters[1]}, opCode);
            }

            return new Instruction(
                new List<BigInteger>
                {
                    mode1, mode2, mode3
                },
                new List<BigInteger>
                {
                    parameters[1], parameters[2], parameters[3]
                },
                opCode);
        }
    }
}