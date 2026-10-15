using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

public class Spechtext
{
    // Speech service API key
    readonly static string _speechKey = "YOUR-KEY";
    // Resource region (e.g. brazilsouth)    
    readonly static string _endpoint = "YOUR-REGION";

    public async static Task<SpeechRecognitionResult> OutputSpeech()
    {
        var speechConfig = SpeechConfig.FromEndpoint(new Uri(_endpoint), _speechKey);
        speechConfig.SpeechRecognitionLanguage = "en-US";

        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        var result = await speechRecognizer.RecognizeOnceAsync();

        return result;
    }
}