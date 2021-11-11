// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuItem.cs" company="Gofra">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The MenuItem.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.BusinessObjects;
    using LIB.Tools.BO;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using LIB.Tools.Utils;
    using System.Data.SqlClient;
    using LIB.Tools.AdminArea;

    /// <summary>
    /// The MenuItem.
    /// </summary>
    [Serializable]
    [Bo(ModulesAccess = (long)(Modulesenum.ControlPanel)
      , DisplayName = "Optii meniu"
      , SingleName = "Optii meniu"
      , LogRevisions = true)]
    public class MenuItem : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemItem"/> class.
        /// </summary>
        public MenuItem()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemItem"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public MenuItem(long id)
            : base(id)
        {
        }
        #endregion

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Common(Order = 1), Template(Mode = Template.SearchDropDown)]
        public MenuType MenuType { get; set; }

        [Common(Order = 2), Template(Mode = Template.SearchDropDown), LookUp(DefaultValue = true)]
        public Page Page { get; set; }

        [Common(Order = 3), Template(Mode = Template.PermissionsSelector), Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Advanced)]
        public long Permission { get; set; }

        [Common(Order = 2), Template(Mode = Template.String)]
        public string Object { get; set; }

        [Common(Order = 2), Template(Mode = Template.String)]
        public string Namespace { get; set; }

        [Common(Order = 4), Template(Mode = Template.CheckBox)]
        public bool Visible { get; set; }

        [Common(Order = 5), Template(Mode = Template.Number)]
        public int SortOrder { get; set; }

        [Common(Order = 6), Template(Mode = Template.ParentDropDown)]
        public MenuGroup MenuGroup { get; set; }

        #endregion
        
        #region Populate Methods

        /// <summary>
        /// The from data table.
        /// </summary>
        /// <param name="dt">
        /// The data table.
        /// </param>
        /// <param name="ds">
        /// The data set.
        /// </param>
        /// <param name="usr">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<long, ItemBase> FromDataTable(DataRow[] dt, DataSet ds, LIB.BusinessObjects.User usr)
        {
            var MenuItems = new Dictionary<long, ItemBase>();
            foreach (var dr in dt)
            {
                var obj = (new MenuItem()).FromDataRow(dr);
                if (!MenuItems.ContainsKey(obj.Id))
                {
                    MenuItems.Add(obj.Id, obj);
                }
            }

            return MenuItems;
        }

        public static Dictionary<long, MenuItem> PopulateRecentMenu(LIB.BusinessObjects.User usr, string Recent)
        {
            var items = new Dictionary<long, MenuItem>();

            if (!string.IsNullOrEmpty(Recent))
            {
                const string StrSql = "MenuItem_PopulateRecent";

                var conn = DataBase.ConnectionFromContext();

                var Command = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };
                Command.Parameters.Add(new SqlParameter("@Recent", SqlDbType.NVarChar, 1000) { Value = Recent.Replace("%2C", ",") });
                if (usr!=null)
                    Command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.BigInt) { Value = usr.Id });

                var ds = new DataSet();

                var da = new SqlDataAdapter();

                da.SelectCommand = Command;
                da.Fill(ds);

                ds.Tables[0].TableName = "MenuItem";

                foreach (DataRow dr in ds.Tables["MenuItem"].Rows)
                {
                    var lMenuItem = (MenuItem)(new MenuItem()).FromDataRow(dr);
                    items.Add(lMenuItem.Id, lMenuItem);
                }
            }

            return items;
        }

        /// <summary>
        /// The from data table.
        /// </summary>
        /// <param name="dt">
        /// The data table.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<long, ItemBase> FromDataTable(DataRow[] dt)
        {
            return dt.Select(dr => (new MenuItem()).FromDataRow(dr)).ToDictionary(obj => obj.Id);
        }

        #endregion

        #region Utils

        public string BuildUrl()
        {
            var Controller = BuildController();

            var Url = URLHelper.GetUrl(Controller + ((this.Page != null && this.Page.Id>0) ? this.Page.PageObject.Type.Name: "Edit/"+this.Object+"/"+this.Namespace));
            
            return Url;
        }

        public string BuildController()
        {
            return (this.Page != null && this.Page.Id>0) ? this.Page.MenuType.Controller + "/" : "ControlPanel/";
        }

        public string BuildIco()
        {
            return "sub-menu-" + MenuType.Alias;
        }
        #endregion
    }
}