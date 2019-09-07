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

            // Requirement one is for only a most two values.
            if (tokens.Length > 2) throw new ArgumentException("Too many values.");

            Parse(tokens);
        }

        /// <summary>
        /// Tokenize the list of integers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string[] Tokenize(string input)
        {
            return input.Split(",");
        }

        /// <summary>
        /// Parse the tokens and perform summation.
        /// </summary>
        /// <param name="tokens"></param>
        private void Parse(string[] tokens)
        {
            foreach (var token in tokens)
            {
                if (int.TryParse(token, out int value))
                {
                    Add(value);
                }
            }

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
