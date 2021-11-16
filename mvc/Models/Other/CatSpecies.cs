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

namespace Gofra.Models.Other
{
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.System
    , ModulesAccess = (long)(Modulesenum.ControlPanel)
    , DisplayName = "CatSpecies"
    , SingleName = "CatSpecies"
    , DoCancel = true
    , LogRevisions = true
    , CustomFormTag = "CatSpecies")]
    public class CatSpecies : ItemBase
    {
        #region construnctors
        public CatSpecies() : base()
        {
        }
        public CatSpecies(long Id) : base(Id)
        {
        }
        #endregion

        #region Properties
        [Common(DisplayName = "Name", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.Name)]
        public string Name { get; set; }

        #endregion
    }
}