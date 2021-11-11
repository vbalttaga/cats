// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Manual.cs" company="Galex">
//   Copyright ©  2018
// </copyright>
// <summary>
//   The static translations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects {
    using System;

    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.AdminArea;
    using LIB.BusinessObjects;
    using LIB.Tools.Utils;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// The Contact.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Settings
      , ModulesAccess = (long)(Modulesenum.SMI)
      , DisplayName = "Manuals"
      , SingleName = "Manual"
      )
    ]
    public class Manual : ItemBase
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Manual"/> class.
        /// </summary>
        public Manual()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Manual"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Manual(long id)
            : base(id)
        {
        }
        #endregion
        #region properties

        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Template(Mode = Template.SearchDropDown)]
        public Language Language { get; set; }

        [Template(Mode = Template.SearchDropDown)]
        public Role Role { get; set; }
        
        [Template(Mode = Template.Document)]
        public Document Document { get; set; }

        #endregion

        public static Manual GetManualsByRoleAndLanguage(Role role, Language lang)
        {

            Manual manual = new Manual() { Document = new Document() };

            const string StrSql = "Manuals_Populate";

            var conn = DataBase.ConnectionFromContext();

            var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.BigInt) { Value = role.Id });
            cmd.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.BigInt) { Value = lang.Id });

            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    manual = (Manual)new Manual().FromDataRow(rdr);

                    manual.Document = (Document)manual.Document.PopulateOne(manual.Document);
                    return manual;
                }

                rdr.Close();
            }

            return manual;
        }
    }
}