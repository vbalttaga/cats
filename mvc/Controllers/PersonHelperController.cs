using GofraLib.BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weblib.Helpers;
using Weblib.Models.Common;
using System.Security.Cryptography;
using System.Text;
/*using Gofra.CNAMService;*/
using System.Globalization;
using LIB.Tools.Utils;
using LIB.Tools.Security;
using Gofra.Models.Objects;
using System.Diagnostics;
using LIB.Helpers;
using LIB.Tools.BO;

namespace Gofra.Controllers
{
    public class PersonHelperController : Gofraweblib.Controllers.FrontEndController
    {
        //
        // GET: /Helper/        
        
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
