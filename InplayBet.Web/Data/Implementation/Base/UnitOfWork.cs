
namespace InplayBet.Web.Data.Implementation.Base
{
    #region Required Namespace(s)
    using InplayBet.Web.Data.Context;
    using InplayBet.Web.Data.Interface.Base;
    using InplayBet.Web.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    #endregion

    public class UnitOfWork<TEntityModel> : IQueryableUnitOfWork
        where TEntityModel : DbContext, new()
    {
        private bool _IsDispose;
        private readonly TEntityModel _dbContext;
        private bool isAuditEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dispose.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is dispose; otherwise, <c>false</c>.
        /// </value>
        public bool IsDispose
        {
            get { return this._IsDispose; }
            set { _IsDispose = value; }
        }

        /// <summary>
        /// Gets the object context.
        /// </summary>
        /// <value>The object context.</value>
        public ObjectContext ObjectContext
        {
            get { return (this._dbContext as IObjectContextAdapter).ObjectContext; }
        }

        /// <summary>
        /// Gets the metadata workspace from context.
        /// </summary>
        /// <value>The metadata workspace from context.</value>
        public MetadataWorkspace MetadataWorkspaceFromContext
        {
            get { return this.ObjectContext.MetadataWorkspace; }
        }

        /// <summary>
        /// Gets or sets the is audit enabled.
        /// </summary>
        /// <value>The is audit enabled.</value>
        public bool IsAuditEnabled
        {
            get { return this.isAuditEnabled; }
            set { this.isAuditEnabled = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        public UnitOfWork()
        {
            this.IsDispose = false;
            this._dbContext = new TEntityModel();

            //Gets or sets a value indicating whether the DetectChanges() method is called automatically by methods of DbContext and related classes.
            //DetectChanges() method,detects changes made to the properties and relationships of POCO entities.
            //Normally DetectChanges is called automatically by many of the methods of DbContext and its related classes.
            //However, it may be desirable, usually for performance reasons, to turn off this automatic calling of DetectChanges using the AutoDetectChangesEnabled flag.
            //Which is done here.
            this._dbContext.Configuration.AutoDetectChangesEnabled = false;

            //Gets or sets a value indicating whether tracked entities should be validated automatically when SaveChanges() is invoked. The default value is true.
            //To make it false will help to increase performance for Insertion and Updation of bulk datas,since validation will not be tracked for each data of that list.
            this._dbContext.Configuration.ValidateOnSaveEnabled = false;
        }

        #region IQueryableUnitOfWork Members
        /// <summary>
        /// Creates the set.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : BaseData
        {
            return this._dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Gets the entry.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public DbEntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : BaseData
        {
            return this._dbContext.Entry<TEntity>(entity);
        }

        public void ChangeState<TEntity>(TEntity entity, System.Data.Entity.EntityState state) where TEntity : BaseData
        {
            this._dbContext.Entry<TEntity>(entity).State = state;
        }

        /// <summary>
        /// Applies the current values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="original">The original.</param>
        /// <param name="current">The current.</param>
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : BaseData
        {
            //if it is not attached, attach original and set current values
            this._dbContext.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public DbTransaction BeginTransaction()
        {
            var connection = this.ObjectContext.Connection;
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Commit all changes made in a container.
        /// </summary>
        /// <remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,
        /// then an exception is thrown
        /// </remarks>
        public void CommitChanges()
        {
            this._dbContext.SaveChanges();
        }

        /// <summary>
        /// Commit all changes made in  a container.
        /// </summary>
        /// <remarks>
        /// If the entity have fixed properties and any optimistic concurrency problem exists,
        /// then 'client changes' are refreshed - Client wins
        /// </remarks>
        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;
            do
            {
                try
                {
                    this._dbContext.SaveChanges();
                    saveFailed = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.ToList().ForEach(entry =>
                    {
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    });
                }
            } while (saveFailed);
        }

        /// <summary>
        /// Rollback tracked changes. See references of UnitOfWork pattern
        /// </summary>
        public void RollbackChanges()
        {
            this._dbContext.ChangeTracker.Entries().ToList()
                .ForEach(entry => entry.State = System.Data.Entity.EntityState.Unchanged);
        }

        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return this._dbContext.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        /// <summary>
        /// Execute arbitrary command into underlying persistence store
        /// </summary>
        /// <param name="sqlCommand">Command to execute
        /// <example>
        /// SELECT idCustomer,Name FROM dbo.[Customers] WHERE idCustomer &gt; {0}
        /// </example></param>
        /// <param name="parameters">A vector of parameters values</param>
        /// <returns>
        /// The number of affected records
        /// </returns>
        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return this._dbContext.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        #region Private Member
        private static bool IsChanged(DbEntityEntry entity)
        {
            return IsStateEqual(entity, System.Data.Entity.EntityState.Added) ||
                   IsStateEqual(entity, System.Data.Entity.EntityState.Deleted) ||
                   IsStateEqual(entity, System.Data.Entity.EntityState.Modified);
        }

        private static bool IsStateEqual(DbEntityEntry entity, System.Data.Entity.EntityState state)
        {
            return (entity.State & state) == state;
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (!this.IsDispose)
                {
                    if (disposing)
                    {
                        this._dbContext.Dispose();
                    }
                    this.IsDispose = true;
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(disposing);
            }
        }

        #endregion
        #endregion
    }
}