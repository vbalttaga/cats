// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayMode.cs" company="Gofra">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The DisplayMode.
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

    /// <inheritdoc />
    /// <summary>
    /// The DisplayMode.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.System
      , ModulesAccess = (long)(Modulesenum.SMI)
      , DisplayName = "Moduri de afișare"
      , SingleName = "Mod de afișare"
      , Icon = "file-pdf-o")]
    public class DisplayMode : ItemBase
    {
        public static DisplayMode Simple = new DisplayMode(1);
        public static DisplayMode Advanced = new DisplayMode(2);
        public static DisplayMode Search = new DisplayMode(3);
        public static DisplayMode AdvancedEdit = new DisplayMode(4);
        public static DisplayMode Print = new DisplayMode(5);
        public static DisplayMode PrintSearch = new DisplayMode(6);
        public static DisplayMode CSV = new DisplayMode(7);
        public static DisplayMode Excell = new DisplayMode(8);

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMode"/> class.
        /// </summary>
        public DisplayMode()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayMode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public DisplayMode(long id)
            : base(id)
        {
        }
        #endregion

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Common(Order = 1), Template(Mode = Template.Number),
        Access(EditableFor = (long)BasePermissionenum.SuperAdmin, VisibleFor = (long)BasePermissionenum.SuperAdmin)]
        public long Value { get; set; }

        #endregion
        
        public static Dictionary<long, ItemBase> FromDataTable(DataRow[] dt, DataSet ds)
        {
            var displayModes = new Dictionary<long, ItemBase>();
            foreach (var dr in dt)
            {
                var obj = (new DisplayMode()).FromDataRow(dr);
                if (!displayModes.ContainsKey(obj.Id))
                {
                    displayModes.Add(obj.Id, obj);
                }
            }

            return displayModes;
        }
    }
}