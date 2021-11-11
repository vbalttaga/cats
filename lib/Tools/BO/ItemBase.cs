// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemBase.cs" company="GalexStudio">
//   Copyright �  2013
// </copyright>
// <summary>
//   Defines the ItemBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;
using LIB.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LIB.Tools.BO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    using LIB.AdvancedProperties;
    using LIB.BusinessObjects;
    using LIB.Tools.Security;
    using LIB.Tools.Utils;

    using Translate = LIB.Tools.Utils.Translate;
    using LIB.Tools.Controls;
    using LIB.Helpers;
    using LIB.Models.Common;

    /// <summary>
    /// The item base.
    /// </summary>
    [Serializable]
    [JsonObject]
    public class ItemBase : IConvertible
    {
        #region fields

        /// <summary>
        /// The zero id.
        /// </summary>
        public const string ZeroId = "0";

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemBase"/> class.
        /// </summary>
        public ItemBase()
        {
            this.Id = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemBase"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public ItemBase(long id)
        {
            this.Id = id;
        }

        #endregion

        #region operators
        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(ItemBase a, ItemBase b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Id == b.Id;
        }

        public static bool operator !=(ItemBase a, ItemBase b)
        {
            return !(a == b);
        }

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Db(_Ignore = true)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        [Db(_Editable = false, _Populate = false)]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        [Db(_Editable = false, _Populate = false, _ReadableOnlyName = true)]
        [JsonIgnore]
        public virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the deleted by.
        /// </summary>
        [Db(_Editable = false, _Populate = false, _ReadableOnlyName = true)]
        [JsonIgnore]
        public virtual User DeletedBy { get; set; }

        [Db(_Ignore = true)]
        [JsonIgnore]
        public string Category { get; set; } // for autocomplete

        [Db(_Ignore = true)]
        [JsonIgnore]
        public Dictionary<long, ItemBase> SearchItems { get; set; } // for search by multyselect

        #endregion
        
        #region methods

        public virtual string GetName()
        {
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(this.GetType());
            var properties = pss.GetProperties(pdc);
            if (properties.Get(GetCaption()) != null)
            {
                var property = properties.Get(GetCaption());
                if (property.PropertyDescriptor.GetValue(this) != null)
                {
                    return property.PropertyDescriptor.GetValue(this).ToString();
                }
                return "";
            }
            return this.GetType().Name;
        }

        public virtual string GetReportGridName()
        {
            return GetName();
        }

        public virtual string GetAutocompleteName()
        {
            return EscapeForJson(GetName());
        }
        static string EscapeForJson(string s)
        {
            string quoted = System.Web.Helpers.Json.Encode(s);
            return quoted.Substring(1, quoted.Length - 2).Replace("\"","''").Replace("\\", "\\\\");
        }
        public virtual string GetValue(string fieldName)
        {
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(this.GetType());
            var properties = pss.GetProperties(pdc);
            if (properties.Get(fieldName) != null)
            {
                var property = properties.Get(fieldName);
                if (property.PropertyDescriptor.GetValue(this) != null)
                {
                    return property.PropertyDescriptor.GetValue(this).ToString();
                }
                return "";
            }
            return this.GetType().Name;
        }

        public virtual string GetGroupField(string fieldName)
        {
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(this.GetType());
            var properties = pss.GetProperties(pdc);
            if (properties.Get(fieldName) != null)
            {
                var property = properties.Get(fieldName);
                if (property.PropertyDescriptor.GetValue(this) != null)
                {
                    if (property.PropertyDescriptor.GetValue(this) as ItemBase != null)
                        return ((ItemBase)property.PropertyDescriptor.GetValue(this)).GetName();

                    return property.PropertyDescriptor.GetValue(this).ToString();
                }
                return "";
            }
            return this.GetType().Name;
        }
        public virtual Object GetStatus()
        {
            return Id;
        }

        public virtual string GetAdvancedValue()
        {
            return "";
        }

        public virtual Object GetId()
        {
            return Id;
        }

        public virtual void SetId(object Id)
        {
            if (Id != null)
            {
                Int64 lId;
                if (Int64.TryParse(Id.ToString(), out lId))
                    this.Id = lId;
            }
        }

        public virtual string GetLinkName()
        {
            return GetName();
        }

        public virtual string GetLink()
        {
            return "DocControl/" + this.GetType().Name + "/" + GetId();
        }

        public virtual void SetName(object name)
        {
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(this.GetType());
            var properties = pss.GetProperties(pdc);
            if (properties.Get(GetCaption()) != null)
            {
                var property = properties.Get(GetCaption());
                property.PropertyDescriptor.SetValue(this, name);
            }
        }
        public virtual void SetName(string name)
        {
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(this.GetType());
            var properties = pss.GetProperties(pdc);
            if (properties.Get(GetCaption()) != null)
            {
                var property = properties.Get(GetCaption());
                property.PropertyDescriptor.SetValue(this, name);
            }
        }

        public virtual void SetNameAddl(IDataReader rdr, DataRow dr, string prefix)
        {
        }

        public virtual string GetCaption()
        {
            return "Name";
        }

        public virtual string AutocompleteControl()
        {
            return "List";
        }
        public virtual string GetAdditionalSelectQuery(AdvancedProperty property)
        {
            return "";
        }

        public virtual string GetAdditionalJoinQuery(AdvancedProperty property)
        {
            return "";
        }

        public virtual string GetAdditionalSimpleSearchQuery(AdvancedProperty property)
        {
            return "";
        }

        public virtual string GetAdvancedLookUpFilter(AdvancedProperty property)
        {
            return "";
        }

        public virtual ItemBase LoadFromString(string val, string field)
        {
            var Item = new ItemBase();
            Item.SetName(val);
            return Item;
        }

        public bool IsLoadedFromDB(Type Type)
        {
            BoAttribute boproperties = null;
            if (Type.GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boproperties = (BoAttribute)Type.GetCustomAttributes(typeof(BoAttribute), true)[0];
            }

            return boproperties==null || (boproperties!=null && boproperties.LoadFromDb);
        }

        public virtual bool ReportSingleItemRedirect(ItemBase item,out string redirectUrl)
        {
            redirectUrl = "";
            return false;
        }

        public virtual Type GetPermissionsType()
        {
            return this.GetType();
        }
        
        public virtual void ClearCache(string action,ItemBase item)
        {
        }
        public virtual void ClearCache(string action, Dictionary<long, ItemBase> dictionary)
        {
        }
        #endregion

        #region Convertable

        /// <summary>
        /// The get type code.
        /// </summary>
        /// <returns>
        /// The <see cref="TypeCode"/>.
        /// </returns>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        /// The get double value.
        /// </summary>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public long GetDoubleValue()
        {
            return this.Id;
        }

        /// <summary>
        /// The to boolean.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return this.Id != 0;
        }

        /// <summary>
        /// The to byte.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/>.
        /// </returns>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this.GetDoubleValue());
        }

        /// <summary>
        /// The to char.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="char"/>.
        /// </returns>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(this.GetDoubleValue());
        }

        /// <summary>
        /// The to date time.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(this.GetDoubleValue());
        }

        /// <summary>
        /// The to decimal.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/>.
        /// </returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this.GetDoubleValue());
        }

        /// <summary>
        /// The to double.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return this.GetDoubleValue();
        }

        /// <summary>
        /// The to integer 16.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="short"/>.
        /// </returns>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this.GetDoubleValue());
        }

        /// <summary>
        /// The to integer 32.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this.GetDoubleValue());
        }

        /// <summary>
        /// The to integer 64.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this.GetDoubleValue());
        }

        /// <summary>
        /// The to s byte.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="sbyte"/>.
        /// </returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this.GetDoubleValue());
        }

        /// <summary>
        /// The to single.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this.GetDoubleValue());
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return string.Format("({0}, {1})", "ID", this.Id);
        }

        /// <summary>
        /// The to type.
        /// </summary>
        /// <param name="conversionType">
        /// The conversion type.
        /// </param>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == typeof(string))
            {
                return Convert.ChangeType(this, conversionType);
            }

            var args = new object[1];
            args[0] = this.Id;

            return conversionType.InvokeMember(string.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, args);
        }

        /// <summary>
        /// The to unsigned integer 16.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="ushort"/>.
        /// </returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this.GetDoubleValue());
        }

        /// <summary>
        /// The to unsigned integer 32.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="uint"/>.
        /// </returns>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this.GetDoubleValue());
        }

        /// <summary>
        /// The to unsigned integer 64.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        /// <returns>
        /// The <see cref="ulong"/>.
        /// </returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this.GetDoubleValue());
        }

        #endregion

        #region CommonMethods

        #region Delete

        public virtual bool Delete(Dictionary<long, ItemBase> dictionary, string Comment = "", SqlConnection connection = null, User user = null)
        {
            var Reason = "";
            return Delete(dictionary, out Reason, Comment, connection, user);
        }
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="dictionary">
        ///     The dictionary.
        /// </param>
        /// <param name="connection">
        ///     The connection.
        /// </param>
        public virtual bool Delete(Dictionary<long, ItemBase> dictionary, out string Reason, string Comment = "", SqlConnection connection = null, User user = null)
        {
            if (connection == null)
            {
                connection = DataBase.ConnectionFromContext();
            }
            Reason = "";
            if (!this.CanDelete(dictionary, connection, out Reason))
            {
                return false;
            }

            BoAttribute boProperties = null;

            if (this.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boProperties = (BoAttribute)this.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
            }

            string strSql;
            var type = 0;
            bool buseGeneric = false;
            if (boProperties != null && !string.IsNullOrEmpty(boProperties.SpDelete))
            {
                strSql = boProperties.SpDelete;
            }
            else if (boProperties != null && (boProperties.UseCustomSp || boProperties.UseCustomSpDelete))
            {
                strSql = this.GetType().Name + "_Delete_" + this.GetType().Name;
            }
            else if (boProperties != null && boProperties.DoCancel)
            {
                strSql = "Generic_Cancel";
                buseGeneric = true;
                type = (int)OperationTypes.Cancel;
                if (string.IsNullOrEmpty(Comment))
                    Comment = "Canceled";
            }
            else
            {
                strSql = "Generic_Delete";
                buseGeneric = true;
                type = (int)OperationTypes.Delete;
                if (string.IsNullOrEmpty(Comment))
                    Comment = "Deleted";
            }

            var lcommand = new SqlCommand(strSql, connection) { CommandType = CommandType.StoredProcedure };
            var lcontinue = true;
            this.CreateDeleteParams(lcommand, dictionary, out lcontinue);

            if (lcontinue)
            {
                var itemIDs = dictionary.Values.Aggregate(string.Empty, (current, item) => current + (item.Id.ToString() + ";"));

                var param = new SqlParameter("@ItemIDs", SqlDbType.VarChar, 2000) { Value = itemIDs };
                lcommand.Parameters.Add(param);

                if (user != null || Authentication.CheckUser())
                {
                    param = new SqlParameter("@CancelUserID", SqlDbType.Int) { Value = (user == null ? Authentication.GetCurrentUser().Id : user.Id) };
                    lcommand.Parameters.Add(param);
                }

                if (buseGeneric)
                {
                    param = new SqlParameter("@TableName", SqlDbType.NVarChar, 400) { Value = this.GetType().Name };
                    lcommand.Parameters.Add(param);
                }

                param = new SqlParameter("@Comment", SqlDbType.NVarChar, 1000) { Value = Comment };
                lcommand.Parameters.Add(param);

                param = new SqlParameter("@Type", SqlDbType.TinyInt) { Value = type };
                lcommand.Parameters.Add(param);

                if (boProperties != null && boProperties.LogRevisions)
                {
                    param = new SqlParameter("@LogRevisions", SqlDbType.Bit) { Value = 1 };
                    lcommand.Parameters.Add(param);

                    param = new SqlParameter("@BOName", SqlDbType.NVarChar, 100) { Value = dictionary.Values.FirstOrDefault(i => i.Id > 0).GetName() };
                    lcommand.Parameters.Add(param);
                }
            }

            lcommand.ExecuteNonQuery();

            ClearCache("delete", dictionary);

            return true;
        }

        /// <summary>
        /// The create delete parameters.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="dictionary">
        /// The dictionary.
        /// </param>
        /// <param name="tocontinue">
        /// The to continue.
        /// </param>
        public virtual void CreateDeleteParams(SqlCommand command, Dictionary<long, ItemBase> dictionary, out bool tocontinue)
        {
            tocontinue = true;
        }

        public virtual bool CanDelete(Dictionary<long, ItemBase> dictionary, SqlConnection connection, out string reason)
        {
            reason = "";
            return true;
        }
        
        #endregion

        #region Update

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="connection">
        ///     The connection.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed. Suppression is OK here."),SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed. Suppression is OK here.")]
        public virtual void Update(ItemBase item, DisplayMode DisplayMode = DisplayMode.Advanced, string Comment = "Updated", SqlConnection connection = null)
        {
            if (connection == null)
            {
                connection = DataBase.ConnectionFromContext();
            }

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
            }
            string strSql;
            var isGeneric = false;
            if (boproperties != null && !string.IsNullOrEmpty(boproperties.SpUpdate))
            {
                strSql = boproperties.SpUpdate;
            }
            else if (boproperties != null && (boproperties.UseCustomSp || boproperties.UseCustomSpUpdate))
            {
                strSql = this.GetType().Name + "_Update";
            }
            else
            {
                strSql = "Generic_Update";
                isGeneric = true;
            }

            var lcommand = new SqlCommand(strSql, connection) { CommandType = CommandType.StoredProcedure };

            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetDbEditProperties(pdc, Authentication.GetCurrentUser(), DisplayMode);

            var tocontinue = true;
            this.CreateUpdateParams(lcommand, item, out tocontinue);

            if (tocontinue)
            {
                if (!isGeneric)
                {
                    #region Update Custom

                    foreach (AdvancedProperty property in properties)
                    {
                        var param = property.PropertyDescriptor.GetValue(item);
                        
                        if (param == null
                            || (param as ItemBase != null && (param as ItemBase).Id == 0)
                            || (param is DateTime && (DateTime)param == DateTime.MinValue))
                        {
                            continue;
                        }

                        var dbparam = new SqlParameter();
                        dbparam.SqlDbType = property.Db.ParamType;
                        dbparam.Size = property.Db.ParamSize;
                        dbparam.ParameterName = "@" + property.Db.ParamName;
                        if (param as ItemBase != null)
                        {
                            dbparam.Value = (param as ItemBase).Id;
                        }
                        else if (param as Dictionary<long, ItemBase> != null)
                        {
                            var itemids = (param as Dictionary<long, ItemBase>).Values.Aggregate(string.Empty, (current, i) => current + (i.Id.ToString(CultureInfo.InvariantCulture) + ";"));
                            dbparam.Value = itemids;
                        }
                        else
                        {
                            dbparam.Value = param;
                        }

                        if (property.Encryption != null && property.Encryption.Encrypted && dbparam.Value != null
                            && !string.IsNullOrEmpty(dbparam.Value.ToString()))
                        {
                            dbparam.Value = Crypt.Encrypt(dbparam.Value.ToString(), ConfigurationManager.AppSettings["CryptKey"]);
                        }

                        lcommand.Parameters.Add(dbparam);
                    }

                    var dparam = new SqlParameter("@" + item.GetType().Name + "ID", SqlDbType.BigInt) { Value = item.Id };
                    lcommand.Parameters.Add(dparam);

                    dparam = new SqlParameter("@TableName", SqlDbType.NVarChar, 400)
                                      {
                                          Value = this.GetType().Name
                                      };
                    lcommand.Parameters.Add(dparam);

                    dparam = new SqlParameter("@UserID", SqlDbType.BigInt) { Value = Authentication.GetCurrentUserId() };
                    lcommand.Parameters.Add(dparam);

                    dparam = new SqlParameter("@UpdatedBy", SqlDbType.BigInt) { Value = Authentication.GetCurrentUserId() };
                    lcommand.Parameters.Add(dparam);

                    dparam = new SqlParameter("@Comment", SqlDbType.NVarChar, 1000) { Value = Comment };
                    lcommand.Parameters.Add(dparam);

                    dparam = new SqlParameter("@Type", SqlDbType.TinyInt) { Value = OperationTypes.Update };
                    lcommand.Parameters.Add(dparam);

                    if (boproperties != null && boproperties.LogRevisions)
                    {
                        dparam = new SqlParameter("@LogRevisions", SqlDbType.Bit) { Value = 1 };
                        lcommand.Parameters.Add(dparam);

                        dparam = new SqlParameter("@BOName", SqlDbType.NVarChar, 100) { Value = item.GetName() };
                        lcommand.Parameters.Add(dparam);
                    }

                    #endregion
                }
                else
                {
                    #region Update Generic

                    var dbparam = new SqlParameter("@TableName", SqlDbType.NVarChar, 400)
                                      {
                                          Value = this.GetType().Name
                                      };
                    lcommand.Parameters.Add(dbparam);

                    dbparam = new SqlParameter("@UpdateQuery", SqlDbType.NVarChar, -1) { Value = string.Empty };
                    foreach (AdvancedProperty property in properties)
                    {
                        var param = property.PropertyDescriptor.GetValue(item);

                        if (param == null 
                            || (param as ItemBase != null && (
                                                            ((param as ItemBase).GetId() is long) && (((long)(param as ItemBase).GetId())==0)
                                                            ||
                                                            ((param as ItemBase).GetId() is string) && string.IsNullOrEmpty(((string)(param as ItemBase).GetId())))
                                                            )
                            || (param is Dictionary<long, ItemBase>)
                            || (property.Translate != null && property.Translate.Translatable)
                            || (property.Common.EditTemplate == EditTemplates.Password && string.IsNullOrEmpty(param.ToString()))
                            || (param is DateTime && (DateTime)param == DateTime.MinValue && property.Db.AllowNull == false))
                        {
                            continue;
                        }
                        
                        string parameterName;

                        this.CreateUpdateParamQuery(property, out parameterName);

                        if (string.IsNullOrEmpty(parameterName))
                        {
                            parameterName = property.Db.ParamName;
                        }

                        dbparam.Value += "["+ parameterName + "]= (SELECT [" + parameterName + "] FROM #ParameterValues WHERE ParamName='" + parameterName + "'),";
                    }

                    if (!string.IsNullOrEmpty(dbparam.Value.ToString()))
                    {
                        dbparam.Value = dbparam.Value.ToString().Remove(dbparam.Value.ToString().Length - 1);
                        lcommand.Parameters.Add(dbparam);
                    }

                    var updateParams = string.Empty;
                    var updateParamValues = string.Empty;
                    var parameterValues = string.Empty;
                    var updateToParameterValues = string.Empty;
                    var updateChildTables = string.Empty;
                    var translationUpdate = string.Empty;

                    foreach (AdvancedProperty property in properties)
                    {
                        var param = property.PropertyDescriptor.GetValue(item);
                        
                        if (param == null
                            || (param as ItemBase != null && (
                                                            ((param as ItemBase).GetId() is long) && (((long)(param as ItemBase).GetId()) == 0)
                                                            ||
                                                            ((param as ItemBase).GetId() is string) && string.IsNullOrEmpty(((string)(param as ItemBase).GetId())))
                                                            )
                            || (property.Common.EditTemplate == EditTemplates.Password && string.IsNullOrEmpty(param.ToString()))
                            || (param is DateTime && (DateTime)param == DateTime.MinValue && property.Db.AllowNull == false))
                        {
                            continue;
                        }

                        var parameterName = property.Db.ParamName;

                        if (param is Dictionary<long, ItemBase>)
                        {
                            if(property.Custom is MultiCheck){
                                var child_type=((MultiCheck)property.Custom).ItemType;

                                var TableName = ((MultiCheck)property.Custom).TableName;
                                if (string.IsNullOrEmpty(TableName))
                                    TableName = this.GetType().Name + child_type.Name;

                                updateChildTables += " DELETE FROM " + TableName + " WHERE " + this.GetType().Name + "Id = " + this.Id.ToString() + "\n\r"; 
                                foreach (var child_item in ((Dictionary<long, ItemBase>)param).Values)
                                {
                                    updateChildTables += " INSERT INTO " + TableName + "(" + this.GetType().Name + "Id," + child_type.Name + "Id) VALUES(" + this.Id.ToString() + "," + child_item.Id.ToString() + ")\n\r";
                                }
                            }
                        }
                        else
                        {
                            if (property.Translate != null && property.Translate.Translatable)
                            {
                                translationUpdate += parameterName + " + ';' +";
                                continue;
                            }

                            updateParams += parameterName + ";";
                            parameterValues += "[" + parameterName + "] " + property.Db.ParamType.ToString() + (property.Db.ParamType == System.Data.SqlDbType.Decimal ? "(14,4)" : string.Empty) + DataBase.GenerateParamSize(property.Db.ParamSize) + " null,";
                            if (property.Type == typeof(DateTime))
                            {
                                if (((DateTime)param) != DateTime.MinValue)
                                {
                                    updateToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType.ToString() + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                    updateToParameterValues += "SET  @" + parameterName + "=convert(datetime,(SELECT sv.value FROM dbo.Split(@UpdateParams,';') sp INNER JOIN dbo.Split(@UpdateParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "'), 120)  \n\r";
                                    updateToParameterValues += "INSERT INTO #ParameterValues(ParamName,[" + parameterName + "])\n\r";
                                    updateToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";
                                }
                                else
                                {
                                    updateToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType.ToString() + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                    updateToParameterValues += "SET  @" + parameterName + "=null  \n\r";
                                    updateToParameterValues += "INSERT INTO #ParameterValues(ParamName,[" + parameterName + "])\n\r";
                                    updateToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";
                                }
                            }
                            else
                            {
                                updateToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType.ToString() + (property.Db.ParamType == System.Data.SqlDbType.Decimal ? "(14,4)" : string.Empty) + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                updateToParameterValues += "SET  @" + parameterName + "=CAST((SELECT sv.value FROM dbo.Split(@UpdateParams,';') sp INNER JOIN dbo.Split(@UpdateParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "') AS " + property.Db.ParamType.ToString() + (property.Db.ParamType == System.Data.SqlDbType.Decimal ? "(14,4)" : string.Empty) + DataBase.GenerateParamSize(property.Db.ParamSize) + ")  \n\r";
                                updateToParameterValues += "INSERT INTO #ParameterValues(ParamName,[" + parameterName + "])\n\r";
                                updateToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";
                            }

                            tocontinue = true;
                            updateParamValues += this.CreateUpdateParamValues(item, param, out tocontinue);
                            if (!tocontinue)
                            {
                                continue;
                            }
                            if (param as ItemBase != null)
                            {
                                updateParamValues += (param as ItemBase).GetId().ToString() + ";";
                            }
                            else if (property.Translate != null && property.Translate.Translatable)
                            {
                                updateParamValues += Translate.ModifyStringToAlias(
                                    param.ToString(), property.Translate.TableName) + ";";
                                PropertyInfo pi = item.GetType().GetProperty(property.Translate.Alias);
                                pi.SetValue(
                                    item,
                                    Translate.ModifyStringToAlias(param.ToString(), property.Translate.TableName), null);

                            }
                            else if (property.Encryption != null && property.Encryption.Encrypted && dbparam.Value != null
                                     && !string.IsNullOrEmpty(param.ToString()))
                            {
                                updateParamValues += Crypt.Encrypt(
                                    param.ToString(), ConfigurationManager.AppSettings["CryptKey"]) + ";";
                            }
                            else if (property.Db.ParamType == SqlDbType.Bit)
                            {
                                updateParamValues += Convert.ToInt32(param).ToString(CultureInfo.InvariantCulture) + ";";
                            }
                            else if (property.Db.ParamType == SqlDbType.DateTime)
                            {
                                if (((DateTime)param) != DateTime.MinValue)
                                    updateParamValues += ((DateTime)param).ToString("yyyy-MM-dd HH:mm:ss") + ";";
                                else
                                    updateParamValues += "null;";
                            }
                            else if (property.Db.ParamType == SqlDbType.Decimal)
                            {
                                updateParamValues += ((decimal)param).ToString(CultureInfo.InvariantCulture) + ";";
                            }
                            else
                            {
                                updateParamValues += param.ToString().Replace(";", "|") + ";";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(updateParams))
                    {
                        dbparam = new SqlParameter("@UpdateParams", SqlDbType.NVarChar, -1) { Value = updateParams };
                        lcommand.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(updateChildTables))
                    {
                        dbparam = new SqlParameter("@updateChildTables", SqlDbType.NVarChar, -1) { Value = updateChildTables };
                        lcommand.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(updateParamValues))
                    {
                        dbparam = new SqlParameter("@UpdateParamValues", SqlDbType.NVarChar, -1)
                                      {
                                          Value = updateParamValues
                                      };
                        lcommand.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(parameterValues))
                    {
                        parameterValues = parameterValues.Remove(parameterValues.Length - 1);

                        dbparam = new SqlParameter("@ParameterValues", SqlDbType.NVarChar, -1)
                                      {
                                          Value = parameterValues
                                      };
                        lcommand.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(updateToParameterValues))
                    {
                        dbparam = new SqlParameter("@UpdateToParameterValues", SqlDbType.NVarChar, -1)
                                      {
                                          Value = updateToParameterValues
                                      };
                        lcommand.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(translationUpdate))
                    {
                        translationUpdate = translationUpdate.Remove(translationUpdate.Length - 8);

                        dbparam = new SqlParameter("@TranslationUpdate", SqlDbType.NVarChar, -1)
                                      {
                                          Value = translationUpdate
                                      };
                        lcommand.Parameters.Add(dbparam);
                    }


                    dbparam = new SqlParameter("@UpdatedBy", SqlDbType.BigInt) { Value = Authentication.GetCurrentUserId() };
                    lcommand.Parameters.Add(dbparam);

                    dbparam = new SqlParameter("@Comment", SqlDbType.NVarChar, 1000) { Value = Comment };
                    lcommand.Parameters.Add(dbparam);

                    dbparam = new SqlParameter("@Type", SqlDbType.TinyInt) { Value = OperationTypes.Update };
                    lcommand.Parameters.Add(dbparam);

                    var idparam = new SqlParameter("@ID", SqlDbType.BigInt) { Value = item.Id };
                    lcommand.Parameters.Add(idparam);
                    
                    if (boproperties != null && boproperties.LogRevisions)
                    {
                        dbparam = new SqlParameter("@LogRevisions", SqlDbType.Bit) { Value = 1 };
                        lcommand.Parameters.Add(dbparam);

                        dbparam = new SqlParameter("@BOName", SqlDbType.NVarChar, 100) { Value = item.GetName() };
                        lcommand.Parameters.Add(dbparam);
                    }

                    #endregion
                }

            }

            var aliases = Convert.ToString(lcommand.ExecuteScalar());

            ClearCache("update", item);

            if (!string.IsNullOrEmpty(aliases))
            {
                var transalionItemsCouter = 0;
                foreach (AdvancedProperty property in properties)
                {
                    if (property.Translate != null && property.Translate.Translatable)
                    {

                        var translationItem = new Translation();

                        translationItem.Text = property.PropertyDescriptor.GetValue(item).ToString();
                        translationItem.Alias = aliases.Split(';')[transalionItemsCouter];
                        translationItem.Language = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

                        Translation.Update(connection, translationItem);

                        transalionItemsCouter++;
                    }
                }
            }
        }

        /// <summary>
        /// The create update parameter query.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="paramName">
        /// The parameter name.
        /// </param>
        public virtual void CreateUpdateParamQuery(AdvancedProperty property, out string paramName)
        {
            paramName = string.Empty;
        }

        /// <summary>
        /// The create update parameter values.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="param">
        /// The parameter.
        /// </param>
        /// <param name="tocontinue">
        /// The to continue.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public virtual object CreateUpdateParamValues(ItemBase item, object param, out bool tocontinue)
        {
            tocontinue = true;
            return string.Empty;
        }

        /// <summary>
        /// The create update parameters.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="itemparameter">
        /// The item.
        /// </param>
        /// <param name="tocontinue">
        /// The to continue.
        /// </param>
        public virtual void CreateUpdateParams(SqlCommand command, ItemBase itemparameter, out bool tocontinue)
        {
            tocontinue = true;
        }

        #endregion

        #region Insert

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="item">
        ///     The item.
        /// </param>
        /// <param name="connection">
        ///     The connection.
        /// </param>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed. Suppression is OK here.")]
        public virtual void Insert(ItemBase item, string Comment = "Created", SqlConnection connection = null,User user=null)
        {
            if (connection == null)
            {
                connection = DataBase.ConnectionFromContext();
            }

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
            }

            string strSql;
            var toUseGeneric = false;
            if (boproperties != null && !string.IsNullOrEmpty(boproperties.SpInsert))
            {
                strSql = boproperties.SpInsert;
            }
            else if (boproperties != null && (boproperties.UseCustomSp || boproperties.UseCustomSpInsert))
            {
                strSql = this.GetType().Name + "_Insert";
            }
            else
            {
                strSql = "Generic_Insert";
                toUseGeneric = true;
            }

            var command = new SqlCommand(strSql, connection) { CommandType = CommandType.StoredProcedure };

            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetDbEditProperties(pdc, null);
            
            var tocontinue = true;
            this.CreateInsertParams(command, item, out tocontinue);

            if (tocontinue)
            {
                if (!toUseGeneric)
                {
                    #region Insert Custom
                    
                    foreach (AdvancedProperty property in properties)
                    {
                        var param = property.PropertyDescriptor.GetValue(item);

                        if (param == null
                               || (param as ItemBase != null && (
                                                               ((param as ItemBase).GetId() is long) && (((long)(param as ItemBase).GetId()) == 0)
                                                               ||
                                                               ((param as ItemBase).GetId() is string) && string.IsNullOrEmpty(((string)(param as ItemBase).GetId())))
                                                               )
                               || (param is DateTime && (DateTime)param == DateTime.MinValue && property.Db.AllowNull == false))
                        {
                            continue;
                        }
                        
                        var dbparam = new SqlParameter
                                          {
                                              SqlDbType = property.Db.ParamType,
                                              Size = property.Db.ParamSize
                                          };
                        if (property.Translate != null && property.Translate.Translatable)
                        {
                            dbparam.ParameterName = "@" + property.Translate.Alias;
                        }

                        if (property.Translate != null && property.Translate.Translatable)
                        {
                            dbparam.Value = Translate.ModifyStringToAlias(param.ToString(), property.Translate.TableName);
                            var pi = item.GetType().GetProperty(property.Translate.Alias);
                            pi.SetValue(item, Translate.ModifyStringToAlias(param.ToString(), property.Translate.TableName), null);
                        }
                        else
                        {
                            if (param as ItemBase != null)
                            {
                                dbparam.Value = (param as ItemBase).Id;
                            }
                            else
                            { 
                                dbparam.Value = param;
                            }

                            if (property.Encryption != null && property.Encryption.Encrypted && dbparam.Value != null
                                && !string.IsNullOrEmpty(dbparam.Value.ToString()))
                            {
                                dbparam.Value = Crypt.Encrypt(
                                    dbparam.Value.ToString(), ConfigurationManager.AppSettings["CryptKey"]);
                            }
                        }

                        command.Parameters.Add(dbparam);
                    }
                    #endregion
                }
                else
                {
                    #region Insert Generic
                    
                    var dbparam = new SqlParameter("@TableName", SqlDbType.NVarChar, 400)
                                      {
                                          Value = this.GetType().Name
                                      };
                    command.Parameters.Add(dbparam);

                    dbparam = new SqlParameter("@InsertQuery", SqlDbType.NVarChar, -1) { Value = string.Empty };
                    foreach (AdvancedProperty property in properties)
                    {
                        var param = property.PropertyDescriptor.GetValue(item);

                        if (param == null
                                || (param as ItemBase != null && (
                                                                ((param as ItemBase).GetId() is long) && (((long)(param as ItemBase).GetId()) == 0)
                                                                ||
                                                                ((param as ItemBase).GetId() is string) && string.IsNullOrEmpty(((string)(param as ItemBase).GetId())))
                                                                )
                                || (param is Dictionary<long, ItemBase>)
                                || (param is DateTime && (DateTime)param == DateTime.MinValue && property.Db.AllowNull == false))
                        {
                            continue;
                        }

                        string parameterName;

                        this.CreateInsertParamQuery(property, out parameterName);

                        if (string.IsNullOrEmpty(parameterName))
                        {
                            parameterName = "["+property.Db.ParamName+"]";
                        }

                        dbparam.Value += parameterName + ",";
                    }

                    dbparam.Value = dbparam.Value.ToString().Remove(dbparam.Value.ToString().Length - 1);
                    command.Parameters.Add(dbparam);

                    var insertParams = string.Empty;
                    var insertParamValues = string.Empty;
                    var parameterValues = string.Empty;
                    var insertToParameterValues = string.Empty;
                    var insertValuesQuery = string.Empty;
                    var insertChildTables = string.Empty;
                    var translationUpdate = string.Empty;

                    foreach (AdvancedProperty property in properties)
                    {
                        var param = property.PropertyDescriptor.GetValue(item);
                        
                        if (param == null
                            || (param as ItemBase != null && (
                                                            ((param as ItemBase).GetId() is long) && (((long)(param as ItemBase).GetId()) == 0)
                                                            ||
                                                            ((param as ItemBase).GetId() is string) && string.IsNullOrEmpty(((string)(param as ItemBase).GetId()))))
                            || (param is DateTime && (DateTime)param == DateTime.MinValue && property.Db.AllowNull == false))
                        {
                            continue;
                        }

                        var parameterName = property.Db.ParamName;

                        if (param is Dictionary<long, ItemBase>)
                        {
                            if (property.Custom is MultiCheck)
                            {
                                var child_type = ((MultiCheck)property.Custom).ItemType;

                                var TableName = ((MultiCheck)property.Custom).TableName; 
                                if (string.IsNullOrEmpty(TableName))
                                    TableName = this.GetType().Name + child_type.Name;

                                foreach (var child_item in ((Dictionary<long, ItemBase>)param).Values)
                                {
                                    insertChildTables += " INSERT INTO " + TableName + "(" + this.GetType().Name + "Id," + child_type.Name + "Id) VALUES(@ID," + child_item.Id.ToString() + ")\n\r";
                                }
                            }
                        }
                        else
                        {
                            insertParams += parameterName + ";";
                            parameterValues += "[" + parameterName + "] " + property.Db.ParamType.ToString() + (property.Db.ParamType == System.Data.SqlDbType.Decimal ? "(14,4)" : string.Empty) + DataBase.GenerateParamSize(property.Db.ParamSize) + " null,";
                            if (property.Type == typeof(DateTime))
                            {
                                if (((DateTime)param) != DateTime.MinValue)
                                {
                                    insertToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType.ToString() + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                    insertToParameterValues += "SET  @" + parameterName + "=convert(datetime,(SELECT sv.value FROM dbo.Split(@InsertParams,';') sp INNER JOIN dbo.Split(@InsertParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "'), 120)  \n\r";
                                    insertToParameterValues += "INSERT INTO #ParameterValues(ParamName,[" + parameterName + "])\n\r";
                                    insertToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";
                                }
                                else
                                {
                                    insertToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType.ToString() + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                    insertToParameterValues += "SET  @" + parameterName + "=null  \n\r";
                                    insertToParameterValues += "INSERT INTO #ParameterValues(ParamName,[" + parameterName + "])\n\r";
                                    insertToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";
                                }
                            }
                            else
                            {
                                insertToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType.ToString() + (property.Db.ParamType == System.Data.SqlDbType.Decimal ? "(14,4)" : string.Empty) + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                insertToParameterValues += "SET  @" + parameterName + "=CAST((SELECT sv.value FROM dbo.Split(@InsertParams,';') sp INNER JOIN dbo.Split(@InsertParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "') AS " + property.Db.ParamType.ToString() + (property.Db.ParamType == System.Data.SqlDbType.Decimal ? "(14,4)" : string.Empty) + DataBase.GenerateParamSize(property.Db.ParamSize) + ")  \n\r";
                                insertToParameterValues += "INSERT INTO #ParameterValues(ParamName,[" + parameterName + "])\n\r";
                                insertToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";
                            }
                            insertValuesQuery += "(SELECT [" + parameterName + "] FROM #ParameterValues WHERE ParamName='" + parameterName + "'),";

                            tocontinue = true;
                            insertParamValues += this.CreateInsertParamValues(item, param, out tocontinue);
                            if (tocontinue)
                            {
                                if (param as ItemBase != null)
                                {
                                    insertParamValues += (param as ItemBase).GetId().ToString() + ";";
                                }
                                else if (property.Translate != null && property.Translate.Translatable)
                                {
                                    insertParamValues += Translate.ModifyStringToAlias(
                                        param.ToString(), property.Translate.TableName) + ";";
                                    PropertyInfo pi = item.GetType().GetProperty(property.Translate.Alias);
                                    pi.SetValue(
                                        item,
                                        Translate.ModifyStringToAlias(
                                            param.ToString(), property.Translate.TableName),
                                        null);

                                    translationUpdate += parameterName + "=" + parameterName
                                                         + "+'_'+CAST(@ID AS varchar(10)),";
                                }
                                else if (property.Encryption != null && property.Encryption.Encrypted && dbparam.Value != null
                                         && !string.IsNullOrEmpty(param.ToString()))
                                {
                                    insertParamValues += Crypt.Encrypt(
                                        param.ToString(), ConfigurationManager.AppSettings["CryptKey"]) + ";";
                                }
                                else if (property.Db.ParamType == SqlDbType.Bit)
                                {
                                    insertParamValues += Convert.ToInt32(param).ToString() + ";";
                                }
                                else if (property.Db.ParamType == SqlDbType.DateTime)
                                {
                                    insertParamValues += ((DateTime)param).ToString("yyyy-MM-dd HH:mm:ss") + ";";
                                }
                                else if (property.Db.ParamType == SqlDbType.Decimal)
                                {
                                    insertParamValues += ((decimal)param).ToString(CultureInfo.InvariantCulture) + ";";
                                }
                                else
                                {
                                    insertParamValues += param.ToString().Replace(";", "|") + ";";
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(insertParams))
                    {
                        dbparam = new SqlParameter("@InsertParams", SqlDbType.NVarChar, -1) { Value = insertParams };
                        command.Parameters.Add(dbparam);
                    }
                    if (!string.IsNullOrEmpty(insertChildTables))
                    {
                        dbparam = new SqlParameter("@insertChildTables", SqlDbType.NVarChar, -1) { Value = insertChildTables };
                        command.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(insertParamValues))
                    {
                        dbparam = new SqlParameter("@InsertParamValues", SqlDbType.NVarChar, -1)
                                      {
                                          Value =
                                              insertParamValues
                                      };
                        command.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(parameterValues))
                    {
                        parameterValues = parameterValues.Remove(parameterValues.Length - 1);

                        dbparam = new SqlParameter("@ParameterValues", SqlDbType.NVarChar, -1)
                                      {
                                          Value = parameterValues
                                      };
                        command.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(insertToParameterValues))
                    {
                        dbparam = new SqlParameter("@InsertToParameterValues", SqlDbType.NVarChar, -1)
                                      {
                                          Value =
                                              insertToParameterValues
                                      };
                        command.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(translationUpdate))
                    {
                        translationUpdate = translationUpdate.Remove(translationUpdate.Length - 1); 

                        dbparam = new SqlParameter("@TranslationUpdate", SqlDbType.NVarChar, -1)
                                      {
                                          Value =
                                              translationUpdate
                                      };
                        command.Parameters.Add(dbparam);
                    }

                    if (!string.IsNullOrEmpty(insertValuesQuery))
                    {
                        insertValuesQuery = insertValuesQuery.Remove(insertValuesQuery.Length - 1);                    

                        dbparam = new SqlParameter("@InsertValuesQuery", SqlDbType.NVarChar, -1)
                                      {
                                          Value =
                                              insertValuesQuery
                                      };
                        command.Parameters.Add(dbparam);
                    }
                    #endregion
                }
                if (user == null)
                {
                    user = new User(Authentication.GetCurrentUserId());
                }

                var uparam = new SqlParameter("@CreatedBy", SqlDbType.BigInt)
                                 {
                                     Value = user.Id
                                 };
                command.Parameters.Add(uparam);


                var logparam = new SqlParameter("@Comment", SqlDbType.NVarChar, 1000) { Value = Comment };
                command.Parameters.Add(logparam);

                logparam = new SqlParameter("@Type", SqlDbType.TinyInt) { Value = OperationTypes.Insert };
                command.Parameters.Add(logparam);

                if (boproperties != null && boproperties.LogRevisions)
                {
                    logparam = new SqlParameter("@LogRevisions", SqlDbType.Bit) { Value = 1 };
                    command.Parameters.Add(logparam);

                    logparam = new SqlParameter("@BOName", SqlDbType.NVarChar, 100) { Value = item.GetName() };
                    command.Parameters.Add(logparam);
                }
            }

            item.Id = (long)command.ExecuteScalar();

            ClearCache("insert", item);

            #region Post process translations
            if (item.Id > 0)
            {
                foreach (AdvancedProperty property in properties)
                {
                    if (property.Translate != null && property.Translate.Translatable)
                    {
                        var pi = item.GetType().GetProperty(property.Translate.Alias);
                        var alias = pi.GetValue(item, null).ToString();
                        var o = property.PropertyDescriptor.GetValue(item);
                        if (o != null)
                        {
                            var value = o.ToString();

                            Translate.NewTranslation(connection, alias + "_" + item.Id.ToString(), value, System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);

                            property.PropertyDescriptor.SetValue(item, value);
                        }
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// The create insert parameters.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="itemparameter">
        /// The item parameter.
        /// </param>
        /// <param name="tocontinue">
        /// The to continue.
        /// </param>
        public virtual void CreateInsertParams(SqlCommand command, ItemBase itemparameter, out bool tocontinue)
        {
            tocontinue = true;
        }

        /// <summary>
        /// The create insert parameter query.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="paramName">
        /// The parameter name.
        /// </param>
        public virtual void CreateInsertParamQuery(AdvancedProperty property, out string paramName)
        {
            paramName = string.Empty;
        }

        /// <summary>
        /// The create insert parameter values.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="param">
        /// The parameter.
        /// </param>
        /// <param name="tocontinue">
        /// The to continue.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public virtual object CreateInsertParamValues(ItemBase item, object param, out bool tocontinue)
        {
            tocontinue = true;
            return string.Empty;
        }

        #endregion

        #region Populate
        public virtual Dictionary<long, ItemBase> PopulateGridTools(SqlConnection conn, 
                                                                ItemBase item, 
                                                                int iPagingStart, 
                                                                int iPagingLen,
                                                                string sSearch,
                                                                List<SortParameter> SortParameters,
                                                                bool ShowCanceled,
                                                                User sUser,
                                                                out long itotal,
                                                                out long idisplaytotal)
        {
            if (SortParameters == null || (SortParameters != null && SortParameters.Count == 0))
            {
                SortParameters = new List<SortParameter>();
                SortParameters.Add(new SortParameter() { Field = this.GetType().Name + "Id", Direction = "desc" });
            }
            return PopulateGeneric(conn,
                                 item,
                                 false,
                                 "",
                                 iPagingStart,
                                 iPagingLen,
                                 sSearch,
                                 SortParameters,
                                 ShowCanceled,
                                 sUser,
                                 false,
                                 false,
                                 out itotal,
                                 out idisplaytotal);
        }

        public virtual Dictionary<long, ItemBase> PopulateReport(SqlConnection conn,
                                                                ItemBase item,
                                                                int iPagingStart,
                                                                int iPagingLen,
                                                                string sSearch,
                                                                List<SortParameter> SortParameters,
                                                                User sUser,
                                                                out long itotal,
                                                                out long idisplaytotal)
        {
            return PopulateGeneric(conn,
                                 item,
                                 false,
                                 "",
                                 iPagingStart,
                                 iPagingLen,
                                 sSearch,
                                 SortParameters,
                                 false,
                                 sUser,
                                 true,
                                 false,
                                 out itotal,
                                 out idisplaytotal);
        }
        public virtual Dictionary<long, ItemBase> PopulateAutocomplete(string Param,string search, string AdvancedFilter = "")
        {
            long total;
            long displaytotal;
            return PopulateGeneric(null, null, false, "", 0, 0, search, null, false, null, false, false, out total, out displaytotal);
        }

        public virtual Dictionary<long, ItemBase> Populate(List<SortParameter> SortParameters)
        {
            long total;
            long displaytotal;
            return PopulateGeneric(null, null, false, "", 0, 0, "", SortParameters, false, null, false, false, out total, out displaytotal);
        }

        public virtual Dictionary<long, ItemBase> Populate(ItemBase item = null,
                                                                SqlConnection conn = null,
                                                                bool sortByName = false,
                                                                string AdvancedFilter = "",
                                                                bool ShowCanceled = false, 
                                                                User sUser = null,
                                                                bool ignoreQueryFilter = false)
        {
            long total;
            long displaytotal;
            return PopulateGeneric(conn, item, sortByName, AdvancedFilter, 0, 0, "", null, ShowCanceled, sUser, false, ignoreQueryFilter, out total, out displaytotal);
        }

        public virtual Dictionary<long, ItemBase> Populate(List<SortParameter> SortParameters, 
                                                        ItemBase item = null,
                                                        SqlConnection conn = null,
                                                        bool sortByName = false,
                                                        string AdvancedFilter = "",
                                                        bool ShowCanceled = false, 
                                                        User sUser = null)
        {
            if (SortParameters == null || (SortParameters != null && SortParameters.Count == 0))
            {
                SortParameters = new List<SortParameter>();
                SortParameters.Add(new SortParameter() { Field = this.GetType().Name + "Id", Direction = "desc" });
            }

            long total;
            long displaytotal;
            return PopulateGeneric(conn, item, sortByName, AdvancedFilter, 0, 0, "", SortParameters, ShowCanceled, sUser, false, false, out total, out displaytotal);
        }


        public virtual Dictionary<long, ItemBase> PopulateFrontEndItems()
        {
            long total;
            long displaytotal;
            return PopulateGeneric(null, null, false, "", 0, 0, "", null, false, null, false, false, out total, out displaytotal);
        }

        public virtual Dictionary<long, ItemBase> PopulateByParent(ItemBase item = null,
                                                                SqlConnection conn = null,
                                                                bool sortByName = false,
                                                                string AdvancedFilter = "",
                                                                bool ShowCanceled = false,
                                                                User sUser = null)
        {
            long total;
            long displaytotal;
            return PopulateGeneric(conn, item, sortByName, AdvancedFilter, 0, 0, "", null, ShowCanceled, sUser, false, false, out total, out displaytotal);
        }
        /// <summary>
        /// The populate tools.
        /// </summary>
        /// <param name="conn">
        /// The connection.
        /// </param>
        /// <param name="item">
        /// The s item.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>Dictionary</cref>
        ///     </see>
        ///     .
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed. Suppression is OK here.")]
        public Dictionary<long, ItemBase> PopulateGeneric(SqlConnection conn,
                                                                ItemBase item,
                                                                bool sortByName,
                                                                string AdvancedFilter,
                                                                int iPagingStart,
                                                                int iPagingLen,
                                                                string sSearch,
                                                                List<SortParameter> SortParameters,
                                                                bool ShowCanceled,
                                                                User sUser,
                                                                bool isReport,
                                                                bool ignoreQueryFilter,
                                                                out long itotal,
                                                                out long idisplaytotal)
        {
            if (conn == null)
            {
                conn = DataBase.ConnectionFromContext();
            }

            itotal = 0;
            idisplaytotal = 0;

            BoAttribute boproperties = null;
            if (this.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boproperties = (BoAttribute)this.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
            }

            string strSql;
            var toUseGeneric = false;
            if (boproperties != null && !string.IsNullOrEmpty(boproperties.SpPopulate))
            { 
                strSql = boproperties.SpPopulate;
            }
            else if (boproperties != null && (boproperties.UseCustomSp || boproperties.UseCustomSpPopulate))
            {
                strSql = this.GetType().Name + "_PopulateTools";
            }
            else
            {
                strSql = isReport ? "Generic_PopulateReport" : "Generic_PopulateTools";
                toUseGeneric = true;
            }

            if(sUser == null){                
                sUser = Authentication.GetCurrentUser();
            }

            if(!ignoreQueryFilter)
                AdvancedFilter += QueryFilter(sUser);

            var PreinitVar = "";
            if (boproperties != null && !string.IsNullOrEmpty(boproperties.DefaultFilter) && item==null && string.IsNullOrEmpty(sSearch))
            {
                AdvancedFilter += " AND " + boproperties.DefaultFilter+" ";
                PreinitVar = boproperties.PreinitVar;
            }

            var command = GetPopulateCommand(item, 
                                             strSql, 
                                             conn, 
                                             sUser,
                                             toUseGeneric, 
                                             sSearch, 
                                             sortByName, 
                                             ShowCanceled, 
                                             SortParameters, 
                                             AdvancedFilter,
                                             PreinitVar,
                                             isReport,
                                             boproperties,
                                             iPagingStart,
                                             iPagingLen);

            var items = new Dictionary<long, ItemBase>();

            var ordinal = 1;

            #region Read Data
            using (var rdr = command.ExecuteReader(CommandBehavior.SingleResult))
            {                
                while (rdr.Read())
                {
                    var databaseItem = (ItemBase)Activator.CreateInstance(this.GetType());
                    if(isReport)
                        databaseItem.Id = ordinal;

                    databaseItem = databaseItem.FromDataRow(rdr);
                    var addItem = true;
                    if (databaseItem != null)
                    {
                        var pss = new PropertySorter();
                        var pdc = TypeDescriptor.GetProperties(databaseItem.GetType());
                        var properties = pss.GetSearchProperties(pdc);

                        if ((from AdvancedProperty property in properties
                             where
                                 (property.Translate != null && property.Translate.Translatable)
                                 || (property.Encryption != null && property.Encryption.Encrypted)
                             let param = property.PropertyDescriptor.GetValue(databaseItem)
                             let sparam = property.PropertyDescriptor.GetValue(databaseItem)
                             where
                                 !string.IsNullOrEmpty((string)sparam)
                                 && !Strings.Like(param.ToString(), sparam.ToString())
                             select param).Any())
                        {
                            addItem = false;
                        }
                    }

                    if (addItem)
                    {
                        items.Add(databaseItem.Id, databaseItem);
                        ordinal++;
                    }
                }

                rdr.Close();

                //itotal = 0;
                itotal = Convert.ToInt64(command.Parameters["@TotalValues"].Value);
                idisplaytotal = Convert.ToInt64(command.Parameters["@TotalDispalyValues"].Value);
            }
            #endregion

            return items;
        }
        
        /// <summary>
        /// The create populate parameter query.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="paramName">
        /// The parameter name.
        /// </param>
        public virtual void CreatePopulateParamQuery(AdvancedProperty property,out string paramName)
        {
            paramName = string.Empty;
        }
        
        public virtual ItemBase PopulateFrontEnd(string additional, ItemBase searchItem, bool ShowCanceled = false, User sUser = null, SqlConnection conn = null)
        {
            return PopulateOne(searchItem, ShowCanceled, sUser, conn);
        }
        /// <summary>
        /// The populate one.
        /// </summary>
        /// <param name="searchItem">
        ///     The search item.
        /// </param>
        /// <param name="conn">
        ///     The connection.
        /// </param>
        /// <returns>
        /// The <see cref="ItemBase"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed. Suppression is OK here.")]
        public virtual ItemBase PopulateOne(ItemBase searchItem, bool ShowCanceled=false, User sUser=null, SqlConnection conn = null)
        {
            if (conn == null)
            {
                conn = DataBase.ConnectionFromContext();
            }

            BoAttribute boproperties = null;
            if (this.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boproperties = (BoAttribute)this.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
            }

            string strSql;
            var toUseGeneric = false;
            if (boproperties != null && !string.IsNullOrEmpty(boproperties.SpPopulate))
            {
                strSql = boproperties.SpPopulate;
            }
            else if (boproperties != null && (boproperties.UseCustomSp || boproperties.UseCustomSpPopulate))
            { 
                strSql = this.GetType().Name + "_PopulateTools";
            }
            else
            {
                strSql = "Generic_PopulateTools";
                toUseGeneric = true;
            }

            if (sUser == null)
            {
                sUser = Authentication.GetCurrentUser();
            }
            
            var command = GetPopulateCommand(searchItem,
                                             strSql,
                                             conn,
                                             sUser,
                                             toUseGeneric,
                                             String.Empty,
                                             false,
                                             ShowCanceled,
                                             null,
                                             String.Empty,
                                             String.Empty,
                                             false,
                                             boproperties,
                                             0,
                                             0);

            using (var rdr = command.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    return searchItem.FromDataRow(rdr);
                }

                rdr.Close();
            }

            return null;
        }

        private SqlCommand GetPopulateCommand(ItemBase item, 
                                                string strSql, 
                                                SqlConnection conn, 
                                                User sUser,
                                                bool toUseGeneric, 
                                                string sSearch, 
                                                bool sortByName, 
                                                bool ShowCanceled, 
                                                List<SortParameter> SortParameters,
                                                string AdvancedFilter,
                                                string PreinitVar,
                                                bool isReport,
                                                BoAttribute boproperties,
                                                int iPagingStart,
                                                int iPagingLen)
        {
            var command = new SqlCommand(strSql, conn) { CommandType = CommandType.StoredProcedure };

            if (item != null && !toUseGeneric)
            {
                #region Populate Custom

                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(item.GetType());
                var properties = pss.GetSearchProperties(pdc);

                foreach (var dbparam in from AdvancedProperty property in properties
                                        let param = property.PropertyDescriptor.GetValue(item)
                                        where
                                            param != null
                                            && !string.IsNullOrEmpty(param.ToString())
                                            && (param as ItemBase != null)
                                            && (((ItemBase)param).Id != 0)
                                            && (!(param is DateTime) || (DateTime)param != DateTime.MinValue)
                                            && (property.Translate == null || !property.Translate.Translatable)
                                            && (property.Encryption == null || !property.Encryption.Encrypted)
                                        select new SqlParameter
                                        {
                                            SqlDbType = property.Db.ParamType,
                                            ParameterName = "@s" + property.Db.ParamName,
                                            Size = property.Db.ParamSize,
                                            Value = param as ItemBase != null ? ((ItemBase)param).Id : param
                                        })
                {
                    command.Parameters.Add(dbparam);
                }

                #endregion
            }

            if (toUseGeneric)
            {
                #region Populate Generic

                var dbparam = new SqlParameter("@TableName", SqlDbType.NVarChar, 400) { Value = this.GetType().Name };
                command.Parameters.Add(dbparam);
                var sortBy = string.Empty;

                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(this.GetType());
                var properties = pss.GetDbProperties(pdc, null);

                var joinQuery = string.Empty;
                var SimpleSearchQuery = string.Empty;

                if (!string.IsNullOrEmpty(sSearch))
                {
                    SimpleSearchQuery = isReport ? " AND ( '' " : " AND ( CAST(t." + this.GetType().Name + "ID AS nvarchar(1000)) ";
                }

                if (boproperties != null && !string.IsNullOrEmpty(boproperties.DefaultSimpleSearch))
                {
                    SimpleSearchQuery += boproperties.DefaultSimpleSearch;
                }
                dbparam = new SqlParameter("@SelectQuery", SqlDbType.NVarChar, -1)
                {
                    Value = isReport?"":"t." + this.GetType().Name + "ID,"
                };

                var fisrtProperty = "";

                if (boproperties != null)
                {
                    dbparam.Value += boproperties.DefaultQuery;
                }
                foreach (AdvancedProperty property in properties)
                {
                    string parameterName;
                    string parameterNameForSort="";

                    this.CreatePopulateParamQuery(property, out parameterName);

                    if (string.IsNullOrEmpty(parameterName))
                    {
                        parameterName = property.Db.ParamName;
                    }

                    if (property.Type == typeof(Graphic))
                    {
                        joinQuery += " LEFT JOIN Graphic " + parameterName.Replace("ID", string.Empty) + " ON t."
                                     + parameterName + "=" + parameterName.Replace("ID", string.Empty) + ".GraphicId ";
                        parameterName = "t."+parameterName+" AS " + property.Db.Prefix + property.PropertyName + "Id,"
                                         + parameterName.Replace("ID", string.Empty) + ".BOName AS "
                                         + property.Db.Prefix + property.PropertyName + "BOName,"
                                         + parameterName.Replace("ID", string.Empty) + ".Name AS "
                                         + property.Db.Prefix + property.PropertyName + "Name,"
                                         + parameterName.Replace("ID", string.Empty) + ".Ext AS "
                                         + property.Db.Prefix + property.PropertyName + "Ext";
                    }
                    else if (property.Type == typeof(Document))
                    {
                        var typename = parameterName.ToLower().Replace("id", string.Empty);

                        joinQuery += " LEFT JOIN Document " + typename + " ON t."
                                     + parameterName + "=" + typename + ".DocumentId ";
                        parameterName = "t." + parameterName + " AS " + property.Db.Prefix + property.PropertyName + "Id,"
                                         + parameterName.Replace("ID", string.Empty) + ".Name AS "
                                         + property.Db.Prefix + property.PropertyName + "Name,"
                                         + parameterName.Replace("ID", string.Empty) + ".Ext AS "
                                         + property.Db.Prefix + property.PropertyName + "Ext";
                    }
                    else if ((property.Type.BaseType == typeof(ItemBase)
                        || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                        || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                        ) && IsLoadedFromDB(property.Type))
                    {
                        if (property.Custom != null && (property.Custom is LookUp) && !((LookUp)property.Custom).JoinTable)
                        {
                            parameterName = "t." + parameterName + " AS " + property.Db.Prefix + parameterName;
                        }
                        else
                        {
                            var sub_item = (ItemBase)Activator.CreateInstance(property.Type);
                            joinQuery += " LEFT JOIN [" + property.Type.Name + "] [" + property.PropertyName + "] ON t." + property.PropertyName + "Id = [" + property.PropertyName + "]." + property.Type.Name + "Id";
                            joinQuery += sub_item.GetAdditionalJoinQuery(property);
                            parameterName = "t." + parameterName + " AS " + property.Db.Prefix + parameterName + ",[" + property.PropertyName + "]." + sub_item.GetCaption() + " AS " + property.Db.Prefix + property.PropertyName + sub_item.GetCaption() + sub_item.GetAdditionalSelectQuery(property);
                            if (string.IsNullOrEmpty(fisrtProperty))
                                fisrtProperty = "t." + property.Db.ParamName;

                            parameterNameForSort = "t." + property.Db.Prefix + property.Db.ParamName;
                        }
                    }
                    else if (property.Type == typeof(Dictionary<long, ItemBase>) && (property.Custom is MultiCheck))
                    {
                        var sub_item_type = (property.Custom as MultiCheck).ItemType;

                        var LinktableName = ((MultiCheck)property.Custom).TableName;
                        if (string.IsNullOrEmpty(LinktableName))
                            LinktableName = this.GetType().Name + sub_item_type.Name;
                        
                        joinQuery += " CROSS APPLY";
                        joinQuery += "(";
                        joinQuery += "SELECT CAST(" + property.PropertyName + "." + sub_item_type.Name + "Id as nvarchar(100)) + ';' ";
                        joinQuery += "FROM [" + LinktableName + "] [" + property.PropertyName + "]  ";
                        joinQuery += "WHERE [" + property.PropertyName + "]." + this.GetType().Name + "Id=t." + this.GetType().Name + "Id ";
                        joinQuery += "FOR XML PATH('') ";
                        joinQuery += ") ["+property.PropertyName+"] (" + property.PropertyName + "_list) ";

                        parameterName = "SUBSTRING([" + property.PropertyName + "]." + property.PropertyName + "_list,1, LEN([" + property.PropertyName + "]." + property.PropertyName + "_list) - 1) " + property.PropertyName;
                    }
                    else
                    {
                        parameterName = "t.[" + parameterName + "]";
                        parameterNameForSort = parameterName;
                        if (string.IsNullOrEmpty(fisrtProperty))
                            fisrtProperty = parameterName;
                    }

                    if (property.Db.Sort != DbSortMode.None && !string.IsNullOrEmpty(parameterNameForSort))
                    {
                        if (!string.IsNullOrEmpty(sortBy))
                        {
                            sortBy += "," + parameterNameForSort + (property.Db.Sort == DbSortMode.Asc ? " asc" : " desc");
                        }
                        else
                        {
                            sortBy = " ORDER BY " + parameterNameForSort + (property.Db.Sort == DbSortMode.Asc ? " asc" : " desc");
                        }
                    }

                    if (property.PropertyName == "SortOrder")
                    {
                        if (!string.IsNullOrEmpty(sortBy))
                        {
                            sortBy += ",t.SortOrder";
                        }
                        else
                        {
                            sortBy = " ORDER BY t.SortOrder";
                        }
                    }

                    if (!string.IsNullOrEmpty(sSearch))
                    {
                        if (property.Type == typeof(string) || property.Type == typeof(int) || property.Type == typeof(int?) || property.Type == typeof(decimal) || property.Type == typeof(decimal?) || property.Type == typeof(long) || property.Type == typeof(long?))
                        {
                            SimpleSearchQuery += " + '|' + COALESCE(CAST(" + parameterName + " as nvarchar(1000)),'')";
                        }
                        if((property.Type.BaseType == typeof(ItemBase)
                        || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                        || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                        ) && IsLoadedFromDB(property.Type))
                        {
                            var sub_item = (ItemBase)Activator.CreateInstance(property.Type);
                            SimpleSearchQuery += "+ '|' + CAST(COALESCE([" + property.PropertyName + "]." + sub_item.GetCaption()+",'') as nvarchar(1000))"+ sub_item.GetAdditionalSimpleSearchQuery(property);
                        }
                    }

                    dbparam.Value += parameterName + ",";
                }

                dbparam.Value = dbparam.Value.ToString().Remove(dbparam.Value.ToString().Length - 1);
                command.Parameters.Add(dbparam);

                if (sortByName)
                {
                    sortBy = " ORDER BY t." + this.GetCaption();
                }

                if (SortParameters != null && SortParameters.Count > 0)
                {
                    sortBy = " ORDER BY ";

                    var ind = 0;
                    foreach (var col in SortParameters)
                    {
                        ind++;
                        sortBy += "t." + col.Field + " " + col.Direction + (ind != SortParameters.Count ? ", " : "");
                    }
                }
                
                if (isReport && string.IsNullOrEmpty(sortBy))
                {
                    sortBy = "Order by "+fisrtProperty;
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    dbparam = new SqlParameter("@SortBy", SqlDbType.NVarChar, -1) { Value = sortBy };
                    command.Parameters.Add(dbparam);
                }

                if (!string.IsNullOrEmpty(sSearch))
                {
                    SimpleSearchQuery += ") like '%'+@sSearch+'%'";

                    dbparam = new SqlParameter("@sSearch", SqlDbType.NVarChar, 100) { Value = sSearch };
                    command.Parameters.Add(dbparam);

                    dbparam = new SqlParameter("@SimpleSearchQuery", SqlDbType.NVarChar, -1) { Value = SimpleSearchQuery };
                    command.Parameters.Add(dbparam);
                }

                if (boproperties != null && !string.IsNullOrEmpty(boproperties.AfterPaginAdditionalQuery))
                {
                    dbparam = new SqlParameter("@AfterPaginAdditionalQuery", SqlDbType.NVarChar, -1) { Value = boproperties.AfterPaginAdditionalQuery };
                    command.Parameters.Add(dbparam);
                }

                if (sUser != null && boproperties != null && boproperties.ReadAllAccess != 0 && !sUser.HasAtLeastOnePermission(boproperties.ReadAllAccess))
                {
                    dbparam = new SqlParameter("@sUser", SqlDbType.BigInt) { Value = sUser.Id };
                    command.Parameters.Add(dbparam);
                }

                if (boproperties != null && !string.IsNullOrEmpty(boproperties.AdditionalJoin))
                {
                    dbparam = new SqlParameter("@AdditionalJoin", SqlDbType.NVarChar,-1) { Value = boproperties.AdditionalJoin };
                    command.Parameters.Add(dbparam);
                }

                if (!string.IsNullOrEmpty(joinQuery))
                {
                    dbparam = new SqlParameter("@JoinQuery", SqlDbType.NVarChar, -1) { Value = joinQuery };
                    command.Parameters.Add(dbparam);
                }

                if (!string.IsNullOrEmpty(AdvancedFilter))
                {
                    dbparam = new SqlParameter("@AdvancedFilter", SqlDbType.NVarChar, -1) { Value = AdvancedFilter };
                    command.Parameters.Add(dbparam);
                }

                if (!string.IsNullOrEmpty(PreinitVar))
                {
                    dbparam = new SqlParameter("@PreinitVar", SqlDbType.NVarChar, -1) { Value = PreinitVar };
                    command.Parameters.Add(dbparam);
                }

                if (ShowCanceled)
                {
                    dbparam = new SqlParameter("@ShowCanceled", SqlDbType.Bit) { Value = ShowCanceled };
                    command.Parameters.Add(dbparam);
                }

                dbparam = new SqlParameter("@PagingStart", SqlDbType.Int) { Value = iPagingStart };
                command.Parameters.Add(dbparam);

                dbparam = new SqlParameter("@PagingLen", SqlDbType.Int) { Value = iPagingLen };
                command.Parameters.Add(dbparam);

                dbparam = new SqlParameter("@TotalValues", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                command.Parameters.Add(dbparam);

                dbparam = new SqlParameter("@TotalDispalyValues", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                command.Parameters.Add(dbparam);

                #region Search Generic
                if (item != null)
                {
                    if (item.Id > 0)
                    {
                        dbparam = new SqlParameter("@sID", SqlDbType.BigInt) { Value = item.Id };
                        command.Parameters.Add(dbparam);
                    }
                    else
                    {
                        pss = new PropertySorter();
                        pdc = TypeDescriptor.GetProperties(this.GetType());
                        properties = pss.GetSearchProperties(pdc);

                        var searchParams = string.Empty;
                        var searchParamValues = string.Empty;
                        var parameterValues = string.Empty;
                        var insertToParameterValues = string.Empty;
                        var searchQuery = string.Empty;

                        foreach (AdvancedProperty property in properties)
                        {
                            if ((property.Translate != null && property.Translate.Translatable)
                                || (property.Encryption != null && property.Encryption.Encrypted)
                                || (property.PropertyName == "Id"))
                            {
                                continue;
                            }

                            var param = property.PropertyDescriptor.GetValue(item);

                            if (param == null
                                || string.IsNullOrEmpty(param.ToString())
                                || (param as ItemBase != null && (param as ItemBase).Id == 0 && (param as ItemBase).SearchItems==null)
                                || (param is bool && !Convert.ToBoolean(param))
                                || (param is int && (int)param == 0)
                                || (param is long && (long)param == 0)
                                || (param is Guid)
                                || (param is DateTime && (DateTime)param == DateTime.MinValue)
                                || (param is DateRange && ((DateRange)param).from == DateTime.MinValue && ((DateRange)param).to == DateTime.MinValue)
                                || (param is NumbersRange && ((NumbersRange)param).from == 0 && ((NumbersRange)param).to == 0))
                            {
                                continue;
                            }

                            var parameterName = property.Db.ParamName;

                            if (param is DateRange)
                            {
                                if (((DateRange)param).from != DateTime.MinValue)
                                {
                                    searchParams += parameterName + "_From;";
                                    parameterValues += parameterName + "_From datetime null,";

                                    insertToParameterValues += "DECLARE @" + parameterName + "_From datetime \n\r";
                                    insertToParameterValues += "SET  @" + parameterName + "_From=convert(datetime,(SELECT sv.value FROM dbo.Split(@SearchParams,';') sp INNER JOIN dbo.Split(@SearchParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "_From'), 120)  \n\r";
                                    insertToParameterValues += "INSERT INTO #ParameterValues(ParamName," + parameterName + "_From)\n\r";
                                    insertToParameterValues += "VALUES('" + parameterName + "_From',@" + parameterName + "_From)\n\r";

                                    searchParamValues += ((DateRange)param).from.ToString("yyyy-MM-dd HH:mm:ss") + ";";
                                }

                                if (((DateRange)param).to != DateTime.MinValue)
                                {
                                    searchParams += parameterName + "_To;";
                                    parameterValues += parameterName + "_To datetime null,";

                                    insertToParameterValues += "DECLARE @" + parameterName + "_To datetime \n\r";
                                    insertToParameterValues += "SET  @" + parameterName + "_To=convert(datetime,(SELECT sv.value FROM dbo.Split(@SearchParams,';') sp INNER JOIN dbo.Split(@SearchParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "_To'), 120)  \n\r";
                                    insertToParameterValues += "INSERT INTO #ParameterValues(ParamName," + parameterName + "_To)\n\r";
                                    insertToParameterValues += "VALUES('" + parameterName + "_To',@" + parameterName + "_To)\n\r";

                                    searchParamValues += ((DateRange)param).to.ToString("yyyy-MM-dd HH:mm:ss") + ";";
                                }
                                if (((DateRange)param).from != DateTime.MinValue && ((DateRange)param).to != DateTime.MinValue)
                                {
                                    searchQuery += " AND  t." + parameterName + " between (SELECT " + parameterName + "_From FROM #ParameterValues WHERE ParamName='" + parameterName + "_From') AND (SELECT " + parameterName + "_To FROM #ParameterValues WHERE ParamName='" + parameterName + "_To')\n\r";
                                }
                                else if(((DateRange)param).from != DateTime.MinValue)
                                {
                                    searchQuery += " AND  t." + parameterName + " >= (SELECT " + parameterName + "_From FROM #ParameterValues WHERE ParamName='" + parameterName + "_From') \n\r";
                                }
                                else
                                {
                                    searchQuery += " AND  t." + parameterName + " <= (SELECT " + parameterName + "_To FROM #ParameterValues WHERE ParamName='" + parameterName + "_To') \n\r";
                                }

                            }
                            else if (param is NumbersRange)
                            {
                                if (((NumbersRange)param).from != 0)
                                {
                                    searchParams += parameterName + "_From;";
                                    parameterValues += parameterName + "_From int null,";

                                    insertToParameterValues += "DECLARE @" + parameterName + "_From int \n\r";
                                    insertToParameterValues += "SET  @" + parameterName + "_From=CAST((SELECT sv.value FROM dbo.Split(@SearchParams,';') sp INNER JOIN dbo.Split(@SearchParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "_From') as int)  \n\r";
                                    insertToParameterValues += "INSERT INTO #ParameterValues(ParamName," + parameterName + "_From)\n\r";
                                    insertToParameterValues += "VALUES('" + parameterName + "_From',@" + parameterName + "_From)\n\r";

                                    searchParamValues += ((NumbersRange)param).from.ToString() + ";";
                                }

                                if (((NumbersRange)param).to != 0)
                                {
                                    searchParams += parameterName + "_To;";
                                    parameterValues += parameterName + "_To int null,";

                                    insertToParameterValues += "DECLARE @" + parameterName + "_To int \n\r";
                                    insertToParameterValues += "SET  @" + parameterName + "_To=convert((SELECT sv.value FROM dbo.Split(@SearchParams,';') sp INNER JOIN dbo.Split(@SearchParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "_To') as int)  \n\r";
                                    insertToParameterValues += "INSERT INTO #ParameterValues(ParamName," + parameterName + "_To)\n\r";
                                    insertToParameterValues += "VALUES('" + parameterName + "_To',@" + parameterName + "_To)\n\r";

                                    searchParamValues += ((NumbersRange)param).to.ToString() + ";";
                                }
                                if (((NumbersRange)param).from != 0 && ((NumbersRange)param).to != 0)
                                {
                                    searchQuery += " AND  t." + parameterName + " between (SELECT " + parameterName + "_From FROM #ParameterValues WHERE ParamName='" + parameterName + "_From') AND (SELECT " + parameterName + "_To FROM #ParameterValues WHERE ParamName='" + parameterName + "_To')\n\r";
                                }
                                else if (((NumbersRange)param).from != 0)
                                {
                                    searchQuery += " AND  t." + parameterName + " >= (SELECT " + parameterName + "_From FROM #ParameterValues WHERE ParamName='" + parameterName + "_From') \n\r";
                                }
                                else
                                {
                                    searchQuery += " AND  t." + parameterName + " <= (SELECT " + parameterName + "_To FROM #ParameterValues WHERE ParamName='" + parameterName + "_To') \n\r";
                                }

                            }
                            else if (param as ItemBase != null && (param as ItemBase).SearchItems != null)
                            {
                                searchQuery += " AND t." + parameterName + " in (" + string.Join(",", (param as ItemBase).SearchItems.Values.Where(i => i.Id > 0).Select(p => p.Id.ToString())) + ")\n\r";
                            }
                            else
                            {
                                searchParams += parameterName + ";";
                                parameterValues += parameterName + " " + property.Db.ParamType.ToString() + DataBase.GenerateParamSize(property.Db.ParamSize) + " null,";

                                insertToParameterValues += "DECLARE @" + parameterName + " " + property.Db.ParamType + DataBase.GenerateParamSize(property.Db.ParamSize) + "\n\r";
                                insertToParameterValues += "SET  @" + parameterName + "=CAST((SELECT sv.value FROM dbo.Split(@SearchParams,';') sp INNER JOIN dbo.Split(@SearchParamValues,';') sv ON sp.Ident=sv.Ident WHERE sp.value='" + parameterName + "') AS " + property.Db.ParamType + DataBase.GenerateParamSize(property.Db.ParamSize) + ")  \n\r";
                                insertToParameterValues += "INSERT INTO #ParameterValues(ParamName," + parameterName + ")\n\r";
                                insertToParameterValues += "VALUES('" + parameterName + "',@" + parameterName + ")\n\r";

                                if (param is string)
                                {
                                    searchQuery += " AND ((SELECT " + parameterName + " FROM #ParameterValues WHERE ParamName='" + parameterName + "') IS NULL OR t." + parameterName + " like '%'+(SELECT " + parameterName + " FROM #ParameterValues WHERE ParamName='" + parameterName + "')+'%')\n\r";
                                }
                                else
                                {
                                    if (property.Custom != null && (property.Custom is LookUp) && !string.IsNullOrEmpty(((LookUp)property.Custom).SearchQuery))
                                    {
                                        searchQuery += " AND ((SELECT " + parameterName + " FROM #ParameterValues WHERE ParamName='" + parameterName + "') IS NULL OR ";
                                        searchQuery += "(SELECT " + parameterName + " FROM #ParameterValues WHERE ParamName='" + parameterName + "') IN "+ ((LookUp)property.Custom).SearchQuery + ")\n\r";
                                    }
                                    else
                                    {
                                        searchQuery += " AND ((SELECT " + parameterName + " FROM #ParameterValues WHERE ParamName='" + parameterName + "') IS NULL OR t." + parameterName + "=(SELECT " + parameterName + " FROM #ParameterValues WHERE ParamName='" + parameterName + "'))\n\r";
                                    }
                                }

                                if (param as ItemBase != null)
                                {
                                    searchParamValues += (param as ItemBase).Id.ToString(CultureInfo.InvariantCulture) + ";";
                                }
                                else
                                {
                                    searchParamValues += param + ";";
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(searchParams))
                        {
                            dbparam = new SqlParameter("@SearchParams", SqlDbType.NVarChar, -1) { Value = searchParams };
                            command.Parameters.Add(dbparam);
                        }

                        if (!string.IsNullOrEmpty(searchParamValues))
                        {
                            dbparam = new SqlParameter("@SearchParamValues", SqlDbType.NVarChar, -1)
                            {
                                Value = searchParamValues
                            };
                            command.Parameters.Add(dbparam);
                        }

                        if (!string.IsNullOrEmpty(parameterValues))
                        {
                            parameterValues = parameterValues.Remove(parameterValues.Length - 1);

                            dbparam = new SqlParameter("@ParameterValues", SqlDbType.NVarChar, -1)
                            {
                                Value = parameterValues
                            };
                            command.Parameters.Add(dbparam);
                        }

                        if (!string.IsNullOrEmpty(insertToParameterValues))
                        {
                            dbparam = new SqlParameter("@InsertToParameterValues", SqlDbType.NVarChar, -1)
                            {
                                Value = insertToParameterValues
                            };
                            command.Parameters.Add(dbparam);
                        }

                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            dbparam = new SqlParameter("@SearchQuery", SqlDbType.NVarChar, -1) { Value = searchQuery };
                            command.Parameters.Add(dbparam);
                        }
                    }
                }
                #endregion

                #endregion
            }

            return command;
        }

        #endregion
        
        #region Form
        
        /// <summary>
        /// The create populate parameter query.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="paramName">
        /// The parameter name.
        /// </param>
        public virtual void CollectFromForm(string prefix="")
        {
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(this);
            var properties = pss.GetFormSaveProperties(pdc, Authentication.GetCurrentUser());

            foreach (AdvancedProperty property in properties)
            {
                //General.TraceWrite(property.PropertyName);
                property.PropertyDescriptor.SetValue(this, property.GetDataProcessor().GetValue(property, prefix));
            }
        }

        /// <summary>
        /// The create populate parameter query.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="paramName">
        /// The parameter name.
        /// </param>
        public virtual RequestResult SaveForm()
        {
            if (this.Id > 0)
            {
                this.Update(this);

                return new RequestResult() { Result = RequestResultType.Reload, RedirectURL = URLHelper.GetUrl("DocControl/"+ this.GetType().Name + "/" + Id.ToString()), Message = "Datele memorate cu success" };
            }

            this.Insert(this);

            return new RequestResult() { Result = RequestResultType.Reload, RedirectURL = URLHelper.GetUrl("DocControl/" + this.GetType().Name + "/" + Id.ToString()), Message = this.GetType().Name+" Creat" };
        }

        public virtual string LoadPageTitle()
        {
            return "";
        }

        public virtual string QueryFilter(User User)
        {
            return "";
        }

        public virtual bool DefaultReportFilter(bool is_search)
        {
            return is_search;
        }
        public virtual string SimpleSearch(string sSearch)
        {
            return sSearch;
        }

        public virtual List<iBaseControlModel> LoadBreadcrumbs()
        {
            return new List<iBaseControlModel>();
        }

        public virtual List<iBaseControlModel> LoadQuickLinks()
        {
            return new List<iBaseControlModel>();
        }

        public virtual List<iBaseControlModel> LoadContextReports()
        {
            return new List<iBaseControlModel>();
        }
        public virtual Dictionary<string, object> LoadFrontEndViewdata()
        {
            var ViewData = new Dictionary<string, object>();

            ViewData.Add("ParentId", Convert.ToInt64(System.Web.HttpContext.Current.Request.Form["ParentId"]));

            return ViewData;
        }
        #endregion

        #region FromDataRow

        /// <summary>
        /// The from data row.
        /// </summary>
        /// <param name="dr">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="ItemBase"/>.
        /// </returns>
        public ItemBase FromDataRow(IDataReader dr)
        {
            return this.FromDataRow(dr, null);
        }

        /// <summary>
        /// The from data row.
        /// </summary>
        /// <param name="dr">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="ItemBase"/>.
        /// </returns>
        public ItemBase FromDataRow(DataRow dr)
        {
            return this.FromDataRow(null, dr);
        }

        /// <summary>
        /// The from data row.
        /// </summary>
        /// <param name="rdr">
        /// The data reader.
        /// </param>
        /// <param name="dr">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="ItemBase"/>.
        /// </returns>
        public ItemBase FromDataRow(IDataReader rdr, DataRow dr)
        {
            return this.FromDataRow(rdr, dr, string.Empty);
        }

        /// <summary>
        /// The from data row.
        /// </summary>
        /// <param name="rdr">
        /// The data reader.
        /// </param>
        /// <param name="dr">
        /// The data row.
        /// </param>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <returns>
        /// The <see cref="ItemBase"/>.
        /// </returns>
        public ItemBase FromDataRow(IDataReader rdr, DataRow dr, string prefix)
        {
            //General.TraceWrite("FromDataRow Type: " + this.GetType().Name);
            var tocontinue = true;
            this.FromDataRow(rdr, dr, prefix, out tocontinue);
            if ((tocontinue && (DataBase.ReaderValueExist(rdr, dr, prefix + this.GetType().Name + "Id") || DataBase.ReaderValueExist(rdr, dr, prefix + "Id"))) || Id>0)
            {
                this.SetId(DataBase.GetExistReaderValue(rdr, dr, prefix + this.GetType().Name + "Id", DataBase.GetExistReaderValue(rdr, dr, prefix + "Id", this.GetId())));
                this.SetName(DataBase.GetExistReaderValue(rdr, dr, prefix + this.GetCaption(), null));
                this.SetNameAddl(rdr, dr, prefix);
                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(this.GetType());
                var properties = pss.GetDbReadProperties(pdc, null);

                foreach (AdvancedProperty property in properties)
                {
                    //General.TraceWrite("FromDataRow: " + property.PropertyName);
                    if (property.Db.Readable==null || property.Db.Readable == true)
                    {
                        if ((property.Type!=null && property.Type.BaseType == typeof(ItemBase))
                            || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                            || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase)))
                        {
                            if (property.Db.ReadableOnlyName==null || property.Db.ReadableOnlyName == false)
                            {
                                if (property.Db.Prefix + property.PropertyName != prefix || string.IsNullOrEmpty(prefix))
                                {
                                    property.PropertyDescriptor.SetValue(
                                        this,
                                        ((ItemBase)
                                         property.Type.InvokeMember(
                                             string.Empty, BindingFlags.CreateInstance, null, null, new object[0])).FromDataRow(
                                                 rdr, dr, property.Db.Prefix + property.PropertyName));
                                }
                            }
                            else
                            {
                                var item = ((ItemBase)
                                     property.Type.InvokeMember(
                                         string.Empty, BindingFlags.CreateInstance, null, null, new object[0]));

                                var value = DataBase.GetExistReaderValue(
                                    rdr, dr, prefix + property.Db.ParamName, property.PropertyDescriptor.GetValue(this));

                                item.Id = Convert.ToInt64(value);

                                var user = item as User;

                                if (user != null)
                                {
                                    user.Login = Convert.ToString(DataBase.GetExistReaderValue(rdr, dr, prefix + "CreatedBylogin", property.PropertyDescriptor.GetValue(this)));
                                }

                                property.PropertyDescriptor.SetValue(
                                    this,
                                   item);
                            }
                        }
                        else if (property.Type != typeof(Dictionary<long, ItemBase>))
                        {
                            var value = DataBase.GetExistReaderValue(
                                rdr, dr, prefix + property.Db.ParamName, property.PropertyDescriptor.GetValue(this));

                            if (property.Translate != null && property.Translate.Translatable)
                            {
                                var pi = this.GetType().GetProperty(property.Translate.Alias);
                                if (pi != null)
                                {
                                    pi.SetValue(this, value, null);
                                }
                                if (!string.IsNullOrEmpty((string)value))
                                {
                                    property.PropertyDescriptor.SetValue(
                                        this, Translate.GetTranslatedValueFromDb(value.ToString()));
                                }
                            }
                            else if (property.Type == typeof(NumbersRange))
                            {
                                value = new NumbersRange()
                                {
                                    from =
                                        value != null ? Convert.ToInt32(value.ToString()) : 0
                                };
                                property.PropertyDescriptor.SetValue(this, value);
                            }
                            else if (property.Type == typeof(DateRange))
                            {
                                value = new DateRange()
                                {
                                    from =
                                        value != null ? Convert.ToDateTime(value.ToString()) :  DateTime.MinValue
                                       
                                };
                                property.PropertyDescriptor.SetValue(this, value);
                            }
                            else if (property.Encryption!=null && property.Encryption.Encrypted)
                            {
                                if (value is DateTime)
                                {
                                    value =
                                        Convert.ToDateTime(
                                            Crypt.Decrypt(
                                                value.ToString(), ConfigurationManager.AppSettings["CryptKey"]));
                                }
                                else if (value is int)
                                {
                                    value =
                                        Convert.ToInt32(
                                            Crypt.Decrypt(
                                                value.ToString(), ConfigurationManager.AppSettings["CryptKey"]));
                                }
                                else if (value is long)
                                {
                                    value =
                                        Convert.ToInt64(
                                            Crypt.Decrypt(
                                                value.ToString(), ConfigurationManager.AppSettings["CryptKey"]));
                                }
                                else if (value is decimal)
                                {
                                    value =
                                        Convert.ToDecimal(
                                            Crypt.Decrypt(
                                                value.ToString(), ConfigurationManager.AppSettings["CryptKey"]));
                                }
                                else if (value != null && !string.IsNullOrEmpty(value.ToString()))
                                {
                                    value = Crypt.Decrypt(
                                        value.ToString(), ConfigurationManager.AppSettings["CryptKey"]);
                                }

                                property.PropertyDescriptor.SetValue(this, value);
                            }
                            else if (property.Type == typeof(string) && value!=DBNull.Value && value!=null)
                            {
                                value = value.ToString().Replace("|", ";");
                                property.PropertyDescriptor.SetValue(this, value);
                            }
                            else
                            {
                                property.PropertyDescriptor.SetValue(this, value);
                            }
                        }
                        else if (property.Custom is MultiCheck)
                        {
                            var value = (string)DataBase.GetExistReaderValue(rdr, dr, prefix + property.Db.ParamName, "");
                            if (!string.IsNullOrEmpty(value))
                            {
                                var list = new Dictionary<long, ItemBase>();
                                foreach(var sId in value.Split(';'))
                                {
                                    var lId = Convert.ToInt64(sId);
                                    var lItem = (ItemBase)Activator.CreateInstance((property.Custom as MultiCheck).ItemType);
                                    lItem.Id = lId;
                                    if(!list.ContainsKey(lId))
                                        list.Add(lId, lItem);
                                }
                                property.PropertyDescriptor.SetValue(this, list);
                            }
                        }
                    }
                }

                this.AfterFromDataRow(rdr, dr, prefix);
            }

            return this;
        }

        /// <summary>
        /// The from data row.
        /// </summary>
        /// <param name="rdr">
        /// The data reader.
        /// </param>
        /// <param name="dr">
        /// The data row.
        /// </param>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        /// <param name="tocontinue">
        /// The to continue.
        /// </param>
        public virtual void FromDataRow(IDataReader rdr, DataRow dr, string prefix,out bool tocontinue)
        {
            tocontinue = true;
        }

        /// <summary>
        /// The after from data row.
        /// </summary>
        /// <param name="rdr">
        /// The data reader.
        /// </param>
        /// <param name="dr">
        /// The data row.
        /// </param>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        public virtual void AfterFromDataRow(IDataReader rdr, DataRow dr, string prefix)
        {
        }

        #endregion

        #endregion

        #region Security
        public virtual bool CheckAutocompleteSecurity()
        {
            return true;
        }

        public virtual bool HaveAccess()
        {
            return true;
        }
        #endregion

        public string ToJson()
        {
            var properties = this.GetType().GetProperties();
            var sb = new StringBuilder();

            sb.Append("{");
            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(JsonIgnoreAttribute))) continue;
                if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var array = property.GetValue(this, null) as IEnumerable;

                    if (array == null)
                    {
                        sb.Append($"\"{property.Name}\":null,");
                        continue;
                    }

                    sb.Append($"\"{property.Name}\":[");

                    foreach (var obj in array)
                    {
                        var nestedProps = obj.GetType().GetProperties();
                        foreach (var np in nestedProps)
                        {
                            if (Attribute.IsDefined(np, typeof(JsonIgnoreAttribute))) continue;
                            var npVal = np.GetValue(obj, null);
                            if (np.PropertyType == typeof(string) || np.PropertyType == typeof(DateTime) || np.PropertyType == typeof(Guid))
                                sb.Append($"\"{np}\": \"{npVal ?? "null"}\",");
                            else
                                sb.Append($"\"{np}\": {npVal ?? "null"},");
                        }
                    }

                    sb.TrimEnd(',');

                    sb.Append("],");
                    continue;
                }

                var propVal = property.GetValue(this, null);

                if (property.PropertyType == typeof(string) || property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Guid))
                    sb.Append($"\"{property.Name}\": \"{propVal ?? "null"}\",");
                else
                    sb.Append($"\"{property.Name}\":{propVal ?? "null"},");
            }

            sb.TrimEnd(',');
            sb.Append("}");
            return sb.ToString();
        }
    }
}