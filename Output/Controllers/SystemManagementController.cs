// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemManagementController.cs" company="Natur Bravo Pilot">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the SystemManagementController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gofra.Controllers
{
    using Gofraweblib;
    using Gofraweblib.Controllers;
    using System;
    using Weblib.Controllers;

    /// <summary>
    /// The SystemManagement controller.
    /// </summary>
    public class SystemManagementController : Gofraweblib.Controllers.SystemManagement
    {

        public override Type[] AdditionalTypes()
        {
            Type[] Gofra = GetTypesInNamespace(this.GetType().Assembly, "Gofra.Models.Objects");
            return Gofra;
        }
    }
}
