// ------------------------------public --------------------------------------------------------------------------------------
// <copyright file="Converter.cs" company="GalexStudio">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the Converter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Helpers
{
    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.Controls;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Weblib.Models.Common.Enums;

    /// <summary>
    /// The converter.
    /// </summary>
    public class CSVExportHelper
    {
        /// <summary>
        /// The get textbox type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static byte[] ExportCSV(Dictionary<long, ItemBase> Items, AdvancedProperties properties, bool includeSystemColumns = false, params string[] ColumnsToTake)
        {
            StringBuilder csv = new StringBuilder();
            var headers = "";
            if (includeSystemColumns)
                headers += "Id";
            foreach (LIB.AdvancedProperties.AdvancedProperty property in properties)
            {
                headers += (!string.IsNullOrEmpty(headers) ? "," : "") + "\"" + property.Common.PrintName + "\"";
            }
            if (includeSystemColumns)
            {
                headers += ",Creat la data";
                headers += ",Autor";
            }
            csv.AppendLine(headers);
            foreach (var pitem in Items.Values)
            {
                var values = "";
                if (includeSystemColumns)
                    values += pitem.Id.ToString();
                foreach (LIB.AdvancedProperties.AdvancedProperty property in properties)
                {
                    values += (!string.IsNullOrEmpty(values) ? "," : "") + "\"" + property.GetDataProcessor().ToString(property.PropertyDescriptor.GetValue(pitem), property, pitem, DisplayMode.CSV) + "\"";
                }
                if (includeSystemColumns)
                {
                    values += ","+ pitem.DateCreated.ToString();
                    values += "," + pitem.CreatedBy.Login;
                }
                csv.AppendLine(values);
            }

            var data = Encoding.UTF8.GetBytes(csv.ToString());
            return Encoding.UTF8.GetPreamble().Concat(data).ToArray();

        }

    } 
}
