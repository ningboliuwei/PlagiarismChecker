using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PlagiarismCheckerClassLibrary;

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
			OpenFileDialog ofdMain = new OpenFileDialog();

			if (ofdMain.ShowDialog() == DialogResult.OK)
			{
				string filePath = ofdMain.FileName;

				
			}
		}
	}
}
