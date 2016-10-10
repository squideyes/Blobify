namespace Blobify.Shared.Helpers
{
    public class ContainerNameAttribute : StringValidationAttribute
    {
        public ContainerNameAttribute(string argName = null)
            : base(argName)
        {
        }

        protected override bool IsValid(string value) =>
            value.IsContainerName();
    }
}
