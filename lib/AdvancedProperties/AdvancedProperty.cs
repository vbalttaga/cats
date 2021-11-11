// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdvancedProperty.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the AdvancedProperty type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.AdvancedProperties
{
    using System;
    using System.ComponentModel;

    using LIB.Tools.Controls;

    /// <summary>
    /// The advanced property.
    /// </summary>
    [Serializable]
    public class AdvancedProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedProperty"/> class.
        /// </summary>
        public AdvancedProperty()
        {
            this.Common = new Common();
        }

        /// <summary>
        /// Gets or sets the property descriptor.
        /// </summary>
        public PropertyDescriptor PropertyDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the common.
        /// </summary>
        public Common Common { get; set; }

        /// <summary>
        /// Gets or sets the Image.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the validation.
        /// </summary>
        public Validation Validation { get; set; }

        /// <summary>
        /// Gets or sets the translate.
        /// </summary>
        public Translate Translate { get; set; }

        /// <summary>
        /// Gets or sets the encryption.
        /// </summary>
        public Encryption Encryption { get; set; }

        /// <summary>
        /// Gets or sets the mark.
        /// </summary>
        public Mark Mark { get; set; }

        /// <summary>
        /// Gets or sets the access.
        /// </summary>
        public Access Access { get; set; }

        /// <summary>
        /// Gets or sets the data base.
        /// </summary>
        public Db Db { get; set; }

        /// <summary>
        /// Gets or sets the custom.
        /// </summary>
        public PropertyItem Custom { get; set; }

        /// <summary>
        /// Gets or sets the servie.
        /// </summary>
        public Service Service { get; set; }

        public string Control
        {
            get
            {
                switch (Common.EditTemplate)
                {
                    case EditTemplates.DateInput:
                    case EditTemplates.SimpleInput:
                    case EditTemplates.MultiLine:
                    case EditTemplates.HtmlInput:
                    case EditTemplates.HtmlPopUpInput:
                    case EditTemplates.DateTimeInput:
                    case EditTemplates.Password:
                    case EditTemplates.DiagnosticInput:
                    case EditTemplates.AutoComplete:
                    case EditTemplates.NotUpdatableInput:
                        return "Input";
                    case EditTemplates.ImageUpload:
                        return "Image";
                    case EditTemplates.DocumentUpload:
                        return "File";
                    case EditTemplates.DropDown:
                    case EditTemplates.DropDownParent:
                    case EditTemplates.SelectList:
                    case EditTemplates.SelectListParent:
                    case EditTemplates.MultiSelect:
                        return "Select";
                    case EditTemplates.CheckBox:
                        return "CheckBox";
                    case EditTemplates.Link:
                    case EditTemplates.LinkItem:
                    case EditTemplates.GlobalLink:
                    case EditTemplates.GlobalLinkItem:
                    case EditTemplates.LinkItems:
                    case EditTemplates.Parent:
                        return "Link";
                    case EditTemplates.MultiCheck:
                    case EditTemplates.PermissionsSelector:
                        return "MultyCheck";
                    case EditTemplates.NumberRange:
                        return "NumberRange";
                    case EditTemplates.DateRange:
                    case EditTemplates.DateTimeRange:
                        return "DateRange";
                }
                return "Input";
            }
        }

        public string ControlView
        {
            get
            {
                return "Default";
            }
        }

        public IDataProcessor GetDataProcessor()
        {
            return (IDataProcessor)Activator.CreateInstance(Type.GetType("Controls." + Control + ".DataProcessor, Controls." + Control + "", true));
        }
    }
}