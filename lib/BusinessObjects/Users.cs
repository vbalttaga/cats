// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserBase.cs" company="GalexStudio">
//   Copyright ©  2013
// </copyright>
// <summary>
//   Defines the UserBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LIB.BusinessObjects
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    using LIB.AdvancedProperties;
    using LIB.Tools.BO;
    using LIB.Tools.Utils;
    using LIB.Tools.AdminArea;
    using System.Collections.Generic;

    /// <summary>
    /// The user base.
    /// </summary>
    [Serializable]
    public class User : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public User(long id)
            : base(id)
        {
            this.Timeout = 1000;
            this.UniqueId = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        public User(string login, string password)
            : base(0)
        {
            this.Login = login;
            this.Password = password;
        }
        #endregion

        #region Properties

        public Graphic _Image;
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        [Common(Order = 0), Template(Mode = Template.Image), Image(ThumbnailWidth = 160, ThumbnailHeight = 160),
         Access(DisplayMode = DisplayMode.Advanced)]
        public Graphic Image {
            get
            {
                if ((_Image == null || _Image.Id==0) && Role != null)
                {
                    return Role.Avatar;
                }

                return _Image;
            }
            set { _Image = value; } 
        }

        [Common(Order = 0, EditTemplate = EditTemplates.SimpleInput, _Sortable=true,_Searchable =true),
         Validation(ValidationType = ValidationTypes.Function, ValidationFunction = "ValidateUserName"),
         Access(DisplayMode = DisplayMode.Search | DisplayMode.Simple | DisplayMode.Advanced)]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Common(Order = 1, EditTemplate = EditTemplates.Password),
        Validation(ValidationType = ValidationTypes.Function, ValidationFunction = "ValidatePassword"),
        Access(DisplayMode = DisplayMode.Simple | DisplayMode.Advanced)]
        public string Password { get; set; }
        
        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        [Common(Order = 2), Template(Mode = Template.Number),
        Validation(ValidationType = ValidationTypes.RegularExpressionRequired),
        Access(DisplayMode = DisplayMode.Advanced)]
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        [Common(Order = 3)]
        [Template(Mode = Template.PermissionsSelector)]
        [Access(EditableFor = (long)BasePermissionenum.SuperAdmin, VisibleFor = (long)BasePermissionenum.SuperAdmin, DisplayMode = DisplayMode.Advanced)]
        public long Permission { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        [Common(Order = 4, ControlClass = CssClass.Large), Template(Mode = Template.SearchDropDown), LookUp(DefaultValue=true),
        Validation(ValidationType = ValidationTypes.Required)]
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        [Common(Order = 5, ControlClass = CssClass.Large), Template(Mode = Template.SearchSelectList), LookUp(DefaultValue = true),
        Validation(ValidationType = ValidationTypes.Required),
        Access(DisplayMode = DisplayMode.Advanced)]
        public Person Person { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Common(Order = 6, _Sortable = true), Template(Mode = Template.CheckBox)]
        public bool Enabled { get; set; }

        [Common(Order = 7, _Sortable = true, DisplayName = "Demonstrează eroarea", DisplayGroup = "Setări"), Template(Mode = Template.CheckBox)]
        public bool DisplayError { get; set; }

        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore, System.Web.Script.Serialization.ScriptIgnore]
        public Guid UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the last login.
        /// </summary>
        [Db(_Editable = false),
        Common(Order = 6, EditTemplate = EditTemplates.DateTimeInput, _Sortable = true),
        Access(DisplayMode = DisplayMode.Advanced | DisplayMode.Simple)]
        public DateTime LastLogin { get; set; }

        [Db(_Editable = false, _Populate = false, _ReadableOnlyName = true)]
        public User UpdatedBy { get; set; }
        #endregion

        /// <summary>
        /// The has permissions.
        /// </summary>
        /// <param name="binaryFlags">
        /// The p binary flags.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasPermissions(long binaryFlags)
        {
            return LIB.Tools.Security.Permissions.HasPermissions(this.Permission, binaryFlags) || (this.Role != null && LIB.Tools.Security.Permissions.HasPermissions(this.Role.Permission, binaryFlags));
        }

        /// <summary>
        /// The has at least one permission.
        /// </summary>
        /// <param name="binaryFlags">
        /// The Binary Flags.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool HasAtLeastOnePermission(long binaryFlags)
        {
            if (binaryFlags == 0)
                return true;

            return LIB.Tools.Security.Permissions.HasAtLeastOnePermission(this.Permission, binaryFlags) || (this.Role != null && LIB.Tools.Security.Permissions.HasAtLeastOnePermission(this.Role.Permission, binaryFlags));
        }

        #region POPULATE Methods
        
        /// <summary>
        /// The check login for uniqueness.
        /// </summary>
        /// <param name="conn">
        /// The connection.
        /// </param>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool CheckLoginForUniqueness(SqlConnection conn, string login, int id)
        {
            const string StrSql = "User_CheckLoginForUniqueness";

            var cmd = new SqlCommand(StrSql, conn) { CommandType = CommandType.StoredProcedure };

            var param = new SqlParameter("@Login", SqlDbType.NVarChar, 100);
            param.Value = Crypt.Encrypt(login, ConfigurationManager.AppSettings["CryptKey"]);
            cmd.Parameters.Add(param);

            param = new SqlParameter("@UserId", SqlDbType.Int);
            param.Value = id;
            cmd.Parameters.Add(param);

            return (bool)cmd.ExecuteScalar();
        }

        public Dictionary<long, User> LoadLatests(long lastestId = 0)
        {
            var conn = DataBase.ConnectionFromContext();

            Dictionary<long, User> Users = new Dictionary<long, User>();

            var cmd = new SqlCommand("User_Populate_Latests", conn) { CommandType = CommandType.StoredProcedure };

            if (lastestId > 0)
                cmd.Parameters.Add(new SqlParameter("@LastestId", SqlDbType.BigInt) { Value = lastestId });

            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var usr = (User)(new User()).FromDataRow(rdr);
                    Users.Add(usr.Id, usr);
                }

                rdr.Close();
            }

            return Users;
        }
        #endregion

        #region Utils Methods
        /*
        /// <summary>
        /// The create update params.
        /// </summary>
        /// <param name="pcommand">
        /// The pcommand.
        /// </param>
        /// <param name="Item">
        /// The item.
        /// </param>
        /// <param name="bcontinue">
        /// The bcontinue.
        /// </param>
        public override void CreateUpdateParams(SqlCommand pcommand, ItemBase Item, out bool bcontinue)
        {

            var param = new SqlParameter("@Password", SqlDbType.NVarChar, 50)
                            {
                                Value =
                                    ((User)Item).GetpasswordHash()
                            };
            pcommand.Parameters.Add(param);

            bcontinue = true;
        }

        /// <summary>
        /// The create insert params.
        /// </summary>
        /// <param name="pcommand">
        /// The pcommand.
        /// </param>
        /// <param name="Item">
        /// The item.
        /// </param>
        /// <param name="bcontinue">
        /// The bcontinue.
        /// </param>
        public override void CreateInsertParams(SqlCommand pcommand, ItemBase Item, out bool bcontinue)
        {

            var param = new SqlParameter("@Password", SqlDbType.NVarChar, 50)
                            {
                                Value =
                                    ((User)Item).GetpasswordHash()
                            };
            pcommand.Parameters.Add(param);

            bcontinue = true;
        }*/
        public virtual bool IsPasswordValid(string password)
        {
            return Password == System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");
        }
        /// <summary>
        /// The getpassword hash.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetpasswordHash()
        {
            if (!string.IsNullOrEmpty(this.Password))
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.Password, "md5");
            }

            return string.Empty;
        }

        public override string GetCaption()
        {
            return "Login";
        }

        public override string GetName()
        {
            return Login;
        }
        #endregion
    }
}