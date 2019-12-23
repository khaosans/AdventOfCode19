using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Day8
{
    public class Picture
    {
        public Dictionary<int, List<int>> Layers = new Dictionary<int, List<int>>();
        public int Height;
        public int Length;
        public List<int> Input;
        public int LayerSize => Height * Length;


        public Picture(List<int> input, int height, int length)
        {
            Height = height;
            Length = length;
            Input = input;
        }


        public Dictionary<int, List<int>> GetLayers()
        {
            var counter = 0;
            while (counter < Input.Count)
            {
              
                List<int> list = Input.Take(LayerSize).ToList();
                Layers.Add(counter, list);

                Input = Input.Skip(LayerSize).ToList();
                
                counter++;
            }

            return Layers;
        }
    }
}