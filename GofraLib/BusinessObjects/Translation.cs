// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Translation.cs" company="Galex">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The static translations.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.AdminArea;
    using LIB.BusinessObjects;

    /// <summary>
    /// The Contact.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Translate
      , DisplayName = "Traduceri"
      , SingleName = "Traduceri"
      , EditAccess = (long)BasePermissionenum.SuperAdmin
      , CreateAccess = (long)BasePermissionenum.SuperAdmin
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , ReadAccess = (long)BasePermissionenum.SuperAdmin
      , LogRevisions = true
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon = "clone"
      )
    ]
    public class Translation : LIB.BusinessObjects.Translation
    {
        #region properties
        /// <summary>
        /// Gets or sets the russian.
        /// </summary>
        [Common(Order = 1, EditTemplate = EditTemplates.SimpleInput, DisplayName = "Admin_RUTrans", ControlClass = CssClass.Wide),
        Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced),
        Service(LanguageAbbr = "ru")]
        public string Russian{ get; set; }

        /// <summary>
        /// Gets or sets the moldavian.
        /// </summary>
        [Common(Order = 2, EditTemplate = EditTemplates.SimpleInput, DisplayName = "Admin_MDTrans", ControlClass = CssClass.Wide),
        Access(DisplayMode = LIB.AdvancedProperties.DisplayMode.Simple | LIB.AdvancedProperties.DisplayMode.Advanced),
        Service(LanguageAbbr = "ro")]
        public string Moldavian { get; set; }

        #endregion
    }
}