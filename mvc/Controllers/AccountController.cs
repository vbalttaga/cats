// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Natur Bravo Pilot">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the AccountController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gofra.Controllers
{
    using LIB.Tools.BO;
    using LIB.Tools.Security;
    using LIB.Tools.Utils;
    using GofraLib.BusinessObjects;
    using Gofraweblib;
    using Gofraweblib.Controllers;
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Weblib.Helpers;
    using LIB.Helpers;
    using System.Web.UI;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml;
    using System.Web.Security;
    using GofraLib.Utils;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : Account
    {
        private X509Certificate2 LoadServiceProviderCertificate()
        {
            return new X509Certificate2(Server.MapPath(Config.GetConfigValue("ServiceCertificate")), Config.GetConfigValue("ServiceCertificatePassword"), X509KeyStorageFlags.MachineKeySet);
        }

        private X509Certificate2 LoadIdentityProviderCertificate()
        {
            return new X509Certificate2(Server.MapPath(Config.GetConfigValue("IdentityProviderCertificate")));
        }

        [OutputCache(Duration = 0, Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult LoginMpass()
        {
            // generate AuthnRequest ID
            var authnRequestID = "_" + Guid.NewGuid();
            Session[SamlMessage.RequestIDSessionKey] = authnRequestID;

            // build a full URL to login action
            var fullAcsUrl = Url.Action("Acs", "Account", null, Request.Url.Scheme);

            // build AuthnRequest
            var authnRequest = SamlMessage.BuildAuthnRequest(authnRequestID, Config.GetConfigValue("SamlLoginDestination"), fullAcsUrl, Config.GetConfigValue("SamlRequestIssuer"));

            // sign AuthnRequest
            authnRequest = SamlMessage.Sign(authnRequest, LoadServiceProviderCertificate());

            // redirect to IdP
            ViewBag.IdPUrl = Config.GetConfigValue("SamlLoginDestination");
            ViewBag.SAMLVariable = "SAMLRequest";
            ViewBag.SAMLMessage = SamlMessage.Encode(authnRequest);
            // NOTE: RelayState must not exceed 80 bytes in length, as specified by [SAMLBind, 3.5.3]
            ViewBag.RelayState = "Sample AuthnRequest Relay State";
            return View("Redirect");
        }

        [HttpPost]
        public ActionResult Acs(string samlResponse, string relayState)
        {
            XmlNamespaceManager ns;
            string sessionIndex;
            string nameID;
            Dictionary<string, IList<string>> attributes;

            // NOTE: Keeping InResponseTo in an in-memory Session means this verification will always fail if the web app is restarted during a request
            SamlMessage.LoadAndVerifyLoginResponse(samlResponse, LoadIdentityProviderCertificate(), Request.Url.ToString(), TimeSpan.Parse(Config.GetConfigValue("SamlMessageTimeout")),
                Session[SamlMessage.RequestIDSessionKey] as string, Config.GetConfigValue("SamlRequestIssuer"), out ns, out sessionIndex, out nameID, out attributes);

            // remove RequestID from session to stop replay attacks
            Session.Remove(SamlMessage.RequestIDSessionKey);
            // save SessionIndex in session
            Session[SamlMessage.SessionIndexSessionKey] = sessionIndex;

            if (!string.IsNullOrEmpty(nameID) && GofraLib.BusinessObjects.User.DoAuthorizationByCNP(nameID))
            {
                FormsAuthentication.SetAuthCookie(nameID, false);
                return RedirectToAction("Index", "DashBoard");
            }
            else
            {
                return Logout();
            }
        }
        // prevent any response caching, as specified by [SAMLBind, 3.5.5.1]
        [OutputCache(Duration = 0, Location = OutputCacheLocation.None, NoStore = true)]
        [Authorize]
        public ActionResult Logout()
        {
            // generate LogoutRequest ID
            var logoutRequestID = "_" + Guid.NewGuid();

            // build LogoutRequest
            var logoutRequest = SamlMessage.BuildLogoutRequest(logoutRequestID, Config.GetConfigValue("SamlLogoutDestination"), Config.GetConfigValue("SamlRequestIssuer"),
                User.Identity.Name, Session[SamlMessage.SessionIndexSessionKey] as string);

            // sign LogoutRequest
            logoutRequest = SamlMessage.Sign(logoutRequest, LoadServiceProviderCertificate());

            // logout locally
            FormsAuthentication.SignOut();
            Session.Clear();

            Session[SamlMessage.RequestIDSessionKey] = logoutRequestID;

            // redirect to IdP
            ViewBag.IdPUrl = Config.GetConfigValue("SamlLogoutDestination");
            ViewBag.SAMLVariable = "SAMLRequest";
            ViewBag.SAMLMessage = SamlMessage.Encode(logoutRequest);
            // NOTE: RelayState may be maximum 80 bytes, as specified by [SAMLBind, 3.5.3]
            ViewBag.RelayState = "Sample LogoutRequest Relay State";
            return View("Redirect");
        }

        [HttpPost]
        public ActionResult AfterLogout(string samlResponse, string relayState)
        {
            XmlNamespaceManager ns;

            // NOTE: Keeping InResponseTo in an in-memory Session means this verification will always fail if the web app is restarted during a request
            SamlMessage.LoadAndVerifyLogoutResponse(samlResponse, LoadIdentityProviderCertificate(), Request.Url.ToString(), TimeSpan.Parse(Config.GetConfigValue("SamlMessageTimeout")),
                Session[SamlMessage.RequestIDSessionKey] as string, out ns);

            // remove SessionIndex from session to stop replay attacks
            Session.Remove(SamlMessage.RequestIDSessionKey);

            return RedirectToAction("Index", "DashBoard");
        }

        // prevent any response caching, as specified by [SAMLBind, 3.5.5.1]
        [OutputCache(Duration = 0, Location = OutputCacheLocation.None, NoStore = true)]
        [HttpPost]
        public ActionResult SingleLogout(string samlRequest, string relayState)
        {
            string logoutRequestID;
            SamlMessage.LoadAndVerifyLogoutRequest(samlRequest, LoadIdentityProviderCertificate(), Request.Url.ToString(), TimeSpan.Parse(Config.GetConfigValue("SamlMessageTimeout")),
                User.Identity.Name, Session[SamlMessage.SessionIndexSessionKey] as string, out logoutRequestID);

            if (Request.IsAuthenticated)
            {
                // logout locally
                FormsAuthentication.SignOut();
                Session.Abandon();
            }

            // build LogoutResponse
            var logoutResponseID = "_" + Guid.NewGuid();
            var logoutResponse = SamlMessage.BuildLogoutResponse(logoutResponseID, Config.GetConfigValue("SamlLogoutDestination"), logoutRequestID, Config.GetConfigValue("SamlRequestIssuer"));

            // sign LogoutResponse
            logoutResponse = SamlMessage.Sign(logoutResponse, LoadServiceProviderCertificate());

            // redirect to IdP
            ViewBag.IdPUrl = Config.GetConfigValue("SamlLogoutDestination");
            ViewBag.SAMLVariable = "SAMLResponse";
            ViewBag.SAMLMessage = SamlMessage.Encode(logoutResponse);
            ViewBag.RelayState = relayState;
            return View("Redirect");
        }
        public ActionResult Manage()
        {
            var usr = GofraLib.BusinessObjects.User.Populate(Authentication.GetCurrentUser());
            ViewData["MainMenu"] = MenuGroup.Populate(Authentication.GetCurrentUser());
            var Recent = System.Web.HttpContext.Current.Request.Cookies[usr.Id.ToString()+"_recentmenuitems"];
            ViewData["RecentMenu"] = MenuItem.PopulateRecentMenu(Authentication.GetCurrentUser(), Recent != null ? Recent.Value : "");
            return View(usr);
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
        public ActionResult Edit()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var Namespace = Request.Form["Namespace"];
            //var email = Request.Form["UserEmail"];
            var oldPassword = Request.Form["OldPassword"];
            var newPassword = Request.Form["Password"];
            var newPasswordConfirm = Request.Form["PasswordConfirm"];
            /*
            if(!LIB.Tools.Utils.CommonHelper.IsValidEmail(email)) {
                var errorFields = new List<string>();
                    errorFields.Add("input[name=UserEmail]");
                    return this.Json(new RequestResult() { Message = "Adresa email nu este valida", Result = RequestResultType.Fail, ErrorFields = errorFields });
            }*/
            
            if(!string.IsNullOrEmpty(oldPassword) || !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(newPasswordConfirm)) {
                var currentUser = Authentication.GetCurrentUser();
                currentUser.Password = oldPassword;
                
                if(Authentication.DoAuthorization(currentUser)) {
                    var item = (User)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
                    item.Id = Convert.ToInt64(Request.Form["Id"]);

                    if(string.IsNullOrEmpty(newPassword)) {
                        var errorFields = new List<string>();
                        errorFields.Add("input[name=Password]");
                        return this.Json(new RequestResult() { Message = "Parola nu poate fi nula", Result = RequestResultType.Fail, ErrorFields = errorFields });
                    } else {
                        item.Password = Request.Form["Password"];
                    }

                    GofraLib.BusinessObjects.User.UpdatePassword(item);
                    //GofraLib.BusinessObjects.Person.UpdateEmail(item, email);

                    return this.Json(new RequestResult() { Result = RequestResultType.Success, Message="Schimbarile au fost memorizate cu succes"});
                } else {
                    var errorFields = new List<string>();
                    errorFields.Add("input[name=OldPassword]");
                    return this.Json(new RequestResult() { Message = "Parola curenta nu este corecta", Result = RequestResultType.Fail, ErrorFields = errorFields });
                    // return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Parola nu a fost modificata!" });
                }
            } else {
                var currentUser = Authentication.GetCurrentUser();
                var item = (User)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
                item.Id = Convert.ToInt64(Request.Form["Id"]);
                //GofraLib.BusinessObjects.Person.UpdateEmail(item, email);
                return this.Json(new RequestResult() { Result = RequestResultType.Success, Message="Schimbarile au fost memorizate cu succes"});
            }
            
        }
    }
}
