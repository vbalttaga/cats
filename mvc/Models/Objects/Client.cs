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

namespace Gofra.Models.Objects
{
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.System
    , ModulesAccess = (long)(Modulesenum.ControlPanel)
    , DisplayName = "Client"
    , SingleName = "Client"
    , DoCancel = true
    , LogRevisions = true
    , CustomFormTag = "Client")]
    public class Client : ItemBase
    {
        #region construnctors
        public Client() : base()
        {
        }
        public Client(long Id) : base(Id)
        {
        }
        #endregion

        #region Properties
        [Common(DisplayName = "IdentifyNumber", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.String)]
        public string IdentifyNumber { get; set; }

        [Common(DisplayName = "Client", DisplayGroup = "Date Generale"), Access(), Template(Mode = Template.ParentDropDown)]
        public Person Person { get; set; }

        #endregion

        #region override
        public override string GetCaption()
        {
            return "PersonId";
           
        }
        #endregion
    }
}