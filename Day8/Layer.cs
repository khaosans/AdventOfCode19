using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    public class Layer
    {
        public List<int> LayerValues;
        public int Height;
        public int Length;
        
        public Layer(List<int> inputValues, int height, int length)
        {
            LayerValues = inputValues;
            Height = height;
            Length = length;
        }
    }
}