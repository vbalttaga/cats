﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadController.cs" company="Natur Bravo Pilot">
//   Copyright 2013
// </copyright>
// <summary>
//   Defines the UploadController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Galex.Controllers
{
    using Gofraweblib;
    using Gofraweblib.Controllers;
    using LIB.BusinessObjects;
    using LIB.Tools.Security;
    using LIB.Tools.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Weblib.Controllers;
    using Weblib.Helpers;
    using LIB.Helpers;
    using Gofra.Models.Objects;
    using GofraLib.BusinessObjects;
    using System.Web;

    /// <summary>
    /// The Upload controller.
    /// </summary>
    public class UploadController : BaseController
    {
        [HttpPost]
        public ActionResult DoUploadImage(System.Web.HttpPostedFileBase file)
        {
            if (!Authentication.CheckUser(this.HttpContext)) //TBD
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
            }
            if (!LIB.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)BasePermissionenum.CPAccess | (long)BasePermissionenum.SMIAccess))
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
            }

            General.TraceWarn("DoUploadImage Start");
            if (file != null
                && !string.IsNullOrEmpty(Request.QueryString["AdminWidth"])
                && !string.IsNullOrEmpty(Request.QueryString["AdminHeight"])
                && !string.IsNullOrEmpty(Request.QueryString["Width"])
                && !string.IsNullOrEmpty(Request.QueryString["Height"]))
            {
                try
                {
                    var BOName = Request.QueryString["BOName"];
                    var AdminWidth = Request.QueryString["AdminWidth"]!="0"?Convert.ToInt32(Request.QueryString["AdminWidth"]):50;
                    var AdminHeight = Request.QueryString["AdminHeight"] != "0" ? Convert.ToInt32(Request.QueryString["AdminHeight"]) : 50;
                    var Width = Request.QueryString["Width"] != "0" ? Convert.ToInt32(Request.QueryString["Width"]) : 50;
                    var Height = Request.QueryString["Height"] != "0" ? Convert.ToInt32(Request.QueryString["Height"]) : 50;

                    string pic = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                    string ext = System.IO.Path.GetExtension(file.FileName);

                    General.TraceWarn(ext);

                    if (ext.ToLower() != ".png" && ext.ToLower() != ".jpg" && ext.ToLower() != ".jpeg" && ext.ToLower() != ".gif" && ext.ToLower() != ".bmp")
                    {
                        return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Wrong Extension" });
                    }
                    if (!IsImage(file))
                    {
                        return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Uploaded File is not an Image" });
                    }
                    if (!Directory.Exists(Server.MapPath(Config.GetConfigValue("UploadPart"))))
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.GetConfigValue("UploadPart")));
                    }
                    if (!Directory.Exists(Server.MapPath(Config.GetConfigValue("UploadPart") + BOName)))
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.GetConfigValue("UploadPart") + BOName));
                    }
                    var i = 0;
                    while (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath(Config.GetConfigValue("UploadPart") + BOName), pic + "_original" + ext)))
                    {
                        i++;
                        pic = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + i.ToString();
                    }
                    string path = System.IO.Path.Combine(Server.MapPath(Config.GetConfigValue("UploadPart") + BOName), pic + "_original" + ext);
                    // file is uploaded
                    file.SaveAs(path);

                    LIB.Tools.Utils.ImageResizer.ResizeImageAndRatio(path, System.IO.Path.Combine(Server.MapPath("~/uploads/" + BOName), pic + "_adminthumb.jpeg"), AdminWidth, AdminHeight);
                    LIB.Tools.Utils.ImageResizer.ResizeImageAndRatio(path, System.IO.Path.Combine(Server.MapPath("~/uploads/" + BOName), pic + ".jpeg"), Width, Height);

                    Graphic uploadedImage = new Graphic();
                    uploadedImage.BOName = BOName;
                    uploadedImage.Ext = ext.Replace(".","");
                    uploadedImage.Name = pic;

                    uploadedImage.Insert(uploadedImage);
                    var Data = new Dictionary<string,object>();
                    Data.Add("Id",uploadedImage.Id);
                    Data.Add("thumb", uploadedImage.AdminThumbnail);

                    return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                }
                catch(Exception ex){

                    General.TraceWarn(ex.ToString());
                    return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed:" + ex.ToString() });
                }
            }

            return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed" });
        }

        [HttpPost]
        public ActionResult DoUploadFile(System.Web.HttpPostedFileBase file)
        {
            if (!Authentication.CheckUser(this.HttpContext)) //TBD
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
            }

            if (file != null)
            {
                try
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                    string ext = System.IO.Path.GetExtension(file.FileName);

                    General.TraceWarn(ext);

                    if (ext.ToLower() != ".pdf" && ext.ToLower() != ".doc" && ext.ToLower() != ".docx" && ext.ToLower() != ".xls" && ext.ToLower() != ".xlsx " && ext.ToLower() != ".png" && ext.ToLower() != ".jpg" && ext.ToLower() != ".jpeg" && ext.ToLower() != ".gif" && ext.ToLower() != ".bmp")
                    {
                        return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Wrong Extension" });
                    }
                    if (!Directory.Exists(Server.MapPath(Config.GetConfigValue("UploadPart"))))
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.GetConfigValue("UploadPart")));
                    }
                    if (!Directory.Exists(Server.MapPath(Config.GetConfigValue("UploadPart") + "Documents")))
                    {
                        Directory.CreateDirectory(Server.MapPath(Config.GetConfigValue("UploadPart") +"Documents"));
                    }
                    var i = 0;
                    while (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath(Config.GetConfigValue("UploadPart") + "Documents"), name + ext)))
                    {
                        i++;
                        name = System.IO.Path.GetFileNameWithoutExtension(file.FileName) + "_" + i.ToString();
                    }
                    string path = System.IO.Path.Combine(Server.MapPath(Config.GetConfigValue("UploadPart") + "Documents"), name  + ext);
                    // file is uploaded
                    file.SaveAs(path);

                    Document uploadedDoc = new Document();
                    uploadedDoc.Ext = ext.Replace(".", "");
                    uploadedDoc.Name = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                    uploadedDoc.FileName = name;

                    uploadedDoc.Insert(uploadedDoc);
                    var Data = new Dictionary<string, object>();
                    Data.Add("Id", uploadedDoc.Id);
                    Data.Add("file", uploadedDoc.File);
                    Data.Add("name", uploadedDoc.Name);

                    return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                }
                catch (Exception ex)
                {

                    return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed:" + ex.ToString() });
                }
            }

            return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "File Uploading Failed" });
        }
        
        public const int ImageMinimumBytes = 512;

        private static bool IsImage(System.Web.HttpPostedFileBase postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }

                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
