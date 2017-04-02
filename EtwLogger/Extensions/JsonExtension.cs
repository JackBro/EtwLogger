#region License

/*
 * Copyright © 2002-2009 the original author or authors.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion
using System;
using System.Web.Script.Serialization;

namespace EtwLogger.Extensions
{
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
