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
using Person = GofraLib.BusinessObjects.Person;
using Gofra.Models.Other;

namespace Gofra.Models.Objects
{
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.System
   , ModulesAccess = (long)(Modulesenum.ControlPanel)
   , DisplayName = "Shop"
   , SingleName = "Shop"
   , DoCancel = true
   , LogRevisions = true
   , CustomFormTag = "Shop")]
    public class Shop : ItemBase
    {
        #region construnctors
        public Shop() : base()
        {
        }
        public Shop(long Id) : base(Id)
        {
        }
        #endregion

        #region Properties
        [Common(DisplayName = "Name", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Common(DisplayName = "IdentifyNumber", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.Name)]
        public string JurName { get; set; }

        [Common(DisplayName = "Client", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public City City { get; set; }

        #endregion

    }
}