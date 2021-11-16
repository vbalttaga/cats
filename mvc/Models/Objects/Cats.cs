using LIB.AdvancedProperties;
using LIB.Tools.Controls;
using GofraLib.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LIB.Tools.BO;
using LIB.Tools.AdminArea;
using LIB.BusinessObjects;
using Gofra.Models.Other;

namespace Gofra.Models.Objects
{
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.System
      , ModulesAccess = (long)(Modulesenum.ControlPanel)
      , DisplayName = "Cats"
      , SingleName = "Cat"
      , DoCancel = true
      , LogRevisions = true
      , CustomFormTag = "cats")]

    public class Cats : ItemBase
    {
        #region construnctors
        public Cats() : base()
        {
        }
        public Cats(long Id) : base(Id)
        {
        }
        #endregion

        #region Properties
        [Common(DisplayName = "Name", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Common(DisplayName = "Birth Date", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.Date)]
        public DateTime Birthdate { get; set; }

        [Common(DisplayName = "Sex", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public Sex Sex { get; set; }

        [Common(DisplayName = "Color", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public Color Color { get; set; }

        [Common(DisplayName = "Source", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public CatSource Source { get; set; }

        [Common(DisplayName = "Owner", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public Client Owner { get; set; }
        #endregion
    }
}