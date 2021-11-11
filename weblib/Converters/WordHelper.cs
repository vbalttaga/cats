﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weblib.Converters
{

    public static class WordHelper
    {
        public static byte[] HtmlToWord(String html)
        {
            const string filename = "test.docx";
            // string html = Properties.Resources.DemoHtml;

            if (File.Exists(filename)) File.Delete(filename);

            using (MemoryStream generatedDocument = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = package.MainDocumentPart;
                    if (mainPart == null)
                    {
                        mainPart = package.AddMainDocumentPart();
                        new DocumentFormat.OpenXml.Wordprocessing.Document(new Body()).Save(mainPart);
                    }

                    /*HtmlConverter converter = new HtmlConverter(mainPart);
                    Body body = mainPart.Document.Body;

                    var paragraphs = converter.Parse(html);
                    for (int i = 0; i < paragraphs.Count; i++)
                    {
                        body.Append(paragraphs[i]);
                    }*/

                    mainPart.Document.Save();
                }

                return generatedDocument.ToArray();
            }
        }
    }
}
