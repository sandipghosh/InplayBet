
namespace InplayBet.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;

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
        public JsonActionResult GetLegues()
        {
            try
            {
                var legues = this._legueDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active))
                    .OrderBy(x => x.LegueName).Select(x => new { Id = x.LegueId, Name = x.LegueName });
                return new JsonActionResult(legues.ToList());
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        /// <summary>
        /// Sets the teams.
        /// </summary>
        /// <param name="name">Name of the legue.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetLegues(string name, int userId)
        {
            try
            {
                LegueModel legue;
                Expression<Func<LegueModel, bool>> criteria = (x) => x.StatusId.Equals((int)StatusCode.Active)
                    && x.LegueName.Equals(name);

                if (!this._legueDataRepository.Exists(criteria))
                {
                    legue = new LegueModel
                    {
                        LegueName = name,
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
                return new JsonActionResult(new { Id = legue.LegueId, Name = legue.LegueName });
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(name);
            }
            return null;
        }
    }
}
