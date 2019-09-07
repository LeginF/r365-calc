using System;
using System.Collections.Generic;
using System.Text;

namespace r365_calc
{
    public class Calc
    {
        private int _sum = 0;

        /// <summary>
        /// Summate list of integers in the input. Ignore none non-integers.
        /// </summary>
        /// <param name="input"></param>
        public void Calculate(string input)
        {
            var tokens = Tokenize(input);

            var values = Parse(tokens);
            CheckForNegatives(values);
            Summate(values);
        }

        /// <summary>
        /// Tokenize the list of integers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string[] Tokenize(string input)
        {
            char[] delims = { ',', '\n' };
            return input.Split(delims);
        }

        /// <summary>
        /// Check that no values are negative. Throw an exception if
        /// found and report what they were.
        /// </summary>
        /// <param name="values"></param>
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
        /// <param name="tokens"></param>
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
        /// <param name="value"></param>
        private void Add(int value)
        {
            _sum += value;
        }

        /// <summary>
        /// Return the value of _sum
        /// </summary>
        /// <returns></returns>
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
