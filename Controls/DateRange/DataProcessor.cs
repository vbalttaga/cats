namespace Controls.DateRange
{
    using System;
    using System.Net.Cache;

    using Controls.DateRange.Models;
    using LIB.AdvancedProperties;
    using LIB.Tools.Controls;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Globalization;
    using LIB.Tools.BO;
    using LIB.Tools.Utils;
    
    public class DataProcessor : IDataProcessor
    {
        public object SetValue(object value, AdvancedProperty property, ItemBase BOItem, bool ReadOnly = false, DisplayMode mode = DisplayMode.Simple)
        {
            var model = new DateRangeModel();
            
            try
            {
                model.ReadOnly = ReadOnly;

                var TextboxTypeVal = TextboxType.DateTime;

                model.Value = value != null ? ((LIB.Tools.Controls.DateRange)value) : new LIB.Tools.Controls.DateRange();

                model.TextBoxFrom = new TextboxModel() { Name = property.PropertyName + "From", Class = "calendar-input small-input", Type = TextboxTypeVal };
                if(model.Value.from!= DateTime.MinValue)
                {
                    model.TextBoxFrom.Value = model.Value.from.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                model.TextBoxTo = new TextboxModel() { Name = property.PropertyName + "To", Class = "calendar-input small-input", Type = TextboxTypeVal };
                if (model.Value.to != DateTime.MinValue)
                {
                    model.TextBoxTo.Value = model.Value.to.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                if (property.Common.EditTemplate == EditTemplates.DateTimeRange)
                {
                    model.ShowTime = true;
                }

                model.Mode = mode;
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, System.Web.HttpContext.Current);
            }

            return model;
        }

        public object GetValue(AdvancedProperty property, string prefix = "", DisplayMode mode = DisplayMode.Simple)
        {
            //General.TraceWrite(property.PropertyName);

            LIB.Tools.Controls.DateRange range = new LIB.Tools.Controls.DateRange();

            try
            {
                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From"]))
                {
                    var date = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From"]) ? DateTime.ParseExact(
                        System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From"],
                        @"dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.MinValue;
                    if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From" + "_Hours"]))
                        date = date.AddHours(Convert.ToInt32(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From" + "_Hours"]));
                    if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From" + "_Minutes"]))
                        date = date.AddMinutes(Convert.ToInt32(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "From" + "_Minutes"]));

                    range.from = date;
                }
                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To"]))
                {
                    var date = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To"]) ? DateTime.ParseExact(
                        System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To"],
                        @"dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.MinValue;
                    if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To" + "_Hours"]))
                        date = date.AddHours(Convert.ToInt32(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To" + "_Hours"]));
                    if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To" + "_Minutes"]))
                        date = date.AddMinutes(Convert.ToInt32(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "To" + "_Minutes"]));

                    range.to = date;
                }
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, System.Web.HttpContext.Current);
            }
            
            return range;
        }
        
        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";

            if (value == null)
                return "";

            var DateRange = (LIB.Tools.Controls.DateRange)value;

            if (DateRange.from != DateTime.MinValue && DateRange.to != DateTime.MinValue)
                strValue = DateRange.from.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + "-" + DateRange.to.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            else if (DateRange.from != DateTime.MinValue)
                strValue = DateRange.from.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            else if (DateRange.to != DateTime.MinValue)
                strValue = DateRange.to.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return ToString(value, property, BOItem, mode);
        }
    }
}
