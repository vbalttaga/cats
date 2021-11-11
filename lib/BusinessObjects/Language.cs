// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the Language type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.Utils;
    using LIB.Tools.AdminArea;

    /// <summary>
    /// The language.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Translate
      , ModulesAccess = (long)Modulesenum.SMI
      , DisplayName = "Languages"
      , SingleName = "Language"
      , EditAccess = (long)BasePermissionenum.SuperAdmin
      , CreateAccess = (long)BasePermissionenum.SuperAdmin
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , ReadAccess = (long)BasePermissionenum.SuperAdmin
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon = "language"
      )
    ]
    public class Language : ItemBase
    {
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Language(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetCaption()
        {
            return "FullName";
        }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>        
        [Common(Order = 0, DisplayName = "Short Name"), Template(Mode = Template.Name)]
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Common(Order = 1, DisplayName = "Full Name"), Template(Mode = Template.Name)]//TranslatableName
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Common(Order = 1, _Searchable = true, DisplayName = "Culture"), Template(Mode = Template.Name)]//TranslatableName
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        [Common(Order = 2, DisplayName = "Image"), Template(Mode = Template.Image)]
        public Graphic Image { get; set; }

        /// <summary>
        /// Gets or sets the enable.
        /// </summary>
        [Common(Order = 3), Template(Mode = Template.CheckBox)]
        public bool Enabled { get; set; }

    }

 }
