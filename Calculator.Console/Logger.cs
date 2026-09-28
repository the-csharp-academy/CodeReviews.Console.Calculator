namespace Calculator.Console;
using System.Diagnostics;

public class Logger : IDisposable
{
    private StreamWriter? _logFile;
    private TextWriterTraceListener? _listener;
    private readonly string _filePath = "Calculator.";

    public Logger(string fileName)
    {
        _filePath += fileName;
        InitLogger();
    }

    private void InitLogger()
    {
        _logFile = File.CreateText(_filePath);
        _listener = new TextWriterTraceListener(_logFile);
        Trace.Listeners.Add(_listener);
        Trace.AutoFlush = true;
        Intro(_filePath);
    }
    
    public void Dispose()
    {
        Trace.WriteLine($"Closing log at {DateTime.Now}");
        if (_listener != null)
        {
            Trace.Listeners.Remove(_listener);
            _listener.Dispose();
            _listener = null;
        }
        if (_logFile != null)
        {
            _logFile.Dispose();
            _logFile = null;
        }
    }

    public void Result(double n1, double n2, Library.Operator op, double result)
    {
        if (_logFile == null)
        {
            InitLogger();
        }
        Trace.WriteLine($"{(op.AmountInput() > 1 ? n1 + " " : "")}{op.GetSymbol()} {n2} = {result}");
    }

    public void DeleteLogs()
    {
        Dispose();
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
    
    private void Intro(string fileName)
    {
        Trace.WriteLine($"Starting {fileName} Log");
        Trace.WriteLine(String.Format($"Started {DateTime.Now}"));
    }
}