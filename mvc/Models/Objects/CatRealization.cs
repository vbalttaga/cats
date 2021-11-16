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

namespace Gofra.Models.Objects
{
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.System
     , ModulesAccess = (long)(Modulesenum.ControlPanel)
     , DisplayName = "CatRealization"
     , SingleName = "CatRealization"
     , DoCancel = true
     , LogRevisions = true
     , CustomFormTag = "CatRealization")]
    public class CatRealization : ItemBase
    {
        #region construnctors
        public CatRealization() : base()
        {
        }
        public CatRealization(long Id) : base(Id)
        {
        }
        #endregion

        #region Properties
        [Common(DisplayName = "Client", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public Client Client { get; set; }

        [Common(DisplayName = "Client", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public Cats Cat { get; set; }

        #endregion

    }
}