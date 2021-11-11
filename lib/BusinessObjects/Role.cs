// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Role.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The role.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools;
    using LIB.Tools.AdminArea;
    using System.Collections.Generic;
    using Tools.Utils;
    using System.Data;
    using System.Data.SqlClient;
    using Tools.Security;
    using Helpers;

    /// <summary>
    /// The role.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.UserManagement
      , ModulesAccess = (long)Modulesenum.SMI
      , DisplayName = "Roluri"
      , SingleName = "Roluri"
      , EditAccess = (long)BasePermissionenum.SuperAdmin
      , CreateAccess = (long)BasePermissionenum.SuperAdmin
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , ReadAccess = (long)BasePermissionenum.SuperAdmin
      , LogRevisions = true
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon = "users"
      )
    ]
    public class Role : AggregateBase
    {

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
            : base(0)
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Role(long id)
            : base(id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="permission">
        /// The permission.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Role(long permission, int id, string name)
        {
            this.Permission = permission;
            this.Id = id;
            this.Name = name;
        }
        #endregion

        #region Role Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        [Common(Order = 1)]
        [Template(Mode = Template.PermissionsSelector)]
        public long Permission { get; set; }

        /// <summary>
        /// Gets or sets the permissions req for user to create user with this permision.
        /// </summary>
        [Common(Order = 1, DisplayName = "Permisiune de a crea utilizator cu aceste permisiuni")]
        [Template(Mode = Template.PermissionsSelector),Access(DisplayMode=DisplayMode.Advanced)]
        public long RoleAccessPermission { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        [Common(Order = 2), Template(Mode = Template.Image), Image(ThumbnailWidth = 160, ThumbnailHeight = 160)]
        public Graphic Avatar { get; set; }

        [Common(EditTemplate = EditTemplates.Hidden), Db(_Editable = false, _Populate = false)]
        public int UserCount { get; set; }
        #endregion

        public override int GetCount()
        {
            return UserCount;
        }

        /// <summary>
        /// The has permissions.
        /// </summary>
        /// <param name="binaryFlags">
        /// The p binary flags.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasPermissions(long binaryFlags)
        {
            return Permissions.HasPermissions(this.Permission,binaryFlags);
        }

        /// <summary>
        /// The has at least one permission.
        /// </summary>
        /// <param name="binaryFlags">
        /// The Binary Flags.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasAtLeastOnePermission(long binaryFlags)
        {
            return Permissions.HasAtLeastOnePermission(this.Permission, binaryFlags);
        }
        
        public override string GetAdditionalSelectQuery(AdvancedProperty property)
        {
            return ",[" + property.PropertyName + "].RoleAccessPermission AS " + property.PropertyName + "RoleAccessPermission";
        }

        public Dictionary<long, AggregateBase> LoadUsersPerRoles()
        {
            var conn = DataBase.ConnectionFromContext();

            Dictionary<long, AggregateBase> Roles = new Dictionary<long, AggregateBase>();

            var cmd = new SqlCommand("Role_Populate_Users", conn) { CommandType = CommandType.StoredProcedure };

            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var role = (Role)(new Role()).FromDataRow(rdr);
                    Roles.Add(role.Id, role);
                }

                rdr.Close();
            }

            return Roles;
        }
    }
}