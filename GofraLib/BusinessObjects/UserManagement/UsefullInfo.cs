// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Partners.cs" company="Gofra">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The Partner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.BusinessObjects;
    using LIB.Tools.BO;
    using LIB.Tools.AdminArea;

    /// <summary>
    /// The Contact.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Settings
      , ModulesAccess = (long)(Modulesenum.ControlPanel)
      , DisplayName = "Informații"
      , SingleName = "Informație"
      , Icon = "exclamation")]
    public class UsefullInfo : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsefullInfo"/> class.
        /// </summary>
        public UsefullInfo()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsefullInfo"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public UsefullInfo(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetName()
        {
            return Id.ToString();
        }

        public override void SetName(object name)
        {
            Text = name!=null?name.ToString():"";
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Html)]
        public string Text { get; set; }
        /*
        /// <summary>
        /// Gets the Image.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Common(Order = 1, EditTemplate = EditTemplates.ImageUpload)]
        [Access(DisplayMode = DisplayMode.Simple)]
        public Graphic Image { get; set; }
        */
        #endregion
    }
}