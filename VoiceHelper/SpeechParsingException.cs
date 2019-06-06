using System;

namespace VoiceHelper
{
    public class SpeechParsingException : Exception
    {
        public SpeechParsingException(string msg) : base(msg)
        {
            
        }
    }
}