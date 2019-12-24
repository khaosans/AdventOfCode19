using System;
using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    public class Picture
    {
        public int Height;
        public int Length;
        public List<int> Input;
        public List<int[,]> layers = new List<int[,]>();
        public int LayerSize => Height * Length;


        public Picture(List<int> input, int length, int height)
        {
            Height = height;
            Length = length;
            Input = new List<int>(input);
        }


        public List<int[,]> GetLayers()
        {
            var counter = 0;

            while (counter < Input.Count)
            {
                List<int> list = Input.Take(LayerSize).ToList();
                var ints = initialize();
                int index = 0;
                for (int i = 0; i < Length; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        ints[i, j] = list[index];
                        index++;
                    }
                }

                layers.Add(ints);

                Input = Input.Skip(LayerSize).ToList();

                counter++;
            }

            return layers;
        }

        public int[,] initialize()
        {
            int[,] output = new int[Length, Height];


            for (int i = 0; i < Length; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    output[i, j] = -1;
                }
            }

            return output;
        }

        public void Decode()
        {
            Dictionary<int, int> pixel = new Dictionary<int, int>();

            var display = CreteCreateLayer();

            Console.Out.WriteLine($"{display}");

            int len = 0;
            for (int i = 0; i < Length; i++)
            {

                for (int j = 0; j < Height; j++)
                {
                    len++;
                    var output = display[i,j];

                    if (output == 0)
                    {
                        Console.Out.Write($" ");

                    }
                    else
                    {
                        Console.Out.Write("X");
                    }
                    if (len % Length == 0)
                    {
                        Console.Out.WriteLine("");
                    }
                    
                }
            }
            

        }

       

        private int[,] CreteCreateLayer()
        {
            var display = initialize();

            foreach (var array in layers)
            {
                for (int i = 0; i < Length; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        var i1 = array[i, j];

                        if (IsConcrete(i1) && display[i, j] == -1)
                        {
                            display[i, j] = i1;
                        }
                        
                    }
                }
            }

            return display;
        }

        private Predicate<int> IsConcrete = color => color == 0 || color == 1;
    }
}