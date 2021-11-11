using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Weblib.Controllers;

namespace Gofra.Controllers
{
    [RoutePrefix("api")]
    public class ApiDataProcessorController : ApiDataProcessor
    {
        public ApiDataProcessorController() : base("GofraLib.BusinessObjects")
        {
        }
    }
}