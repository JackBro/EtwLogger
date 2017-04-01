
namespace EtwLogger.Helpers
{
    /*
        MIT License

        Copyright (c) 2017 BuddyWork

        Permission is hereby granted, free of charge, to any person obtaining a copy
        of this software and associated documentation files (the "Software"), to deal
        in the Software without restriction, including without limitation the rights
        to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        copies of the Software, and to permit persons to whom the Software is
        furnished to do so, subject to the following conditions:

        The above copyright notice and this permission notice shall be included in all
        copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
        AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
        SOFTWARE.     
     */

    /// <summary>
    /// StringHelper
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Removes the spaces.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string RemoveSpaces(this string value)
        {
            return value.Replace(" ", "");
        }

        /// <summary>
        /// Determines whether the specified value is numeric.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string value)
        {
            int result;
            return int.TryParse(value, out result);
        }

        /// <summary>
        /// Determines whether the specified value is double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool IsDouble(this string value)
        {
            double result;
            return double.TryParse(value, out result);
        }

    }
}
