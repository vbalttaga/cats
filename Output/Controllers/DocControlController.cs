using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

using LIB.Tools.BO;
using LIB.Tools.Security;
using LIB.Tools.Utils;
using Weblib.Helpers;
using Gofra.Models;
using LIB.BusinessObjects;
using GofraLib.BusinessObjects;
using Gofra.Models.Objects;
using LIB.AdvancedProperties;
using System.ComponentModel;
using Weblib.Models.Common;
using LIB.Helpers;
using LIB.Models.Common;
using GofraLib.Security;
/*using Gofra.Models.Print;*/

namespace Gofra.Controllers
{
    public class DocControlController : Gofraweblib.Controllers.PageObjectController
    {
        //
        // GET: /DocControl/

        public ActionResult Edit(string Model, string Id = "", string additional = "")
        {
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("Gofra.Models.Objects." + Model, true));
            var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
            if (MenuItems != null)
            {
                var usr = Authentication.GetCurrentUser();
                if (!Authorization.hasPageAccess(MenuItems, usr, item))
                {
                    return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
                }

                BoAttribute boproperties = null;
                if (item.GetType().GetCustomAttributes(typeof(LIB.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    boproperties = (LIB.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(LIB.AdvancedProperties.BoAttribute), true)[0];
                }

                base.LoadMenu(Model);

                if (!string.IsNullOrEmpty(Id))
                {
                    item.Id = Convert.ToInt64(Id);

                    item = item.PopulateFrontEnd(additional, (ItemBase)item);

                    ViewData["ParentId"] = item.Id;

                    if (!item.HaveAccess())
                    {
                        return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
                    }
                }

                var PageTitle = item.LoadPageTitle();
                if (!string.IsNullOrEmpty(PageTitle))
                    ViewBag.Title = PageTitle;

                ViewData["Breadcrumbs"] = item.LoadBreadcrumbs();
                ViewData["QuickLinks"] = item.LoadQuickLinks();
                ViewData["ReportMenu"] = item.LoadContextReports();
                return View(Model, item);
            }
            return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
        }

        [HttpPost]
        [ValidateInput(false)] 
        [ValidateAntiForgeryToken]
        public ActionResult Save()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var Namespace = Request.Form["Namespace"];
                var Object = Request.Form["Object"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
                var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                if (MenuItems != null)
                {
                    var usr = Authentication.GetCurrentUser();
                    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    {
                        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                    }
                    item.Id = Convert.ToInt64(Request.Form["Id"]);
                    item.CollectFromForm();
                    return this.Json(item.SaveForm());
                }
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var Namespace = Request.Form["Namespace"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
                var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                if (MenuItems != null)
                {
                    var usr = Authentication.GetCurrentUser();
                    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    {
                        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                    }
                    item.Id = Convert.ToInt64(Request.Form["Id"]);
                    var deletearray = new Dictionary<long, ItemBase>();
                    deletearray.Add(item.Id, item);
                    item.Delete(deletearray);

                    return this.Json(new RequestResult() { Result = RequestResultType.Reload, Message = "Anularea Effectuata", RedirectURL = URLHelper.GetUrl("") });
                }
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }
    }
}
