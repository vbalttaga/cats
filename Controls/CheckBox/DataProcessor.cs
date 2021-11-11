namespace Controls.CheckBox
{
    using System;
    using System.Net.Cache;

    using Controls.CheckBox.Models;
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
            var model = new CheckBoxModel();

            //General.TraceWrite(property.PropertyName);
            try
            {
                model.ReadOnly = ReadOnly;

                if (value == null)
                    value = false;
                model.Value = (bool)value;

                model.Checkbox = new CheckboxModel() { Name = property.PropertyName,Id = new Random().Next().ToString(), Checked = model.Value, Class = "flat-red" };
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

            try
            {
                return System.Web.HttpContext.Current.Request.Form[property.PropertyName] == "1";
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, System.Web.HttpContext.Current);
            }

            return false;
        }

        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";
            if (value == null)
                value = false;

            if ((bool)value)
                strValue = "Da";
            else
                strValue = "Nu";

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return ToString(value, property, BOItem, mode);
        }
    }
}
