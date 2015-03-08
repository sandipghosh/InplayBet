
namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Utilities;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Configuration;

    public class RegisterUserController : BaseController
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IBookMakerDataRepository _bookMakerDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserController"/> class.
        /// </summary>
        /// <param name="userDataRepository">The user data repository.</param>
        public RegisterUserController(IUserDataRepository userDataRepository,
            IBookMakerDataRepository bookMakerDataRepository)
        {
            this._userDataRepository = userDataRepository;
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
                    UserId = CommonUtility.GenarateRandomString(10, 10),
                    StatusId = (int)StatusCode.Inative,
                    CreatedOn = DateTime.Now,
                    CreatedBy = 1,
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
                    return View("Index", u);
                };
                if (ModelState.IsValid)
                {
                    if (IsUserEmailExists(user))
                    {
                        ModelState.AddModelError("EmailId", "This email id already exists. Please try a valid one.");
                        return process(user);
                    }
                    else
                    {
                        user.DateOfBirth = new DateTime(user.DobYear, user.DobMonth, user.DobDay);
                        this._userDataRepository.Insert(user);
                        SentConfirmationMail(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    return process(user);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
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
                    Value = x.ToString()
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
                    SSL = bool.Parse(ConfigurationManager.AppSettings["MAIL_SERVER_SSL"].ToString()),
                    Subject = "Inplay Bet Registration Confirmation",
                    To = user.EmailId
                };
                string mailBody = Utilities.CommonUtility.RenderViewToString
                    ("_RagistrationMailTemplate", user, this,
                    new Dictionary<string, object>() { { "ActivationUrl", "http://google.com" } });

                email.SendMailAsync(mailBody, () =>
                { });
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
                return (this._userDataRepository.GetCount(x => x.EmailId.Equals(emailId)) > 0);
            }
            catch (Exception ex)
            {
                ex.ExceptionValueTracker(user);
            }
            return false;
        }
    }
}
