using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace EtwLogger.Logger
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

    internal class EtwLoggerEventSource : EventSource
    {
        #region Public Property

        public static Type StackBoundaryType = typeof(EtwLogger);
        #endregion

        #region Public Variables
        /// <summary>
        /// ETWFileLoggerEventSource static Object
        /// </summary>
        public static EtwLoggerEventSource Log = new EtwLoggerEventSource();
        #endregion

        #region Public Methods
        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        [Event(1, Level = EventLevel.Verbose)]
        public void Verbose(string message)
        {
            WriteEvent(1, message, "VERBOSE", GetClassName, GetThreadName);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        [Event(2, Level = EventLevel.Informational)]
        public void Info(string message)
        {
            WriteEvent(2, message, "INFO", GetClassName, GetThreadName);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        [Event(3, Level = EventLevel.Warning)]
        public void Warn(string message)
        {
            WriteEvent(3, message, "WARN", GetClassName, GetThreadName);
        }

        /// <summary>
        /// Error the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [Event(4, Level = EventLevel.Error)]
        public void Error(string message)
        {
            WriteEvent(4, message, "ERROR", GetClassName, GetThreadName);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="message">The message.</param>
        [Event(5, Level = EventLevel.Critical)]
        public void Critical(string message)
        {
            WriteEvent(5, message, "CRITICAL", GetClassName, GetThreadName);
        }

        /// <summary>
        /// SQLs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [Event(6, Level = EventLevel.LogAlways)]
        public void Sql(string message)
        {
            WriteEvent(6, message, "SQL", GetClassName, GetThreadName);
        }
        #endregion

        #region Local Methods
        /// <summary>
        /// Gets the name of the get class.
        /// </summary>
        /// <value>
        /// The name of the get class.
        /// </value>
        internal string GetClassName
        {
            get
            {
                var className = "?";
                try
                {
                    var declareType = StackBoundaryType;
                    var st = new StackTrace(false);
                    var frameIndex = 0;

                    // skip frames that is from the logging stack class
                    while (frameIndex < st.FrameCount)
                    {
                        var frame = st.GetFrame(frameIndex);
                        if (frame != null && frame.GetMethod().DeclaringType == declareType)
                        {
                            // Check if the next type contains Log.ETWLog
                            // This code is required because the stack trace when running in Console Mode
                            // includes Log.ETWLog whereas when running as a non console it does NOT, don't
                            // know why though.
                            if ((frameIndex + 1) < st.FrameCount)
                            {
                                var nextFrame = st.GetFrame(frameIndex + 1);
                                var declaringType = nextFrame.GetMethod().DeclaringType;
                                if (declaringType?.ToString().IndexOf("EtwLogger.EtwLog", StringComparison.Ordinal) > -1)
                                {
                                    declareType = nextFrame.GetMethod().DeclaringType;
                                    frameIndex = frameIndex + 1;
                                }
                            }
                            break;
                        }
                        frameIndex++;
                    }

                    // skip frames from from the logging stack class
                    while (frameIndex < st.FrameCount)
                    {
                        var frame = st.GetFrame(frameIndex);
                        if (frame != null && frame.GetMethod().DeclaringType != declareType)
                        {
                            break;
                        }
                        frameIndex++;
                    }

                    if (frameIndex < st.FrameCount)
                    {
                        var locationFrame = st.GetFrame(frameIndex);

                        if (locationFrame != null)
                        {
                            var method = locationFrame.GetMethod();
                            if (method != null)
                            {
                                if (method.DeclaringType != null)
                                {
                                    className = method.DeclaringType.FullName;
                                    var plusPos = className.IndexOf("+<>", StringComparison.Ordinal);
                                    if (plusPos > -1)
                                    {
                                        if (method.DeclaringType != null &&
                                            method.DeclaringType.DeclaringType != null &&
                                            method.DeclaringType.DeclaringType.BaseType != null)
                                            className = method.DeclaringType.DeclaringType.BaseType.FullName;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (SecurityException) { }

                return className;
            }
        }

        /// <summary>
        /// Gets the name of the get thread.
        /// </summary>
        /// <value>
        /// The name of the get thread.
        /// </value>
        internal string GetThreadName => Thread.CurrentThread.Name ??
                                        Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);

        #endregion

    }
}
