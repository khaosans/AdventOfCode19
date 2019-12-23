using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Day8
{
    public class Picture
    {
        public List<Layer> Layers = new List<Layer>();
        public int Height;
        public int Length;
        public int LayerSize => Height * Length;


        public Picture(List<int> input, int height, int length)
        {
            Height = height;
            Length = length;

            var cursor = 0;
            while (cursor < input.Count)
            {
                Layers.Add(new Layer(input.Take(LayerSize).ToList(), height, length));
            }
        }
    }
}