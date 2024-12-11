using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.Event.ValidationAttributes
{
    public class EndDateMustBeLaterThanStartDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                var theEvent = (EventForUpdateDto)validationContext.ObjectInstance;

                if (theEvent.StartDate == null && theEvent.StartDate == null)
                {
                    // Allow both to be null
                    return ValidationResult.Success;
                }

                if (theEvent.StartDate != null && theEvent.EndDate != null)
                {
                    if (theEvent.StartDate > theEvent.EndDate)
                    {
                        ErrorMessage = "The end date should be later than or the same as the start date.";
                    }
                    else if (theEvent.StartDate == theEvent.EndDate)
                    {
                        // Event takes place on one day, verify the times
                        if (theEvent.EndDate.Value.TimeOfDay < theEvent.StartDate.Value.TimeOfDay)
                        {
                            ErrorMessage = "The end time should be later than the start time.";
                        }
                    }
                }
                else
                {
                    // Either start date or end date is null; not allowed
                    ErrorMessage = "Please enter a valid date range for the event.";
                }

                return (string.IsNullOrEmpty(ErrorMessage)) 
                    ? ValidationResult.Success 
                    : new ValidationResult(ErrorMessage, [nameof(EventForUpdateDto)]);
            }
            catch 
            {
                ErrorMessage = "An error occurred validating event start and end dates.";
                return new ValidationResult(ErrorMessage, [nameof(EventForUpdateDto)]);
            }
        }
    }
}
