
namespace InplayBet.Web.Models.Base
{
    using System;

    public abstract class SafeDisposableBaseClass : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Construct
        /// </summary>
        protected SafeDisposableBaseClass()
        {
        }

        #endregion

        #region IDisposable Related Functions

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Function to override in order to dispose objects
        /// </summary>
        /// <param name="Managed">If true, managed and unmanaged objects should be disposed. Otherwise unmanaged objects only.</param>
        protected abstract void Dispose(bool Managed);

        #endregion

        #region Finalizer

        /// <summary>
        /// Destructor
        /// </summary>
        ~SafeDisposableBaseClass()
        {
            Dispose(false);
        }

        #endregion
    }
}