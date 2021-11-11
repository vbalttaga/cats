// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Permission.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the Permission type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using LIB.Tools.AdminArea;
    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using System.Collections.Generic;

    /// <summary>
    /// The permissions.
    /// </summary>
    [Bo(Group = AdminAreaGroupenum.UserManagement
      , ModulesAccess = (long)Modulesenum.SMI
      , DisplayName = "Nivel de acces"
      , SingleName = "Nivel de acces"
      , EditAccess = (long)BasePermissionenum.SuperAdmin
      , CreateAccess = (long)BasePermissionenum.SuperAdmin
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , ReadAccess = (long)BasePermissionenum.SuperAdmin
      , LogRevisions = false
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon ="laptop")]
    public class Permission : ItemBase
    {
        #region Static Permission

        /// <summary>
        /// The none.
        /// </summary>
        public static readonly Permission None = new Permission(0, 0);

        /// <summary>
        /// The administrator area access.
        /// </summary>
        public static readonly Permission CPAccess = new Permission(1, 1) { Name = "Control Panel Access" };

        /// <summary>
        /// The super admin.
        /// </summary>
        /// 
        public static readonly Permission SuperAdmin = new Permission(2,2) { Name = "Super Administrator" };

        public static readonly Permission SMIAccess = new Permission(3, 4) { Name = "Sistem Management Interface Access" };


        #endregion

        public override string GetName()
        {
            return Name;
        }
        public static Dictionary<long, ItemBase> LoadPermissions(long Permissions)
        {
            var Perms = new Dictionary<long, ItemBase>();

            if (Tools.Security.Permissions.HasPermissions(Permissions, CPAccess.Value))
                Perms.Add(CPAccess.Id, CPAccess);
            if (Tools.Security.Permissions.HasPermissions(Permissions, SuperAdmin.Value))
                Perms.Add(SuperAdmin.Id, SuperAdmin);           
            if (Tools.Security.Permissions.HasPermissions(Permissions, SMIAccess.Value))
                Perms.Add(SMIAccess.Id, SMIAccess);

            return Perms;
        }
        public static Permission LoadPermission(long id)
        {
            if (CPAccess.Id == id)
                return CPAccess;
            if (SuperAdmin.Id == id)
                return SuperAdmin;
            if (SMIAccess.Id == id)
                return SMIAccess;

            return None;
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class.
        /// </summary>
        public Permission()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Permission(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public Permission(long id, long value)
            : base(id)
        {
            this.Value = value;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Common(Order = 0), Template(Mode = Template.Name)]
        [Validation(ValidationType = ValidationTypes.Required),
         Access(DisplayMode = DisplayMode.Search | DisplayMode.Simple | DisplayMode.Advanced,
             EditableFor = (long)BasePermissionenum.SuperAdmin)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Common(Order = 1), Template(Mode = Template.Number),
        Access(EditableFor = (long)BasePermissionenum.SuperAdmin, VisibleFor = (long)BasePermissionenum.SuperAdmin)]
        public long Value { get; set; }

        #endregion
    }
}
