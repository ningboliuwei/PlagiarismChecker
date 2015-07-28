using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PlagiarismChecker.Models
{
    class ContentFileInfo
    {
        public FileInfo Info { get; private set; }
        public string Md5Hash { get; private set; }

        public ContentFileInfo(string filePath)
        {
            Info = new FileInfo(filePath);
            Md5Hash = GetMd5HashFromFile(filePath);
        }

        private static string GetMd5HashFromFile(string filePath)
        {
            try
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileStream);
                fileStream.Close();
                var sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
