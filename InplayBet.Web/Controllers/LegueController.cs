
namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    public class LegueController : BaseController
    {
        private readonly ILegueDataRepository _legueDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegueController"/> class.
        /// </summary>
        /// <param name="legueDataRepository">The legue data repository.</param>
        public LegueController(ILegueDataRepository legueDataRepository)
        {
            this._legueDataRepository = legueDataRepository;
        }

        /// <summary>
        /// Gets the legues.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public JsonActionResult GetLegues(string filter)
        {
            try
            {
                var legues = this._legueDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                    && x.LegueName.StartsWith(filter), x => x.LegueName, true)
                    .Select(x => new { label = x.LegueName, value = x.LegueId });

                return new JsonActionResult(legues.ToList());
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(filter);
            }
            return null;
        }

        /// <summary>
        /// Sets the teams.
        /// </summary>
        /// <param name="name">Name of the legue.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult SetLegues(string searchName, int userId)
        {
            try
            {
                LegueModel legue;
                Expression<Func<LegueModel, bool>> criteria = (x) => x.StatusId.Equals((int)StatusCode.Active)
                    && x.LegueName.Equals(searchName);

                if (!this._legueDataRepository.Exists(criteria))
                {
                    legue = new LegueModel
                    {
                        LegueName = searchName,
                        StatusId = (int)StatusCode.Active,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    };
                    this._legueDataRepository.Insert(legue);
                }
                else
                {
                    legue = this._legueDataRepository.GetList(criteria).FirstOrDefault();
                }
                return new JsonActionResult(new { label = legue.LegueName, value = legue.LegueId });
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(searchName, userId);
            }
            return null;
        }
    }
}
