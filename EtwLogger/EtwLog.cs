using System;

namespace EtwLogger
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

    public static class EtwLog
    {
        #region Constructor
        /// <summary>
        /// Initializes the <see cref="EtwLog"/> class.
        /// </summary>
        static EtwLog()
        {
            Logger.EtwLogger.IsVerboseEnabled = true;
            Logger.EtwLogger.IsInfoEnabled = true;
            Logger.EtwLogger.IsWarnEnabled = true;
            Logger.EtwLogger.IsErrorEnabled = true;
            Logger.EtwLogger.IsCriticalEnabled = true;
            Logger.EtwLogger.IsSqlEnabled = false;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public static int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is debug enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is debug enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsVerboseEnabled
        {
            get { return Logger.EtwLogger.IsVerboseEnabled; }
            set { Logger.EtwLogger.IsVerboseEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is information enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is information enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInfoEnabled
        {
            get { return Logger.EtwLogger.IsInfoEnabled; }
            set { Logger.EtwLogger.IsInfoEnabled = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [is warn enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is warn enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsWarnEnabled
        {
            get { return Logger.EtwLogger.IsWarnEnabled; }
            set { Logger.EtwLogger.IsWarnEnabled = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [is error enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is error enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsErrorEnabled
        {
            get { return Logger.EtwLogger.IsErrorEnabled; }
            set { Logger.EtwLogger.IsErrorEnabled = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [is fatal enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is fatal enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsCriticalEnabled
        {
            get { return Logger.EtwLogger.IsCriticalEnabled; }
            set { Logger.EtwLogger.IsCriticalEnabled = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [is SQL enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is SQL enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsSqlEnabled
        {
            get { return Logger.EtwLogger.IsSqlEnabled; }
            set { Logger.EtwLogger.IsSqlEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the SMTP host.
        /// </summary>
        /// <value>
        /// The SMTP host.
        /// </value>
        public static string SmtpHost
        {
            get { return Logger.EtwLogger.SmtpHost; }
            set { Logger.EtwLogger.SmtpHost = value; }
        }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public static int Port
        {
            get { return Logger.EtwLogger.Port; }
            set { Logger.EtwLogger.Port = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable SSL].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable SSL]; otherwise, <c>false</c>.
        /// </value>
        public static bool EnableSsl
        {
            get { return Logger.EtwLogger.EnableSsl; }
            set { Logger.EtwLogger.EnableSsl = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log to file when sending email].
        /// </summary>
        /// <value>
        /// <c>true</c> if [log to file when sending email]; otherwise, <c>false</c>.
        /// </value>
        public static bool LogToFileWhenSendingEmail
        {
            get { return Logger.EtwLogger.LogToFileWhenSendingEmail; }
            set { Logger.EtwLogger.LogToFileWhenSendingEmail = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is desk top application].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is desk top application]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDeskTopApp
        {
            get { return Logger.EtwLogger.IsDeskTopApp; }
            set { Logger.EtwLogger.IsDeskTopApp = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Exits this instance.
        /// </summary>
        public static void Exit()
        {
            Logger.EtwLogger.Exit();
        }

        #region Verbose
        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Verbose(object message)
        {
            Logger.EtwLogger.Verbose(message);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Verbose(object message, Exception exception)
        {
            Logger.EtwLogger.Verbose(message, exception);
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void VerboseFormat(string format, params object[] args)
        {
            Logger.EtwLogger.VerboseFormat(format, args);
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void VerboseFormat(string format, object arg0)
        {
            Logger.EtwLogger.VerboseFormat(format, arg0);
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void VerboseFormat(string format, object arg0, object arg1)
        {
            Logger.EtwLogger.VerboseFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public static void VerboseFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.EtwLogger.VerboseFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void VerboseFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.EtwLogger.VerboseFormat(provider, format, args);
        }
        #endregion

        #region Info
        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(object message)
        {
            Logger.EtwLogger.Info(message);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Info(object message, Exception exception)
        {
            Logger.EtwLogger.Info(message, exception);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void InfoFormat(string format, params object[] args)
        {
            Logger.EtwLogger.InfoFormat(format, args);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void InfoFormat(string format, object arg0)
        {
            Logger.EtwLogger.InfoFormat(format, arg0);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void InfoFormat(string format, object arg0, object arg1)
        {
            Logger.EtwLogger.InfoFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.EtwLogger.InfoFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.EtwLogger.InfoFormat(provider, format, args);
        }
        #endregion

        #region Warning
        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(object message)
        {
            Logger.EtwLogger.Warn(message);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Warn(object message, Exception exception)
        {
            Logger.EtwLogger.Warn(message, exception);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WarnFormat(string format, params object[] args)
        {
            Logger.EtwLogger.WarnFormat(format, args);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void WarnFormat(string format, object arg0)
        {
            Logger.EtwLogger.WarnFormat(format, arg0);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void WarnFormat(string format, object arg0, object arg1)
        {
            Logger.EtwLogger.WarnFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.EtwLogger.WarnFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.EtwLogger.WarnFormat(provider, format, args);
        }
        #endregion

        #region Error
        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(object message)
        {
            Logger.EtwLogger.Error(message);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(object message, Exception exception)
        {
            Logger.EtwLogger.Error(message, exception);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void ErrorFormat(string format, params object[] args)
        {
            Logger.EtwLogger.ErrorFormat(format, args);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void ErrorFormat(string format, object arg0)
        {
            Logger.EtwLogger.ErrorFormat(format, arg0);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            Logger.EtwLogger.ErrorFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.EtwLogger.ErrorFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.EtwLogger.ErrorFormat(provider, format, args);
        }
        #endregion

        #region Critical
        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Critical(object message)
        {
            Logger.EtwLogger.Critical(message);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Critical(object message, Exception exception)
        {
            Logger.EtwLogger.Critical(message, exception);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void CriticalFormat(string format, params object[] args)
        {
            Logger.EtwLogger.CriticalFormat(format, args);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void CriticalFormat(string format, object arg0)
        {
            Logger.EtwLogger.CriticalFormat(format, arg0);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void CriticalFormat(string format, object arg0, object arg1)
        {
            Logger.EtwLogger.CriticalFormat(format, arg0, arg1);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public static void CriticalFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.EtwLogger.CriticalFormat(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void CriticalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.EtwLogger.CriticalFormat(provider, format, args);
        }
        #endregion

        #region Sql
        /// <summary>
        /// Log SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        public static void Sql(string sqlQuery)
        {
            Logger.EtwLogger.Sql(sqlQuery);
        }
        #endregion

        #endregion
    }

}
