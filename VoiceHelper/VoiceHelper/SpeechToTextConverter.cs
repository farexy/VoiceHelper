using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using NAudio.Wave;

namespace VoiceHelper.VoiceHelper
{
    public class SpeechToTextConverter
    {
        private readonly string _subscriptionKey;
        private readonly string _region;

        public SpeechToTextConverter(string subscriptionKey, string region)
        {
            _subscriptionKey = subscriptionKey;
            _region = region;
        }

        public async Task<string> ConvertAsync(string fileName)
        {
            var outputFile = fileName.Replace("blob.mp3", "out.wav");
            using(var reader = new Mp3FileReader(fileName))
            {
                WaveFileWriter.CreateWaveFile(outputFile, reader);
            }
//            using(var inputStream = File.OpenRead(fileName))
//            using (var reader = new WaveFileReader(inputStream))
//            {
//                var newFormat = new WaveFormat(16000, 16, 1); 
//                using (var conversionStream = new WaveFormatConversionStream(newFormat, reader))
//                {
//                    WaveFileWriter.CreateWaveFile(outputFile, conversionStream);
//                } 
//            }
            
            using (var inputSpeech = AudioConfig.FromWavFileInput(outputFile))
            using (var recognizer = new SpeechRecognizer(
                SpeechConfig.FromSubscription(_subscriptionKey, _region),
                inputSpeech))
            {
                string result = string.Empty;
                var stopRecognition = new TaskCompletionSource<int>();
                recognizer.Recognizing += (s, e) =>
                {
                    result = e.Result.Text;
                };
                recognizer.Recognized += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        result = e.Result.Text;
                    }
                    else if (e.Result.Reason == ResultReason.NoMatch)
                    {
                        result = "Error";
                    }
                };
                recognizer.Canceled += (s, e) =>
                {
                    result = "Error" + e.Reason;
                    if (e.Reason == CancellationReason.Error)
                    {
                        result = "ErrorCode" + e.ErrorCode + e.ErrorDetails;
                    }
                    stopRecognition.TrySetResult(0);
                };
                recognizer.SessionStarted += (s, e) =>
                {
                };
                recognizer.SessionStopped += (s, e) =>
                {
                    result = "Session stopped event";
                    stopRecognition.TrySetResult(0);
                };
                await recognizer.RecognizeOnceAsync();
                
                return result;
            }
        }
    }
}