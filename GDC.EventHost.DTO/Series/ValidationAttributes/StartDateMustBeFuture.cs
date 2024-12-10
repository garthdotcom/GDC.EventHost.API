using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.Series.ValidationAttributes
{
    public class StartDateMustBeFuture : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var seriesDto = (SeriesForUpdateDto)validationContext.ObjectInstance;

            bool dateIsValid = DateTime.TryParse(seriesDto.StartDate.ToString(), out DateTime parsedDate);

            if (!dateIsValid)
            {
                ErrorMessage = "You should enter a valid date for your start date.";
                return new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
            }

            if (parsedDate < DateTime.Now)
            {
                ErrorMessage = "You should enter a start date that is in the future.";
                return new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
            }

            return ValidationResult.Success;
        }
    }
}
