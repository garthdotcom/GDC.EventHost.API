using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.Event.ValidationAttributes
{
    public class StartDateMustBeFuture : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                var theEventDto = (EventForUpdateDto)validationContext.ObjectInstance;

                if (!theEventDto.StartDate.HasValue)
                {
                    // Allow null
                    return ValidationResult.Success;
                }

                bool dateIsValid = DateTime.TryParse(theEventDto.StartDate.ToString(), out DateTime parsedDate);

                if (!dateIsValid)
                {
                    ErrorMessage = "You should enter a valid date for your event's start date.";
                }
                else
                {
                    if (parsedDate < DateTime.Now)
                    {
                        ErrorMessage = "You should enter an event start date and time that is in the future.";
                    }
                }

                return (string.IsNullOrEmpty(ErrorMessage))
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage, [nameof(EventForUpdateDto)]);
            }
            catch
            {
                ErrorMessage = "An error occurred validating the event start date.";
                return new ValidationResult(ErrorMessage, [nameof(EventForUpdateDto)]);
            }
        }
    }
}
