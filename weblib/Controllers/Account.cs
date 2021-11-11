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
    using LIB.Helpers;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : BaseController
    {
        public AccountController()
            : base()
        {
            System.Web.HttpContext.Current.Session["IsSafary"] = false;
            System.Web.HttpContext.Current.Session["IsChrome"] = false;
            System.Web.HttpContext.Current.Session["IsFF"] = false;
            System.Web.HttpContext.Current.Session["IsMacSafary"] = false;
            System.Web.HttpContext.Current.Session["IsMacChrome"] = false;
            System.Web.HttpContext.Current.Session["IsIE10"] = false;
            System.Web.HttpContext.Current.Session["IsIE11"] = false;
            System.Web.HttpContext.Current.Session["IsIE9"] = false;
            System.Web.HttpContext.Current.Session["IsAndroidFF"] = false;
            System.Web.HttpContext.Current.Session["IsAndroidNative"] = false;
            System.Web.HttpContext.Current.Session["IsIPad"] = false;
            System.Web.HttpContext.Current.Session["IsMacFF"] = false;
            System.Web.HttpContext.Current.Session["IsIEOld"] = false;
            System.Web.HttpContext.Current.Session["Browser_specific"] = "";

            var ua = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

            if (ua.ToLower().IndexOf("windows") != -1 && ua.ToLower().IndexOf("safari") != -1 && ua.ToLower().IndexOf("chrome") == -1)
            {
                System.Web.HttpContext.Current.Session["IsSafary"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "win_safari";
            }
            else if (ua.ToLower().IndexOf("windows") != -1 && ua.ToLower().IndexOf("chrome") != -1)
            {
                System.Web.HttpContext.Current.Session["IsChrome"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "win_chrome";
            }
            else if (ua.ToLower().IndexOf("windows") != -1 && ua.ToLower().IndexOf("firefox") != -1)
            {
                System.Web.HttpContext.Current.Session["IsFF"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "win_firefox";
            }
            else if (ua.ToLower().IndexOf("macintosh") != -1 && ua.ToLower().IndexOf("safari") != -1 && ua.ToLower().IndexOf("chrome") == -1)
            {
                System.Web.HttpContext.Current.Session["IsMacSafary"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "mac_safari";
            }
            else if (ua.ToLower().IndexOf("macintosh") != -1 && ua.ToLower().IndexOf("chrome") != -1)
            {
                System.Web.HttpContext.Current.Session["IsMacChrome"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "mac_chrome";
            }
            else if (ua.ToLower().IndexOf("rv:11.0") != -1)
            {
                System.Web.HttpContext.Current.Session["IsIE11"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "ie11";
            }
            else if (ua.ToLower().IndexOf("msie 10") != -1)
            {
                System.Web.HttpContext.Current.Session["IsIE10"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "ie10";
            }
            else if (ua.ToLower().IndexOf("msie 9") != -1)
            {
                System.Web.HttpContext.Current.Session["IsIE9"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "ie9";
            }
            else if (ua.ToLower().IndexOf("msie") != -1)
            {
                System.Web.HttpContext.Current.Session["IsIEOld"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "ieold";
            }
            else if (ua.ToLower().IndexOf("android") != -1 && ua.ToLower().IndexOf("firefox") != -1)
            {
                System.Web.HttpContext.Current.Session["IsAndroidFF"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "android_firefox";
            }
            else if (ua.ToLower().IndexOf("android") != -1)
            {
                System.Web.HttpContext.Current.Session["IsAndroidNative"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "android";
            }
            else if (ua.ToLower().IndexOf("ipad") != -1)
            {
                System.Web.HttpContext.Current.Session["'IsIPad'"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "ipad";
            }
            else if (ua.ToLower().IndexOf("macintosh") != -1 && ua.ToLower().IndexOf("firefox") != -1)
            {
                System.Web.HttpContext.Current.Session["IsMacFF"] = true;
                System.Web.HttpContext.Current.Session["Browser_specific"] = "mac_firefox";
            }
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
        protected ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Script = "login";
            
            var model = new LoginModel
                            {
                                Login = new TextboxModel() { Name = "Login", Type = TextboxType.Text, ValidationType = LIB.AdvancedProperties.ValidationTypes.Required },
                                Password = new TextboxModel() { Name = "Password", Type = TextboxType.Password, ValidationType = LIB.AdvancedProperties.ValidationTypes.Required, OnType = "submit_on_enter(this,event)" }
                            };

            return this.View(model);
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
        protected ActionResult Login(User user, string returnUrl)
        {
            if (LIB.Tools.Security.Authentication.DoAuthorization(user))
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = LIB.Tools.Utils.URLHelper.GetUrl("");
                return this.Json(new RequestResult() { RedirectURL = returnUrl, Result = RequestResultType.Success });
            }
            var errorFields = new List<string>();
            errorFields.Add("input[name=Login]");
            errorFields.Add("input[name=Password]");
            return this.Json(new RequestResult() { Message = "Acest utilizator nu exista", Result = RequestResultType.Fail, ErrorFields = errorFields });
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
        protected ActionResult CPLogin(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Script = "login";

            ViewData["LoginFail"] = null;
            return this.View();
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
        protected ActionResult CPLogin(User user, string returnUrl)
        {
            if (LIB.Tools.Security.Authentication.DoAuthorization(user, null, null, Modulesenum.ControlPanel))
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = LIB.Tools.Utils.URLHelper.GetUrl("ControlPanel");
                return this.Redirect(returnUrl);
            }
            ViewData["LoginFail"] = true;

            return this.View();
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
        protected ActionResult SMILogin(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Script = "login";

            ViewData["LoginFail"] = null;
            return this.View();
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
        protected ActionResult SMILogin(User user, string returnUrl)
        {
            if (LIB.Tools.Security.Authentication.DoAuthorization(user, null, null, Modulesenum.SMI))
            {
                if (string.IsNullOrEmpty(returnUrl))
                    returnUrl = LIB.Tools.Utils.URLHelper.GetUrl("SystemManagement");
                return this.Redirect(returnUrl);
            }
            ViewData["LoginFail"] = true;

            return this.View();
        }

        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult LogOff()
        {
            LIB.Tools.Security.Authentication.LogOff();
            return this.RedirectToAction("Login", "Account");
        }

        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult CPLogOff()
        {
            LIB.Tools.Security.Authentication.LogOff();
            return this.RedirectToAction("CPLogin", "Account");
        }

        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult SMILogOff()
        {
            LIB.Tools.Security.Authentication.LogOff();
            return this.RedirectToAction("SMILogin", "Account");
        }
    }
}
