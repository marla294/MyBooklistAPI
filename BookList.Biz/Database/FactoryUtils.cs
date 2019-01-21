using System;

namespace BookList.Biz.Database
{
    public static class FactoryUtils
    {
        // Checks any input fields
        // If the input is invalid, returns null
        // If not, ensures the string is < the maxLength
        // Returns valid input string
        public static string CheckInput(string input, int minLength, int maxLength)
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
