using LIB.BusinessObjects;
using LIB.Helpers;
using LIB.Tools.Security;
using LIB.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weblib.Helpers;

namespace MedProject.Controllers
{
    public class LanguageController : Controller
    {
        //
        // GET: /Language/

        public ActionResult Change(long LanguageId)
        {
            var Language = new Language(LanguageId);
            Language= (Language)Language.PopulateOne(Language);
            Session[SessionItems.Language] = Language;
            CultureHelper.Language = Language;
            return Json(new RequestResult() { Result = RequestResultType.Reload });
        }

    }
}
