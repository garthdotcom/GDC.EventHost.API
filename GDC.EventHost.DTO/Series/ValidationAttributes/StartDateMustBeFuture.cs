﻿using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.Series.ValidationAttributes
{
    public class StartDateMustBeFuture : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                var seriesDto = (SeriesForUpdateDto)validationContext.ObjectInstance;

                if (!seriesDto.StartDate.HasValue)
                {
                    // Allow null
                    return ValidationResult.Success;
                }

                bool dateIsValid = DateTime.TryParse(seriesDto.StartDate.ToString(), out DateTime parsedDate);

                if (!dateIsValid)
                {
                    ErrorMessage = "You should enter a valid date for your series' start date.";
                }
                else
                {
                    if (parsedDate < DateTime.Now)
                    {
                        ErrorMessage = "You should enter a series start date and time that is in the future.";
                    }
                }

                return (string.IsNullOrEmpty(ErrorMessage))
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
            }
            catch
            {
                ErrorMessage = "An error occurred validating series start date.";
                return new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
            }
        }
    }
}
