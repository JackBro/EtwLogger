using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using EtwLogger.Extensions;
using EtwLogger.Helpers;

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

    public class EtwLoggerEventListener : EventListener
    {
        #region EtwLoggerEventListenerData Class
        /// <summary>
        /// EtwLoggerData class
        /// </summary>
        internal class EtwLoggerEventListenerData
        {
            private const string FormatNotAsync = "{0:yyyy-MM-dd HH:mm:ss,fff} [{1}] {2} {3} - {4}";
            private const string Format = "{0:yyyy-MM-dd HH:mm:ss,fff} [Async->{1:HH:mm:ss,fff}] [{2}] {3} {4} - {5}";
            private const string SqlFormatNotAsync = "--{0:yyyy-MM-dd HH:mm:ss,fff} [{1}] {2} {3}\r\n{4}";
            private const string SqlFormat = "--{0:yyyy-MM-dd HH:mm:ss,fff} [Async->{1:HH:mm:ss,fff}] [{2}] {3} {4}\r\n{5}";

            /// <summary>
            /// Initializes a new instance of the <see cref="EtwLoggerEventListenerData" /> class.
            /// </summary>
            /// <param name="loggedDateTime">The logged date time.</param>
            /// <param name="threadIdentifier">The thread identifier.</param>
            /// <param name="level">The level.</param>
            /// <param name="callingMethod">The calling method.</param>
            /// <param name="message">The message.</param>
            /// <param name="isAsyncLoggingDisabled">if set to <c>true</c> [is asynchronous logging disabled].</param>
            public EtwLoggerEventListenerData(DateTime loggedDateTime, string threadIdentifier, string level, string callingMethod, string message, bool isAsyncLoggingDisabled)
            {
                LoggedDateTime = loggedDateTime;
                ThreadIdentifier = threadIdentifier;
                Level = level;
                CallingMethod = callingMethod;
                Message = message;
                _isAsyncLoggingDisabled = isAsyncLoggingDisabled;
            }

            private readonly bool _isAsyncLoggingDisabled;
            public DateTime LoggedDateTime { get; }
            public string ThreadIdentifier { get; }
            public string Level { get; }
            public string CallingMethod { get; }
            public string Message { get; }

            /// <summary>
            /// Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String" /> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                if (_isAsyncLoggingDisabled)
                    return string.Format(Level.ToUpper() == "SQL" ? SqlFormatNotAsync : FormatNotAsync, LoggedDateTime, ThreadIdentifier, Level, CallingMethod, Message);
                return string.Format(Level.ToUpper() == "SQL" ? SqlFormat : Format, LoggedDateTime, DateTime.UtcNow,
                    ThreadIdentifier, Level, CallingMethod, Message);
            }
        }

        #endregion

        #region Local Variables

        private const int MaxWaitTimeInSeconds = 30000; // 30 seconds
        private readonly string _logPath;
        private readonly Type _eventSourceBindType;

        private readonly string _fileName;

        /// <summary>
        /// Gets the file name only.
        /// </summary>
        /// <value>
        /// The file name only.
        /// </value>
        private string FileNameOnly => string.Concat(_fileName, ".", DateTime.UtcNow.ToString("yyyyMMdd"), ".", _extension);

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        private string FileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _logPath, FileNameOnly);

        private readonly string _extension;
        private readonly long _maxFileLength;
        private long _fileCurrentLength;
        private readonly ConcurrentQueue<EtwLoggerEventListenerData> _logQueue;
        private FileStream _fileStream;
        private StreamWriter _fileStreamWriter;
        private readonly bool _sqlType;
        private string _currentFileName;
        private Thread _logQueueThread;
        private bool _isExit;

        private readonly bool _udpLoggingEnabled;
        private readonly string _udpDestIpAddress;
        private readonly int _udpDestIpPort;
        private readonly ConcurrentQueue<string> _logUdpQueue;
        private Thread _logUdpQueueThread;

        #endregion

        #region Public Variable
        /// <summary>
        /// Gets or sets a value indicating whether [is desk top application].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is desk top application]; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeskTopApp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is exit].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is exit]; otherwise, <c>false</c>.
        /// </value>
        public bool IsExit
        {
            get { return _isExit; }
            set
            {
                _isExit = value;

                if (_isExit)
                {
                    if (_logQueueThread != null)
                    {
                        _logQueueThread.Join(MaxWaitTimeInSeconds);
                        if (_logQueueThread.IsAlive)
                            _logQueueThread.Abort();

                        _logQueueThread = null;
                    }

                    if (_logUdpQueueThread != null)
                    {
                        _logUdpQueueThread.Join(MaxWaitTimeInSeconds);
                        if (_logUdpQueueThread.IsAlive)
                            _logUdpQueueThread.Abort();

                        _logUdpQueueThread = null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [is asynchronous logging disabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is asynchronous logging disabled]; otherwise, <c>false</c>.
        /// </value>
        public bool IsAsyncLoggingDisabled { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="EtwLoggerEventListener" /> class.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="extension">The extension.</param>
        /// <param name="sqlType">if set to <c>true</c> [SQL type].</param>
        public EtwLoggerEventListener(string logPath, string fileName, string extension, bool sqlType)
        {
            _logPath = logPath != null ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logPath) : string.Empty;
            if (!string.IsNullOrEmpty(_logPath))
            {
                if (!Directory.Exists(_logPath))
                    Directory.CreateDirectory(_logPath);
            }

            _fileName = fileName;
            _extension = extension;
            _eventSourceBindType = typeof(EtwLoggerEventSource);
            _sqlType = sqlType;
            _maxFileLength = ConfigurationManager.AppSettings["EtwLogFileLengthInBytes"].ToInt64(1024 * 1024 * 4); // Default 4MB
            _fileCurrentLength = File.Exists(FileName) ? new FileInfo(FileName).Length : 0;
            _logQueue = new ConcurrentQueue<EtwLoggerEventListenerData>();

            // Get Udp settings
            _udpLoggingEnabled = ConfigurationManager.AppSettings["UdpLoggingEnabled"].ToBool(false); // Default disabled
            _udpDestIpPort = ConfigurationManager.AppSettings["UdpDestIpPort"].ToInt32(12001); // Default port
            _udpDestIpAddress = ConfigurationManager.AppSettings["UdpDestIpAddress"].ToString("localhost"); // Default localhost
            _logUdpQueue = new ConcurrentQueue<string>();

            if (!IsAsyncLoggingDisabled)
            {
                _logQueueThread = new Thread(() =>
                {
                    while (!IsExit)
                    {
                        try
                        {
                            EtwLoggerEventListenerData etwLoggerEventListenerData;
                            while (_logQueue.TryDequeue(out etwLoggerEventListenerData))
                            {
                                LogMessageToFile(etwLoggerEventListenerData);
                            }
                        }
                        catch (Exception ex)
                        {
                            FileLogger.Error(ex.Message);
                        }

                        finally
                        {
                            Thread.Sleep(1);
                        }
                    }

                    CheckCloseFileStream();
                })
                { IsBackground = false, Name = "Etw Async Thread" }; // You must call Exit to exit logging otherwise this thread will be left running
                _logQueueThread.Start();
            }

            if (_udpLoggingEnabled)
            {
                _logUdpQueueThread = new Thread(() =>
                {
                    while (!IsExit)
                    {
                        try
                        {
                            string data;
                            while (_logUdpQueue.TryDequeue(out data))
                            {
                                SendUdp(data);
                            }
                        }
                        catch (Exception ex)
                        {
                            FileLogger.Error(ex.Message);
                        }

                        finally
                        {
                            Thread.Sleep(1);
                        }
                    }
                })
                { IsBackground = false, Name = "Etw Async Udp Thread" }; // You must call Exit to exit logging otherwise this thread will be left running
                _logUdpQueueThread.Start();
            }

        }

        /// <summary>
        /// Logs the message to file.
        /// </summary>
        /// <param name="etwLoggerEventListenerData">The etw logger data.</param>
        private void LogMessageToFile(EtwLoggerEventListenerData etwLoggerEventListenerData)
        {
            try
            {
                // UDP the message
                PublishUdpMessage(etwLoggerEventListenerData);

                var lineToWrite = etwLoggerEventListenerData + "\r\n";
                var lengthInBytes = Encoding.UTF8.GetBytes(lineToWrite).Length;
                RollOverFile(lengthInBytes);
                CheckOpenFileStream();
                if (_fileStreamWriter == null) return;
                _fileStreamWriter.Write(lineToWrite);
                _fileStreamWriter.Flush();
                if (Environment.UserInteractive && !_sqlType)
                    WriteToConsole(etwLoggerEventListenerData);
                _fileCurrentLength += lengthInBytes;
            }
            catch (Exception ex)
            {
                FileLogger.Error(ex.Message);
            }
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Writes the automatic console.
        /// </summary>
        /// <param name="etwLoggerEventListenerData">The etw logger data.</param>
        private void WriteToConsole(EtwLoggerEventListenerData etwLoggerEventListenerData)
        {
            if (IsDeskTopApp) return;

            Console.ResetColor();
            switch (etwLoggerEventListenerData.Level)
            {
                case "DEBUG":
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "INFO":
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "WARN":
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "ERROR":
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "FATAL":
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            Console.WriteLine(etwLoggerEventListenerData);
            Console.ResetColor();
        }

        /// <summary>
        /// Checks the open file stream.
        /// </summary>
        private void CheckOpenFileStream()
        {
            if (_fileStream == null)
            {
                OpenFileStream();
            }
            else if (_currentFileName != FileName)
            {
                CheckCloseFileStream();
                OpenFileStream();
            }
        }

        /// <summary>
        /// Opens the file stream.
        /// </summary>
        private void OpenFileStream()
        {
            try
            {
                _fileStream = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                _fileStreamWriter = new StreamWriter(_fileStream);
                _currentFileName = FileName;
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        /// Checks the close file stream.
        /// </summary>
        private void CheckCloseFileStream()
        {
            if (_fileStream != null)
            {
                _fileStreamWriter.Close();
                _fileStreamWriter = null;
                _fileStream.Close();
                _fileStream = null;
                _currentFileName = null;
            }
        }

        /// <summary>
        /// Rolls the over file.
        /// </summary>
        /// <param name="lineCount">The line count.</param>
        private void RollOverFile(long lineCount)
        {
            if (!File.Exists(FileName)) return;

            if ((_fileCurrentLength + lineCount) < _maxFileLength) return;
            // we need to roll over the files
            // test.txt to test.txt.1
            // if test.txt.1 exists then
            // test.txt.1 to test.txt.2
            // test.txt to test.txt.1
            var files = Directory.GetFiles(_logPath, FileNameOnly + ".*");

            // need to get the maximum file number
            var maxFileNumber = 0;
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file);
                if (extension == null) continue;
                if (extension.StartsWith(".")) extension = extension.Substring(1);
                if (!extension.IsNumeric()) continue;
                var val = int.Parse(extension);
                if (val > maxFileNumber)
                    maxFileNumber = val;
            }

            CheckCloseFileStream();

            for (var m = maxFileNumber; m >= 1; m--)
            {
                RenameFile(FileName + "." + m, FileName + "." + (m + 1));
            }
            RenameFile(FileName, FileName + ".1");
            _fileCurrentLength = 0;
        }

        /// <summary>
        /// Renames the file.
        /// </summary>
        /// <param name="fromFileName">Name of from file.</param>
        /// <param name="toFileName">Name of the automatic file.</param>
        private void RenameFile(string fromFileName, string toFileName)
        {
            if (File.Exists(fromFileName))
            {
                // No choice but to delete to file
                if (File.Exists(toFileName))
                    File.Delete(toFileName);

                try
                {
                    File.Move(fromFileName, toFileName);
                }
                catch (Exception ex)
                {
                    FileLogger.Error(ex.Message);
                }
            }
            else
            {
                FileLogger.Error("EtwLoggerEventListener.RenameFile - Cannot rename file as fromFilename " + fromFileName + " not found");
            }
        }

        /// <summary>
        /// Publishes the UDP message.
        /// </summary>
        /// <param name="etwLoggerEventListenerData">The ETW logger data.</param>
        private void PublishUdpMessage(EtwLoggerEventListenerData etwLoggerEventListenerData)
        {
            if (!_udpLoggingEnabled) return;

            var outData = new Dictionary<string, string>
             {
                 {"DateTime", etwLoggerEventListenerData.LoggedDateTime.ToString("yyyy-MM-dd HH:mm:ss,fff")},
                 {"ProcessName", AssemblyHelper.GetEntryAssemblyName()},
                 {"ThreadId", etwLoggerEventListenerData.ThreadIdentifier},
                 {"Level", etwLoggerEventListenerData.Level},
                 {"Method", etwLoggerEventListenerData.CallingMethod},
                 {"Message", etwLoggerEventListenerData.Message}
             };

            var result = outData.JavaSerializeToJson();

            _logUdpQueue.Enqueue(result);
        }

        /// <summary>
        /// Sends the UDP.
        /// </summary>
        /// <param name="data">The data.</param>
        private void SendUdp(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);

            using (var udpClient = new UdpClient())
            {
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Send(bytes, bytes.Length, _udpDestIpAddress, _udpDestIpPort);
            }
        }
        #endregion

        #region EventListener Members
        /// <summary>
        /// Called whenever an event has been written by an event source for which the event listener has enabled events.
        /// </summary>
        /// <param name="eventData">The event arguments that describe the event.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (_fileName == null) return;

            if (eventData.EventSource.GetType() != _eventSourceBindType) return;
            if (eventData.Payload.Count != 1) return;
            if (eventData.Level == EventLevel.LogAlways && _sqlType)
            {
                var etwLoggerEventListenerData = new EtwLoggerEventListenerData(DateTime.UtcNow, ((EtwLoggerEventSource)eventData.EventSource).GetThreadName,
                    "SQL", ((EtwLoggerEventSource)eventData.EventSource).GetClassName, eventData.Payload[0].ToString(),
                    IsAsyncLoggingDisabled);

                if (IsAsyncLoggingDisabled)
                    LogMessageToFile(etwLoggerEventListenerData);
                else
                    _logQueue.Enqueue(etwLoggerEventListenerData);
            }
            else if (eventData.Level != EventLevel.LogAlways && !_sqlType)
            {
                var etwLoggerEventListenerData = new EtwLoggerEventListenerData(DateTime.UtcNow, ((EtwLoggerEventSource)eventData.EventSource).GetThreadName,
                    eventData.Level.ToString().ToUpper(), ((EtwLoggerEventSource)eventData.EventSource).GetClassName, eventData.Payload[0].ToString(),
                    IsAsyncLoggingDisabled);

                if (IsAsyncLoggingDisabled)
                    LogMessageToFile(etwLoggerEventListenerData);
                else
                    _logQueue.Enqueue(etwLoggerEventListenerData);
            }
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            CheckCloseFileStream();

            base.Dispose();
        }
        #endregion

    }
}
