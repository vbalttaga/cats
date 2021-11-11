// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the Document type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using LIB.Tools.BO;
    using LIB.AdvancedProperties;
    using LIB.Tools.Utils;

    /// <summary>
    /// The document.
    /// </summary>
    [Serializable]
    [Bo(
        LogRevisions = false
      )
    ]
    public class Document : ItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Document(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        [Db(_Ignore = true)]
        private string _RelativePath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Ext { get; set; }

        [Db(_Ignore = true)]
        public string File
        {
            get
            {
                if (!string.IsNullOrEmpty(_RelativePath))
                    return LIB.Tools.Utils.URLHelper.GetUrl(_RelativePath);

                if (!string.IsNullOrEmpty(FileName))
                    return LIB.Tools.Utils.URLHelper.GetUrl(Config.GetConfigValue("UploadURL") + "/Documents/" + FileName + "." + Ext);

                if (!string.IsNullOrEmpty(Name))
                    return LIB.Tools.Utils.URLHelper.GetUrl(Config.GetConfigValue("UploadURL") + "/Documents/" + Name + "." + Ext);

                return "";
            }
        }
    }
}