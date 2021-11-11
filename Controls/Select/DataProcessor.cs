namespace Controls.Select
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Globalization;

    using Controls.Select.Models;
    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.Controls;
    using LIB.Tools.Utils;

    using Weblib.Models.Common;
    using LIB.BusinessObjects;
    using LIB.Tools.Security;
    using System.Web;
    using Weblib.Models.Common.Enums;

    public class DataProcessor : IDataProcessor
    {
        public object SetValue(object value, AdvancedProperty property, ItemBase BOItem, bool ReadOnly = false, DisplayMode mode = DisplayMode.Simple)
        {

            //General.TraceWrite(property.PropertyName);
            try
            {
                #region SelectList
                if (!ReadOnly && property.Common.Editable == true && (property.Common.EditTemplate == EditTemplates.SelectList || property.Common.EditTemplate == EditTemplates.SelectListParent))
                {
                    var slmodel = new SelectListModel();

                    slmodel.ReadOnly = ReadOnly || property.Common.Editable == false;
                    slmodel.Value = (ItemBase)value;
                    slmodel.Mode = mode;

                    var Module = "ControlPanel";
                    if (HttpContext.Current != null)
                    {
                        var context = new HttpContextWrapper(HttpContext.Current);
                        if (context.Session[SessionItems.Module] != null)
                            Module = context.Session[SessionItems.Module].ToString();
                    }

                    if (slmodel.Value == null)
                    {
                        slmodel.Value = (ItemBase)Activator.CreateInstance(property.Type);
                    }

                    if (!ReadOnly)
                    {
                        if (slmodel.Value.Id > 0)
                        {
                            var url = URLHelper.GetUrl(Module + "/EditItem/" + slmodel.Value.GetType().Name + "/" + HttpUtility.UrlEncode(slmodel.Value.GetType().Namespace) + "/" + slmodel.Value.Id.ToString());
                            slmodel.Link = new LIB.Models.LinkModel() { Caption = slmodel.Value.GetName(), Href = url };
                        }
                        else
                        {
                            slmodel.Link = new LIB.Models.LinkModel();
                        }
                    }

                    slmodel.SelectList = new TextboxModel()
                    {
                        Name = property.PropertyName,
                        HtmlValue = slmodel.Value.GetId()!=null?slmodel.Value.GetId().ToString():"",
                        Value = slmodel.Value.GetName()!=null?slmodel.Value.GetName().Trim():"",
                        Type = TextboxType.AutoComplete,
                        AutocompleteType = property.Type,
                        AutocompleteServer = true,
                        AutocompleteMinLen =2,
                        EditCssClass = property.Common.EditCssClass
                    };
                    if (property.Custom != null && property.Custom is LookUp && !string.IsNullOrEmpty(((LookUp)property.Custom).AdvancedFilter))
                    {

                        slmodel.SelectList.AutocompleteFilter = ((LookUp)property.Custom).AdvancedFilter;

                    }
                    if (property.Custom != null && property.Custom is LookUp && ((LookUp)property.Custom).AutocompleteMinLen!=-1)
                    {
                        slmodel.SelectList.AutocompleteMinLen = ((LookUp)property.Custom).AutocompleteMinLen;
                    }
                    if (property.Custom != null && property.Custom is LookUp && !string.IsNullOrEmpty(((LookUp)property.Custom).OnChange))
                    {

                        slmodel.SelectList.OnChange = property.Custom != null && property.Custom is LookUp ? ((LookUp)property.Custom).OnChange : "";

                    }
                    if (property.Validation != null)
                    {
                        if (mode != DisplayMode.Search)
                        {
                            slmodel.SelectList.ValidationType = property.Validation.ValidationType;
                        }
                        slmodel.SelectList.ValidationFuction = property.Validation.ValidationFunction;
                        if (!string.IsNullOrEmpty(property.Validation.AlertMessage))
                            slmodel.SelectList.RequiredMessage = property.Validation.AlertMessage;
                    }
                    if (property.Custom != null && property.Custom is InputBox && !string.IsNullOrEmpty(((InputBox)property.Custom).OnChange)){
                        slmodel.SelectList.OnChange = ((InputBox)property.Custom).OnChange;
                    }
                    return slmodel;
                }
                #endregion
            }
            catch (Exception ex)
            {
                General.TraceWarn(property.PropertyName);

                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, System.Web.HttpContext.Current);

                return new SelectListModel();
            }
            var model = new SelectModel();
            try
            {
                model.ReadOnly = ReadOnly || property.Common.Editable == false;
                if (property.Common.EditTemplate != EditTemplates.MultiSelect)
                {
                    model.Value = (ItemBase)value;
                }
                model.Mode = mode;

                if ((property.Custom != null && property.Custom is LookUp && (((LookUp)property.Custom).SearchAsMulticheck || ((LookUp)property.Custom).SearchAsMultiSelect)) && (mode == DisplayMode.Search || mode == DisplayMode.PrintSearch))
                {
                    #region SearchAsMulticheck
                    if (((LookUp)property.Custom).SearchAsMulticheck)
                    {
                        model.MultyCheckModel = new MultyCheckModel();
                        model.MultyCheckModel.Mode = mode;
                        model.MultyCheckModel.ReadOnly = model.ReadOnly;
                        model.MultyCheckModel.MultyCheck = new MultyCheckWidgetModel() { Name = property.PropertyName, InputClass = " flat-red" };

                        var LookUpCollection = (Dictionary<string, Dictionary<long, ItemBase>>)ContextItemsHolder.ObjectFromContext(ContextItemsHolder.DropDownLookUp);
                        if (LookUpCollection == null)
                        {
                            LookUpCollection = new Dictionary<string, Dictionary<long, ItemBase>>();
                        }

                        if (!LookUpCollection.ContainsKey(property.PropertyName))
                        {
                            var item = (ItemBase)Activator.CreateInstance(property.Type);
                            var AdvancedFilter = BOItem.GetAdvancedLookUpFilter(property);
                            AdvancedFilter += property.Custom != null && property.Custom is LookUp ? ((LookUp)property.Custom).AdvancedFilter : "";

                            var collection = item.Populate(null, null, true, AdvancedFilter);
                            LookUpCollection.Add(property.PropertyName, collection);
                        }
                        if (property.Custom != null && property.Custom is LIB.AdvancedProperties.LookUp && !string.IsNullOrEmpty(((LIB.AdvancedProperties.LookUp)property.Custom).GpoupByField))
                        {
                            model.MultyCheckModel.MultyCheck.Groups = new Dictionary<string, Dictionary<long, ItemBase>>();
                            foreach (var item in LookUpCollection[property.PropertyName].Values)
                            {
                                var GroupItem = item.GetGroupField(((LIB.AdvancedProperties.LookUp)property.Custom).GpoupByField);
                                if (!model.MultyCheckModel.MultyCheck.Groups.ContainsKey(GroupItem))
                                    model.MultyCheckModel.MultyCheck.Groups.Add(GroupItem, new Dictionary<long, ItemBase>());

                                model.MultyCheckModel.MultyCheck.Groups[GroupItem].Add(item.Id, item);
                            }
                        }
                        else
                        {
                            model.MultyCheckModel.MultyCheck.Options = LookUpCollection[property.PropertyName];
                        }
                        ContextItemsHolder.ObjectToContext(ContextItemsHolder.DropDownLookUp, LookUpCollection);

                        var Values = new Dictionary<long, ItemBase>();
                        if (model.Value != null && model.Value.SearchItems != null)
                        {
                            Values = model.Value.SearchItems;
                        }
                        else if (model.Value != null && model.Value.Id > 0)
                        {
                            Values.Add(model.Value.Id, model.Value);
                        }

                        model.MultyCheckModel.Values = Values;

                        if (model.MultyCheckModel.Values.Count > 0)
                        {
                            foreach (var key in model.MultyCheckModel.Values.Keys)
                            {
                                model.MultyCheckModel.Values[key].SetName(LookUpCollection[property.PropertyName].Values.FirstOrDefault(i => i.Id == model.MultyCheckModel.Values[key].Id).GetName());
                            }
                        }

                        model.MultyCheckModel.MultyCheck.Values = model.MultyCheckModel.Values;
                    }
                    #endregion

                    #region SearchAsMultiSelect
                    if (((LookUp)property.Custom).SearchAsMultiSelect)
                    {
                        model.DropDown = new DropDownModel() { Name = property.PropertyName };

                        var LookUpCollection = (Dictionary<string, Dictionary<long, ItemBase>>)ContextItemsHolder.ObjectFromContext(ContextItemsHolder.DropDownLookUp);
                        if (LookUpCollection == null)
                        {
                            LookUpCollection = new Dictionary<string, Dictionary<long, ItemBase>>();
                        }
                        if (!LookUpCollection.ContainsKey(property.PropertyName + "s"))
                        {
                            ItemBase item;

                            item = (ItemBase)Activator.CreateInstance(property.Type);
                            var AdvancedFilter = BOItem.GetAdvancedLookUpFilter(property);
                            AdvancedFilter += property.Custom != null && property.Custom is LookUp ? ((LookUp)property.Custom).AdvancedFilter : "";

                            //General.TraceWrite("AdvancedFilter:" + AdvancedFilter);
                            var collection = item.Populate(null, null, true, AdvancedFilter);
                            LookUpCollection.Add(property.PropertyName + "s", collection);
                        }
                        if (property.Custom != null && property.Custom is LookUp && !string.IsNullOrEmpty(((LookUp)property.Custom).GpoupByField))
                        {
                            model.DropDown.Groups = new Dictionary<string, Dictionary<long, ItemBase>>();
                            foreach (var item in LookUpCollection[property.PropertyName + "s"].Values)
                            {
                                var GroupItem = item.GetGroupField(((LookUp)property.Custom).GpoupByField);
                                if (!model.DropDown.Groups.ContainsKey(GroupItem))
                                    model.DropDown.Groups.Add(GroupItem, new Dictionary<long, ItemBase>());

                                model.DropDown.Groups[GroupItem].Add(item.Id, item);
                            }
                        }
                        else
                        {
                            model.DropDown.Options = LookUpCollection[property.PropertyName + "s"];
                        }
                        List<ItemBase> ExcludeOptions = null;
                        if (property.Type == typeof(Role))
                        {
                            var User = Authentication.GetCurrentUser();
                            ExcludeOptions = new List<ItemBase>();
                            foreach (var item in model.DropDown.Options.Values)
                            {
                                if (!User.HasAtLeastOnePermission(((Role)item).RoleAccessPermission))
                                {
                                    ExcludeOptions.Add(item);
                                }
                            }
                        }
                        model.DropDown.ExcludeOptions = ExcludeOptions;
                        model.DropDown.AllowDefault = mode == DisplayMode.Search || (property.Custom is LookUp ? ((LookUp)property.Custom).DefaultValue : false);
                        if (property.Validation != null)
                        {
                            if (mode != DisplayMode.Search)
                            {
                                model.DropDown.ValidationType = property.Validation.ValidationType;
                            }
                            model.DropDown.ValidationFuction = property.Validation.ValidationFunction;
                            if (!string.IsNullOrEmpty(property.Validation.AlertMessage))
                                model.DropDown.RequiredMessage = property.Validation.AlertMessage;
                        }
                        ContextItemsHolder.ObjectToContext(ContextItemsHolder.DropDownLookUp, LookUpCollection);

                        model.DropDown.Multiple = true;
                        model.DropDown.ShowOptions = ((LookUp)property.Custom).ShowOptions;
                        model.DropDown.ItemType = property.Type;
                        model.DropDown.AllowDefault = false;

                        /*
                        model.DropDown.Values = (Dictionary<long, ItemBase>)value;
                        if (model.DropDown.Values != null)
                        {
                            model.DropDown.StrValues = string.Join(",", model.DropDown.Values.Values.Select(o => o.Id.ToString()));
                        }
                        if (model.DropDown.Values == null)
                        {
                            model.DropDown.Values = new Dictionary<long, ItemBase>();
                        }
                        foreach (var key in model.DropDown.Values.Keys)
                        {
                            model.DropDown.Values[key].SetName(LookUpCollection[property.PropertyName + "s"].Values.FirstOrDefault(i => i.Id == model.DropDown.Values[key].Id).GetName());
                        }*/
                    }
                    #endregion
                }
                else
                {
                    #region DropDown
                    var Module = "ControlPanel";
                    if (HttpContext.Current != null)
                    {
                        var context = new HttpContextWrapper(HttpContext.Current);
                        if (context.Session[SessionItems.Module] != null)
                            Module = context.Session[SessionItems.Module].ToString();
                    }

                    if (property.Common.EditTemplate != EditTemplates.MultiSelect)
                    {
                        if (model.Value == null)
                        {
                            model.Value = (ItemBase)Activator.CreateInstance(property.Type);
                        }

                        if (!ReadOnly)
                        {
                            if (model.Value.Id > 0)
                            {
                                var url = URLHelper.GetUrl(Module + "/EditItem/" + model.Value.GetType().Name + "/" + HttpUtility.UrlEncode(model.Value.GetType().Namespace) + "/" + model.Value.Id.ToString());
                                model.Link = new LIB.Models.LinkModel() { Caption = model.Value.GetName(), Href = url };
                            }
                            else
                            {
                                model.Link = new LIB.Models.LinkModel();
                            }
                        }
                    }

                    model.DropDown = new DropDownModel() { Name = property.PropertyName };

                    if (!ReadOnly && property.Common.Editable == true && (property.Common.EditTemplate == EditTemplates.DropDown || property.Common.EditTemplate == EditTemplates.DropDownParent || property.Common.EditTemplate == EditTemplates.MultiSelect))
                    {
                        var LookUpCollection = (Dictionary<string, Dictionary<long, ItemBase>>)ContextItemsHolder.ObjectFromContext(ContextItemsHolder.DropDownLookUp);
                        if (LookUpCollection == null)
                        {
                            LookUpCollection = new Dictionary<string, Dictionary<long, ItemBase>>();
                        }
                        if (!LookUpCollection.ContainsKey(property.PropertyName + "s"))
                        {
                            ItemBase item;

                            if (property.Common.EditTemplate != EditTemplates.MultiSelect)
                            {
                                item = (ItemBase)Activator.CreateInstance(property.Type);
                            }
                            else
                            {
                                item = (ItemBase)Activator.CreateInstance((property.Custom as MultiCheck).ItemType);
                            }
                            var AdvancedFilter = BOItem.GetAdvancedLookUpFilter(property);
                            AdvancedFilter += property.Custom != null && property.Custom is LookUp ? ((LookUp)property.Custom).AdvancedFilter : "";

                            model.DropDown.OnChange = property.Custom != null && property.Custom is LookUp ? ((LookUp)property.Custom).OnChange : "";

                            //General.TraceWrite("AdvancedFilter:" + AdvancedFilter);
                            var collection = item.Populate(null, null, true, AdvancedFilter);
                            LookUpCollection.Add(property.PropertyName + "s", collection);
                        }
                        if (property.Custom != null && property.Custom is LookUp && !string.IsNullOrEmpty(((LookUp)property.Custom).GpoupByField))
                        {
                            model.DropDown.Groups = new Dictionary<string, Dictionary<long, ItemBase>>();
                            foreach (var item in LookUpCollection[property.PropertyName + "s"].Values)
                            {
                                var GroupItem = item.GetGroupField(((LookUp)property.Custom).GpoupByField);
                                if (!model.DropDown.Groups.ContainsKey(GroupItem))
                                    model.DropDown.Groups.Add(GroupItem, new Dictionary<long, ItemBase>());

                                model.DropDown.Groups[GroupItem].Add(item.Id, item);
                            }
                        }
                        else
                        {
                            model.DropDown.Options = LookUpCollection[property.PropertyName + "s"];
                        }
                        List<ItemBase> ExcludeOptions = null;
                        if (property.Type == typeof(Role))
                        {
                            var User = Authentication.GetCurrentUser();
                            ExcludeOptions = new List<ItemBase>();
                            foreach (var item in model.DropDown.Options.Values)
                            {
                                if (!User.HasAtLeastOnePermission(((Role)item).RoleAccessPermission))
                                {
                                    ExcludeOptions.Add(item);
                                }
                            }
                        }
                        model.DropDown.ExcludeOptions = ExcludeOptions;
                        model.DropDown.AllowDefault = mode == DisplayMode.Search || (property.Custom is LookUp ? ((LookUp)property.Custom).DefaultValue : false);
                        if (property.Validation != null)
                        {
                            if (mode != DisplayMode.Search)
                            {
                                model.DropDown.ValidationType = property.Validation.ValidationType;
                            }
                            model.DropDown.ValidationFuction = property.Validation.ValidationFunction;
                            if (!string.IsNullOrEmpty(property.Validation.AlertMessage))
                                model.DropDown.RequiredMessage = property.Validation.AlertMessage;
                        }
                        ContextItemsHolder.ObjectToContext(ContextItemsHolder.DropDownLookUp, LookUpCollection);

                        if (property.Common.EditTemplate == EditTemplates.MultiSelect)
                        {
                            model.DropDown.Multiple = true;
                            model.DropDown.AllowDefault = false;
                            if (property.Custom is MultiCheck)
                            {
                                model.DropDown.ShowOptions = (property.Custom as MultiCheck).ShowOptions; 
                                model.DropDown.ItemType = (property.Custom as MultiCheck).ItemType; 
                            }
                            model.DropDown.Values = (Dictionary<long, ItemBase>)value;
                            if (model.DropDown.Values != null)
                            {
                                model.DropDown.StrValues = string.Join(",", model.DropDown.Values.Values.Select(o => o.Id.ToString()));
                            }
                            if (model.DropDown.Values == null)
                            {
                                model.DropDown.Values = new Dictionary<long, ItemBase>();
                            }
                            foreach (var key in model.DropDown.Values.Keys)
                            {
                                model.DropDown.Values[key].SetName(LookUpCollection[property.PropertyName + "s"].Values.FirstOrDefault(i => i.Id == model.DropDown.Values[key].Id).GetName());
                            }
                        }
                        else
                        {
                            model.DropDown.Value = model.Value.GetId().ToString();
                        }
                    }
                    else
                    {
                        model.DropDown.ReadOnly = true;
                        model.DropDown.ValueName = model.Value.GetName();
                    }
                    #endregion
                }
                model.CssView += property.Common.ViewCssClass;
                model.CssEdit += property.Common.EditCssClass;
            }
            catch (Exception ex)
            {
                General.TraceWarn(property.PropertyName);
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, System.Web.HttpContext.Current);
            }

            return model;
        }

        public object GetValue(AdvancedProperty property, string prefix = "", DisplayMode mode = DisplayMode.Simple)
        {

            //General.TraceWrite("DropDown:"+property.PropertyName);

            ItemBase item=null;
            if (property.Common.EditTemplate != EditTemplates.MultiSelect)
            {
                item = (ItemBase)Activator.CreateInstance(property.Type);
            }
            var items = new Dictionary<long, ItemBase>();

            try
            {
                if (property.Common.EditTemplate == EditTemplates.SelectList || property.Common.EditTemplate == EditTemplates.SelectListParent)
                {
                    var pId = System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "_id"];
                    if (!string.IsNullOrEmpty(pId))
                    {
                        long numid = 0;
                        if (!long.TryParse(pId, out numid) && !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "_id"]))
                        {
                            pId = System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "_id"];
                            numid = Convert.ToInt64(pId);
                        }
                        item.SetId(numid);
                    }

                    if (string.IsNullOrEmpty(pId) &&
                        string.IsNullOrEmpty(HttpContext.Current.Request.Form[prefix + property.PropertyName]))
                    {
                        item = null;
                    }
                    else
                    {
                        item.SetName(HttpContext.Current.Request.Form[prefix + property.PropertyName]);
                        if (mode == DisplayMode.PrintSearch && item.Id > 0)
                        {
                            item = item.PopulateOne(item);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix+property.PropertyName]) ||
                    !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName+"[]"]))
                {
                    if ((property.Custom != null && property.Custom is LookUp && (((LookUp)property.Custom).SearchAsMulticheck || ((LookUp)property.Custom).SearchAsMultiSelect)) 
                        && (mode == DisplayMode.Search || mode == DisplayMode.PrintSearch))
                    {
                        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName]) ||
                             !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "[]"]))
                        {
                            var delim = ((LookUp)property.Custom).SearchAsMulticheck?';':',';
                            var ids = (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName]) ? System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName].Split(delim) : System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "[]"].Split(delim));
                            foreach (var sid in ids)
                            {
                                if (!string.IsNullOrEmpty(sid))
                                {
                                    var id = Convert.ToInt64(sid);
                                    var it = (ItemBase)Activator.CreateInstance(property.Type);
                                    it.Id = id;
                                    items.Add(id, it);
                                }
                            }
                        }
                        if (items.Values.Any(i=>i.Id>0))
                            item.SearchItems = items;
                    }
                    else if (property.Common.EditTemplate == EditTemplates.MultiSelect)
                    {
                        if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName]) ||
                            !string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName + "[]"]))
                        {
                            var ids = (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName]) ? System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName].Split(',') : System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName+"[]"].Split(','));
                            foreach (var sid in ids)
                            {
                                if (!string.IsNullOrEmpty(sid))
                                {
                                    var id = Convert.ToInt64(sid);
                                    var it = (ItemBase)Activator.CreateInstance((property.Custom as MultiCheck).ItemType);
                                    it.Id = id;
                                    items.Add(id, it);
                                }
                            }
                        }
                    }
                    else
                    {
                        item.SetId(System.Web.HttpContext.Current.Request.Form[prefix + property.PropertyName]);
                        if (mode == DisplayMode.PrintSearch && item.Id>0)
                        {
                            item = item.PopulateOne(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, System.Web.HttpContext.Current);
            }

            if (property.Common.EditTemplate == EditTemplates.MultiSelect)
            {
                return items;
            }

            return item;
        }
        
        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";
            if (value == null)
                value = false;

            var item = (ItemBase)value;
            strValue = item.GetLinkName();

            if (mode == DisplayMode.CSV)
                strValue = strValue.Replace("&lt;br/&gt;", ";");
            else
                strValue = strValue.Replace("&lt;br/&gt;", "\n").TrimEnd(new char[] { '\r', '\n' });

            strValue = strValue.Replace("&lt;", "<");
            strValue = strValue.Replace("&gt;", ">");

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return ToString(value, property, BOItem, mode);
        }
    }
}
