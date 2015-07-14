using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using PlagiarismChecker.Models;
using PlagiarismChecker.Utilities;
using NetOffice;


namespace PlagiarismChecker
{
	public partial class frmMain : Form
	{
		private List<string> _files=new List<string>();
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
			var ofdMain = new OpenFileDialog {Filter = "doc, docx文件|*.doc;*.docx", Multiselect = true};

			if (ofdMain.ShowDialog() == DialogResult.OK)
			{
				foreach (var fileName in ofdMain.FileNames)
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

				var infos = targetFiles.Select(targetFile => new TargetDocumentInfo(targetFile)).ToList();

				foreach (var info in infos)
				{
					info.GetDocumentContentFiles();
				}

				
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