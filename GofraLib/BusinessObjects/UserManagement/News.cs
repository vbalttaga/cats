// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contacts.cs" company="Gofra">
//   Copyright ©  2013
// </copyright>
// <summary>
//   The Contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GofraLib.BusinessObjects
{
    using System;

    using LIB.AdvancedProperties;
    using LIB.BusinessObjects;
    using LIB.Tools.BO;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using LIB.Tools.Utils;
    using System.Data.SqlClient;
    using LIB.Tools.AdminArea;

    /// <summary>
    /// The MenuItem.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Navigation
      , ModulesAccess = (long)(Modulesenum.ControlPanel)
      , DisplayName = "Noutăți"
      , SingleName = "Noutate"
      , Icon = "rss-square")]
    public class News : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public News()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public News(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetCaption()
        {
            return "Title";
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Title { get; set; }

        [Common(Order = 0), Template(Mode = Template.Date)]
        public DateTime Date { get; set; }
        
        [Common(Order = 0), Template(Mode = Template.Html)]
        public string Text { get; set; }

        [Common(Order = 0), Template(Mode = Template.CheckBox)]
        public bool Active { get; set; }

        #endregion
    }
}