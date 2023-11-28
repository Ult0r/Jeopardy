using System;
using Microsoft.Extensions.Logging;

namespace Jeopardy.Helper.Logging;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;

    public FileLoggerProvider(string path)
    {
        _path = path;
    }
    
    void IDisposable.Dispose()
    {
        GC.SuppressFinalize(this);
    }
    
    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_path);
    }
}