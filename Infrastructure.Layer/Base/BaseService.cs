using Infrastructure.Layer.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Layer.Base
{
    public class BaseService : BaseCommunicationMessage, IBaseService, IBaseCommunicationMessage
    {
        public virtual IList<ValidationResult> ValidationWithBusinessResults()
        {
            return this.ValidationResults;
        }

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
