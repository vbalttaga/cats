using LIB.Tools.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GofraLib.BusinessObjects
{
    public abstract class ReportBase : ModelBase
    {
        public virtual string getConditionalClass()
        {
            return "";
        }

        public virtual ItemBase getSearchItem(ItemBase Item)
        {
            return Item;
        }
    }
}
