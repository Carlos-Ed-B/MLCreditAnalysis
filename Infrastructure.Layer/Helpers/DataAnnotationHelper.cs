using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Layer.Helpers
{
    public static class DataAnnotationHelper
    {
        public static bool ValidateDataAnnotation(object objectToValidate, out IList<ValidationResult> validationResults)
        {
            var context = new ValidationContext(objectToValidate, serviceProvider: null, items: null);

            validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(objectToValidate, context, validationResults, validateAllProperties: true);
        }

        public static IList<ValidationResult> GetValidationResult(object objectToValidate)
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            DataAnnotationHelper.ValidateDataAnnotation(objectToValidate, out validationResults);

            return validationResults;
        }
    }
}
