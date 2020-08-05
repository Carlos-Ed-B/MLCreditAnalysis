using Infrastructure.Layer.Base.Interfaces;
using Infrastructure.Layer.Extensions;
using Infrastructure.Layer.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infrastructure.Layer.Base
{
    public class BaseCommunicationMessage : IBaseCommunicationMessage
    {
        protected IList<ValidationResult> _validationResults = new List<ValidationResult>();

        public virtual IList<ValidationResult> ValidationResults
        {
            get { return this._validationResults; }
            set { this._validationResults = value; }
        }

        public string GetFirstError()
        {
            return this.ValidationResults.FirstOrDefault()?.ErrorMessage;
        }

        public string GetMessageByStatus()
        {
            if (this.IsValid())
            {
                return this.MessageSuccess;
            }

            return this.GetFirstError();
        }

        public void AddError(string message)
        {
            this.ValidationResults.Add(new ValidationResult(message));
        }

        public bool IsValidModel(object objectToValidate)
        {
            var errorResult = DataAnnotationHelper.GetValidationResult(objectToValidate);

            this.ValidationResults.AddRange(errorResult);

            return this.IsValid();
        }

        public bool IsValid()
        {
            return !this.ValidationResults.Any();
        }

        public string MessageSuccess { get; set; }
    }
}
