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
    using LIB.Tools.AdminArea;

    /// <summary>
    /// The Contact.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Settings
      , ModulesAccess = (long)(Modulesenum.SMI)
      , DisplayName = "Setări globale"
      , SingleName = "Setare globală"
      , Icon = "wrench")]
    public class KeyValueSetting : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public KeyValueSetting()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public KeyValueSetting(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetName()
        {
            return Key;
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Key { get; set; }

        [Common(Order = 1), Template(Mode = Template.Name)]
        public string Value { get; set; }


        #endregion
    }
}