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
using System.Configuration;
using System.Diagnostics.Tracing;
using EtwLogger.Extensions;

namespace EtwLogger.Logger
{
    /// <summary>
    /// This class is main factory
    /// </summary>
    public class EtwLoggerFactory
    {
        #region Private Variables
        private readonly string _logPath;
        private readonly string _filename;
        private readonly bool _useSqlAppender;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="EtwLoggerFactory" /> class.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        /// <param name="filename">The filename.</param>
        public EtwLoggerFactory(string logPath, string filename) : this (logPath, filename, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EtwLoggerFactory" /> class.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="useSqlAppender">If you want to use the sql appender</param>
        public EtwLoggerFactory(string logPath, string filename, bool useSqlAppender)
        {
            _logPath = logPath;
            _filename = filename;
            _useSqlAppender = useSqlAppender;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            EtwLogger.EtwLoggerEventListener = new EtwLoggerEventListener(_logPath, _filename, "txt", false);
            EtwLog.IsSqlEnabled = _useSqlAppender;
            if (EtwLogger.IsSqlEnabled)
            {
                EtwLogger.EtwSqlLoggerEventListener = new EtwLoggerEventListener(_logPath, _filename, "sql", true);
                EtwLogger.EtwSqlLoggerEventListener.EnableEvents(EtwLoggerEventSource.Log, EventLevel.Verbose);
            }

            // Over the logging min and max level
            var etwMinLevel = ConfigurationManager.AppSettings["ETWMinLevel"];
            var minLevel = EventLevel.Verbose;
            if (!string.IsNullOrEmpty(etwMinLevel))
                minLevel = GetEventLevel(etwMinLevel);
            SetLevel(minLevel);
            EtwLogger.EtwLoggerEventListener.EnableEvents(EtwLoggerEventSource.Log, minLevel);

            var asyncLoggingDisabled = ConfigurationManager.AppSettings["AsyncLoggingDisabled"].ToBool(false);
            EtwLogger.EtwLoggerEventListener.IsAsyncLoggingDisabled = asyncLoggingDisabled;
            if (EtwLogger.IsSqlEnabled) EtwLogger.EtwSqlLoggerEventListener.IsAsyncLoggingDisabled = asyncLoggingDisabled;
            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the event level.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns></returns>
        private EventLevel GetEventLevel(string eventType)
        {
            switch (eventType)
            {
                case "VERBOSE":
                    return EventLevel.Verbose;
                case "INFO":
                    return EventLevel.Informational;
                case "WARN":
                    return EventLevel.Warning;
                case "ERROR":
                    return EventLevel.Error;
                case "CRITICAL":
                    return EventLevel.Critical;
                default:
                    return EventLevel.Verbose;
            }
        }

        /// <summary>
        /// Sets the level.
        /// </summary>
        /// <param name="eventLevel">The event level.</param>
        private void SetLevel(EventLevel eventLevel)
        {
            switch (eventLevel)
            {
                case EventLevel.Informational:
                    EtwLogger.IsVerboseEnabled = false;
                    break;
                case EventLevel.Warning:
                    EtwLogger.IsVerboseEnabled = false;
                    EtwLogger.IsInfoEnabled = false;
                    break;
                case EventLevel.Error:
                    EtwLogger.IsVerboseEnabled = false;
                    EtwLogger.IsInfoEnabled = false;
                    EtwLogger.IsWarnEnabled = false;
                    break;
            }
        }
        #endregion

    }
}
