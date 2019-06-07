using System.Collections.Generic;
using System.Linq;

namespace VoiceHelper.VoiceHelper
{
    public class SpeechParser
    {
        private static readonly string[] HelpWords = {"me", "by"};
        private static readonly Dictionary<string, TokenType> TokenTypesByWords = new Dictionary<string, TokenType>
        {
            ["find"] = TokenType.FindRequest,
            ["find"] = TokenType.FindRequest,
            ["search"] = TokenType.FindRequest,
            ["show"] = TokenType.FindRequest,
            ["order"] = TokenType.SortBy,
            ["sort"] = TokenType.SortBy,
            ["then"] = TokenType.SortByNext,
            ["descending"] = TokenType.SortDesc
        };
        
        public List<Token> Parse(string speech)
        {
            var tokens = new List<Token>();
            var words = speech.Replace(",","").Replace(".","").Split(' ');

            foreach (var word in words)
            {
                if (HelpWords.Contains(word) && tokens.LastOrDefault()?.Type != TokenType.Text)
                {
                    continue;
                }
                var tokenType = TokenTypesByWords
                    .FirstOrDefault(t => StringsAreClose(t.Key, word))
                    .Value;
                
                tokens.Add(new Token
                {
                    Type = tokenType,
                    Value = word
                });
            }
            
            return tokens;
        }

        private static bool StringsAreClose(string s1, string s2)
        {
            var commonChars = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if (i < s2.Length && s1[i] == s2[i])
                {
                    commonChars++;
                }
            }

            return s1.Length - commonChars <= 2;
        }
    }
}