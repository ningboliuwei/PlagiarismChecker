using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetOffice.WordApi.Enums;

namespace PlagiarismChecker.Utilities
{
    class FileHelper
    {
        public static void ProcessFiles(IEnumerable<string> files)
        {
            var docFiles = new List<string>();
            var xlsFiles = new List<string>();
            var pptFiles = new List<string>();

            foreach (var file in files)
            {
                var extName = Path.GetExtension(file);

                switch (extName)
                {
                    case ".doc":
                        docFiles.Add(file);
                        break;
                    case ".xls":
                        xlsFiles.Add(file);
                        break;
                    case ".ppt":
                        pptFiles.Add(file);
                        break;
                }
            }

        }

        public static void ConvertToDocx(IEnumerable<string> docFiles)
        {
            using (var word = new NetOffice.WordApi.Application())
            {
                word.DisplayAlerts = WdAlertLevel.wdAlertsNone;

                foreach (var file in docFiles)
                {
                    var doc = word.Documents.Open(file, null, true);
                    var extName = ".docx";

                    string newName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + extName);
                    doc.SaveAs2(newName, WdSaveFormat.wdFormatXMLDocument, null, null, null, null, null, null, null, null, null, null,
                        null, null, null, null, WdCompatibilityMode.wdCurrent);

                    doc.Close();
                }
                word.Quit();
            }
        }
    }
}
