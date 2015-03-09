
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

    public class TeamController : BaseController
    {
        private readonly ITeamDataRepository _teamDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamController"/> class.
        /// </summary>
        /// <param name="teamDataRepository">The team data repository.</param>
        public TeamController(ITeamDataRepository teamDataRepository)
        {
            this._teamDataRepository = teamDataRepository;
        }

        /// <summary>
        /// Gets the teams.
        /// </summary>
        /// <returns></returns>
        public JsonActionResult GetTeams()
        {
            try
            {
                var teams = this._teamDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active))
                    .OrderBy(x => x.TeamName).Select(x => new { Id = x.TeamId, Name = x.TeamName });
                return new JsonActionResult(teams);
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
        /// <param name="teamName">Name of the team.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetTeams(string teamName, int userId)
        {
            try
            {
                if (!this._teamDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                    && x.TeamName.Equals(teamName)).Any())
                {
                    this._teamDataRepository.Insert(new TeamModel
                    {
                        TeamName = teamName,
                        StatusId = (int)StatusCode.Active,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    });
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(teamName);
            }
            return null;
        }
    }
}
