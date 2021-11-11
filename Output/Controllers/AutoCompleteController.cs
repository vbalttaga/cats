using LIB.Tools.BO;
using LIB.Tools.Security;
using LIB.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weblib.Helpers;
using Gofra.Models;
using LIB.BusinessObjects;
using GofraLib.BusinessObjects;
using System.Globalization;
using Gofra.Models.Objects;
using LIB.AdvancedProperties;
using System.ComponentModel;
using LIB.Helpers;

namespace Gofra.Controllers
{
    public class AutoCompleteController : Gofraweblib.Controllers.FrontEndController
    {
        //
        // GET: /AutoComplete/

        public ActionResult List(string Model,string Namespace,string Param)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            if (Model == "none")
                Model = "";
            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
            var Items = item.PopulateAutocomplete(!string.IsNullOrEmpty(Param)? Param: Request.QueryString["cond"], Request.QueryString["term"]);
            ViewData["Model"] = Model;
            ViewData["Param"] = Param;
            return View(!string.IsNullOrEmpty(Model)?item.AutocompleteControl(): "Search", Items);
        }
    }
}
