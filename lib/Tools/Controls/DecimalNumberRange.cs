
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Tools.Controls
{
    public class DecimalNumberRange
    {
        public DecimalNumberRange()
        {
            from = 0;
            to = 0;
        }
        public decimal from { get; set; }

        public decimal to { get; set; }
    }
}
