namespace Blobify.Shared.Helpers
{
    public class LocalPathAttribute : StringValidationAttribute
    {
        public LocalPathAttribute(string argName = null)
            : base(argName)
        {
        }

        protected override bool IsValid(string value) =>
            value.IsLocalPath();
    }
}
