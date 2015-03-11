
namespace InplayBet.Web.Data.Interface.Base
{
    #region Required Namespace(s)
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using InplayBet.Web.Data.Context;
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
        /// Gets the object context.
        /// </summary>
        /// <value>The object context.</value>
        ObjectContext ObjectContext { get; }

        /// <summary>
        /// Gets the metadata workspace from context.
        /// </summary>
        /// <value>The metadata workspace from context.</value>
        MetadataWorkspace MetadataWorkspaceFromContext { get; }

        /// <summary>
        /// Gets or sets the is audit enabled.
        /// </summary>
        /// <value>The is audit enabled.</value>
        bool IsAuditEnabled { get; set; }

        /// <summary>
        /// Creates the set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : BaseData;

        /// <summary>
        /// Gets the entry.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        DbEntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : BaseData;

        /// <summary>
        /// Changes the state.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="state">The state.</param>
        void ChangeState<TEntity>(TEntity entity, EntityState state) where TEntity : BaseData;

        /// <summary>
        /// Applies the current values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : BaseData;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();
    }
}
