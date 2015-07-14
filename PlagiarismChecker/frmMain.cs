using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PlagiarismChecker.Models;
using PlagiarismChecker.Utilities;

namespace PlagiarismChecker
{
	public partial class frmMain : Form
	{
		private List<string> _files = new List<string>();

		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var docFiles = new List<string>();
			var targetFiles = new List<string>();
			var ofdMain = new OpenFileDialog { Filter = "doc, docx文件|*.doc;*.docx", Multiselect = true };

			if (ofdMain.ShowDialog() == DialogResult.OK)
			{
				foreach (string fileName in ofdMain.FileNames)
				{
					if (Path.GetExtension(fileName) == ".doc")
					{
						docFiles.Add(fileName);
						//得到转换后的 .docx 扩展名
						targetFiles.Add(fileName + "x");
					}
					else
					{
						targetFiles.Add(fileName);
					}
				}

				DocxHelper.ConvertToDocx(docFiles.ToArray());

				//				var bgw = new BackgroundWorker();
				//				bgw.DoWork += bgw_DoWork;
				//				bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
				//				bgw.RunWorkerAsync(targetFiles);
				//
				//				
				ZipHelper.UnZip(targetFiles);


				var documents = targetFiles.Select(targetFile => new TargetDocumentInfo(targetFile)).ToList();

				foreach (TargetDocumentInfo document in documents)
				{
					document.GetDocumentContentFiles();
				}

				var results = from document in documents
							  from contentFile in document.DocumentContentFiles
							  orderby contentFile.Md5Hash
							  select new
							  {
								  Sno = document.StudentNo,
								  Sname = document.StudentName,
								  document.ExperimentNo,
								  document.ExperimentName,
								  contentFile.FilePath,
								  contentFile.Md5Hash
							  };


				dataGridView1.DataSource = results.ToList();
				dataGridView1.AutoResizeColumns();
			}
		}

		private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Done");
		}

		private void bgw_DoWork(object sender, DoWorkEventArgs e)
		{
		}
	}
}