namespace Blobify.Shared.Helpers
{
    public class RegexPatternAttribute : StringValidationAttribute
    {
        public RegexPatternAttribute(string argName = null)
            : base(argName)
        {
        }

        protected override bool IsValid(string value) =>
            value.IsRegexPattern();
    }
}
