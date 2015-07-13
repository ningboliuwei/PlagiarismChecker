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
			OpenFileDialog ofdMain = new OpenFileDialog();
			ofdMain.Filter = "doc文件|*.doc";
			ofdMain.Multiselect = true;

			if (ofdMain.ShowDialog() == DialogResult.OK)
			{
				foreach (var fileName in ofdMain.FileNames)
				{
					if (Path.GetExtension(fileName) == ".doc")
					{
						docFiles.Add(fileName);
					}
				}

				DocxHelper.ConvertToDocx(docFiles.ToArray());
			}
		}
	}
}