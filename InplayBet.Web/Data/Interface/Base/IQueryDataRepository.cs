
namespace InplayBet.Web.Data.Interface.Base
{
    #region Required Namespace(s)
    using InplayBet.Web.Models.Base;
    using System.Collections.Generic;
    #endregion

    public interface IQueryDataRepository
    {
        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="sqlQuery">The SQL query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        IEnumerable<TModel> ExecuteQuery<TModel>(string sqlQuery, params object[] parameters)
             where TModel : BaseModel;

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteCommand(string sqlCommand, params object[] parameters);
    }
}
