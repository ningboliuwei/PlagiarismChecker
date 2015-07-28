using System.IO;
using Ionic.Zip;

namespace PlagiarismChecker.Utilities
{
    internal class ZipHelper
    {
        public static void UnZip(string file)
        {
            using (var currentFile = ZipFile.Read(file))
            {
                currentFile.ExtractAll(
                    Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)),
                    ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}