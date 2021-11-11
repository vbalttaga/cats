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
using LIB.Tools.Revisions;
using LIB.Helpers;

namespace Gofra.Controllers
{
    public class DynamicControlController : Gofraweblib.Controllers.FrontEndController
    {

        [HttpPost]
        public ActionResult Load()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var Namespace = Request.Form["Namespace"];

            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(LIB.AdvancedProperties.BoAttribute), true).Length > 0)
            {
                boproperties = (LIB.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(LIB.AdvancedProperties.BoAttribute), true)[0];
            }

            var items = item.PopulateFrontEndItems();

            var viewData = item.LoadFrontEndViewdata();
            if (viewData != null && viewData.Count > 0)
            {
                foreach (var key in viewData.Keys)
                {
                    ViewData[key] = viewData[key];
                }
            }
            return View(item.GetType().Name, items);
        }

        [HttpPost]
        public ActionResult LoadNewItem(string ParentId)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var Namespace = Request.Form["Namespace"];
            ViewData["ParentId"] = Convert.ToInt64(ParentId);

            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

            return View(item.GetType().Name+"AddRow");
        }

        [HttpPost]
        [ValidateInput(false)] 
        public ActionResult Save()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var Namespace = Request.Form["Namespace"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
                
                BoAttribute boproperties = null;
                if (item.GetType().GetCustomAttributes(typeof(LIB.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    boproperties = (LIB.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(LIB.AdvancedProperties.BoAttribute), true)[0];
                }
                
                item.CollectFromForm();
                item.SaveForm();

                var items = item.PopulateFrontEndItems();
                
                var viewData = item.LoadFrontEndViewdata();
                if (viewData != null && viewData.Count > 0)
                {
                    foreach (var key in viewData.Keys)
                    {
                        ViewData[key] = viewData[key];
                    }
                }

                ViewData["NewItemId"] = item.Id;

                return View(item.GetType().Name, items);

            }
            catch (Exception ex)
            {
                //throw ex;
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
                item.Id = Convert.ToInt64(Request.Form["Id"]);

                Revision.Insert(new Revision() { BOId = item.Id, BOName = "", Comment = "Cancel", Date = DateTime.Now, Table = item.GetType().Name, Type = OperationTypes.Cancel });

                var itemsToDelete = new Dictionary<long, ItemBase>();
                itemsToDelete.Add(item.Id, item);
                item.Delete(itemsToDelete);

                if (Request.Form["ClientReload"] == "0")
                {
                    var items = item.PopulateFrontEndItems();

                    var viewData = item.LoadFrontEndViewdata();
                    if (viewData != null && viewData.Count > 0)
                    {
                        foreach (var key in viewData.Keys)
                        {
                            ViewData[key] = viewData[key];
                        }
                    }

                    return View(item.GetType().Name, items);
                }

                return this.Json(new RequestResult() { Result = RequestResultType.Success });
            }
            catch (Exception ex)
            {
                //throw ex;
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }
    }
}
