// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Account.cs" company="Natur Bravo">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gofraweblib.Controllers
{
    using System.Web.Mvc;

    using GofraLib.BusinessObjects;
    using System.Collections.Generic;
    using LIB.Tools.BO;

    /// <summary>
    /// The account.
    /// </summary>
    public class Account : Weblib.Controllers.AccountController
    {
        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult Login(string returnUrl)
        {
            var SortParameters =  new List<SortParameter>();
            SortParameters.Add(new SortParameter(){Field="Date",Direction="desc"});
            this.ViewData["News"] = (new News()).Populate(SortParameters);
            this.ViewData["Information"] = (new UsefullInfo()).Populate();
            return base.Login(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string returnUrl)
        {
            return base.Login(user, returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult CPLogin(string returnUrl)
        {
            return base.CPLogin(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CPLogin(User user, string returnUrl)
        {
            return base.CPLogin(user, returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult SMILogin(string returnUrl)
        {
            return base.SMILogin(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SMILogin(User user, string returnUrl)
        {
            return base.SMILogin(user, returnUrl);
        }
    }
    
}
