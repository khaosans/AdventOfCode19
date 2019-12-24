using System;
using System.Collections.Generic;
using System.Numerics;

namespace Day9
{
    public class ListOfBigIntAndMemory
    {
        public List<BigInteger> Code;

        public Dictionary<BigInteger, BigInteger> mem = new Dictionary<BigInteger, BigInteger>();

        public ListOfBigIntAndMemory(List<BigInteger> code)
        {
            Code = code;
        }

        BigInteger GetByIndex(BigInteger index)
        {
            if (index > Code.Count && mem.ContainsKey(index))
            {
                return mem[index];
            }
            else
            {
                return Code[(int) index];
            }
        }

        void SetByIndexAndValue(BigInteger index, BigInteger value)
        {
            if (index > Code.Count && mem.ContainsKey(index))
            {
                mem[index] = value;
            }
            else
            {
                Code[(int) index] = value;
            }
        }
    }
}