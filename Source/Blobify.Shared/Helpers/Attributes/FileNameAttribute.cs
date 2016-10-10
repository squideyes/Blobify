namespace Blobify.Shared.Helpers
{
    public class FileNameAttribute : StringValidationAttribute
    {
        private bool mustBeRooted;

        public FileNameAttribute(string argName = null, bool mustBeRooted = true)
            : base(argName)
        {
            this.mustBeRooted = mustBeRooted;
        }

        protected override bool IsValid(string value) =>
            value.IsFileName(mustBeRooted);
    }
}
