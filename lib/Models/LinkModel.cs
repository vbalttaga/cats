using LIB.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Models
{
    public class LinkModel : iBaseControlModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the OnClick.
        /// </summary>
        public string Action { get; set; }
    }
}
