using System;
using System.Web.Script.Serialization;

namespace EtwLogger.Extensions
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
    public static class JsonExtension
    {
        /// <summary>
        /// Serializes an object to string using Java Json serialization.
        /// </summary>
        /// <param name="obj">instance to serialize</param>
        /// <returns>string representation</returns>
        public static string JavaSerializeToJson(this object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }


        /// <summary>
        /// De-serializes a string to object using Java Json serialization.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="toType">type to serialize to</param>
        /// <returns>
        /// hydrated instance
        /// </returns>
        public static object JavaDeserializeFromJson(this string value, Type toType)
        {
            var deserializer = new JavaScriptSerializer();
            return deserializer.Deserialize(value, toType);
        }
    }
}
