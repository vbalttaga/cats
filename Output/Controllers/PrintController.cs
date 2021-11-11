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
using System.IO;
using LIB.Helpers;
using Weblib.Converters;

namespace Gofra.Controllers
{
    public class PrintController : Gofraweblib.Controllers.PrintController
    {
        //[HttpPost]
        public ActionResult Print()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                ViewData["SandboxPrint"] = false;
                ViewData["Styles"] = System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/common.css"));
                var Namespace = Request.Form["Namespace"];

                var item = (PrintBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

                var Filters = new Dictionary<string, string>();             
                foreach (var postItem in Request.Form.AllKeys)
                {
                    Filters.Add(postItem, Request.Form[postItem]);
                }
                item.Filters = Filters;
                
                var View = item.LoadReport(ControllerContext, ViewData, TempData);

                if (!string.IsNullOrEmpty(item.StylesFile) && System.IO.File.Exists(Server.MapPath(@"~/Content/Print/" + item.StylesFile + ".css")))
                {
                    ViewData["Styles"] += System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/" + item.StylesFile + ".css"));
                }

                return this.View(View, item);
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }

        [ValidateInput(false)]
        public FileResult ExportWord()
        {
            var filedownload = new HttpCookie("fileDownload")
            {
                Expires = DateTime.Now.AddDays(1),
                Value = "true"
            };
            Response.Cookies.Add(filedownload);
            var path = new HttpCookie("path")
            {
                Expires = DateTime.Now.AddDays(1),
                Value = "/"
            };
            Response.Cookies.Add(path);
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return File(WordHelper.HtmlToWord(""), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            try
            {
                ViewData["SandboxPrint"] = false;
                ViewData["Styles"] = "";
                var Namespace = Request.Form["Namespace"];

                var item = (PrintBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

                var Filters = new Dictionary<string, string>();
                foreach (var postItem in Request.Form.AllKeys)
                {
                    Filters.Add(postItem, Request.Form[postItem]);
                }
                item.Filters = Filters;

                var View = item.LoadReport(ControllerContext, ViewData, TempData, ExportType.Word);

                var viewName = "~/Views/Export/Word/" + View + ".cshtml";
                using (StringWriter sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                    ViewData.Model = item;
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return File(WordHelper.HtmlToWord(sw.GetStringBuilder().ToString()), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", item.FileDownloadName);
                }
            }
            catch (Exception ex)
            {
                return File(WordHelper.HtmlToWord(ex.ToString()), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
        }

        public ActionResult SandboxPrint(string View = "", string Postfix = "")
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                ViewData["SandboxPrint"] = true;
                ViewData["Styles"] = System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/common.css"));

                if (!string.IsNullOrEmpty(View))
                {
                    if (System.IO.File.Exists(Server.MapPath(@"~/Content/Print/" + View + ".css")))
                    {
                        ViewData["Styles"] += System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/" + View + ".css"));
                    }
                    var titem = new PrintTest();
                    var viewName = "~/Views/Print/Templates/"+ View + Postfix + ".cshtml";
                    using (StringWriter sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);

                        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                        viewResult.View.Render(viewContext, sw);

                        titem.PrintTemplate = sw.GetStringBuilder().ToString();
                    }
                    return this.View("Generic", titem);
                }

                var Namespace = Request.QueryString["Namespace"];

                var item = (PrintBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

                var Filters = new Dictionary<string, string>();
                foreach (var postItem in Request.QueryString.AllKeys)
                {
                    Filters.Add(postItem, Request.QueryString[postItem]);
                }
                item.Filters = Filters;

                View = item.LoadReport(ControllerContext, ViewData, TempData);

                if (!string.IsNullOrEmpty(item.StylesFile) && System.IO.File.Exists(Server.MapPath(@"~/Content/Print/" + item.StylesFile + ".css")))
                {
                    ViewData["Styles"] += System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/" + item.StylesFile + ".css"));
                }

                return this.View(View, item);
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult SandboxExportWord(string View = "", string Postfix = "")
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return File(WordHelper.HtmlToWord(""), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            try
            {
                ViewData["SandboxPrint"] = true;
                var viewName = "";
                if (!string.IsNullOrEmpty(View))
                {
                    var titem = new PrintTest();
                    viewName = "~/Views/Export/Word/Templates/" + View + Postfix + ".cshtml";
                    using (StringWriter sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);

                        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                        viewResult.View.Render(viewContext, sw);

                        titem.PrintTemplate = sw.GetStringBuilder().ToString();
                    }
                    viewName = "~/Views/Export/Word/Generic.cshtml";
                    using (StringWriter sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                        ViewData.Model = titem;
                        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                        viewResult.View.Render(viewContext, sw);

                        return File(WordHelper.HtmlToWord(sw.GetStringBuilder().ToString()), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    }
                }

                var Namespace = Request.QueryString["Namespace"];

                var item = (PrintBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

                var Filters = new Dictionary<string, string>();
                foreach (var postItem in Request.QueryString.AllKeys)
                {
                    Filters.Add(postItem, Request.QueryString[postItem]);
                }
                item.Filters = Filters;

                View = item.LoadReport(ControllerContext, ViewData, TempData, ExportType.Word);

                viewName = "~/Views/Print/Export/Word/" + View + ".cshtml";
                using (StringWriter sw = new StringWriter())
                {
                    var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                    ViewData.Model = item;
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);

                    return File(WordHelper.HtmlToWord(sw.GetStringBuilder().ToString()), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                }
            }
            catch (Exception ex)
            {
                return File(WordHelper.HtmlToWord(ex.ToString()), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
        }

    }
}
