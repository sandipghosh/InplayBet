
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
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public JsonActionResult GetTeams(string filter)
        {
            try
            {
                var teams = this._teamDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                    && x.TeamName.StartsWith(filter), x => x.TeamName, true)
                    .Select(x => new { label = x.TeamName, value = x.TeamId });
                return new JsonActionResult(teams.ToList());
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
        /// <param name="searchName">Name of the team.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult SetTeams(string searchName, int userId)
        {
            try
            {
                TeamModel team;
                Expression<Func<TeamModel, bool>> criteria = (x) => x.StatusId.Equals((int)StatusCode.Active)
                    && x.TeamName.Equals(searchName);

                if (!this._teamDataRepository.Exists(criteria))
                {
                    team = new TeamModel
                    {
                        TeamName = searchName,
                        StatusId = (int)StatusCode.Active,
                        CreatedBy = userId,
                        CreatedOn = DateTime.Now
                    };
                    this._teamDataRepository.Insert(team);
                }
                else
                {
                    team = this._teamDataRepository.GetList(criteria).FirstOrDefault();
                }
                return new JsonActionResult(new { label = team.TeamName, value = team.TeamId });
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(searchName);
            }
            return null;
        }
    }
}
