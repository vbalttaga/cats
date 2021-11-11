using LIB.Tools.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GofraLib.BusinessObjects
{
    public class ObjectPageBase: ModelBase
    {
        public ObjectPageBase()
            : base(0)
        {
        }

        public ObjectPageBase(long id)
            : base(id)
        {
        }
    }
}
