
namespace InplayBet.Web.Data.Interface.Base
{
    #region Required Namespace(s)
    using InplayBet.Web.Models.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    #endregion

    public interface IRepository<TModel> where TModel : BaseModel
    {
        /// <summary>
        /// Get the unit of work in this repository
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">Entity key value</param>
        /// <returns></returns>        
        TModel Get(int id);

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">Entity key value</param>
        /// <returns></returns>
        TModel Get(Guid id);

        #region GetCount overloaded function
        /// <summary>
        /// Gets the count of an Entity.
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// Gets the count of an Entity.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        int GetCount(Expression<Func<TModel, bool>> filter);

        #endregion

        #region GetList overloaded functions
        /// <summary>
        /// Get all elements of type TEntity in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IQueryable<TModel> GetList();

        /// <summary>
        /// Get  elements of type TEntity in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IQueryable<TModel> GetList(Expression<Func<TModel, bool>> filter);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IQueryable<TModel> GetList(int pageIndex, int pageCount, Expression<Func<TModel, bool>> filter);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">The ascending.</param>
        /// <returns></returns>
        IQueryable<TModel> GetList<KProperty>(Expression<Func<TModel, bool>> filter,
            Expression<Func<TModel, KProperty>> orderByExpression, bool ascending);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageCount">The page count.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <param name="ascending">The ascending.</param>
        /// <returns></returns>
        IQueryable<TModel> GetList<KProperty>(int pageIndex, int pageCount, Expression<Func<TModel, bool>> filter,
            Expression<Func<TModel, KProperty>> orderByExpression, bool ascending);

        /// <summary>
        /// Get all elements of type TEntity in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IQueryable<TModel> GetList<KProperty>(int pageIndex, int pageCount,
            Expression<Func<TModel, KProperty>> orderByExpression, bool ascending);
        #endregion

        /// <summary>
        /// Existses the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        bool Exists(Expression<Func<TModel, bool>> filter);

        double Sum(Expression<Func<TModel, bool>> filter, Expression<Func<TModel, double>> sumExpression);

        #region Insert/Update/Delete operation
        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Insert(TModel item, bool doSaveChanges = true);

        /// <summary>
        /// Adds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        void Insert(IEnumerable<TModel> items);

        /// <summary>
        /// Set item as modified
        /// </summary>
        /// <param name="item">Item to modify</param>
        void Update(TModel item, bool doSaveChanges = true);

        /// <summary>
        /// Modifies the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        void Update(IEnumerable<TModel> items);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Delete(TModel item, bool doSaveChanges = true);

        /// <summary>
        /// Removes the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        void Delete(IEnumerable<TModel> items);
        #endregion

        /// <summary>
        ///Track entity into this repository, really in UnitOfWork. 
        ///In EF this can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void TrackItem(TModel item);

        /// <summary>
        /// Sets modified entity into the repository. 
        /// When calling Commit() method in UnitOfWork 
        /// these changes will be saved into the storage
        /// </summary>
        /// <param name="persisted">The persisted item</param>
        /// <param name="current">The current item</param>
        void Merge(TModel persisted, TModel current);
    }
}
