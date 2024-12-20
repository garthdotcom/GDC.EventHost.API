﻿using System.ComponentModel.DataAnnotations;

namespace GDC.EventHost.DTO.Series.ValidationAttributes
{
    public class EndDateMustBeLaterThanStartDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                var series = (SeriesForUpdateDto)validationContext.ObjectInstance;

                if (series.StartDate == null && series.StartDate == null)
                {
                    // Allow both to be null
                    return ValidationResult.Success;
                }

                if (series.StartDate != null && series.EndDate != null)
                {
                    if (series.StartDate > series.EndDate)
                    {
                        ErrorMessage = "The end date should be later than or the same as the start date.";
                    }
                    else if (series.StartDate == series.EndDate)
                    {
                        // Series takes place on one day, verify the times
                        if (series.EndDate.Value.TimeOfDay < series.StartDate.Value.TimeOfDay)
                        {
                            ErrorMessage = "The end time should be later than the start time.";
                        }
                    }
                }
                else
                {
                    // Either start date or end date is null; not allowed
                    ErrorMessage = "Please enter a valid date range for the series.";
                }

                return (string.IsNullOrEmpty(ErrorMessage)) 
                    ? ValidationResult.Success 
                    : new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
            }
            catch 
            {
                ErrorMessage = "An error occurred validating series start and end dates.";
                return new ValidationResult(ErrorMessage, [nameof(SeriesForUpdateDto)]);
            }
        }
    }
}
