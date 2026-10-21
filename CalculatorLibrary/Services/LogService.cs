using CalculatorLibrary.Models;
using System.Diagnostics;
using System.Text.Json;

namespace CalculatorLibrary.Services;

internal class LogService
{
    private readonly string _logFilePath = Path.Combine(AppContext.BaseDirectory, "logs", "calculator-log.jsonl");
    internal void Log(CalculationLogEntry entry)
    {
        try
        {
            var directory = Path.GetDirectoryName(_logFilePath);

            if (directory is not null)
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonSerializer.Serialize(entry);
            File.AppendAllText(_logFilePath, json + Environment.NewLine);
        }
        catch (IOException ex)
        {
            Trace.TraceError($"Could not write the calculator log: {ex}");
        }
        catch (UnauthorizedAccessException ex)
        {
            Trace.TraceError($"No permission to write the calculator log: {ex}");
        }
    }
}