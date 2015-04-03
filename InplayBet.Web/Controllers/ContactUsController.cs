

namespace InplayBet.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Utilities;

    public class ContactUsController : BaseController
    {
        private readonly IContactDataRepository _contactDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactUsController" /> class.
        /// </summary>
        /// <param name="contactDataRepository">The contact data repository.</param>
        public ContactUsController(IContactDataRepository contactDataRepository)
        {
            this._contactDataRepository = contactDataRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult Index()
        {
            ContactModel contact = new ContactModel()
            {
                StatusId = (int)StatusCode.Active,
            };
            return View(contact);
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post),
        OutputCache(NoStore = true, Duration = 0, VaryByHeader = "*")]
        public ActionResult UpdateContact(ContactModel model)
        {
            try
            {
                model.StatusId = (int)StatusCode.Active;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = 1;
                this._contactDataRepository.Insert(model);
                if (model.CotactUsId > 0)
                {
                    string mailContent = CommonUtility.RenderViewToString("_EnquiryMailNotifiaction",
                        model, this, new Dictionary<string, object>());

                    SharedFunctionality shared = new SharedFunctionality();
                    shared.MassMailing(new List<string> { CommonUtility.GetConfigData<string>("MAIL_SENDER_RECIPIENT") }, mailContent, "Inplay Enquiry");
                }
                return RedirectToActionPermanent("Index");
            }
            catch (System.Exception ex)
            {
                ex.ExceptionValueTracker(model);
            }
            return null;
        }
    }
}
