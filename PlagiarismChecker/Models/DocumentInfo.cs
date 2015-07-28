using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlagiarismChecker.Models
{
    internal class DocumentInfo
    {
        public DocumentInfo(string filePath)
        {
            Info = new FileInfo(filePath);
            ExtractedDirectoryPath = Path.Combine(Path.GetDirectoryName(filePath),
                Path.GetFileNameWithoutExtension(filePath));
            var mainFileName = Path.GetFileNameWithoutExtension(filePath);
            if (mainFileName != null)
            {
                var elements = mainFileName.Split('+');

                ExperimentNo = elements[0];
                ExperimentName = elements[1];
                StudentNo = elements[3].Substring(0, 9);
                StudentName = elements[3].Substring(9, elements[3].Length - 9);

                var submittedTimeStr = elements[2];
                var year = Convert.ToInt32(submittedTimeStr.Substring(0, 4));
                var month = Convert.ToInt32(submittedTimeStr.Substring(4, 2));
                var day = Convert.ToInt32(submittedTimeStr.Substring(6, 2));
                var hour = Convert.ToInt32(submittedTimeStr.Substring(8, 2));
                var minute = Convert.ToInt32(submittedTimeStr.Substring(10, 2));
                var second = Convert.ToInt32(submittedTimeStr.Substring(12, 2));
                SubmittedTime = new DateTime(year, month, day, hour, minute, second);

                GetContentFiles();
            }
        }

        public FileInfo Info { get; }
        public string ExtractedDirectoryPath { get; }
        public string StudentNo { get; }
        public string StudentName { get; }
        public DateTime SubmittedTime { get; }
        public string ExperimentNo { get; }
        public string ExperimentName { get; }
        public List<ContentFileInfo> ContentFiles { get; } = new List<ContentFileInfo>();

        public void GetContentFiles()
        {
            var files = new List<string>();

            ScanFiles(ExtractedDirectoryPath, files);

            foreach (var file in files)
            {
                ContentFiles.Add(new ContentFileInfo(file));
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