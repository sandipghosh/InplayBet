

namespace InplayBet.Web.Controllers
{
    using InplayBet.Web.Controllers.Base;
    using InplayBet.Web.Models;
    using InplayBet.Web.Models.Base;
    using InplayBet.Web.Data.Interface;
    using InplayBet.Web.Utilities;
    using System.Web.Mvc;
    using System;
    using System.Collections.Generic;

    public class ContactUsController : BaseController
    {
        private readonly IContactDataRepository _contactDataRepository;

        public ContactUsController(IContactDataRepository contactDataRepository)
        {
            this._contactDataRepository = contactDataRepository;
        }

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
                    SharedFunctionality shared = new SharedFunctionality();
                    shared.MassMailing(new List<string> { model.EmailId }, "Test", "Test");
                }
            }
            catch (System.Exception ex)
            {
                ex.ExceptionValueTracker(model);
            }
            return null;
        }
    }
}
