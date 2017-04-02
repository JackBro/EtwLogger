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
using EtwLogger;
using EtwLogger.Logger;

namespace EtwLoggerConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var etwLoggerFactory = new EtwLoggerFactory(System.IO.Directory.GetCurrentDirectory(), "TestLog", true);
            etwLoggerFactory.Start();

            EtwLog.Sql("select * from Orders");
            EtwLog.Verbose($"This is debug log at {DateTime.UtcNow}");
            EtwLog.Info($"This is information log at {DateTime.UtcNow}");
            EtwLog.Warn($"This is warning log at {DateTime.UtcNow}");
            EtwLog.Error($"This is error log at {DateTime.UtcNow}");
            EtwLog.Critical($"This is critical log at {DateTime.UtcNow}");

            Console.Write("Press any key to exit...");
            Console.ReadKey();
            EtwLog.Exit();
        }
    }
}
