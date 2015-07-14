using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Drawing;
using Ionic.Zip;
using Path = System.IO.Path;

namespace PlagiarismChecker.Utilities
{
	internal class ZipHelper
	{
		public static void UnZip(List<string> files)
		{
			foreach (var file in files)
			{
				using (ZipFile currentFile = ZipFile.Read(file))
				{
					currentFile.ExtractAll(
						Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)), ExtractExistingFileAction.OverwriteSilently);
				}
			}
		}
	}
}