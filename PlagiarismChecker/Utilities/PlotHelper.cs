using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlagiarismChecker.Utilities
{
	using System.Drawing;
	using System.Drawing.Imaging;

	class PlotHelper
	{
		private static void DrawPicture(List<string> names)
		{
			Pen p = new Pen(new SolidBrush(Color.Red));
			int width = 1000;
			int height = 1000;
			var buffer = new Bitmap(width, height);

			Graphics g = Graphics.FromImage(buffer);
			g.FillRectangle(new SolidBrush(Color.White), 0, 0, width, height);

			//Font characterFont = new Font(Font.FontFamily, 10, FontStyle.Bold);

			Random random = new Random(DateTime.Now.Millisecond);
			foreach (var name in names)
			{
				int r = 50;
				int x = random.Next(r, width - r);
				int y = random.Next(r, height - r);
				g.DrawEllipse(p, new Rectangle(x, y, r, r));
				//g.DrawString(name, characterFont, new SolidBrush(Color.Black), x, y);
			}
			buffer.Save("r:\\1.png", ImageFormat.Png);
		}
	}
}
