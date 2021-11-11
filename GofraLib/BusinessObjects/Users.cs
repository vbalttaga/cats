// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Users.cs" company="Gofra">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.BusinessObjects;
    using System.Collections.Generic;
    using LIB.Tools.Utils;
    using System.Data.SqlClient;
    using System.Data;
    using LIB.Tools.BO;
    using LIB.Tools.AdminArea;
    using LIB.Tools.Security;
    using System.Web;

    /// <summary>
    /// The user.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.UserManagement
      , ModulesAccess = (long)(Modulesenum.SMI)
      , DisplayName = "Utilizatori"
      , SingleName = "Utilizator"
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , LogRevisions = true
      , AllowCopy = false
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon = "user"
      )
    ]
    public class User : LIB.BusinessObjects.User
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public User(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        public User(string login, string password)
            : base(login, password)
        {
        }
        #endregion
        

        #region Properties

        [Common(Order = 4)]
        [Template(Mode = Template.LinkItems), LinkItem(LinkType = typeof(Message))]
        public Dictionary<long, ItemBase> Messages { get; set; }

        #endregion

        #region UPDATE Methods

        public static bool DoAuthorizationByCNP(string CNP, Modulesenum module = Modulesenum.BaseModule)
        {
            var conn = DataBase.ConnectionFromContext();

            const string StrSql = "User_DoLoginByCNP";

            var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.Add(new SqlParameter("@CNP", SqlDbType.NVarChar, 50) { Value = CNP });

            using (var rdrUsers = cmd.ExecuteReader())
            {
                while (rdrUsers.Read())
                {
                    var user = (User)(new User()).FromDataRow(rdrUsers);

                    switch (module)
                    {
                        case Modulesenum.ControlPanel:
                            if (!user.HasPermissions((long)BasePermissionenum.CPAccess))
                            {
                                return false;
                            }
                            break;

                        case Modulesenum.SMI:
                            if (!user.HasPermissions((long)BasePermissionenum.SMIAccess))
                            {
                                return false;
                            }
                            break;
                    }

                    Authentication.UpdateLastLogin(conn, user);
                    var context = new HttpContextWrapper(HttpContext.Current);
                    Authentication.InsertUserToSession(user, context);

                    return true;
                }
            }

            return false;
        }

        public static void UpdatePassword(LIB.BusinessObjects.User usr)
        {
            var conn = DataBase.ConnectionFromContext();
            var cmd = new SqlCommand("User_UpdatePassword", conn) { CommandType = CommandType.StoredProcedure };
            var param = new SqlParameter("@UserId", SqlDbType.BigInt) { Value = usr.Id };
            cmd.Parameters.Add(param);
            param = new SqlParameter("@Password", SqlDbType.NVarChar, 50) { Value = usr.GetpasswordHash() };
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
        }
        #endregion

        #region POPULATE Methods

        /// <summary>
        /// The populate.
        /// </summary>
        /// <param name="conn">
        /// The connection.
        /// </param>
        /// <param name="usr">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static User Populate(LIB.BusinessObjects.User usr)
        {
            const string StrSql = "User_Populate";

            var conn = DataBase.ConnectionFromContext();

            var selectCommand = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

            var ds = new DataSet();

            var param = new SqlParameter("@UserId", SqlDbType.Int);
            param.Value = usr.Id;
            selectCommand.Parameters.Add(param);

            var da = new SqlDataAdapter();
            da.SelectCommand = selectCommand;
            da.Fill(ds);

            ds.Tables[0].TableName = "User";
            ds.Tables[1].TableName = "UserMessages";

            ds.Relations.Add(
            ds.Tables["User"].Columns["UserId"],
            ds.Tables["UserMessages"].Columns["UserId"]);

            ds.Relations[0].Nested = true;

            ds.Relations[0].RelationName = "User_Messages";

            var user = new User();

            foreach (DataRow dr in ds.Tables["User"].Rows)
            {
                user = (User)(new User()).FromDataRow(dr);
                user.Messages = new Dictionary<long, ItemBase>();
                var datarowsMenues = dr.GetChildRows(ds.Relations["User_Messages"]);
                user.Messages = Message.FromDataTable(datarowsMenues, ds, usr);              
                return user;
            }

            return user;
        }


        #endregion
    }
}