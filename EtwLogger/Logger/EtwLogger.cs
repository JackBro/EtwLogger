using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using EtwLogger.Helpers;
using EtwLogger.Exceptions;

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

    public static class EtwLogger
    {
        #region Constructor
        /// <summary>
        /// Initializes the <see cref="EtwLogger"/> class.
        /// </summary>
        static EtwLogger()
        {
            IsVerboseEnabled = true;
            IsInfoEnabled = true;
            IsWarnEnabled = true;
            IsErrorEnabled = true;
            IsCriticalEnabled = true;
            IsSqlEnabled = false;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether [is desk top application].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is desk top application]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDeskTopApp
        {
            get
            {
                return EtwLoggerEventListener.IsDeskTopApp;
            }
            set
            {
                EtwLoggerEventListener.IsDeskTopApp = value;
                if (EtwSqlLoggerEventListener != null) EtwSqlLoggerEventListener.IsDeskTopApp = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [is debug enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is debug enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsVerboseEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is information enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is information enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInfoEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is warn enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is warn enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsWarnEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is error enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is error enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsErrorEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is fatal enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is fatal enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsCriticalEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is SQL enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is SQL enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsSqlEnabled { get; set; }
        /// <summary>
        /// Gets or sets the SMTP host.
        /// </summary>
        /// <value>
        /// The SMTP host.
        /// </value>
        public static string SmtpHost { get; set; }
        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        public static int Port { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [enable SSL].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable SSL]; otherwise, <c>false</c>.
        /// </value>
        public static bool EnableSsl { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [log to file when sending email].
        /// </summary>
        /// <value>
        /// <c>true</c> if [log to file when sending email]; otherwise, <c>false</c>.
        /// </value>
        public static bool LogToFileWhenSendingEmail { get; set; }
        #endregion

        #region Internal Properties
        /// <summary>
        /// Gets or sets the ETW file logger event listener.
        /// </summary>
        /// <value>
        /// The ETW file logger event listener.
        /// </value>
        internal static EtwLoggerEventListener EtwLoggerEventListener { get; set; }
        /// <summary>
        /// Gets or sets the ETW SQL file logger event listener.
        /// </summary>
        /// <value>
        /// The ETW SQL file logger event listener.
        /// </value>
        internal static EtwLoggerEventListener EtwSqlLoggerEventListener { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Exits this instance.
        /// </summary>
        public static void Exit()
        {
            EtwLoggerEventListener.IsExit = true;
            if (EtwSqlLoggerEventListener != null) EtwSqlLoggerEventListener.IsExit = true;
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Verbose(object message)
        {
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(message.ToString());
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Verbose(object message, Exception exception)
        {
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(message + "\r\n" + exception);
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void VerboseFormat(string format, params object[] args)
        {
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(string.Format(format, args));
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void VerboseFormat(string format, object arg0)
        {
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(string.Format(format, arg0));
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void VerboseFormat(string format, object arg0, object arg1)
        {
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(string.Format(format, arg0, arg1));
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
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// Verbose message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void VerboseFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsVerboseEnabled) return;
            EtwLoggerEventSource.Log.Verbose(string.Format(provider, format, args));
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(object message)
        {
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(message.ToString());
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Info(object message, Exception exception)
        {
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(message + "\r\n" + exception);
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void InfoFormat(string format, params object[] args)
        {
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(string.Format(format, args));
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void InfoFormat(string format, object arg0)
        {
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(string.Format(format, arg0));
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void InfoFormat(string format, object arg0, object arg1)
        {
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(string.Format(format, arg0, arg1));
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
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// Information message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsInfoEnabled) return;
            EtwLoggerEventSource.Log.Info(string.Format(provider, format, args));
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(object message)
        {
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(message.ToString());
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Warn(object message, Exception exception)
        {
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(message + "\r\n" + exception);
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WarnFormat(string format, params object[] args)
        {
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(string.Format(format, args));
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void WarnFormat(string format, object arg0)
        {
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(string.Format(format, arg0));
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void WarnFormat(string format, object arg0, object arg1)
        {
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(string.Format(format, arg0, arg1));
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
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// Warning message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsWarnEnabled) return;
            EtwLoggerEventSource.Log.Warn(string.Format(provider, format, args));
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(object message)
        {
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(message.ToString());
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(object message, Exception exception)
        {
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(message + "\r\n" + exception);
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void ErrorFormat(string format, params object[] args)
        {
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(string.Format(format, args));
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void ErrorFormat(string format, object arg0)
        {
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(string.Format(format, arg0));
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(string.Format(format, arg0, arg1));
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
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// Error message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsErrorEnabled) return;
            EtwLoggerEventSource.Log.Error(string.Format(provider, format, args));
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Critical(object message)
        {
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(message.ToString());
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Critical(object message, Exception exception)
        {
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(message + "\r\n" + exception);
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void CriticalFormat(string format, params object[] args)
        {
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(string.Format(format, args));
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public static void CriticalFormat(string format, object arg0)
        {
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(string.Format(format, arg0));
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public static void CriticalFormat(string format, object arg0, object arg1)
        {
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(string.Format(format, arg0, arg1));
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
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(string.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// Critical message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void CriticalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (!IsCriticalEnabled) return;
            EtwLoggerEventSource.Log.Critical(string.Format(provider, format, args));
        }

        /// <summary>
        /// Log SQL query.
        /// </summary>
        /// <param name="sqlQuery">The SQL query.</param>
        public static void Sql(string sqlQuery)
        {
            if (!IsSqlEnabled) return;
            EtwLoggerEventSource.Log.Sql(sqlQuery);
        }

        /// <summary>
        /// Send Email alert.
        /// </summary>
        /// <param name="toAddress">To address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void EmailAlert(string toAddress, string subject, string body)
        {
            EmailAlert(string.Empty, toAddress, subject, body, null, false);
        }

        /// <summary>
        /// Send Email alert.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="toAddress">The automatic address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        public static void EmailAlert(string fromAddress, string toAddress, string subject, string body)
        {
            EmailAlert(fromAddress, toAddress, subject, body, null, false);
        }

        /// <summary>
        /// Emails the alert.
        /// </summary>
        /// <param name="toAddress">The automatic address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="highPriority">if set to <c>true</c> [high priority].</param>
        public static void EmailAlert(string toAddress, string subject, string body, bool highPriority)
        {
            EmailAlert(string.Empty, toAddress, subject, body, null, highPriority);
        }

        /// <summary>
        /// Emails the alert.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="toAddress">The automatic address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        public static void EmailAlert(string fromAddress, string toAddress, string subject, string body, List<string> attachments)
        {
            EmailAlert(fromAddress, toAddress, subject, body, attachments, false);
        }

        /// <summary>
        /// Send Email alert.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="toAddress">To address.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="highPriority">if set to <c>true</c> [high priority].</param>
        /// <exception>Cannot find attachment  + filename
        ///     <cref>EtwLogger.Exceptions.EtwException</cref>
        /// </exception>
        public static void EmailAlert(string fromAddress, string toAddress, string subject, string body, List<string> attachments, bool highPriority)
        {
            if (LogToFileWhenSendingEmail)
            {
                Info(body);
            }

            var smtpClient = new SmtpClient
            {
                Host = string.IsNullOrEmpty(SmtpHost) ? "smtphost.test.com" : SmtpHost,
                Port = Port == 0 ? 25 : Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = EnableSsl
            };

            using (var mailMessage = new MailMessage())
            {
                mailMessage.Body = body;
                // Check if the Body email is HTML
                if (!string.IsNullOrEmpty(body) && body.ToUpper().StartsWith("<HTML>"))
                {
                    mailMessage.IsBodyHtml = true;
                }
                var fromAddressOverride = !string.IsNullOrEmpty(fromAddress)
                                              ? fromAddress
                                              : $"{Environment.MachineName}@test.com";
                mailMessage.From = new MailAddress(fromAddressOverride, AssemblyHelper.GetEntryAssemblyName().RemoveSpaces());
                var toList = toAddress.Split(';');
                foreach (var to in toList)
                    mailMessage.To.Add(to);
                mailMessage.Subject = subject;
                mailMessage.Priority = highPriority ? MailPriority.High : MailPriority.Normal;

                if (attachments != null)
                {
                    foreach (var filename in attachments)
                    {
                        if (!File.Exists(filename))
                            throw new EtwException("Cannot find attachment " + filename);

                        var attachment = new Attachment(filename, MediaTypeNames.Application.Octet);
                        var disposition = attachment.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(filename);
                        disposition.ModificationDate = File.GetLastWriteTime(filename);
                        disposition.ReadDate = File.GetLastAccessTime(filename);
                        disposition.FileName = Path.GetFileName(filename);
                        disposition.Size = new FileInfo(filename).Length;
                        disposition.DispositionType = DispositionTypeNames.Attachment;
                        mailMessage.Attachments.Add(attachment);
                    }
                }

                smtpClient.Send(mailMessage);
            }
        }
        #endregion

    }
}
