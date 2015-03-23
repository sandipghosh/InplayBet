

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Utilities;
    using InplayBet.Web.Models.Base;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Collections.Generic;

    public class FollowController : Controller
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IFollowDataRepository _followDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowController"/> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        /// <param name="followDataRepository">The follow data repository.</param>
        public FollowController(IUserDataRepository userDataRepository,
            IFollowDataRepository followDataRepository)
        {
            this._userDataRepository = userDataRepository;
            this._followDataRepository = followDataRepository;
        }

        /// <summary>
        /// Sets the specified follow by.
        /// </summary>
        /// <param name="followBy">The follow by.</param>
        /// <param name="followTo">The follow to.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Set(int followBy, int followTo)
        {
            try
            {
                SharedFunctionality shared = new SharedFunctionality();
                return Json(new { followCount = shared.AddFollowers(followBy, followTo) },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(followBy, followTo);
            }
            return null;
        }

        /// <summary>
        /// Shows the following users.
        /// </summary>
        /// <param name="followedTo">The followed by.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ShowFollowingUsers(int followedTo)
        {
            try
            {
                var followers = this._followDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                    && x.FollowTo.Equals(followedTo)).ToList();
                if (!followers.IsEmptyCollection())
                {
                    List<int> ids = followers.Select(y => y.FollowBy).ToList();
                    var users = this._userDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                        && ids.Contains(x.UserKey)).ToList();

                    if (!users.IsEmptyCollection())
                    {
                        ViewBag.FollowTo = _userDataRepository.Get(followedTo).UserId;
                        return PartialView(users);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(followedTo);
            }
            return null;
        }
    }
}
