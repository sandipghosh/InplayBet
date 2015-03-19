

namespace InplayBet.Web.Utilities
{
    using System;
    using InplayBet.Web.Data.Implementation;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Models;
    using SimpleInjector;

    public class SharedFunctionality
    {
        private readonly IFollowDataRepository _followDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedFunctionality"/> class.
        /// </summary>
        public SharedFunctionality()
        {
            Container container = new Container();
            this._followDataRepository = container.GetInstance<FollowDataRepository>();
        }

        /// <summary>
        /// Gets the followers.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        public int GetFollowers(int userKey)
        {
            try
            {
                return this._followDataRepository.GetCount(x => x.FollowTo.Equals(userKey)
                    && x.StatusId.Equals((int)StatusCode.Active));
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return 0;
        }

        /// <summary>
        /// Adds the followers.
        /// </summary>
        /// <param name="followBy">The follow by.</param>
        /// <param name="followTo">The follow to.</param>
        /// <returns></returns>
        public int AddFollowers(int followBy, int followTo)
        {
            try
            {
                if (!this.IsFollowing(followBy, followTo))
                {
                    FollowModel follow = new FollowModel
                    {
                        FollowBy = followBy,
                        FollowTo = followTo,
                        StatusId = (int)StatusCode.Active,
                        CreatedBy = followBy,
                        CreatedOn = DateTime.Now
                    };
                    this._followDataRepository.Insert(follow);
                    if (follow.FollowId > 1)
                    {
                        return GetFollowers(followTo);
                    }
                }
                else
                {
                    FollowModel follow = this._followDataRepository.GetList(x => x.StatusId.Equals((int)StatusCode.Active)
                        && x.FollowBy.Equals(followBy) && x.FollowTo.Equals(followTo)).FirstOrDefaultCustom();

                    if (follow != null)
                    {
                        follow.StatusId = (int)StatusCode.Inative;
                        follow.UpdatedBy = followBy;
                        follow.UpdatedOn = DateTime.Now;
                        this._followDataRepository.Update(follow);
                        return GetFollowers(followTo);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(followBy, followTo);
            }
            return 0;
        }

        /// <summary>
        /// Determines whether the specified follow by is following.
        /// </summary>
        /// <param name="followBy">The follow by.</param>
        /// <param name="followTo">The follow to.</param>
        /// <returns></returns>
        public bool IsFollowing(int followBy, int followTo)
        {
            try
            {
                return this._followDataRepository.Exists(x => x.FollowTo.Equals(followTo)
                    && x.FollowBy.Equals(followBy) && x.StatusId.Equals((int)StatusCode.Active));
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(followBy, followTo);
            }
            return false;
        }
    }
}