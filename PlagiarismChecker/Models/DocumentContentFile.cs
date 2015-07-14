using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PlagiarismChecker.Models
{
	internal class DocumentContentFile
	{
		public string FilePath { get; set; }
		public string Md5Hash { get; set; }

		public DocumentContentFile(string filePath)
		{
			FilePath = filePath;
			Md5Hash = GetMd5HashFromFile(filePath);
		}

		public string GetMd5HashFromFile(string fileName)
		{
			try
			{
				var file = new FileStream(fileName, FileMode.Open);
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				file.Close();
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

