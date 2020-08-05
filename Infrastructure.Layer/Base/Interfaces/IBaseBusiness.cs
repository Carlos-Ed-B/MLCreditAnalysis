using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Layer.Base.Interfaces
{
    public interface IBaseBusiness
    {
        IList<ValidationResult> ValidationResults { get; }
        bool IsValid();
        string GetFirstError();
        void AddError(string message);
    }
}
