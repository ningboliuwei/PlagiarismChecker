using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using PlagiarismChecker.Utilities;
using NetOffice;


namespace PlagiarismChecker
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			List<string> docFiles = new List<string>();
			List<string> targetFiles = new List<string>();
			OpenFileDialog ofdMain = new OpenFileDialog();
			ofdMain.Filter = "doc, docx文件|*.doc;*.docx";
			ofdMain.Multiselect = true;

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

				BackgroundWorker bgw = new BackgroundWorker();
				bgw.DoWork += bgw_DoWork;
				bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
				bgw.RunWorkerAsync(targetFiles);
			}
		}

		private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Done");
		}

		private void bgw_DoWork(object sender, DoWorkEventArgs e)
		{
			ZipHelper.UnZip((List<string>) e.Argument);
		}
	}
}