using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Helper.Logging;

public class FileLogger : ILogger
{
    // ReSharper disable once InconsistentNaming LF is common as all uppercase abbreviation
    private static readonly string LF = Environment.NewLine;
    private static readonly object Lock = new();
    
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
    }
    
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string>? formatter)
    {
        if (formatter is not null)
        {
            lock (Lock)
            {
                var exceptionStr = "";
                if (exception is not null)
                {
                    exceptionStr = $"{LF}{exception.GetType()}: {exception.Message}{LF}{exception.StackTrace}{LF}";
                }
                File.AppendAllText(_filePath, $"[{DateTime.Now:u}] {logLevel.ToString()}: {formatter(state, exception)}{LF}{exceptionStr}");
            }
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}