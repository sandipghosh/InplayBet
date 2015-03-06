
namespace InplayBet.Web.Data.Interface.Base
{
    #region Required Namespace(s)
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    #endregion

    /// <summary>
    /// The UnitOfWork contract for EF implementation
    /// <remarks>
    /// This contract extend IUnitOfWork for use with EF code
    /// </remarks>
    /// </summary>
    public interface IQueryableUnitOfWork : IUnitOfWork, ISql
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is dispose.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dispose; otherwise, <c>false</c>.
        /// </value>
        bool IsDispose { get; set; }

        /// <summary>
        /// Creates the set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets the entry.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        DbEntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Sets the unchanged.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void SetUnchanged<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Sets the inserted.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void SetInserted<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Sets the modified.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void SetModified<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Sets the deleted.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="item">The item.</param>
        void SetDeleted<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Applies the current values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : class;

        /// <summary>
        /// Gets the metadata workspace from context.
        /// </summary>
        /// <returns></returns>
        MetadataWorkspace GetMetadataWorkspaceFromContext();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();
    }
}
