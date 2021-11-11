using LIB.AdvancedProperties;
using LIB.Tools.Controls;
using GofraLib.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gofra.Models.Reports
{
    [Bo(DisplayName = "Lista Utilizatorilor")]
    public class UserList : ReportBase
    {
        public UserList()
            : base()
        {

        }

        public override string GetLink()
        {
            return "SystemManagement/EditItem/User/GofraLib.BusinessObjects/" + UserId.ToString();
        }

        [Common(DisplayName = "ID", _Sortable = true), Template(Mode = Template.Name), Db(Sort = DbSortMode.Desc, _Populate = false)]
        public long UserId { get; set; }

        [Template(Mode = Template.LinkItem), Common(DisplayName = "Login", _Sortable = true)
            , Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced | LIB.AdvancedProperties.DisplayMode.Print)]
        public User User { get; set; }

        [Common(DisplayName = "Person", _Sortable = true), Template(Mode = Template.ParentSelectList)
            , Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced | LIB.AdvancedProperties.DisplayMode.Search | LIB.AdvancedProperties.DisplayMode.Print)]
        public Person Person { get; set; }

        [Common(DisplayName = "Role", _Sortable = true), Template(Mode = Template.ParentDropDown)
            , Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced | LIB.AdvancedProperties.DisplayMode.Search | LIB.AdvancedProperties.DisplayMode.Print)]
        public LIB.BusinessObjects.Role Role { get; set; }

        [Common(EditTemplate = EditTemplates.DateRange, DisplayName = "LastLogin", _Sortable = true, _Searchable = true), LookUp(DefaultValue = true)
       , Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced | LIB.AdvancedProperties.DisplayMode.Search | LIB.AdvancedProperties.DisplayMode.Print)]
        public DateRange LastLogin { get; set; }

        [Common(EditTemplate = EditTemplates.DateRange, DisplayName = "DateCreated", _Sortable = true, _Searchable = true), LookUp(DefaultValue = true)
       , Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced | LIB.AdvancedProperties.DisplayMode.Search | LIB.AdvancedProperties.DisplayMode.Print)]
        public DateRange DateCreated { get; set; }

        [Template(Mode = Template.LinkItem), Common(DisplayName = "CreatedBy", _Sortable = true)
            , Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced | LIB.AdvancedProperties.DisplayMode.Print)]
        public User CreatedBy { get; set; }
    }
}