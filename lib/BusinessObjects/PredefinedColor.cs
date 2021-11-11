// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PredefinedColor.cs" company="GalexStudio">
//   Copyright ©  2018
// </copyright>
// <summary>
//   Defines the PredefinedColor type.
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
    [Bo(Group = AdminAreaGroupenum.Settings
      , ModulesAccess = (long)Modulesenum.SMI
      , DisplayName = "Culori predefinite"
      , SingleName = "Culoare"
      , EditAccess = (long)BasePermissionenum.SuperAdmin
      , CreateAccess = (long)BasePermissionenum.SuperAdmin
      , DeleteAccess = (long)BasePermissionenum.SuperAdmin
      , ReadAccess = (long)BasePermissionenum.SuperAdmin
      , RevisionsAccess = (long)BasePermissionenum.SuperAdmin
      , Icon = "paint-brush")]
    public class PredefinedColor : ItemBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor"/> class.
        /// </summary>
        public PredefinedColor()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public PredefinedColor(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedColor"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public PredefinedColor(int id, string name)
            : base(id)
        {
            this.Name = name;
        }

        #endregion

        public override string GetAdditionalSelectQuery(AdvancedProperty property)
        {
            return ",[" + property.PropertyName + "].Code" + " AS " + property.PropertyName + "Code,[" + property.PropertyName + "].Color" + " AS " + property.PropertyName + "Color";
        }

        #region PredefinedColor Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Template(Mode = Template.Name)]
        public string Code { get; set; }

        [Template(Mode = Template.ColorPicker), Access(DisplayMode = DisplayMode.Simple | DisplayMode.Advanced)]
        public string Color { get; set; }

        #endregion

    }
}