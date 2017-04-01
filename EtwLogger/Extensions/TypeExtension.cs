using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    public static class TypeExtension
    {
        /// <summary>
        /// Types to string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string TypeToString(this Type obj)
        {
            return obj == null ? null : $"{obj.FullName}, {obj.Assembly.GetName().Name}";
        }

        /// <summary>
        /// Strings to type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type StringToType(this string typeName)
        {
            return typeName == null ? null : Type.GetType(typeName);
        }

        /// <summary>
        /// Types the name.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string TypeName(this Type obj)
        {
            var typeName = obj.FullName;
            try
            {
                var pos = typeName.IndexOf(", Version=", StringComparison.Ordinal);
                if (pos > -1)
                {
                    var endPos = typeName.IndexOf("]", pos, StringComparison.Ordinal);
                    if (endPos > -1)
                    {
                        var replaceData = typeName.Substring(pos, endPos - pos);
                        var newTypeName = typeName.Replace(replaceData, string.Empty);
                        return newTypeName;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.EtwLogger.Error(ex.Message, ex);
            }
            return typeName;
        }

        /// <summary>
        /// Objects to byte array.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static byte[] ObjectToByteArray(this object obj)
        {
            byte[] ret;
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ret = ms.ToArray();
            }

            return ret;
        }

        /// <summary>
        /// Bytes the array to object.
        /// </summary>
        /// <param name="arrBytes">The arr bytes.</param>
        /// <returns></returns>
        public static object ByteArrayToObject(this byte[] arrBytes)
        {
            object ret;
            var bf = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                ret = bf.Deserialize(memStream);
            }
            return ret;
        }

        /// <summary>
        /// Types to byte array.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public static byte[] TypeToByteArray(this Type obj)
        {
            byte[] ret;
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ret = ms.ToArray();
            }

            return ret;
        }
        /// <summary>
        /// Bytes the type of the array to.
        /// </summary>
        /// <param name="arrBytes">The arr bytes.</param>
        /// <returns></returns>
        public static Type ByteArrayToType(this byte[] arrBytes)
        {
            Type ret;
            var bf = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                ret = (Type)bf.Deserialize(memStream);
            }
            return ret;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string ToString(this string item, string defaultValue)
        {
            return string.IsNullOrEmpty(item) ? defaultValue : item;
        }

        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static bool ToBool(this string item)
        {
            return Convert.ToBoolean(item);
        }

        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool ToBool(this string item, bool defaultValue)
        {
            return string.IsNullOrEmpty(item) ? defaultValue : Convert.ToBoolean(item);
        }

        /// <summary>
        /// Converts to int32.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static int ToInt32(this string item)
        {
            return Convert.ToInt32(item);
        }

        /// <summary>
        /// Converts to int32.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ToInt32(this string item, int defaultValue)
        {
            return string.IsNullOrEmpty(item) ? defaultValue : Convert.ToInt32(item);
        }

        /// <summary>
        /// Converts to int64.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static long ToInt64(this string item)
        {
            return Convert.ToInt64(item);
        }

        /// <summary>
        /// To the int64.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long ToInt64(this string item, long defaultValue)
        {
            return string.IsNullOrEmpty(item) ? defaultValue : Convert.ToInt64(item);
        }

        /// <summary>
        /// Converts to double.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static double ToDouble(this string item)
        {
            return Convert.ToDouble(item);
        }

        /// <summary>
        /// Converts to double.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double ToDouble(this string item, int defaultValue)
        {
            return string.IsNullOrEmpty(item) ? defaultValue : Convert.ToDouble(item);
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string item)
        {
            return Convert.ToDecimal(item);
        }

        /// <summary>
        /// Converts to decimal.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string item, int defaultValue)
        {
            return string.IsNullOrEmpty(item) ? defaultValue : Convert.ToDecimal(item);
        }

    }

}
