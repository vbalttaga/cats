// ------------------------------------public --------------------------------------------------------------------------------
// <copyright file="TextboxModel.cs" company="GalexStudio">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the TextboxModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Models.Common
{
    using LIB.AdvancedProperties;
    using LIB.Models.Common;
    using System;
    using Weblib.Models.Common.Enums;

    /// <summary>
    /// The textbox model.
    /// </summary>
    public class TextboxModel : iBaseControlModel
    {
        public TextboxModel()
        {
            RequiredMessage = "Câmpul Este Obligator";
            PopUpClick = "return false";
            UseFancyBox = true;
            AutocompleteClear = true;
            MaxLength = 0;
            MinLength = 0;
            MaxYear = DateTime.Now.Year;
            AutocompleteMinLen = 2;
        }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public TextboxType Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string PlaceHolder { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string HtmlValue { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public DateTime DateValue { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int MaxYear { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string EditCssClass { get; set; }

        /// <summary>
        /// Gets or sets if textbox is required.
        /// </summary>
        public ValidationTypes ValidationType { get; set; }

        /// <summary>
        /// Gets or sets if textbox is ReadOnly.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets if textbox is Disabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets textbox required message.
        /// </summary>
        public string RequiredMessage { get; set; }

        /// <summary>
        /// Gets or sets textbox regular expression.
        /// </summary>
        public string RegularExpression { get; set; }

        /// <summary>
        /// Gets or sets the MaxLength.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Gets or sets the MinLength.
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string OnType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Min { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Max { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string OnKeyUp { get; set; }

        /// <summary>
        /// Gets or sets the KeyPress.
        /// </summary>
        public string OnKeyPress { get; set; }
        
        /// <summary>
        /// Gets or sets Change event.
        /// </summary>
        public string OnChange { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string ValidationFuction { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string PopUpParam { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string PopUpClick { get; set; }

        public bool UseFancyBox { get; set; }

        /// <summary>
        /// Gets or sets the autocompletefilter.
        /// </summary>
        public string AutocompleteFilter { get; set; }

        /// <summary>
        /// Gets or sets the AutocompleteType.
        /// </summary>
        public Type AutocompleteType { get; set; }

        /// <summary>
        /// Gets or sets the AutocompleteServer.
        /// </summary>
        public bool AutocompleteServer { get; set; }

        /// <summary>
        /// Gets or sets the AutocompleteMinLen.
        /// </summary>
        public int AutocompleteMinLen { get; set; }

        /// <summary>
        /// Gets or sets the AutocompleteName.
        /// </summary>
        public string AutocompleteName { get; set; }

        /// <summary>
        /// Gets or sets the AutocompleteAllowNew.
        /// </summary>
        public bool AutocompleteAllowNew { get; set; }

        /// <summary>
        /// Gets or sets the AutocompleteAllowNew.
        /// </summary>
        public bool AutocompleteClear { get; set; }
    }
}
