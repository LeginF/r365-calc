using System;
using System.Collections.Generic;
using System.Text;

namespace r365_calc
{
    public class Calc
    {
        private int _sum = 0;
        private List<string> _delims;

        public Calc()
        {
            // by default, newlines and commas are supported as delimiters.
            _delims = new List<string>() { ",", "\n" };
        }

        /// <summary>
        /// Summate list of integers in the input. Ignore none non-integers.
        /// </summary>
        /// <param name="input">A String of tokens to perform calculation on.</param>
        public void Calculate(string input)
        {
            input = SetCustomDelimiter(input);
            var tokens = Tokenize(input);

            var values = Parse(tokens);
            CheckForNegatives(values);
            Summate(values);
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
                   if (token.Length > 0) _delims.Add(token);
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
        private void CheckForNegatives(int[] values)
        {
            string negatives = string.Empty;

            foreach(var value in values)
            {
                if (value < 0)
                {
                    negatives += $"{value},";
                }
            }

            if (negatives.Length > 0)
            {
                negatives = negatives.Substring(0, negatives.Length - 1);
                throw new ApplicationException("Negatives are not allowed. You had: " + negatives);
            }
        }

        /// <summary>
        /// Parse the tokens and return an array of integers.
        /// </summary>
        /// <param name="tokens">Array of tokens to parse.</param>
        private int[] Parse(string[] tokens)
        {
            var values = new List<int>();
            foreach (var token in tokens)
            {
                if ((int.TryParse(token, out int value)) && (value <= 1000))
                {
                    values.Add(value);
                }
            }

            return values.ToArray();
        }

        /// <summary>
        /// Summate the array of integers.
        /// </summary>
        /// <param name="values"></param>
        private void Summate(int[] values)
        {
            foreach (var value in values)
                Add(value);
        }

        /// <summary>
        /// Add value to _sum.
        /// </summary>
        /// <param name="value">Value to add to _sum</param>
        private void Add(int value)
        {
            _sum += value;
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
        }
    }
}
