// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Message.cs" company="Gofra">
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
      , DisplayName = "Mesaje"
      , SingleName = "Mesaj"
      , Icon = "comment-o")]
    public class Message : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public Message()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Message(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetCaption()
        {
            return "Title";
        }

        #region Properties

        [Common(Order = 0, DisplayName = "Titlu"), Template(Mode = Template.Name)]
        public string Title { get; set; }

        [Common(Order = 1), Template(Mode = Template.Description)]
        public string Text { get; set; }

        [Common(Order = 2), Template(Mode = Template.DateTime)]
        public DateTime Date { get; set; }

        [Common(Order = 3, DisplayName = "Utilizator"), Template(Mode = Template.ParentDropDown)]
        public User User { get; set; }

        #endregion

        #region Populate Methods

        /// <summary>
        /// The from data table.
        /// </summary>
        /// <param name="dt">
        /// The data table.
        /// </param>
        /// <param name="ds">
        /// The data set.
        /// </param>
        /// <param name="usr">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<long, ItemBase> FromDataTable(DataRow[] dt, DataSet ds, LIB.BusinessObjects.User usr)
        {
            var Messages = new Dictionary<long, ItemBase>();
            foreach (var dr in dt)
            {
                var obj = (new Message()).FromDataRow(dr);
                {
                    Messages.Add(obj.Id, obj);
                }
            }

            return Messages;
        }

        #endregion
    }
}