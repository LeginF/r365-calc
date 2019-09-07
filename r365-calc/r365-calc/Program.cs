using System;

// Requirement 1: Support a maximum of 2 numbers using a comma delimiter

namespace r365_calc
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure one parameter is passed to app.
            if (args.Length == 1)
            {
                try
                {
                    // Perform calculation on parameter and output result.
                    var calc = new Calc();
                    calc.Calculate(args[0]);
                    Console.WriteLine("{0}", calc.Output());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Input requires numbers separated by commas, no spaces.");
            }

        }
    }
}
