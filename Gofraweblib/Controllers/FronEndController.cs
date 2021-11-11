// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Account.cs" company="GalexStudio">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gofraweblib.Controllers
{
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using LIB.BusinessObjects;
    using LIB.Tools.Security;
    using LIB.Tools.Utils;

    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;

    using GofraLib.BusinessObjects;
    using System;
    using GofraLib;
    using LIB.Tools.BO;

    /// <summary>
    /// The account controller.
    /// </summary>
    [AuthAction]
    public class FrontEndController : Weblib.Controllers.FrontEndController
    {
        public FrontEndController()
            : base()
        {
            if (System.Web.HttpContext.Current.Session[SessionItems.Person] == null && Authentication.GetCurrentUser()!=null)
            {
                GofraLib.BusinessObjects.Person.AddPersonInfo(Authentication.GetCurrentUser());
            }
            ViewBag.Title = "Gofra";
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

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"]))
            {
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
        }

        public void LoadMenu(string Model)
        {
            var usr = Authentication.GetCurrentUser();
            if (ViewData["MainMenu"] != null && ViewData["MainMenu"] is Dictionary<long, GofraLib.BusinessObjects.MenuGroup>)
            {
                var menues = (Dictionary<long, GofraLib.BusinessObjects.MenuGroup>)ViewData["MainMenu"];
                var menuGroup = menues.Values.FirstOrDefault(mg => mg.MenuItems != null && mg.MenuItems.Values.Any(m => ((MenuItem)m).Page != null && ((MenuItem)m).Page.PageObject != null && ((MenuItem)m).Page.PageObject.Type != null && ((MenuItem)m).Page.PageObject.Type.Name == Model));
                if (menuGroup != null)
                {
                    var menuitem = menuGroup.MenuItems.Values.FirstOrDefault(m => ((MenuItem)m).Page!=null && ((MenuItem)m).Page.PageObject!=null && ((MenuItem)m).Page.PageObject.Type.Name == Model);
                    string menuitems = string.Empty;
                    if (Request.Cookies[usr.Id.ToString() + "_recentmenuitems"] != null)
                    {
                        menuitems = Request.Cookies[usr.Id.ToString() + "_recentmenuitems"].Value.ToString();
                    }
                    if (!string.IsNullOrEmpty(menuitems))
                    {
                        if (!menuitems.Split(',').Any(s => s == menuitem.Id.ToString()) || (menuitems.IndexOf(",") == -1 && menuitems != menuitem.Id.ToString()))
                        {
                            menuitems += "," + menuitem.Id.ToString();
                        }
                        if (menuitems.Split(',').Length > 10)
                        {
                            var arrmenuitems = menuitems.Split(',');
                            var newids = string.Empty;
                            for (int i = arrmenuitems.Length - 1; i >= arrmenuitems.Length - 11; i--)
                            {
                                newids += arrmenuitems[i] + ",";
                            }
                            menuitems = newids.Remove(newids.Length - 2);
                        }
                    }
                    else
                    {
                        menuitems = menuitem.Id.ToString();
                    }
                    Response.Cookies[usr.Id.ToString() + "_recentmenuitems"].Value = menuitems;
                    Response.Cookies[usr.Id.ToString() + "_recentmenuitems"].Expires = DateTime.Now.AddDays(7); // add expiry time
                }
            }
        }
    }
}
