using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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


				List<TargetDocumentInfo> documents = targetFiles.Select(targetFile => new TargetDocumentInfo(targetFile)).ToList();

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

				var filteredGroups = from line in results
					group line by line.Md5Hash
					into grp
					where grp.Count() > 1
					select new
					{
						Md5Hash = grp.Key,
						Count = grp.Count()
					};
					

				var filteredResults = from line in results
									  join g in filteredGroups
										  on line.Md5Hash equals g.Md5Hash
									  select new
									  {
										  line.Sno,
										  line.Sname,
										  line.ExperimentNo,
										  line.ExperimentName,
										  line.FilePath,
										  line.Md5Hash
									  };


			

				dataGridView1.DataSource = filteredResults.ToList();
				dataGridView1.AutoResizeColumns();
				ChangeColor(5, dataGridView1);
			}
		}

		private void ChangeColor(int keyIndex, DataGridView grid)
		{
			var colorList = new List<Color>
			{
				Color.LightPink,
//			Color.LightCyan,
//			Color.LightGreen,
//			Color.LightYellow,
//			Color.LightSlateGray,
//			Color.LightSeaGreen,
//			Color.LightGray,
//			Color.LightCoral,
//			Color.LightBlue,
//			Color.LightGoldenrodYellow,
//			Color.LightSteelBlue,
//			Color.LightSalmon,
				Color.LightSkyBlue
			};


			int colorIndex = 0;
			int sameCount = 0;

			grid.Rows[0].DefaultCellStyle.BackColor = colorList[colorIndex];
			for (int i = 1; i < grid.Rows.Count; i++)
			{


				if (grid.Rows[i].Cells[keyIndex].Value.ToString() != grid.Rows[i - 1].Cells[keyIndex].Value.ToString())
				{
					colorIndex++;

					if (colorIndex >= colorList.Count)
					{
						colorIndex = 0;
					}
				}

				grid.Rows[i].DefaultCellStyle.BackColor = colorList[colorIndex];
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