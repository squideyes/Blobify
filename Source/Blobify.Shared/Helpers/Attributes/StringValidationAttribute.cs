using System.ComponentModel.DataAnnotations;

namespace Blobify.Shared.Helpers
{
    public abstract class StringValidationAttribute : ValidationAttribute
    {
        private string argName;

        public StringValidationAttribute(string argName = null)
        {
            this.argName = argName;
        }

        protected abstract bool IsValid(string value);

        protected override ValidationResult IsValid(
            object value, ValidationContext context)
        {
            if (value == null)
                return ValidationResult.Success;

            var stringValue = value as string;

            if (IsValid(stringValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                if (argName == null)
                    argName = context.MemberName.ToUpper();

                return new ValidationResult(
                    $"The \"{stringValue}\" /{argName} is invalid.");
            }
        }
    }
}
