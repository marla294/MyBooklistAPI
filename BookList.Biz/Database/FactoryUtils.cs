using System;
using System.Text.RegularExpressions;


namespace BookList.Biz.Database
{
    public static class FactoryUtils
    {
        // Checks any input fields
        // If the input is invalid, returns null
        // If not, crops the string to be < maxLength
        // Returns valid input string
        public static string CheckInput(string input, int minLength, int maxLength)
        {
            return CheckInputLength(input, minLength, maxLength);
        }

        // Same as above except this one takes a regex (in string form) to check
        // the input against
        public static string CheckInput(string input, int minLength, int maxLength, string regExString)
        {
            string slicedInput = CheckInputLength(input, minLength, maxLength);

            if (slicedInput == null)
            {
                return null;
            }

            return new Regex(regExString).IsMatch(slicedInput) ? slicedInput : null;
        }

        private static string CheckInputLength(string input, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length < minLength)
            {
                return null;
            }

            string slicedInput = input;

            if (input.Length > maxLength)
            {
                slicedInput = input.Substring(0, maxLength);
            }

            return slicedInput;
        }

    }
}
