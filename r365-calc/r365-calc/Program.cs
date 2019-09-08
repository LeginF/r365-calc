using System;

// Requirement 1: Support a maximum of 2 numbers using a comma delimiter
// Requirement 2: Support an unlimited number of numbers e.g. 1,2,3,4,5,6,7,8,9,10,11,12 will return 78
// Requirement 3: Support \n as delimiter
// Requirement 4: No negatives. Throw exception and report.
// Requirement 5: Ignore numbers over 1000.
// Requirement 6: Allow custom delimiter
// Requirement 7: Allow custom delimiter of any length
// Requirement 8: Support multiple delmiiters of any length
// Strech 1: Display formula

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
                    Console.WriteLine("{1} = {0}", calc.Output(), calc.History());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ApplicationException ex)
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
