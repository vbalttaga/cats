// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Field.cs" company="Gofra">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The Field.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.BusinessObjects;
    using LIB.Tools.BO;
    using System.Collections.Generic;
    using LIB.Tools.AdminArea;
    using System.Web;
    using LIB.Tools.Utils;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// The Contact.
    /// </summary>
    [Serializable]
    [Bo(ModulesAccess = (long)(Modulesenum.SMI)
       , DisplayName = "Fields"
       , SingleName = "Field"
       , LogRevisions = true)]
    public class Field : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        public Field()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Field"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Field(long id)
            : base(id)
        {
        }
        #endregion

        #region Properties

        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Template(Mode = Template.Name)]
        public string FieldName { get; set; }

        [Template(Mode = Template.ParentDropDown)]
        public Page Page { get; set; }

        [Template(Mode = Template.MultiCheck), MultiCheck(ItemType = typeof(DisplayMode))]
        public Dictionary<long, ItemBase> DisplayModes { get; set; }

        [Template(Mode = Template.String)]
        public string PrintName { get; set; }

        [Template(Mode = Template.PermissionsSelector), Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Advanced)]
        public long Permission { get; set; }

        #endregion

        #region Populate

        public static Dictionary<long, Field> LoadByPage(string PageObjectId)
        {
            Dictionary<long, Field> Fields;

            var conn = DataBase.ConnectionFromContext();

            Fields = new Dictionary<long, Field>();

            var cmd = new SqlCommand("Field_Populate_Page", conn) { CommandType = CommandType.StoredProcedure };

            var ds = new DataSet();

            var da = new SqlDataAdapter();

            cmd.Parameters.Add(new SqlParameter("@PageObjectId", SqlDbType.NVarChar,50) { Value = PageObjectId });

            da.SelectCommand = cmd;
            da.Fill(ds);

            ds.Tables[0].TableName = "Fields";
            ds.Tables[1].TableName = "DisplayModes";
            
            ds.Relations.Add(
            ds.Tables["Fields"].Columns["FieldId"],
            ds.Tables["DisplayModes"].Columns["FieldId"]);

            ds.Relations[0].Nested = true;

            ds.Relations[0].RelationName = "DisplayModes";

            foreach (DataRow dr in ds.Tables["Fields"].Rows)
            {
                var obj = (Field)(new Field()).FromDataRow(dr);
                var datarowsDisplayModes = dr.GetChildRows(ds.Relations["DisplayModes"]);
                obj.DisplayModes = DisplayMode.FromDataTable(datarowsDisplayModes, ds);
                Fields.Add(obj.Id, obj);
            }

            return Fields;
        }

        #endregion
    }
}