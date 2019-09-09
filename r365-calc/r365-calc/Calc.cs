using System;
using System.Collections.Generic;
using System.Text;

namespace r365_calc
{
    public class Calc
    {
        private int _sum = 0;
        private List<string> _delims;
        private List<string> _history;
        private Queue<Step> _steps;

        private delegate void Operation(int Value);

        /// <summary>
        /// A step in the calculation
        /// </summary>
        private class Step
        {
            private Operation _operation;
            public int? Value { get; set; }
                        
            public Step(Operation operation, int value)
            {
                _operation = operation;
                Value = value;
            }

            public Step(Operation operation)
            {
                _operation = operation;
                Value = null;
            }

            public static Step Factory(string operation, Calc calc)
            {
                switch(operation)
                {
                    case "+": return new Step(calc.Add);
                    case "-": return new Step(calc.Subtract);
                    case "*": return new Step(calc.Multiply);
                    case "/": return new Step(calc.Divide);
                }
                return null;
            }

            public void Execute()
            {
                if (Value == null) throw new InvalidOperationException("No value defined.");
                _operation((int)Value);
            }
        }

        public int UpperBound { get; set; }
        public bool NoNegatives { get; set; }

        public Calc()
        {
            // by default, newlines and commas are supported as delimiters.
            _delims = new List<string>() { ",", "\n" };
            _history = new List<string>();
            UpperBound = int.MaxValue;
            _steps = new Queue<Step>();
        }

        /// <summary>
        /// Summate list of integers in the input. Ignore none non-integers.
        /// </summary>
        /// <param name="input">A String of tokens to perform calculation on.</param>
        public void Calculate(string input)
        {
            input = SetCustomDelimiter(input);
            var tokens = Tokenize(input);

            Parse(tokens);
            CheckForNegatives();
            Summate();
        }

        /// <summary>
        /// If the input string starts with "//" use the subsequent string,
        /// up to the next newline, as a delimiter.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The remainder of the string containing values.</returns>
        private string SetCustomDelimiter(string input)
        {
            // Look for "//<<delimiter>>\n<<values>>"
            if (input.StartsWith("//["))
            {
                char[] delims = { '[', ']' };
                int crPosition = input.IndexOf('\n');
                var tokens = input.Substring(3, crPosition - 3).Split(delims);
                foreach(var token in tokens)
                {
                    if (token.Length > 0) AddDelimiter(token);
                }
                return input.Substring(crPosition + 1);
            }
            if (input.StartsWith("//"))
            {
                int valuesIndex = input.IndexOf('\n');
                _delims.Add(input.Substring(2, valuesIndex - 2));
                return input.Substring(valuesIndex + 1);
            }

            return input;
        }

        public void AddDelimiter(string delimiter)
        {
            _delims.Add(delimiter);
        }

        /// <summary>
        /// Tokenize the list of integers. Tokens are identified by delimiters.
        /// </summary>
        /// <param name="input">string to tokenize</param>
        /// <returns>Array of tokens.</returns>
        private string[] Tokenize(string input)
        {
            var tokens = new List<string>();
            int start = 0;
            int delimIndex;

            do
            {
                delimIndex = FindNextDelimiter(input, start, out int delimLen);
                int end = (delimIndex > 0) ? delimIndex : input.Length;
                var token = input.Substring(start, end - start);
                tokens.Add(token);
                start = delimLen + delimIndex;
            } while (delimIndex >= 0);

            return tokens.ToArray();
        }

        /// <summary>
        /// Searches the input string for the first delimiter from the start position.
        /// </summary>
        /// <param name="input">string to search to delimiter</param>
        /// <param name="start">position to start searching from</param>
        /// <param name="delimLen">the length of the delimiter that was found</param>
        /// <returns>Index of the delimiter found. -1 if none are found.</returns>
        private int FindNextDelimiter(string input, int start, out int delimLen)
        {
            int returnIndex = input.Length;
            delimLen = -1;

            foreach(var delim in _delims)
            {
                int index = input.IndexOf(delim, start);
                if ((index > -1) && (index < returnIndex))
                {
                    returnIndex = index;
                    delimLen = delim.Length;
                }
            }

            if (returnIndex < input.Length)
                return returnIndex;
            else
                return -1;
           
        }

        /// <summary>
        /// Check that no values are negative. Throw an exception if
        /// found and report what they were.
        /// </summary>
        /// <param name="values">Array of values to check for negative values.</param>
        private void CheckForNegatives()
        {
            string negatives = string.Empty;

            foreach(var step in _steps)
            {
                if ((NoNegatives) && (step.Value < 0))
                {
                    negatives += $"{step.Value},";
                }
            }

            if (negatives.Length > 0)
            {
                negatives = negatives.Substring(0, negatives.Length - 1);
                throw new ApplicationException("Negatives are not allowed. You had: " + negatives);
            }
        }

        /// <summary>
        /// Parse the tokens and create a queue of steps.
        /// </summary>
        private void Parse(string[] tokens)
        {
            bool operand = false;
            Step step = null;
            foreach (var token in tokens)
            {
                // Enqueue explicit operators
                if (IsOperator(token, out var newStep))
                {
                    step = newStep;
                    _steps.Enqueue(step);
                    operand = true;
                    continue;
                }
                if ((int.TryParse(token, out int value)) && (value <= UpperBound))
                {
                    // If there was no operator then addition is implied
                    if (!operand) _steps.Enqueue(new Step(this.Add, value));
                    // If there was a prior operator, apply the value to the step
                    else step.Value = value;
                    operand = false;
                }
                else
                {
                    // Assume addition of zero on garbage input
                    _steps.Enqueue(new Step(this.Add, 0));
                }
            }
        }

        /// <summary>
        /// Returns true is input was +, -, * or /
        /// </summary>
        /// <param name="input"></param>
        /// <returns>true is input was an operator</returns>
        private bool IsOperator(string input, out Step step)
        {
            step = Step.Factory(input, this);
            return (step != null);
        }

        /// <summary>
        /// Summate the array of integers.
        /// </summary>
        /// <param name="values"></param>
        private void Summate()
        {
            while (_steps.Count > 0)
            {
                var step = _steps.Dequeue();
                step.Execute();
            }
        }

        /// <summary>
        /// Add value to _sum.
        /// </summary>
        /// <param name="value">Value to add to _sum</param>
        private void Add(int value)
        {
            _sum += value;
            _history.Add($"+{value}");
        }

        /// <summary>
        /// Subtract value from _sum
        /// </summary>
        /// <param name="value"></param>
        private void Subtract(int value)
        {
            _sum -= value;
            _history.Add($"-{value}");
        }

        /// <summary>
        /// Multiply _sum by value
        /// </summary>
        /// <param name="value"></param>
        private void Multiply(int value)
        {
            _sum *= value;
            _history.Add($"*{value}");
        }

        /// <summary>
        /// Divide _sum by value
        /// </summary>
        /// <param name="value"></param>
        private void Divide(int value)
        {
            _sum /= value;
            _history.Add($"/{value}");
        }
        /// <summary>
        /// Return the value of _sum
        /// </summary>
        /// <returns>The value of the calculation.</returns>
        public int Output()
        {
            return _sum;
        }

        /// <summary>
        /// Reset _sum to zero.
        /// </summary>
        public void Reset()
        {
            _sum = 0;
            _history.Clear();
        }

        public string History()
        {
            string history = string.Empty;
            foreach(var operation in _history)
            {
                history += operation;
            }

            if (history.StartsWith("+"))
                return history.Substring(1);
            else
                return history;
        }
    }
}
