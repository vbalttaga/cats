using LIB.AdvancedProperties;
using LIB.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Tools.BO
{
    [Serializable]
    public class AggregateBase: ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateBase"/> class.
        /// </summary>
        public AggregateBase()
            : base(0)
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateBase"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public AggregateBase(long id)
            : base(id)
        {
            this.Id = id;
        }
        #endregion

        [Common(EditTemplate = EditTemplates.Hidden), Db(_Editable = false, _Populate = false, _Ignore = true)]
        public string Color { get; set; }

        public virtual string GetColor()
        {
            if (string.IsNullOrEmpty(Color))
                Color = ColorHelper.HexConverter(ColorHelper.generateRandomColor(new System.Drawing.Color(), Convert.ToInt32(Id)));

            return Color;
        }
        public virtual int GetCount()
        {
            return 0;
        }
        public virtual string GetChartName()
        {
            return GetName();
        }
    }
}
