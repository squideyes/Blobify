namespace Blobify.Shared.Helpers
{
    public class ConnStringAttribute : StringValidationAttribute
    {
        public ConnStringAttribute(string argName = null)
            : base(argName)
        {
        }

        protected override bool IsValid(string value) =>
            value.IsConnString();
    }
}
