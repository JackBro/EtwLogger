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
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EtwLogger
{
    [DataContract]
    [Serializable]
    public enum Severity
    {
        /// <summary>
        /// Critical severity
        /// </summary>
        [EnumMember]
        [Description("Critical")]
        Critical = 1,
        /// <summary>
        /// Caution severity
        /// </summary>
        [EnumMember]
        [Description("Caution")]
        Caution = 2,
        /// <summary>
        /// Warning severity
        /// </summary>
        [EnumMember]
        [Description("Warning")]
        Warning = 3,
        /// <summary>
        /// Information severity
        /// </summary>
        [EnumMember]
        [Description("Information")]
        Info = 4,
        /// <summary>
        /// Error severity
        /// </summary>
        [EnumMember]
        [Description("Error")]
        Error = 5
    }
}
