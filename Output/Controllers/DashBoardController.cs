using LIB.BusinessObjects;
using LIB.Helpers;
using LIB.Tools.BO;
using LIB.Tools.Security;
using LIB.Tools.Utils;
using Gofra.Models.Objects;
using GofraLib.BusinessObjects;
using Gofraweblib.Dashboards;
using Gofraweblib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using weblib.Dashboards;
using Weblib.Controllers;

namespace Gofra.Controllers
{
    public class DashBoardController : FrontEndController
    {
        //
        // GET: /DashBoard/
        private IDashboard Dashboard
        {
            get
            {
                var currentUser = Authentication.GetCurrentUser();

                if (currentUser.HasAtLeastOnePermission((long)BasePermissionenum.SuperAdmin))
                {
                    return new UserAdmin();
                }


                return null;
            }
        }

        public ActionResult Index()
        {
            var currentUser = Authentication.GetCurrentUser();

            if (currentUser == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            ViewData["MainMenu"] = GofraLib.BusinessObjects.MenuGroup.Populate(currentUser);
            var Recent = System.Web.HttpContext.Current.Request.Cookies[currentUser.Id.ToString() + "_recentmenuitems"];
            ViewData["RecentMenu"] = MenuItem.PopulateRecentMenu(currentUser, Recent != null ? Recent.Value : "");

            ViewData["DashboardListCount"] = 0;

            if (Dashboard != null)
                Dashboard.Load(ViewData, currentUser);            

            return View();
        }

        public ActionResult Refresh(long lastId, int count)
        {
            var currentUser = Authentication.GetCurrentUser();

            var Data = new Dictionary<string, object>();

            if (Dashboard != null)
                Data= Dashboard.RefreshWidget(ViewData, ControllerContext, TempData, currentUser, lastId, count,Request.Form["widgetitems"]);

            return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
        }
        
    }
}
