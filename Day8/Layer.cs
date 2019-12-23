using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    public class Layer
    {
        private List<int> InputValues;
        public int Height;
        public int Length;
        
        private List<int> layer => InputValues.Take(Height * Length).ToList();


        public Layer(List<int> inputValues, int height, int length)
        {
            InputValues = inputValues;
            Height = height;
            Length = length;
        }
    }
}