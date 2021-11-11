using LIB.BusinessObjects;
using LIB.Tools.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace weblib.Dashboards
{
    public class BaseDashboard: IDashboard
    {
        public virtual void Load(ViewDataDictionary ViewData, User currentUser)
        {
            throw new NotImplementedException();
        }

        public virtual Dictionary<string, object> RefreshWidget(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, User currentUser, long lastId, int count, string widgetitems)
        {
            throw new NotImplementedException();
        }

        public void LoadNewListItems(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, Dictionary<string, object> Data, string control)
        {
            var viewName = "~/Views/DashBoard/Controls/"+ control + ".cshtml";
            using (StringWriter sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                Data.Add("List", sw.GetStringBuilder().ToString());
            }
        }
        public void LoadChart(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, Dictionary<string, object> Data)
        {
            var viewName = "~/Views/DashBoard/Controls/_Chart.cshtml";
            using (StringWriter sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                Data.Add("ChartData", sw.GetStringBuilder().ToString());
            }
        }
        public void LoadNotification(Dictionary<string, object> Data, string Title, string Body, string Id, string url, string icoUrl = "")
        {
            if (string.IsNullOrEmpty(icoUrl))
                icoUrl = URLHelper.GetUrl("Images/simplified/logo_dark.png");

            var Notification = new Dictionary<string, object>();
            Notification.Add("Title", Title);
            Notification.Add("Body", Body);
            Notification.Add("Id", Id);
            Notification.Add("URL", url);
            Notification.Add("Thumb", icoUrl);
            Data.Add("Notification", Notification);
        }
        public void LoadListTotals(Dictionary<string, object> Data, long ItemId, int ItemCount)
        {
            Data.Add("ItemId", ItemId);
            Data.Add("ItemCount", ItemCount);
        }
    }
}
