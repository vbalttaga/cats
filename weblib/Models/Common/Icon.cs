using LIB.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weblib.Models.Common
{
    public class Icon : iBaseControlModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Class { get; set; }

        public static Icon Search = new Icon { Class = "btn-search" };
        public static Icon Add = new Icon { Class = "btn-add" };
        public static Icon Logout = new Icon { Class = "btn-logout" };
    }
}
