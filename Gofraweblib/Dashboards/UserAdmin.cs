using LIB.BusinessObjects;
using LIB.Tools.Utils;
using Gofraweblib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using weblib.Dashboards;

namespace Gofraweblib.Dashboards
{
    public class UserAdmin : BaseDashboard
    {
        public DashboardEnum DashboardType = DashboardEnum.UserAnmin;
        public override void Load(ViewDataDictionary ViewData, User currentUser)
        {
            ViewData["List"] = currentUser.LoadLatests();
            ViewData["GroupedList"] = currentUser.Role.LoadUsersPerRoles();
            ViewData["Dashboard"] = DashboardType;
            ViewData["DashboardControl"] = "_UserList";
        }

        public override Dictionary<string, object> RefreshWidget(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, User currentUser, long lastId, int count, string widgetitems)
        {
            ViewData["DashboardListCount"] = count;
            ViewData["Dashboard"] = DashboardType;

            var Data = new Dictionary<string, object>();

            var List = currentUser.LoadLatests(lastId);

            if (List != null)
            {
                ViewData["List"] = List;
                LoadNewListItems(ViewData, ControllerContext, TempData, Data, "_UsersRows");

                if (List.Count > 0)
                {
                    ViewData["GroupedList"] = currentUser.Role.LoadUsersPerRoles();

                    LoadChart(ViewData, ControllerContext, TempData, Data);
                    if (List.Values.Any(r => r.CreatedBy.Id != currentUser.Id))
                    {
                        var item = List.Values.First(r => r.CreatedBy.Id != currentUser.Id);
                        LoadNotification(Data, "New user: " + item.Login, item.Person.GetName(), item.Id.ToString(), URLHelper.GetUrl("SystemManagement/EditItem/User/GofraLib.BusinessObjects/" + item.Id));
                    }
                    LoadListTotals(Data, List.Values.First().Id, count + List.Count);
                }
            }
            LoadChangedData(Data, ViewData, ControllerContext, TempData, currentUser, lastId, count, widgetitems);

            return Data;
        }
        private void LoadChangedData(Dictionary<string, object> Data, ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, User currentUser, long lastId, int count, string widgetitems)
        {
            var List = currentUser.LoadLatests();
            if (List.Count > 0)
            {
                Data.Add("DataList", new List<Dictionary<string, object>>());
                foreach (var usr in List.Values)
                {
                    var reqData = new Dictionary<string, object>();

                    reqData.Add("Id", usr.Id);
                    reqData.Add("Login", usr.Login);
                    reqData.Add("Person", usr.Person.GetName());

                    ((List<Dictionary<string, object>>)Data["DataList"]).Add(reqData);
                }

                var ChangedList = new Dictionary<long, LIB.BusinessObjects.User>();

                if (!string.IsNullOrEmpty(widgetitems))
                {
                    foreach (var item in widgetitems.Split(','))
                    {
                        if (List.ContainsKey(Convert.ToInt64(item.Split(':')[0])) && List[Convert.ToInt64(item.Split(':')[0])].Role.Id != Convert.ToInt64(item.Split(':')[1]))
                        {
                            ChangedList.Add(List[Convert.ToInt64(item.Split(':')[0])].Id, List[Convert.ToInt64(item.Split(':')[0])]);
                        }
                    }
                }

                if (ChangedList.Values.Any(r => r.Id > 0))
                {
                    if (!Data.ContainsKey("ChartData"))
                    {
                        LoadChart(ViewData, ControllerContext, TempData, Data);
                    }

                    Data.Add("ChangedDataList", new List<Dictionary<string, object>>());
                    foreach (var usr in ChangedList.Values)
                    {
                        var reqData = new Dictionary<string, object>();

                        reqData.Add("Id", usr.Id);
                        reqData.Add("RoleId", usr.Role.Id);
                        reqData.Add("RoleName", usr.Role.Name);
                        reqData.Add("Login", usr.Login);
                        reqData.Add("Person", usr.Person.GetName());

                        ((List<Dictionary<string, object>>)Data["ChangedDataList"]).Add(reqData);
                    }

                    if (!Data.ContainsKey("Notification"))
                    {
                        if (ChangedList.Values.Any(r => r.UpdatedBy.Id != currentUser.Id))
                        {
                            var item = List.Values.First(r => r.CreatedBy.Id != currentUser.Id);
                            LoadNotification(Data, item.Login, item.Person.GetName(), item.Id.ToString(), URLHelper.GetUrl("SystemManagement/EditItem/User/GofraLib.BusinessObjects/" + item.Id));
                        }
                    }
                }
            }
        }
    }
}
