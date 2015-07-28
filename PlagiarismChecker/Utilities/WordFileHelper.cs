using System.Collections.Generic;
using System.IO;
using NetOffice.WordApi;
using NetOffice.WordApi.Enums;

namespace PlagiarismChecker.Utilities
{
    internal class WordFileHelper
    {
        public static Application GetInstance()
        {
            return new Application();
        }

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

        public static void ConvertToDocx(Application instance, string file)
        {

            instance.DisplayAlerts = WdAlertLevel.wdAlertsNone;

            var doc = instance.Documents.Open(file, null, true);
            var extName = ".docx";

            var newName = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + extName);
            doc.SaveAs2(newName, WdSaveFormat.wdFormatXMLDocument, null, null, null, null, null, null, null, null,
                null, null,
                null, null, null, null, WdCompatibilityMode.wdCurrent);
            //necessary
            doc.Close();
        }
    }
}