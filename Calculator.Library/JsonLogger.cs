using Newtonsoft.Json;

namespace Calculator.Library;

public  class JsonLogger : IDisposable
{
    private JsonWriter? _writer;
    private const string FilePath = "CalculatorLog.json";

    public JsonLogger()
    {
        InitLogger();
    }

    private void InitLogger()
    {
        var logFile = File.CreateText(FilePath);
        logFile.AutoFlush = true;
        _writer = new JsonTextWriter(logFile);
        _writer.Formatting = Formatting.Indented;
        _writer.WriteStartObject();
        _writer.WritePropertyName("Operations");
        _writer.WriteStartArray();
    }

    public void LogResult(double n1, double n2, Operator op, double result)
    {
        if (_writer == null)
        {
            InitLogger();
        }
        _writer!.WriteStartObject();
        _writer.WritePropertyName("Operand1");
        _writer.WriteValue(n1);
        if (op.AmountInput() > 1)
        {
            _writer.WritePropertyName("Operand2");
            _writer.WriteValue(n2);
        }
        _writer.WritePropertyName("Operation");
        _writer.WriteValue(op.GetSymbol());
        _writer.WritePropertyName("Result");
        _writer.WriteValue(result);
        _writer.WriteEndObject();
    }

    public void DeleteLogs()
    {
        Dispose();
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
    }
    
    public void Dispose()
    {
        if (_writer == null) return;
        
        _writer.WriteEndArray();
        _writer.WriteEndObject();
        _writer.Close();
        ((IDisposable)_writer).Dispose();
        _writer = null;
    }
}