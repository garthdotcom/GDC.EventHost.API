using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.Series.ValidationAttributes
{
    public class EndDateMustBeLaterThanStartDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var series = (SeriesForUpdateDto)validationContext.ObjectInstance;

            if (series.EndDate != null)
            {
                if (series.StartDate > series.EndDate)
                {
                    ErrorMessage = "If you enter an end date, it should be later than the start date.";
                    return new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
                }
            }

            return ValidationResult.Success;
        }
    }
}
