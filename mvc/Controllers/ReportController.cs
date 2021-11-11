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
using LIB.AdvancedProperties;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
//using Gofra.Models.Reports;
using Weblib.Models.Common;
using LIB.Helpers;
using Gofra.Models.Reports;
using GofraLib.Security;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Gofra.Controllers
{
    public class ReportController : Gofraweblib.Controllers.ReportObjectController
    {
        public ActionResult View(string Model, string BOLink = "ItemBase", string NamespaceLink = "Lib.BusinessObjects", string Id = "")
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var sSearch = "";

            base.LoadMenu(Model);

            var is_search = false;
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("Gofra.Models.Reports." + Model, true));
            var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
            if (MenuItems != null)
            {
                var usr = Authentication.GetCurrentUser();
                if (!Authorization.hasPageAccess(MenuItems, usr, item))
                {
                    return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
                }
                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(item.GetType());
                var search_properties = pss.GetFilterControlProperties(pdc, Authentication.GetCurrentUser());
                var lookup_properties = pss.GetSearchProperties(pdc);
                var DBProperties = Field.LoadByPage(item.GetType().FullName);
                AdvancedProperties properties=null;;

                if(Session["Display_" + Model] != null)
                {
                    properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), (List<string>)Session["Display_" + Model]);
                }
                else if (DBProperties != null && DBProperties.Count > 0)
                {
                    properties = new AdvancedProperties();
                    var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                    foreach (AdvancedProperty property in tprops)
                    {
                        if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                        {
                            var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                            property.Common.DisplayName = DBField.Name;
                            property.Common.PrintName = DBField.PrintName;

                            if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Simple) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Display_" + Model] == null)
                            {
                                properties.Add(property);
                            }
                        }
                    }
                    properties.Sort();
                }
                else
                {
                    properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
                }          

                ViewData["Model"] = Model;

                BoAttribute boproperties = null;
                if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                {
                    boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                    if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                    {
                        ViewBag.Title = "Gofra: " + boproperties.DisplayName;
						ViewData["Report_Name"] = boproperties.DisplayName;
                    }
                    if (boproperties != null)
                    {
                        ViewData["NewTab"] = boproperties.OpenInNewTab;
                    }
                    else
                    {
                        ViewData["NewTab"] = false;
                    }
                }
                else
                {
                    ViewData["NewTab"] = false;
                }

                General.TraceWarn("ID:" +Id);
                if (!string.IsNullOrEmpty(Id))
                {
                    if (!string.IsNullOrEmpty(NamespaceLink) && NamespaceLink!="null")
                    {
                        var LinkItem = Activator.CreateInstance(Type.GetType(NamespaceLink + "." + BOLink + ", " + NamespaceLink.Split('.')[0], true));
                        ((ItemBase)LinkItem).Id = Convert.ToInt64(Id);
                        foreach (AdvancedProperty property in lookup_properties)
                        {
                            if (
                                (property.Common.EditTemplate == EditTemplates.Parent
                                || property.Common.EditTemplate == EditTemplates.DropDownParent
                                || property.Common.EditTemplate == EditTemplates.SelectListParent)
                                && property.Type.Name == LinkItem.GetType().Name
                                )
                            {
                                General.TraceWarn("LinkItem.GetType().Name:" + LinkItem.GetType().Name);
                                is_search = true;
                                property.PropertyDescriptor.SetValue(item, LinkItem);
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (AdvancedProperty property in lookup_properties)
                        {
                            General.TraceWarn("roperty.PropertyName:" + property.PropertyName);
                            if (property.PropertyName == BOLink)
                            {
                                General.TraceWarn("Bingo!!!");
                                is_search = true;
                                property.PropertyDescriptor.SetValue(item, Convert.ChangeType(Id, property.Type));
                                break;
                            }
                        }
                    }
                }

                is_search = item.DefaultReportFilter(is_search);

                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    sSearch = Request.QueryString["s"];
                    is_search = true;
                }

                ViewData["sSearch"] = sSearch;
                sSearch = item.SimpleSearch(sSearch);

                ViewData["Breadcrumbs"] = item.LoadBreadcrumbs();
                ViewData["QuickLinks"] = item.LoadQuickLinks();
                ViewData["ReportMenu"] = item.LoadContextReports();

                long itotal;
                long idisplaytotal;

                var Items = item.PopulateReport(null, is_search ? item : null, 0, boproperties.RecordsPerPage, sSearch, null, null, out itotal, out idisplaytotal);

                var redirectUrl = "";
                if (Items.Count == 1 && item.ReportSingleItemRedirect(Items.Values.FirstOrDefault(i => i.Id > 0), out redirectUrl))
                {
                    return Redirect(redirectUrl);
                }

                ViewData["Count"] = idisplaytotal;
                ViewData["CountPerPage"] = boproperties.RecordsPerPage;
                ViewData["PageNum"] = 0;
                ViewData["BuildPaginng"] = BuildPaginng(idisplaytotal, boproperties.RecordsPerPage, 0);

                ViewData["DataItems"] = Items;

                ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
                ViewData["Search_Item"] = ((ReportBase)item).getSearchItem(item);

                ViewData["Search_Properties"] = search_properties;
                ViewData["Properties"] = properties;

                return View();
            }
            return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OptionsSave(string Model)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("Gofra.Models.Reports." + Model, true));
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
            var DisplayProperties = new List<string>();
            var PrintProperties = new List<string>();
            foreach (AdvancedProperty property in properties)
            {
                if(Request.Form["Display_"+ property.PropertyName] == "1")
                {
                    DisplayProperties.Add(property.PropertyName);
                }
                if (Request.Form["Print_" + property.PropertyName] == "1")
                {
                    PrintProperties.Add(property.PropertyName);
                }
            }
            Session["Print_" + Model] = PrintProperties;
            Session["Display_" + Model] = DisplayProperties;

            return this.Json(new RequestResult() {  Result = RequestResultType.Success });
        }
        public ActionResult Options(string Model)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            var usr = Authentication.GetCurrentUser();
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("Gofra.Models.Reports." + Model, true));
            var DBProperties = Field.LoadByPage(item.GetType().FullName);
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetAvailableProperties(pdc, usr);

            AdvancedProperties DisplayProperties = null;
            AdvancedProperties PrintProperties = null;

            if (DBProperties != null && DBProperties.Count > 0)
            {
                DisplayProperties = new AdvancedProperties();
                PrintProperties = new AdvancedProperties();
                foreach (AdvancedProperty property in properties)
                {
                    if(DBProperties.Values.Any(f => f.FieldName== property.PropertyName))
                    {
                        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                        property.Common.DisplayName = DBField.Name;
                        property.Common.PrintName = DBField.PrintName;

                        if(DBField.DisplayModes!=null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Simple) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Display_" + Model] == null)
                        {
                            DisplayProperties.Add(property);
                        }

                        if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Print) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Print_" + Model] == null)
                        {
                            PrintProperties.Add(property);
                        }
                    }
                }
                if(Session["Display_" + Model] != null)
                {
                    DisplayProperties = pss.GetAvailableProperties(pdc, usr, (List<string>)Session["Display_" + Model]);
                }
                if (Session["Print_" + Model] != null)
                {
                    PrintProperties = pss.GetAvailableProperties(pdc, usr, (List<string>)Session["Print_" + Model]);
                }
            }
            else
            {
                DisplayProperties = Session["Display_" + Model] != null ? pss.GetAvailableProperties(pdc, usr, (List<string>)Session["Display_" + Model]) : pss.GetProperties(pdc, usr);

                PrintProperties = Session["Print_" + Model] != null ? pss.GetAvailableProperties(pdc, usr, (List<string>)Session["Print_" + Model]) : pss.GetPrintProperties(pdc, usr);
            }

            ViewData["Properties"] = properties;
            ViewData["DisplayProperties"] = DisplayProperties;
            ViewData["PrintProperties"] = PrintProperties;

            return View();
        }
        private string BuildPaginng(long Count, int CountPerPage, int PageNum)
        {
            var pagingstr = "";

            if (Count / CountPerPage <= 6)
            {
                for (int p = 1; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p-1).ToString() + ")' class='pagination-page" + ((p == PageNum+1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }
            else if (PageNum <= 3 || PageNum >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
            {
                for (int p = 1; p <= 3; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                pagingstr += "<li>";
                pagingstr += "<a href='#' onclick='return show_report_page(3)' class='pagination-page'>...</a>";
                pagingstr += "</li>";

                for (int p = Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }
            else
            {
                for (int p = 1; p <= 3; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                var pstart = PageNum - 1;
                var pend = PageNum + 1;

                if (pstart <= 3)
                {
                    pstart++;
                    pend++;
                }
                else if (pend >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
                {
                    pstart--;
                    pend--;
                }

                if (pstart <= 3)
                {
                    pstart++;
                }

                if (pend >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
                {
                    pend--;
                }

                if (pstart != 4)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(3)' class='pagination-page'>...</a>";
                    pagingstr += "</li>";
                }

                for (int p = pstart; p <= pend; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                if (pend != Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 3)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2).ToString() + ")' ' class='pagination-page'>...</a>";
                    pagingstr += "</li>";
                }

                for (int p = Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }

            return pagingstr;
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Search()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                
                var sSearch = "";

                var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                if (MenuItems != null)
                {
                    var usr = Authentication.GetCurrentUser();
                    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    {
                        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc);

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", LIB.AdvancedProperties.DisplayMode.Search));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }
                    
                    sSearch = item.SimpleSearch(sSearch);

                    var DBProperties = Field.LoadByPage(item.GetType().FullName);
                    AdvancedProperties properties = null; ;

                    if (Session["Display_" + item.GetType().Name] != null)
                    {
                        properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), (List<string>)Session["Display_" + item.GetType().Name]);
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        properties = new AdvancedProperties();
                        var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        foreach (AdvancedProperty property in tprops)
                        {
                            if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                            {
                                var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                                property.Common.DisplayName = DBField.Name;
                                property.Common.PrintName = DBField.PrintName;

                                if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Simple) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Display_" + item.GetType().Name] == null)
                                {
                                    properties.Add(property);
                                }
                            }
                        }
                        properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];

                        ViewData["NewTab"] = boproperties.OpenInNewTab;
                    }
                    else
                    {
                        ViewData["NewTab"] = false;
                    }

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = !string.IsNullOrEmpty(Request.Form["CountPerPage"]) ? Convert.ToInt32(Request.Form["CountPerPage"]) : boproperties.RecordsPerPage;
                    var PageNum = Convert.ToInt32(Request.Form["PageNum"]);
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    ViewData["SortCol"] = Request.Form["SortCol"];
                    ViewData["SortDir"] = Request.Form["SortDir"];

                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, out itotal, out idisplaytotal);
                    ViewData["Count"] = idisplaytotal;
                    ViewData["CountPerPage"] = CountPerPage;
                    ViewData["PageNum"] = PageNum;
                    ViewData["BuildPaginng"] = BuildPaginng(idisplaytotal, CountPerPage, PageNum);

                    ViewData["DataItems"] = Items;

                    ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
                    ViewData["Search_Item"] = item;

                    ViewData["Search_Properties"] = search_properties;
                    ViewData["Properties"] = properties;

                    var viewName = "~/Views/Report/Search.cshtml";

                    var Data = new Dictionary<string, object>();

                    using (StringWriter sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName,
                                                                               null);

                        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                        viewResult.View.Render(viewContext, sw);

                        Data["Search_Result"] = sw.GetStringBuilder().ToString();
                    }

                    return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                }
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Print()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var sSearch = "";
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                if (MenuItems != null)
                {
                    var usr = Authentication.GetCurrentUser();
                    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    {
                        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc, Authentication.GetCurrentUser());

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", LIB.AdvancedProperties.DisplayMode.PrintSearch));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }

                    sSearch = item.SimpleSearch(sSearch);

                    var DBProperties = Field.LoadByPage(item.GetType().FullName);
                    AdvancedProperties properties = null; ;

                    if (Session["Display_" + item.GetType().Name] != null)
                    {
                        properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), (List<string>)Session["Print_" + item.GetType().Name]);
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        properties = new AdvancedProperties();
                        var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        foreach (AdvancedProperty property in tprops)
                        {
                            if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                            {
                                var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                                property.Common.DisplayName = DBField.Name;
                                property.Common.PrintName = DBField.PrintName;

                                if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Print) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Print_" + item.GetType().Name] == null)
                                {
                                    properties.Add(property);
                                }
                            }
                        }
                        properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetPrintProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                        if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                        {
                            ViewData["Report_Name"] = boproperties.DisplayName;
                        }
                        if (boproperties != null)
                        {
                            ViewData["Show_Header"] = !boproperties.HideReportHeader;
                        }
                    }

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = 0;
                    var PageNum = 0;
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, out itotal, out idisplaytotal);

                    ViewData["DataItems"] = Items;

                    ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
                    ViewData["Search_Item"] = item;
                    ViewData["Search_Properties"] = search_properties;
                    ViewData["Properties"] = properties;
                    ViewData["Styles"] = System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/common.css"));
                    ViewData["Styles"] += System.IO.File.ReadAllText(Server.MapPath(@"~/Content/Print/report.css"));

                    var viewName = "~/Views/Report/Print.cshtml";

                    var Data = new Dictionary<string, object>();

                    using (StringWriter sw = new StringWriter())
                    {
                        var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName,
                                                                               null);

                        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                        viewResult.View.Render(viewContext, sw);

                        Data["Print_Result"] = sw.GetStringBuilder().ToString();
                    }

                    return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                }
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public FileResult ExportCSV()
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
                return File(Encoding.UTF8.GetBytes("Authentification Error"), "application/vnd.ms-excel", "Error.csv");
            }
            try
            {
                var sSearch = "";
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                if (MenuItems != null)
                {
                    var usr = Authentication.GetCurrentUser();
                    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    {
                        return File("", "application/vnd.ms-excel");
                    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc, Authentication.GetCurrentUser());

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", LIB.AdvancedProperties.DisplayMode.PrintSearch));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }

                    sSearch = item.SimpleSearch(sSearch);

                    var DBProperties = Field.LoadByPage(item.GetType().FullName);
                    AdvancedProperties properties = null; ;

                    if (Session["Display_" + item.GetType().Name] != null)
                    {
                        properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), (List<string>)Session["Print_" + item.GetType().Name]);
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        properties = new AdvancedProperties();
                        var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        foreach (AdvancedProperty property in tprops)
                        {
                            if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                            {
                                var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                                property.Common.DisplayName = DBField.Name;
                                property.Common.PrintName = DBField.PrintName;

                                if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Print) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Print_" + item.GetType().Name] == null)
                                {
                                    properties.Add(property);
                                }
                            }
                        }
                        properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetPrintProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                        if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                        {
                            ViewData["Report_Name"] = boproperties.DisplayName;
                        }
                    }

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = 0;
                    var PageNum = 0;
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, out itotal, out idisplaytotal);

                    return File(CSVExportHelper.ExportCSV(Items,properties), "application/vnd.ms-excel", ViewData["Report_Name"] + ".csv");
                }
                return File(Encoding.UTF8.GetBytes("No Access"), "application/vnd.ms-excel", "Error.csv");
            }
            catch (Exception ex)
            {
                return File(Encoding.UTF8.GetBytes("Error:" + ex.ToString()), "application/vnd.ms-excel", "Error.csv");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public FileResult ExportExcell()
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
                return File(Encoding.UTF8.GetBytes("Authentification Error"), ExcelExportHelper.ExcelContentType, "Error.xlsx");
            }
            try
            {
                var sSearch = "";
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                if (MenuItems != null)
                {
                    var usr = Authentication.GetCurrentUser();
                    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    {
                        return File("", "application/ms-excel");
                    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc, Authentication.GetCurrentUser());

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", LIB.AdvancedProperties.DisplayMode.PrintSearch));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }

                    sSearch = item.SimpleSearch(sSearch);

                    var DBProperties = Field.LoadByPage(item.GetType().FullName);
                    AdvancedProperties properties = null; ;

                    if (Session["Display_" + item.GetType().Name] != null)
                    {
                        properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), (List<string>)Session["Print_" + item.GetType().Name]);
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        properties = new AdvancedProperties();
                        var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        foreach (AdvancedProperty property in tprops)
                        {
                            if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                            {
                                var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                                property.Common.DisplayName = DBField.Name;
                                property.Common.PrintName = DBField.PrintName;

                                if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == GofraLib.BusinessObjects.DisplayMode.Print) && usr.HasAtLeastOnePermission(DBField.Permission) && Session["Print_" + item.GetType().Name] == null)
                                {
                                    properties.Add(property);
                                }
                            }
                        }
                        properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetPrintProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                        if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                        {
                            ViewData["Report_Name"] = boproperties.DisplayName;
                        }
                    }

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = 0;
                    var PageNum = 0;
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, out itotal, out idisplaytotal);
                    
                    return File(ExcelExportHelper.ExportExcel(Items, properties, ViewData["Report_Name"].ToString()), ExcelExportHelper.ExcelContentType, ViewData["Report_Name"] + ".xlsx");
                }
                return File(Encoding.UTF8.GetBytes("No Access"), ExcelExportHelper.ExcelContentType, "Error.xlsx");
            }
            catch (Exception ex)
            {
                return File(Encoding.UTF8.GetBytes("Error:" + ex.ToString()), ExcelExportHelper.ExcelContentType, "Error.xlsx");
            }

        }
    }
}
