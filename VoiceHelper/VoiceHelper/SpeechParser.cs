using System.Collections.Generic;

namespace VoiceHelper.VoiceHelper
{
    public class SpeechParser
    {
        public List<Token> Parse(string speech)
        {
            var tokens = new List<Token>();
            var words = speech.Split(' ');
            
            return tokens;
        }
    }
}