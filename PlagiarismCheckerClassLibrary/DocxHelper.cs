using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace PlagiarismCheckerClassLibrary
{
	using DocumentFormat.OpenXml.Packaging;

	public class DocumentHelper
	{
		public void GetDocument(string filePath)
		{
			using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
			{
				
			}
		}
		public void GetPicture()
		{
			
		}
	}
}
