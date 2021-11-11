// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Common.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the Common type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.AdvancedProperties
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The common.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Common : PropertyItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Common"/> class.
        /// </summary>
        public Common()
        {
            this.EditTemplate = EditTemplates.SimpleInput;
            this.DisplayGroup = "";
            this.ControlClass = CssClass.None.ToString(CultureInfo.InvariantCulture);
            this.EditCssClass = CssClass.None.ToString(CultureInfo.InvariantCulture);
            this.ViewCssClass = CssClass.None.ToString(CultureInfo.InvariantCulture);
            this.Editable = true;
            this.Visible = true;
            this.Order = 1;
        }

        /// <summary>
        /// Gets or sets the edit template.
        /// </summary>
        public EditTemplates EditTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets the edit css class.
        /// </summary>
        public string EditCssClass { get; set; }

        /// <summary>
        /// Gets or sets the view css class.
        /// </summary>
        public string ViewCssClass { get; set; }

        /// <summary>
        /// Gets or sets the view css class.
        /// </summary>
        public string PrintCssClass { get; set; }
        
        /// <summary>
        /// Gets or sets the view css class.
        /// </summary>
        public int PrintWidth { get; set; }

        /// <summary>
        /// Gets or sets the control class.
        /// </summary>
        public string ControlClass { get; set; }

        /// <summary>
        /// Gets or sets the control display group.
        /// </summary>
        public string DisplayGroup { get; set; }

        /// <summary>
        /// The display name.
        /// </summary>
        private string displayName = string.Empty;

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.displayName))
                {
                    return global::LIB.Tools.Utils.Translate.GetTranslatedValue(this.displayName, "BO", this.displayName.Replace("Admin_", string.Empty).Replace("_", " "));
                }
                return "";
            }
            set
            {
                this.displayName = value;
            }
        }

        /// <summary>
        /// The print name.
        /// </summary>
        private string printName = string.Empty;

        /// <summary>
        /// Gets or sets the print name.
        /// </summary>
        public string PrintName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.printName))
                {
                    return global::LIB.Tools.Utils.Translate.GetTranslatedValue(this.printName, "BO", this.printName.Replace("Admin_", string.Empty).Replace("_", " "));
                }
                return DisplayName;
            }
            set
            {
                this.printName = value;
            }
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The property description.
        /// </summary>
        private string _PropertyDescription = string.Empty;

        /// <summary>
        /// Gets or sets the property description.
        /// </summary>
        public string PropertyDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(this._PropertyDescription))
                {
                    return global::LIB.Tools.Utils.Translate.GetTranslatedValue(this._PropertyDescription, "AdminArea", this._PropertyDescription.Replace("Admin_", string.Empty).Replace("_", " "));
                }
                return "";
            }
            set
            {
                this._PropertyDescription = value;
            }
        }

        public bool _Editable
        {
            get
            {
                return Editable == true;
            }
            set
            {
                Editable = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether editable.
        /// </summary>
        public bool? Editable { get; set; }

        public bool _Visible
        {
            get
            {
                return Visible == true;
            }
            set
            {
                Visible = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether editable.
        /// </summary>
        public bool? Visible { get; set; }

        public bool _Sortable
        {
            get
            {
                return Sortable == true;
            }
            set
            {
                Sortable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Sortable.
        /// </summary>
        public bool? Sortable { get; set; }

        public bool _Searchable
        {
            get
            {
                return Searchable == true;
            }
            set
            {
                Searchable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Searchable.
        /// </summary>
        public bool? Searchable { get; set; }

        /// <summary>
        /// The copy user fields.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public override void CopyUserFields(PropertyItem item)
        {
            if (item != null)
            {
                this.EditTemplate = ((Common)item).EditTemplate;
                if (string.IsNullOrEmpty(this.Template))
                {
                    this.Template = ((Common)item).Template;
                }
                if (string.IsNullOrEmpty(this.EditCssClass))
                {
                    this.EditCssClass = ((Common)item).EditCssClass;
                }
                if (string.IsNullOrEmpty(this.DisplayGroup))
                {
                    this.DisplayGroup = ((Common)item).DisplayGroup;
                }
                if (string.IsNullOrEmpty(this.ViewCssClass))
                {
                    this.ViewCssClass = ((Common)item).ViewCssClass;
                }
                if (string.IsNullOrEmpty(this.ControlClass))
                {
                    this.ControlClass = ((Common)item).ControlClass;
                }
                if (string.IsNullOrEmpty(this.DisplayName))
                {
                    this.DisplayName = ((Common)item).DisplayName;
                }
                if (string.IsNullOrEmpty(this.PropertyDescription))
                {
                    this.PropertyDescription = ((Common)item).PropertyDescription;
                }
                if (this.Sortable == null)
                {
                    this.Sortable = ((Common)item).Sortable;
                }
                if (this.Editable == null)
                {
                    this.Editable = ((Common)item).Editable;
                }
                if (this.Visible == null)
                {
                    this.Visible = ((Common)item).Visible;
                }
                if (this.Searchable == null)
                {
                    this.Searchable = ((Common)item).Searchable;
                }
            }
        }

    }
}