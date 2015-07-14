using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PlagiarismChecker.Models
{
	class TargetDocumentInfo
	{
		public string FilePath { get; set; }
		public string ExtractedDirectoryPath { get; set; }
		public string StudentNo { get; set; }
		public string StudentName { get; set; }
		public DateTime SubmittedTime { get; set; }
		public string ExperimentNo { get; set; }
		public string ExperimentName { get; set; }
		public List<DocumentContentFile> DocumentContentFiles { get; set; }

		public TargetDocumentInfo(string filePath)
		{
			DocumentContentFiles = new List<DocumentContentFile>();
			FilePath = filePath;
			ExtractedDirectoryPath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));

			string mainFileName = Path.GetFileNameWithoutExtension(filePath);
			if (mainFileName != null)
			{
				string[] elements = mainFileName.Split('+');

				ExperimentNo = elements[0];
				ExperimentName = elements[1];
				StudentNo = elements[3].Substring(0, 9);
				StudentName = elements[3].Substring(9, elements[3].Length - 9);

				string submittedTimeStr = elements[2];
				int year = Convert.ToInt32(submittedTimeStr.Substring(0, 4));
				int month = Convert.ToInt32(submittedTimeStr.Substring(4, 2));
				int day = Convert.ToInt32(submittedTimeStr.Substring(6, 2));
				int hour = Convert.ToInt32(submittedTimeStr.Substring(8, 2));
				int minute = Convert.ToInt32(submittedTimeStr.Substring(10, 2));
				int second = Convert.ToInt32(submittedTimeStr.Substring(12, 2));
				SubmittedTime = new DateTime(year, month, day, hour, minute, second);
			}
		}

		public void GetDocumentContentFiles()
		{
			var files = new List<string>();

			ScanFiles(ExtractedDirectoryPath, files);

			foreach (var file in files)
			{
				DocumentContentFiles.Add(new DocumentContentFile(file));
			}
		}

		private void ScanFiles(string directory, List<string> files)
		{
			var subDirectories = Directory.GetDirectories(directory);
			if (subDirectories.Count() != 0)
			{
				foreach (var subDirectory in subDirectories)
				{
					ScanFiles(subDirectory, files);
				}
			}

			files.AddRange(Directory.GetFiles(directory));
		}


	}
}
