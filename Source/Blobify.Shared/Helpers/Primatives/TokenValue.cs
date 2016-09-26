namespace Blobify.Shared.Helpers
{
    internal class TokenValue
    {
        public TokenValue(string chunk)
        {
            var index = chunk.IndexOf(':');

            if (index == -1)
            {
                Token = chunk.ToUpper();
            }
            else
            {
                Token = chunk.Substring(0, index).ToUpper();
                Value = chunk.Substring(index + 1).Trim();
            }
        }

        public string Token { get; }
        public string Value { get; }

        public bool HasValue
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Value);
            }
        }

        public override string ToString()
        {
            return $"{Token}={Value}";
        }
    }
}
