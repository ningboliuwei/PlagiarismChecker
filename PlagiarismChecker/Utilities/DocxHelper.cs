using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml.Wordprocessing;
using NetOffice.WordApi.Enums;
using Picture = DocumentFormat.OpenXml.Drawing.Pictures.Picture;

namespace PlagiarismChecker.Utilities
{
	public class DocxHelper
	{
		public static NetOffice.WordApi.Application GetWordInstance()
		{
			return new NetOffice.WordApi.Application();
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

		public static void GetAllPictures(NetOffice.WordApi.Application _instance, string file)
		{



		}
	}
}
