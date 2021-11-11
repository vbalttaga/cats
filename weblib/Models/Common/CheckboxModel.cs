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
    using LIB.Models.Common;
    using Weblib.Models.Common.Enums;

    /// <summary>
    /// The textbox model.
    /// </summary>
    public class CheckboxModel : iBaseControlModel
    {
        public CheckboxModel()
        {
            Value = "1";
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string AdditionalControl { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public bool Checked { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the OnClick.
        /// </summary>
        public string OnClick { get; set; }
    }
}
