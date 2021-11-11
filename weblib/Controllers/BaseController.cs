// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Account.cs" company="GalexStudio">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Controllers
{
    using System.Web.Mvc;

    using LIB.BusinessObjects;

    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Collections.Generic;
    using LIB.Tools.Utils;
    using System.Linq;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class BaseController : Controller
    {
        public BaseController()
        {
            var Languages = (new Language()).Populate();

            var CurrentLanguage = (Language)Languages.Values.FirstOrDefault();

            if (System.Web.HttpContext.Current.Session[SessionItems.Language] == null && CurrentLanguage != null)
            {
                System.Web.HttpContext.Current.Session[SessionItems.Language] = CurrentLanguage;
            }
            else
            {
                CurrentLanguage = (Language)System.Web.HttpContext.Current.Session[SessionItems.Language];
            }

            CultureHelper.Language = CurrentLanguage;

            ViewData["Languages"] = Languages;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //Response.Redirect("http://lims.e-agricultura.md/");

            base.OnResultExecuted(filterContext);

            DataBase.CloseConnection();
        }
    }
}
