using LIB.Tools.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GofraLib.BusinessObjects
{
    public class ModelBase: ItemBase
    {
        public ModelBase()
            : base(0)
        {
        }

        public ModelBase(long id)
            : base(id)
        {
        }
    }
}
