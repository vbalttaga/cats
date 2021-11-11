using LIB.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace weblib.Dashboards
{
    public interface IDashboard
    {
        void Load(ViewDataDictionary ViewData, User currentUser);

        Dictionary<string, object> RefreshWidget(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, User currentUser, long lastId, int count, string widgetitems);
    }
}
