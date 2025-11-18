using System.ComponentModel.DataAnnotations;

namespace SimpleLinkShrinkLibrary.Web.SharedRazorClassLibrary.Attributes.Validation
{
    public class HttpUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var inputText = value as string;

            // todo: is this part necessary?
            if (string.IsNullOrWhiteSpace(inputText))
                return new ValidationResult("This field is required.");

            if (!Uri.TryCreate(inputText, UriKind.Absolute, out var parsedUri))
                return new ValidationResult("Invalid input. The supplied value isn't a valid url.");

            if (parsedUri.Scheme != Uri.UriSchemeHttp && parsedUri.Scheme != Uri.UriSchemeHttps)
                return new ValidationResult("Invalid input. The supplied value isn't a http / https url.");

            return ValidationResult.Success;
        }
    }
}
