using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Layer.Base.Interfaces
{
    public interface IBaseService: IDisposable
    {
        IList<ValidationResult> ValidationResults { get; }
    }
}
