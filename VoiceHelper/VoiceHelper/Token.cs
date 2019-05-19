namespace VoiceHelper.VoiceHelper
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
    }

    public enum TokenType
    {
        Text, FindRequest, SortBy, Category
    }
}