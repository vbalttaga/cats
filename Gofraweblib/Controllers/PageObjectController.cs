// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageObjectController.cs" company="GalexStudio">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gofraweblib.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Linq;

    using LIB.BusinessObjects;

    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Collections.Generic;
    using GofraLib.BusinessObjects;
    using LIB.Tools.Security;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class PageObjectController : Gofraweblib.Controllers.FrontEndController
    {
        public PageObjectController()
            : base()
        {
            var usr = Authentication.GetCurrentUser();
            if (usr != null)
            {
                Dictionary<long, MenuGroup> menues = null;
                if (Session != null && Session["MainMenu"] != null)
                {
                    menues = (Dictionary<long, MenuGroup>)Session["MainMenu"];
                }
                else
                {
                    menues = GofraLib.BusinessObjects.MenuGroup.Populate(usr);
                    if(Session!=null)
                        Session["MainMenu"] = menues;
                }
                ViewData["MainMenu"] = menues;
                var Recent = System.Web.HttpContext.Current.Request.Cookies[usr.Id.ToString() + "_recentmenuitems"];
                Dictionary<long, MenuItem> RecentMenu = new Dictionary<long, MenuItem>();

                if (Recent != null && Recent.Values.Count > 0)
                {

                    foreach (var menuGroup in menues.Values)
                    {
                        foreach (var omenuItem in menuGroup.MenuItems.Values)
                        {
                            foreach (var id in Recent.Values)
                            {
                                if (Convert.ToInt64(id) == omenuItem.Id)
                                {
                                    if (!RecentMenu.ContainsKey(omenuItem.Id))
                                    {
                                        RecentMenu.Add(omenuItem.Id, (MenuItem)omenuItem);
                                    }
                                }
                            }
                        }
                    }
                }
                ViewData["RecentMenu"] = RecentMenu;
            }
        }

    }
}
