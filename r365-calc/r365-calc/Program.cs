using System;

// Requirement 1: Support a maximum of 2 numbers using a comma delimiter
// Requirement 2: Support an unlimited number of numbers e.g. 1,2,3,4,5,6,7,8,9,10,11,12 will return 78
// Requirement 3: Support \n as delimiter
// Requirement 4: No negatives. Throw exception and report.
// Requirement 5: Ignore numbers over 1000.
// Requirement 6: Allow custom delimiter
// Requirement 7: Allow custom delimiter of any length
// Requirement 8: Support multiple delmiiters of any length
// Stretch 1: Display formula
// Stretch 2: Keep accepting entries
// Stretch 3: Command line arguments

namespace r365_calc
{
    class Program
    {
        static void Main(string[] args)
        {
            var calc = new Calc();

            ProcessArguments(args, calc);

            while (true)
            {
                calc.Reset();
                var line = Console.ReadLine();
                if (line.StartsWith("//"))
                    line += '\n' + Console.ReadLine();
                try
                {
                    // Perform calculation on parameter and output result.
                    calc.Calculate(line);
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
        }

        private static void ProcessArguments(string[] args, Calc calc)
        {
            bool setDelimiter = false;
            bool setUpperBound = false;

            foreach (var arg in args)
            {
                if (setDelimiter)
                {
                    calc.AddDelimiter(arg);
                    setDelimiter = false;
                    continue;
                }

                if (setUpperBound)
                {
                    if (!int.TryParse(arg, out int upperBound))
                        Console.WriteLine($"Invalid upper bound {arg}. Must be a number.");
                    else
                        calc.UpperBound = upperBound;

                    setUpperBound = false;
                    continue;
                }

                setDelimiter = arg.StartsWith("--Delimiter");
                setUpperBound = arg.StartsWith("--Upperbound");

                if (arg.StartsWith("--NoNegatives"))
                    calc.NoNegatives = true;
            }
        }
    }
}
