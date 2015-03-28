

namespace InplayBet.Web.Utilities
{
    using InplayBet.Web.Data.Implementation;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using SimpleInjector;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SharedFunctionality
    {
        private readonly IFollowDataRepository _followDataRepository;
        private readonly IUserDataRepository _userDataRepository;
        private readonly IBetDataRepository _betDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedFunctionality"/> class.
        /// </summary>
        public SharedFunctionality()
        {
            Container container = new Container();
            this._followDataRepository = container.GetInstance<FollowDataRepository>();
            this._userDataRepository = container.GetInstance<UserDataRepository>();
            this._betDataRepository = container.GetInstance<BetDataRepository>();
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
        /// Gets the following users.
        /// </summary>
        /// <param name="followedTo">The followed to.</param>
        /// <returns></returns>
        public List<UserModel> GetFollowingUsers(int followedTo)
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
                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(followedTo);
            }
            return new List<UserModel>();
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

        /// <summary>
        /// Masses the mailing.
        /// </summary>
        /// <param name="mailIds">The mail ids.</param>
        /// <param name="content">The content.</param>
        /// <param name="subjects">The subjects.</param>
        public void MassMailing(List<string> mailIds, string content, string subjects)
        {
            if (CommonUtility.GetConfigData<bool>("MAIL_ENABLE"))
            {
                System.Threading.Tasks.Parallel.ForEach(mailIds, x =>
                    {
                        EmailSender email = new EmailSender
                        {
                            To = x,
                            From = CommonUtility.GetConfigData<string>("MAIL_SENDER_UID"),
                            FromSenderName = CommonUtility.GetConfigData<string>("MAIL_SENDER_FROM"),
                            Subject = subjects
                        };
                        email.SendMailAsync(content);
                    }); 
            }
        }

        public int GetConsicutiveWinByUser(int userKey)
        {
            try
            {
                var rtn = _betDataRepository.GetConsicutiveBetWins(userKey);
                return rtn;
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return 0;
        }
    }
}