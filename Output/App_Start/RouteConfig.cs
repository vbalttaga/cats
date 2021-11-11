using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gofra
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "AccountLogin",
                url: "Account/{action}",
                defaults: new { controller = "Account" },
                namespaces: new[] { "Gofra.Controllers" }
            );

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new { controller = "Account" },
                namespaces: new[] { "Weblib.Controllers", "Gofraweblib.Controllers" }
            );

            routes.MapRoute(
                "Language",
                "Language/Change/{LanguageId}",
                new { controller = "Language", action = "Change", LanguageId = UrlParameter.Optional },
                namespaces: new[] { "MedProject.Controllers" }
            );

            routes.MapRoute(
                "Print",
                "Print/Print",
                new { controller = "Print", action = "Print" },
                namespaces: new[] { "Gofra.Controllers" }
            );

            routes.MapRoute(
                "GofraEditDelete",
                "DocControl/Delete",
                new { controller = "DocControl", action = "Delete" }
            );

            routes.MapRoute(
                "GofraEditSave",
                "DocControl/Save",
                new { controller = "DocControl", action = "Save"}
            );

            routes.MapRoute(
                "GofraEdit",
                "DocControl/{Model}/{Id}/{additional}",
                new { controller = "DocControl", action = "Edit", Model = UrlParameter.Optional, Id = UrlParameter.Optional, additional = UrlParameter.Optional }
            );

            routes.MapRoute(
                "AutoComplete",
                "AutoComplete/{Model}/{Namespace}/{Param}",
                new { controller = "AutoComplete", action = "List", Model = UrlParameter.Optional, Namespace = UrlParameter.Optional, Param = UrlParameter.Optional }
            );

            routes.MapRoute(
                "MultySelect",
                "MultySelect/{Namespace}/{Param}",
                new { controller = "MultySelect", action = "Options", Namespace = UrlParameter.Optional, Param = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ErrorHandling",
                "Error/{action}",
                new { controller = "Error", action = "Index" }
            );


            routes.MapRoute(
                "ReportSearch",
                "Report/Search",
                new { controller = "Report", action = "Search" }
            );

            routes.MapRoute(
                "ReportPrint",
                "Report/Print",
                new { controller = "Report", action = "Print" }
            );

            routes.MapRoute(
                "ReportExcell",
                "Report/ExportExcell",
                new { controller = "Report", action = "ExportExcell" }
            );

            routes.MapRoute(
                "ReportCSV",
                "Report/ExportCSV",
                new { controller = "Report", action = "ExportCSV" }
            );

            routes.MapRoute(
                "ReportOptions",
                "Report/Options/{Model}",
                new { controller = "Report", action = "Options", Model = UrlParameter.Optional }
            );

            routes.MapRoute(
                "ReportOptionsSave",
                "Report/OptionsSave/{Model}",
                new { controller = "Report", action = "OptionsSave", Model = UrlParameter.Optional }
            );


            routes.MapRoute(
                "Report",
                "Report/{Model}/{BOLink}/{NamespaceLink}/{Id}",
                new { controller = "Report", action = "View", Model = UrlParameter.Optional, BOLink = UrlParameter.Optional, NamespaceLink = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CPProfile",
                url: "ControlPanel/Profile/{Id}",
                defaults: new { controller = "ControlPanel", action = "Profile", Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CPTable",
                url: "ControlPanel/Edit/{BO}/{Namespace}/{BOLink}/{NamespaceLink}/{Id}",
                defaults: new { controller = "ControlPanel", action = "Edit", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, BOLink = UrlParameter.Optional, NamespaceLink = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Create",
                url: "ControlPanel/CreateItem/{BO}/{Namespace}/{BOLink}/{NamespaceLink}/{Id}",
                defaults: new { controller = "ControlPanel", action = "CreateItem", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, BOLink = UrlParameter.Optional, NamespaceLink = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Edit",
                url: "ControlPanel/{action}/{BO}/{Namespace}/{Id}",
                defaults: new { controller = "ControlPanel", action = "Edit", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SMIProfile",
                url: "SystemManagement/Profile/{Id}",
                defaults: new { controller = "SystemManagement", action = "Profile", Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SMITable",
                url: "SystemManagement/Edit/{BO}/{Namespace}/{BOLink}/{NamespaceLink}/{Id}",
                defaults: new { controller = "SystemManagement", action = "Edit", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, BOLink = UrlParameter.Optional, NamespaceLink = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SMICreate",
                url: "SystemManagement/CreateItem/{BO}/{Namespace}/{BOLink}/{NamespaceLink}/{Id}",
                defaults: new { controller = "SystemManagement", action = "CreateItem", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, BOLink = UrlParameter.Optional, NamespaceLink = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SMIEdit",
                url: "SystemManagement/{action}/{BO}/{Namespace}/{Id}",
                defaults: new { controller = "SystemManagement", action = "Edit", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, Id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LoadDataGrid",
                url: "DataProcessor/Load/{BO}/{Namespace}/{BOLink}/{NamespaceLink}/{Id}",
                defaults: new { controller = "DataProcessor", action = "Load", BO = UrlParameter.Optional, Namespace = UrlParameter.Optional, BOLink = UrlParameter.Optional, NamespaceLink = UrlParameter.Optional, Id = UrlParameter.Optional }
            );          

            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}",
                defaults: new { controller = "DashBoard", action = "Index" }
            );

            routes.MapRoute(
                name: "Error",
                url: "Error",
                defaults: new { controller = "Error", action = "Index" },
                namespaces: new[] { "Gofra.Controllers" }
            );

            routes.MapRoute(
                name: "ErrorHandle",
                url: "Error/{action}",
                defaults: new { controller = "Error" },
                namespaces: new[] { "Gofra.Controllers" }
            );
        }
    }
}