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
using System.IO;
using EtwLogger.Helpers;

namespace EtwLogger.Logger
{
    public static class FileLogger
    {
        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public static string Filename { get; private set; }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(string message)
        {
            if (string.IsNullOrEmpty(Filename))
            {
                Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    $"{AssemblyHelper.GetEntryAssemblyName()}.Unhandled.{DateTime.UtcNow:yyyyMMdd}.{Process.GetCurrentProcess().Id}.txt");
            }
            using (var writer = new StreamWriter(Filename, true))
            {
                writer.WriteLine($"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss,fff} [FileLogger] ERROR {message}");
            }
        }
    }
}
