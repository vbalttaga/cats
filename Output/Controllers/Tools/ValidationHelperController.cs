// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationHelperController.cs" company="Natur Bravo Pilot">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the ValidationHelperController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Galex.Controllers
{
    using Gofraweblib;
    using Gofraweblib.Controllers;
    using LIB.BusinessObjects;
    using LIB.Tools.Security;
    using LIB.Tools.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Weblib.Controllers;
    using Weblib.Helpers;
    using LIB.Helpers;
    using System.Web;

    /// <summary>
    /// The ValidationHelper controller.
    /// </summary>
    public class ValidationHelperController : BaseController
    {
        [HttpPost]
        public ActionResult ValidateUserName()
        {
            if (!Authentication.CheckUser(this.HttpContext)) //TBD
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
            }
            if (!LIB.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)BasePermissionenum.CPAccess | (long)BasePermissionenum.SMIAccess))
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
            }

            var Login = Request.Form["Login"];
            var Userid = Convert.ToInt64(Request.Form["Userid"]);

            var user = new User() { Login = Login, Timeout=0 };
            
            user = (User)user.PopulateOne(user);
            if (user == null || (user != null && (Userid != 0 || user.Id == Userid)))
                return this.Json(new RequestResult() { Result = RequestResultType.Fail });

            return this.Json(new RequestResult() { Result = RequestResultType.Success, Message = "Acest nume este utilizat deja" });
        }    
    }
}
