
namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class RegisterUserController : BaseController
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly ICurrencyDataRepository _currencyDataRepository;
        private readonly IBookMakerDataRepository _bookMakerDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserController"/> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        public RegisterUserController(IUserDataRepository userDataRepository,
            ICurrencyDataRepository currencyDataRepository,
            IBookMakerDataRepository bookMakerDataRepository)
        {
            this._userDataRepository = userDataRepository;
            this._currencyDataRepository = currencyDataRepository;
            this._bookMakerDataRepository = bookMakerDataRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            try
            {
                GenerateAdditionalData();
                UserModel user = new UserModel()
                {
                    //UserId = CommonUtility.GenarateRandomString(10, 10),
                    StatusId = (int)StatusCode.Inative,
                    CreatedOn = DateTime.Now,
                    CreatedBy = 1,
                    IsAdmin = false,
                    AvatarPath = @"~/Images/Users/Default.jpg"
                };

                return View(user);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        /// <summary>
        /// Updates the specified user key.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Update(int userKey)
        {
            try
            {
                UserModel user = this._userDataRepository.Get(userKey);
                if (user != null)
                {
                    user.DobDay = user.DateOfBirth.Day;
                    user.DobMonth = user.DateOfBirth.Month;
                    user.DobYear = user.DateOfBirth.Year;
                    user.Password = string.Empty;
                    user.AvatarPath = CommonUtility.SaveImageFromDataUrl(user.AvatarPath, user.UserKey, user.UserId);

                    GenerateAdditionalData();
                    return View("Index", user);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(userKey);
            }
            return null;
        }

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult SignUp(UserModel user)
        {
            try
            {
                Func<UserModel, ActionResult> process = (u) =>
                {
                    GenerateAdditionalData();
                    return PartialView("_SignUp", u);
                };
                if (ModelState.IsValid)
                {
                    if (user.UserKey > 0)
                    {
                        return UpdateProfile(user);
                    }
                    else
                    {
                        if (IsUserEmailExists(user))
                        {
                            ModelState.AddModelError("EmailId", "This email id already exists. Please try a valid one.");
                            return process(user);
                        }
                        if (IsUserIdExists(user))
                        {
                            ModelState.AddModelError("UserId", "This user id already exists. Please try a valid one.");
                            return process(user);
                        }
                        else
                        {
                            user.DateOfBirth = new DateTime(user.DobYear, user.DobMonth, user.DobDay);
                            user.AvatarPath = CommonUtility.SaveImageFromDataUrl(user.AvatarPath, user.UserKey, user.UserId);

                            if (CommonUtility.GetConfigData<string>("EnableUserMailActivation").AsString() == "true")
                            {
                                this._userDataRepository.Insert(user);
                                SentConfirmationMail(user);
                            }
                            else
                            {
                                user.StatusId = (int)StatusCode.Active;
                                this._userDataRepository.Insert(user);
                                if (user.UserKey > 0)
                                {
                                    SetSessionData(user);
                                }
                            }
                            return new JsonActionResult(user);
                        }
                    }
                }
                else
                {
                    GenerateAdditionalData();
                    return View("_SignUp", user);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
            }
            return null;
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="userkey">The userkey.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ActivateUser(string timestamp, int userkey)
        {
            try
            {
                DateTime startDate = new DateTime(long.Parse(timestamp));
                var hours = (DateTime.Now - startDate);
                if (hours.TotalHours <= 48)
                {
                    UserModel user = this._userDataRepository.Get(userkey);
                    if (user != null)
                    {
                        user.StatusId = (int)StatusCode.Active;
                        SetSessionData(user);
                        return RedirectToAction("Index", "MemberProfile");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(timestamp, userkey);
            }
            return null;
        }

        /// <summary>
        /// Signs the in.
        /// </summary>
        /// <param name="signIn">The sign in.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult SignIn(SignInModel signIn)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel user = this._userDataRepository.GetList
                        (x => (x.UserId.Equals(signIn.UserOrEmail) || x.EmailId.Equals(signIn.UserOrEmail))
                        && x.Password.Equals(signIn.Password) && x.StatusId.Equals((int)StatusCode.Active))
                        .FirstOrDefaultCustom();

                    if (user != null)
                    {
                        SetSessionData(user);
                        return new JsonActionResult(new { Status = true, Url = Url.Action("Index", "MemberProfile") });
                    }
                    else
                    {
                        ModelState.AddModelError("UserOrEmail", "User Id, Email, Password is invalid");
                        return PartialView("_SignIn", signIn);
                    }
                }
                else
                {
                    return PartialView("_SignIn", signIn);
                }
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(signIn);
            }
            return new JsonActionResult(new { Status = false });
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult SignOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult ShowImageCropper()
        {
            try
            {
                return PartialView("_ImageCroper");
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
            return null;
        }

        /// <summary>
        /// Generates the additional data.
        /// </summary>
        private void GenerateAdditionalData()
        {
            try
            {
                ViewBag.Months = Enumerable.Range(1, 12).Select(x => new SelectListItem()
                {
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x),
                    Value = x.ToString(),
                });
                ViewBag.Days = Enumerable.Range(1, 31).Select(x => new SelectListItem()
                {
                    Text = x.ToString("00"),
                    Value = x.ToString()
                });
                ViewBag.Years = Enumerable.Range(DateTime.Now.Year - 100, 82)
                    .OrderByDescending(x => x)
                    .Select(x => new SelectListItem()
                    {
                        Text = x.ToString("0000"),
                        Value = x.ToString()
                    });
                ViewBag.Sex = (new string[] { "Male", "Female" }).Select(x => new SelectListItem()
                {
                    Text = x,
                    Value = x.First().ToString()
                });
                ViewBag.Currencies = this._currencyDataRepository
                    .GetList(x => x.StatusId.Equals((int)StatusCode.Active)).ToList()
                    .Select(y => new SelectListItem()
                    {
                        Text = string.Format("{0} ({1})", y.CurrencyName, y.CurrencySymbol),
                        Value = y.CurrencyId.ToString()
                    });
                ViewBag.BookMakers = this._bookMakerDataRepository
                    .GetList(x => x.StatusId.Equals((int)StatusCode.Active))
                    .Select(y => new SelectListItem()
                    {
                        Text = y.BookMakerName,
                        Value = y.BookMakerId.ToString()
                    });
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker();
            }
        }

        /// <summary>
        /// Sents the confirmation mail.
        /// </summary>
        /// <param name="user">The user.</param>
        private void SentConfirmationMail(UserModel user)
        {
            try
            {
                EmailSender email = new EmailSender
                {
                    SSL = bool.Parse(CommonUtility.GetConfigData<string>("MAIL_SERVER_SSL")),
                    Subject = "Inplay Bet Registration Confirmation",
                    To = user.EmailId
                };

                var url = Url.Action("ActivateUser", "RegisterUser", new { area = "", timestamp = DateTime.Now.Ticks.ToString(), userid = user.UserKey });

                string mailBody = Utilities.CommonUtility.RenderViewToString
                    ("_RagistrationMailTemplate", user, this,
                    new Dictionary<string, object>() { { "ActivationUrl", url } });

                email.SendMail(mailBody);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
            }
        }

        /// <summary>
        /// Determines whether [is user email exists] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private bool IsUserEmailExists(UserModel user)
        {
            try
            {
                string emailId = user.EmailId;
                return this._userDataRepository.Exists(x => x.EmailId.Equals(emailId));
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is user identifier exists] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private bool IsUserIdExists(UserModel user)
        {
            try
            {
                string userId = user.UserId;
                return this._userDataRepository.Exists(x => x.EmailId.Equals(userId));
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
            }
            return false;
        }

        /// <summary>
        /// Sets the session data.
        /// </summary>
        /// <param name="user">The user.</param>
        private void SetSessionData(UserModel user)
        {
            SessionVeriables.SetSessionData<int>(SessionVeriables.UserKey, user.UserKey);
            SessionVeriables.SetSessionData<string>(SessionVeriables.UserId, user.UserId);
            SessionVeriables.SetSessionData<string>(SessionVeriables.UserName,
                string.Format("{0} {1}", user.FirstName, user.LastName));
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private ActionResult UpdateProfile(UserModel user)
        {
            try
            {
                user.UpdatedBy = user.UserKey;
                user.UpdatedOn = DateTime.Now;
                user.DateOfBirth = new DateTime(user.DobYear, user.DobMonth, user.DobDay);
                user.AvatarPath = CommonUtility.SaveImageFromDataUrl(user.AvatarPath, user.UserKey, user.UserId);
                this._userDataRepository.Update(user);
                return new JsonActionResult(user);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
            }
            return null;
        }
    }
}
