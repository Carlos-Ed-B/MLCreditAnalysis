using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Layer.Base.Interfaces
{
    public interface IBaseCommunicationMessage
    {
        IList<ValidationResult> ValidationResults { get; }
        bool IsValid();
        string GetFirstError();
        string GetMessageByStatus();
        void AddError(string message);
        public string MessageSuccess { get; set; }
    }
}
