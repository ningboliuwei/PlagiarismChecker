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
        private List<string> files = new List<string>();

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void GenerateAdjancencyList(List<dynamic> para)
        {
        }

        private void ChangeColor(int keyIndex, DataGridView dataGridView)
        {
            var colorList = new List<Color>
            {
                Color.LightPink,

                #region Unused Colors

                //Color.LightCyan,
                //Color.LightGreen,
                //Color.LightYellow,
                //Color.LightSlateGray,
                //Color.LightSeaGreen,
                //Color.LightGray,
                //Color.LightCoral,
                //Color.LightBlue,
                //Color.LightGoldenrodYellow,
                //Color.LightSteelBlue,
                //Color.LightSalmon,
                #endregion
                Color.LightSkyBlue
            };

            var colorIndex = 0;

            if (dataGridView.Rows.Count != 0)
            {
                dataGridView.Rows[0].DefaultCellStyle.BackColor = colorList[colorIndex];
            }

            for (var i = 1; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Cells[keyIndex].Value.ToString() !=
                    dataGridView.Rows[i - 1].Cells[keyIndex].Value.ToString())
                {
                    colorIndex++;

                    if (colorIndex >= colorList.Count)
                    {
                        //reset the color from the start
                        colorIndex = 0;
                    }
                }

                dataGridView.Rows[i].DefaultCellStyle.BackColor = colorList[colorIndex];
            }
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Done");
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
        }

        private void ShowFilesInTreeView(List<DocumentInfo> documents)
        {
            var root = new TreeNode();

            foreach (var document in documents)
            {
                var node = new TreeNode(Path.GetFileNameWithoutExtension(document.Info.FullName));
                root.Nodes.Add(node);
                foreach (var file in document.ContentFiles)
                {
                    node.Nodes.Add(new TreeNode(Path.GetFileNameWithoutExtension(file.Info.FullName)));
                }
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sourceFiles = new List<string>();
            var targetFiles = new List<string>();
            var ofdMain = new OpenFileDialog
            {
                Filter =
                    "doc, docx文件|*.doc;*.docx|xls, xlsx文件|*.xls;*.xlsx|ppt, pptx文件|*.ppt;*.pptx|rar文件|*.rar|所有文件|*.*",
                Multiselect = true
            };

            if (ofdMain.ShowDialog() == DialogResult.OK)
            {
                foreach (var fileName in ofdMain.FileNames)
                {
                    var extName = Path.GetExtension(fileName);

                    if (extName == ".doc"
                        || extName == ".xls"
                        || extName == ".ppt")
                    {
                        sourceFiles.Add(fileName);
                        //得到 .docx/.xlsx/.pptx 扩展名
                        targetFiles.Add(fileName + "x");
                    }
                    else
                    {
                        targetFiles.Add(fileName);
                    }
                }

                //Convert .doc files to .docx files
                using (var word = WordFileHelper.GetInstance())
                {
                    for (var i = 0; i < sourceFiles.Count; i++)
                    {
                        toolStripProgressBar.Value = i * 100 / targetFiles.Count;
                        WordFileHelper.ConvertToDocx(word, sourceFiles[i]);
                    }
                    //necessary
                    word.Quit();
                }

                //				var bgw = new BackgroundWorker();
                //				bgw.DoWork += bgw_DoWork;
                //				bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
                //				bgw.RunWorkerAsync(targetFiles);
                //

                //unzip .docx files to directries	
                for (var i = 0; i < targetFiles.Count; i++)
                {
                    toolStripProgressBar.Value = i * 100 / targetFiles.Count;
                    ZipHelper.UnZip(targetFiles[i]);
                }


                var documents = targetFiles.Select(targetFile => new DocumentInfo(targetFile)).ToList();

                var results = from document in documents
                              from contentFile in document.ContentFiles
                              orderby contentFile.Md5Hash
                              select new
                              {
                                  DocumentName = document.Info.Name,
                                  Sno = document.StudentNo,
                                  Sname = document.StudentName,
                                  document.ExperimentNo,
                                  document.ExperimentName,
                                  FilePath = contentFile.Info.FullName,
                                  contentFile.Md5Hash
                              };

                var x = 1;
                var filteredGroups =
                    results.GroupBy(line => line.Md5Hash)
                        .Where(grouping => grouping.Count() > x)
                        .Select(grouping => new
                        {
                            Md5Hash = grouping.Key
                        });

                var filteredResults = from line in results
                                      join g in filteredGroups
                                          on line.Md5Hash equals g.Md5Hash
                                      select new
                                      {
                                          line.DocumentName,
                                          line.Sno,
                                          line.Sname,
                                          line.ExperimentNo,
                                          line.ExperimentName,
                                          line.FilePath,
                                          line.Md5Hash
                                      };


                var result2 = (from line in filteredResults
                               orderby line.DocumentName, line.FilePath
                               select line).Distinct();

                dgvFiles.DataSource = filteredResults.ToList();
                dgvFiles.AutoResizeColumns();


                foreach (var a in result2.ToList())
                {
                    tvwFiles.Nodes.Add(new TreeNode(a.FilePath));
                }


                if (dgvFiles.Rows.Count != 0)
                {
                    //5 means judge by hashcode
                    ChangeColor(5, dgvFiles);
                }

                //                    var names = (from line in filteredResults
                //                        select line.Sname).Distinct();
                //
                //                    #region
                //
                //                    var al = new AdjacencyList<string>();
                //
                //                    foreach (var a in names)
                //                    {
                //                        al.AddVertex(a);
                //                    }
                //
                //                    MessageBox.Show(al.ToString());
                //
                //                    #endregion
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(this, "确定退出程序吗?", "问题", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
        }

        private void statusStrip1_SizeChanged(object sender, EventArgs e)
        {
        }

        private void statusStrip1_Resize(object sender, EventArgs e)
        {
            toolStripProgressBar.Size = new Size(statusStrip.Width - 20, toolStripProgressBar.Height);
        }
    }
}