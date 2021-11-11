using LIB.Tools.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GofraLib.Tools.AdminArea
{
    public class AdminAreaGroup : LIB.Tools.AdminArea.AdminAreaGroup
    {
        public AdminAreaGroupenum Group { get; set; }
        public AdminAreaGroup Parent { get; set; }

        public static AdminAreaGroup UserManagement = new AdminAreaGroup() { Group = AdminAreaGroupenum.UserManagement, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("User_Management", "BO"), Icon = "users"  };
        public static AdminAreaGroup Translate = new AdminAreaGroup() { Group = AdminAreaGroupenum.Translate, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("Translation", "BO"), Icon = "language"  };
        public static AdminAreaGroup Settigs = new AdminAreaGroup() { Group = AdminAreaGroupenum.Settings, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("Settigs", "BO"), Icon = "cogs" };
        public static AdminAreaGroup Navigation = new AdminAreaGroup() { Group = AdminAreaGroupenum.Navigation, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("Navigation", "BO"), Icon = "navicon" };
        public static AdminAreaGroup Rapoarte = new AdminAreaGroup() { Group = AdminAreaGroupenum.Rapoarte, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("Reports", "BO") };
        public static AdminAreaGroup Documents = new AdminAreaGroup() { Group = AdminAreaGroupenum.Documents, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("Documents", "BO") };
        public static AdminAreaGroup System = new AdminAreaGroup() { Group = AdminAreaGroupenum.System, Name = LIB.Tools.Utils.Translate.GetTranslatedValue("System", "BO") };


        public static Dictionary<AdminAreaGroupenum, AdminAreaGroup> Groups
        {
            get
            {
                Dictionary<AdminAreaGroupenum, AdminAreaGroup> list = new Dictionary<AdminAreaGroupenum, AdminAreaGroup>();

                list.Add(UserManagement.Group, UserManagement);
                list.Add(Translate.Group, Translate);
                list.Add(Settigs.Group, Settigs);
                list.Add(Navigation.Group, Navigation);
                list.Add(Rapoarte.Group, Rapoarte);
                list.Add(Documents.Group, Documents);
                list.Add(System.Group, System);

                return list;
            }
        }
    }
}
