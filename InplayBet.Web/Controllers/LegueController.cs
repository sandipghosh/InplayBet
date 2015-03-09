
namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using System;
    using System.Linq;
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
        public JsonActionResult GetLegues()
        {
            try
            {
                var legues = this._legueDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active))
                    .OrderBy(x => x.LegueName).Select(x => new { Id = x.LegueId, Name = x.LegueName });
                return new JsonActionResult(legues);
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
        /// <param name="legueName">Name of the legue.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetLegues(string legueName, int userId)
        {
            try
            {
                if (!this._legueDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                    && x.LegueName.Equals(legueName)).Any())
                {
                    this._legueDataRepository.Insert(new LegueModel
                    {
                        LegueName = legueName,
                        StatusId = (int)StatusCode.Active,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(legueName);
            }
            return null;
        }
    }
}
