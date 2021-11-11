// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlPanelController.cs" company="Natur Bravo Pilot">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the ControlPanelController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gofra.Controllers
{
    using Gofraweblib;
    using Gofraweblib.Controllers;
    using System;
    using System.Reflection;
    using Weblib.Controllers;

    /// <summary>
    /// The ControlPanel controller.
    /// </summary>
    public class ControlPanelController : Gofraweblib.Controllers.ControlPanel
    {        
        public override Type[] AdditionalTypes()
        {
            Type[] Gofra = GetTypesInNamespace(this.GetType().Assembly, "Gofra.Models.Objects");
            return Gofra;
        }
    }
}
