namespace Blobify.Shared.Helpers
{
    public class FolderNameAttribute : StringValidationAttribute
    {
        public FolderNameAttribute(string argName = null)
            : base(argName)
        {
        }

        protected override bool IsValid(string value) =>
            value.IsFolderName(false);
    }
}
