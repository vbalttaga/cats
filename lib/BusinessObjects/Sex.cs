// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sex.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the Sex type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.AdminArea;

    /// <summary>
    /// The sex.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.UserManagement
      , ModulesAccess = (long)Modulesenum.SMI
      , DisplayName = "Sex"
      , SingleName = "Sex"
      , EditAccess = (long)BasePermissionenum.SuperAdmin
      , CreateAccess = (long)BasePermissionenum.SuperAdmin
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , ReadAccess = (long)BasePermissionenum.SuperAdmin
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon = "intersex"
      )
    ]
    public class Sex : ItemBase
    {
        #region Static Sex
        
        /// <summary>
        /// The male.
        /// </summary>
        public static readonly Sex Male = new Sex(1, "Male");

        /// <summary>
        /// The female.
        /// </summary>
        public static readonly Sex Female = new Sex(2, "Female");

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sex"/> class.
        /// </summary>
        public Sex()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sex"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Sex(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sex"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Sex(int id, string name)
            : base(id)
        {
            this.Name = name;
        }

        #endregion

        #region Sex Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Common(Order = 0), Template(Mode = Template.TranslatableName)]
        [Validation(ValidationType = ValidationTypes.Required),
         Access(DisplayMode = DisplayMode.Search | DisplayMode.Simple | DisplayMode.Advanced,
             EditableFor = (long)BasePermissionenum.SuperAdmin)]
        public string Name { get; set; }
        
        #endregion        
        
    }
}