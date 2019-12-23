using System;
using System.Collections.Generic;
using System.Linq;

namespace Day8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            List<int> input = "123456789012".Select(x => x.ToString()).Select(int.Parse).ToList();

            Console.Out.WriteLine($"{input}");
            //to make sure the image wasn't corrupted during transmission, the Elves would like you to find the layer that contains the fewest 0 digits. On that layer, what is the number of 1 digits multiplied by the number of 2 digits?
        }
        
    }
}