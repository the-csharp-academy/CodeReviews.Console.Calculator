namespace CalculatorLibrary;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

public class Calculator
{
    readonly JsonWriter writer;

     public Calculator()
     {
        StreamWriter logFile = File.CreateText("calculatorlog.json");
        logFile.AutoFlush = true;
        writer = new JsonTextWriter(logFile)
        {
            Formatting = Formatting.Indented
        };
        writer.WriteStartObject();
        writer.WritePropertyName("Operations");
        writer.WriteStartArray();
     }
   
    public double DoOperation(double num1, double num2, string op)
     {
         double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
         writer.WriteStartObject();
         writer.WritePropertyName("Operand1");
         writer.WriteValue(num1);
         writer.WritePropertyName("Operand2");
         writer.WriteValue(num2);
         writer.WritePropertyName("Operation");
         // Use a switch statement to do the math.
         switch (op)
         {
             case "a":
                 result = num1 + num2;
                 writer.WriteValue("Add");
                 break;
             case "s":
                 result = num1 - num2;
                 writer.WriteValue("Subtract");
                 break;
             case "m":
                 result = num1 * num2;
                 writer.WriteValue("Multiply");
                 break;
             case "d":
                 // Ask the user to enter a non-zero divisor.
                 if (num2 != 0)
                 {
                     result = num1 / num2;
                 }
                 writer.WriteValue("Divide");
                 break;
             // Return text for an incorrect option entry.
             default:
                 break;
         }
         writer.WritePropertyName("Result");
         writer.WriteValue(result);
         writer.WriteEndObject();

         return result;
     }
     
    public void Finish()
    {
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Close();
    }

    private async Task<SpeechRecognitionResult> RecognizeOnceAsync(bool setInitialSilenceTimeout = false)
    {
        string keyFilePath = Path.Combine(AppContext.BaseDirectory, "speech-key.txt");
        if (!File.Exists(keyFilePath))
        {
            throw new InvalidOperationException($"Speech key file not found: {keyFilePath}");
        }

        string subscriptionKey = File.ReadAllText(keyFilePath).Trim();
        if (string.IsNullOrWhiteSpace(subscriptionKey) || subscriptionKey == "PASTE_YOUR_NEW_AZURE_SPEECH_KEY_HERE")
        {
            throw new InvalidOperationException("Put your Azure Speech key in Calculator/speech-key.txt.");
        }

        var config = SpeechConfig.FromSubscription(subscriptionKey, "eastus");
        if (setInitialSilenceTimeout)
        {
            config.SetProperty(PropertyId.SpeechServiceConnection_InitialSilenceTimeoutMs, "10000");
        }

        using var audioConfig = Microsoft.CognitiveServices.Speech.Audio.AudioConfig.FromDefaultMicrophoneInput();
        using var recognizer = new SpeechRecognizer(config, audioConfig);
        return await recognizer.RecognizeOnceAsync();
    }

    public async Task<double> GetNumber()
    {
        Console.WriteLine("Please say a number.");

        var result = await RecognizeOnceAsync();
        double number = 0;

        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            //check if the recognized text can be parsed to a double
            while (!double.TryParse(result.Text, out number))
            {
                Console.WriteLine("Please say a valid number.");
                result = await RecognizeOnceAsync();
            }
            Console.WriteLine($"Recognized: {result.Text}");
            return number;
        }
        else if (result.Reason == ResultReason.NoMatch)
        {
            var noMatch = NoMatchDetails.FromResult(result);
            Console.WriteLine($"NOMATCH: Speech could not be recognized. Reason: {noMatch.Reason}");
            return double.NaN;
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = CancellationDetails.FromResult(result);
            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
            }

            return double.NaN;
        }

        return double.NaN;
    }

    public async Task<string> GetOperationMethod()
    {
        Console.WriteLine("Please say an operation: add, subtract, multiply, or divide.");

        var result = await RecognizeOnceAsync();

        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            Console.WriteLine($"Recognized: {result.Text}");
            // You can process the recognized operation here
            
            return result.Text.ToLower(); // Return the recognized operation in lowercase
        }
        else if (result.Reason == ResultReason.NoMatch)
        {
            var noMatch = NoMatchDetails.FromResult(result);
            Console.WriteLine($"NOMATCH: Speech could not be recognized. Reason: {noMatch.Reason}");
            return string.Empty; // Return an empty string if no match
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = CancellationDetails.FromResult(result);
            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                Console.WriteLine($"CANCELED: Did you update the subscription info?");
            }
        }
        return string.Empty;
    }

    public async Task<string> ContinueOrNot()
    {
        var result = await RecognizeOnceAsync(setInitialSilenceTimeout: true);

        if (result.Reason == ResultReason.RecognizedSpeech)
        {
            // FIX: Trim spaces and strip out structural punctuation like periods or question marks
            string cleanedText = result.Text.Trim().TrimEnd('.', '?', '!').ToLower();
            
            Console.WriteLine($"Recognized: {cleanedText}");
            return cleanedText; 
        }
        else if (result.Reason == ResultReason.NoMatch)
        {
            var noMatch = NoMatchDetails.FromResult(result);
            Console.WriteLine($"NOMATCH: Speech could not be recognized. Reason: {noMatch.Reason}");
            return string.Empty; 
        }
        else if (result.Reason == ResultReason.Canceled)
        {
            var cancellation = CancellationDetails.FromResult(result);
            Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            if (cancellation.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
            }
        }
        return string.Empty;
    }
}
