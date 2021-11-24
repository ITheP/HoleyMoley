using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HoleyMoley
{
    public partial class About : Form
    {
        int x;
        int y;
        const double rad = 360D * (Math.PI / 180D);

        private Bitmap RawTextBitmap { get; set; }
        Graphics RawTextGraphics { get; set; }
        private Bitmap ScrollerBitmap { get; set; }
        //  Graphics G { get; set; }
        Graphics ScrollerGraphics { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        int FontHeight { get; set; }
        int FontCenteringOffset { get; set; }
        Point FontOffset { get; set; }
        Font ScrollerFont { get; set; }

        public About()
        {
            InitializeComponent();

            Width = Scroller.Width;
            Height = Scroller.Height;

            RawTextBitmap = new Bitmap(Width, Height);
            RawTextGraphics = Graphics.FromImage(RawTextBitmap);
            ScrollerBitmap = new Bitmap(Width, Height);
            ScrollerGraphics = Graphics.FromImage(ScrollerBitmap);

            //   G = Graphics.FromImage(ScrollerBitmap);
            //   G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //  G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            ScrollerFont = new Font("Impact", 48, FontStyle.Regular, GraphicsUnit.Pixel);

            Scroller.Image = ScrollerBitmap;

            var fontSize = TextRenderer.MeasureText(RawTextGraphics, "Ay", ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue));
            FontHeight = fontSize.Height;
            FontCenteringOffset = (int)((Height - FontHeight) * 0.5);
            FontOffset = new Point(0, FontCenteringOffset);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double offsetx = (double)(System.Environment.TickCount % 2000) / 2000D;
            double offsety = (double)(System.Environment.TickCount % 3000) / 3000D;

            // offsetx *= 360;
            // offsety *= 360;

            Info.Left = x + (int)(Math.Sin(offsetx * rad) * 10D);
            Info.Top = y + (int)(Math.Sin(offsety * rad) * 10D);

            RawTextGraphics.Clear(Color.Green);

            double count = (double)(System.Environment.TickCount * 0.005);
            double textOffset = 0; // (int)(Math.Sin(count++) * 20.0d);

            TextRenderer.DrawText(RawTextGraphics, "Holey Moley Holey Moley Holey Moley Holey Moley Holey Moley", ScrollerFont, FontOffset, Color.Red);
            
            ScrollerGraphics.Clear(Color.Orange);

            IntPtr hdc = ScrollerGraphics.GetHdc();
            IntPtr src = NativeMethods.CreateCompatibleDC(hdc);
            IntPtr hOldObject = NativeMethods.SelectObject(src, RawTextBitmap.GetHbitmap());

            //count+= 1.0;
            for (int i = 0; i < Width; i++)
            {
                double offset = (int)(Math.Sin(count + (i * 0.02)) * 20.0d);
                var r = NativeMethods.BitBlt(hdc, i, (int)offset, 1, Height, src, i, 0, PatBltType.SrcCopy);
            }
            //for (int j = 0; j < Height; j++)
            //{
            //    double offset = (int)(Math.Sin((count * 1.17) + (j * 0.02)) * 5.0d);
            //    var r = NativeMethods.BitBlt(hdc, (int)offset,0, Width,1, src, 0, j, PatBltType.SrcCopy);
            //}

            if (hOldObject != IntPtr.Zero)
                NativeMethods.SelectObject(src, hOldObject);

            NativeMethods.DeleteDC(src);
            ScrollerGraphics.ReleaseHdc(hdc);

            Scroller.Refresh();
        }

        private void About_Load(object sender, EventArgs e)
        {
            x = Info.Left;
            y = Info.Top;

            timer1.Start();
        }

        private void About_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();

            ScrollerFont?.Dispose();
            ScrollerFont = null;
            ScrollerGraphics?.Dispose();
            ScrollerGraphics = null;
            NativeMethods.DeleteObject(ScrollerBitmap.GetHbitmap());
            ScrollerBitmap = null;

            RawTextGraphics?.Dispose();
            RawTextGraphics = null;
            NativeMethods.DeleteObject(RawTextBitmap.GetHbitmap());
            RawTextBitmap = null;
        }
    }
}
