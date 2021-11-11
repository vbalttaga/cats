// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Graphic.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the Graphic type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using LIB.Tools.BO;
    using LIB.Tools.Utils;
    using LIB.AdvancedProperties;

    /// <summary>
    /// The graphic.
    /// </summary>
    [Serializable]
    [Bo(
        LogRevisions = false
      )
    ]
    public class Graphic : ItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Graphic"/> class.
        /// </summary>
        public Graphic()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Graphic"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Graphic(long id)
            : base(id)
        {
        }
        
        #region properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// Gets or sets the relative path.
        /// </summary>
        public string BOName { get; set; }

        [Db(_Ignore = true)]
        private string _RelativePath { get; set; }

        [Db(_Ignore = true)]
        public string AdminThumbnail
        {
            get
            {
                if (!string.IsNullOrEmpty(_RelativePath))
                    return LIB.Tools.Utils.URLHelper.GetUrl(_RelativePath);

                if (!string.IsNullOrEmpty(Name))
                    return LIB.Tools.Utils.URLHelper.GetUrl(Config.GetConfigValue("UploadURL") + "/" + BOName + "/" + Name + "_adminthumb.jpeg");

                return "";
            }
        }

        [Db(_Ignore = true)]
        public string Thumbnail
        {
            get
            {
                if (!string.IsNullOrEmpty(_RelativePath))
                    return LIB.Tools.Utils.URLHelper.GetUrl(_RelativePath);

                if (!string.IsNullOrEmpty(Name))
                    return LIB.Tools.Utils.URLHelper.GetUrl(Config.GetConfigValue("UploadURL") + "/" + BOName + "/" + Name + ".jpeg");

                return "";
            }
        }

        #endregion

        public static Graphic ToolsPlaceHolder = new Graphic() { _RelativePath = @"Images\placeholders\ToolsPlaceHolder.png" };
    }
}