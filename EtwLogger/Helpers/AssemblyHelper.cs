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
using System.Diagnostics;
using System.Reflection;

namespace EtwLogger.Helpers
{
    /// <summary>
    /// AssemblyHelper class
    /// </summary>
    public static class AssemblyHelper
    {
        private static string _currentProcessName;

        /// <summary>
        /// Gets the name of the entry assembly.
        /// </summary>
        /// <returns></returns>
        public static string GetEntryAssemblyName()
        {
            return Assembly.GetEntryAssembly() != null ? Assembly.GetEntryAssembly().GetName().Name : string.Empty;
        }

        /// <summary>
        /// Gets the name of the current process.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentProcessName()
        {
            try
            {
                return _currentProcessName ?? (_currentProcessName = Process.GetCurrentProcess().ProcessName);
            }
            catch (Exception)
            {
                return "Not an Application";
            }
        }
    }
}
