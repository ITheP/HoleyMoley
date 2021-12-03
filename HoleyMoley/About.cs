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
        //const double rad = 360D * (Math.PI / 180D);
        private Bitmap RawTextBitmap { get; set; }
        Graphics RawTextGraphics { get; set; }
        private Bitmap MiddleBitmap { get; set; }
        Graphics MiddleGraphics { get; set; }
        private Bitmap ScrollerBitmap { get; set; }
        //  Graphics G { get; set; }
        Graphics ScrollerGraphics { get; set; }
        private Bitmap LogoBitmap { get; set; } = (Bitmap)HoleyMoley.Properties.Resources.ResourceManager.GetObject("Logo");
        int ScrollerWidth { get; set; }
        int PaddedWidth { get; set; }
        int ScrollerHeight { get; set; }
        int ApproxFontHeight { get; set; }
        int FontCenteringOffset { get; set; }
        Font ScrollerFont { get; set; }

        private int HorizontalWobbleScale { get; set; } = 10;
        private int VerticalWobbleScale { get; set; } = 24;
        private string Message { get; set; } = "►Holey Moley◄ ☺ ► ©2010-2021 I-The-P ◄ ☺ ►Shout-outs go to my kitty cats, IRC and bored lunchtimes◄ ☺ ";
        private int MessagePos { get; set; } = 0;
        private double RenderOffset { get; set; } = 0;
        private double ScrollSpeed { get; set; } = 5.0d;

        private Color[] ScrollerColours = { Color.Pink, Color.Moccasin, Color.LightGoldenrodYellow, Color.PaleGreen, Color.LightCyan, Color.PowderBlue, Color.Thistle };


        public About()
        {
            InitializeComponent();

            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            ScrollerWidth = Scroller.Width;
            ScrollerHeight = Scroller.Height;
            PaddedWidth = ScrollerWidth + HorizontalWobbleScale + HorizontalWobbleScale;

            RawTextBitmap = new Bitmap(PaddedWidth, ScrollerHeight);
            RawTextGraphics = Graphics.FromImage(RawTextBitmap);
            MiddleBitmap = new Bitmap(PaddedWidth, ScrollerHeight);   // Extra space at edges which may become blank - we can clip these
            MiddleGraphics = Graphics.FromImage(MiddleBitmap);
            ScrollerBitmap = new Bitmap(ScrollerWidth, ScrollerHeight);
            ScrollerGraphics = Graphics.FromImage(ScrollerBitmap);

            MiddleGraphics.Clear(Color.Black);
            ScrollerGraphics.Clear(Color.Black);

            //   G = Graphics.FromImage(ScrollerBitmap);
            //   G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //  G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            ScrollerFont = new Font("Impact", 48, FontStyle.Regular, GraphicsUnit.Pixel);

            Scroller.Image = ScrollerBitmap;
            this.DoubleBuffered = true;
            // Approximate font height based on a couple of letters (ascending/decending content, plus some padding)
            var fontSize = TextRenderer.MeasureText(RawTextGraphics, "Ay", ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue));
            ApproxFontHeight = fontSize.Height;
            FontCenteringOffset = (int)((ScrollerHeight - ApproxFontHeight) * 0.5);
        }

        private int ColourPos = 0;
        private int Frame = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Could convert frame based animation to time based if we want!

            //double offsetx = (double)(System.Environment.TickCount % 2000) / 2000D;
            //double offsety = (double)(System.Environment.TickCount % 3000) / 3000D;

            //offsetx *= 360;
            //offsety *= 360;

            //Info.Left = x + (int)(Math.Sin(offsetx * rad) * 10D);
            //Info.Top = y + (int)(Math.Sin(offsety * rad) * 10D);

            RawTextGraphics.Clear(Color.Black);

            double count = (double)(System.Environment.TickCount * 0.007);

            // Not an optional scroller - renders each character stopping when we've gone over the width of the bitmap, rather than just keeping previous rendered content, shifting to the left and filling the end.
            // But keeping track of a variable end point, keeping space for buffering off the end etc = a pain in the backside
            // so this will do for now - it's only a basic bit of fun - and allows for animated text, changing font per character, etc.

            //  Get width of first character
            int messageLength = Message.Length - 1;
            string letter = Message.Substring(MessagePos, 1); //Message[(MessagePos % messageLength)..1]; // Message.Substring(MessageOffset);
            var fontSize = TextRenderer.MeasureText(RawTextGraphics, letter, ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);

            //  Move everything to the left
            RenderOffset -= ScrollSpeed;


            //  while (left offset + width) < 0
            while (RenderOffset + fontSize.Width < 0)
            {
                // left offset += width of current 1st character (result should never be > 0)
                RenderOffset += fontSize.Width;

                // byebye to current 1st character...
                MessagePos++;
                if (MessagePos > messageLength)
                    MessagePos = 0;

                //letter = Message[(messagePos++ % messageLength)..(messagePos % messageLength)];
                letter = Message.Substring(MessagePos, 1);   // Already got this. Could optimise this fetch away

                fontSize = TextRenderer.MeasureText(RawTextGraphics, letter, ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                
                // And around we go for another check!                
            }

            // Get to here? Nice! Render out message
            double renderOffset = RenderOffset;
            int messagePos = MessagePos;
            int colourPos = ColourPos;
            if (Frame % 10 == 0)
            {
                if (--ColourPos < 0)
                    ColourPos = ScrollerColours.Length - 1;
            }

            while (renderOffset < PaddedWidth)
            {
                //letter = Message[(messagePos++ % messageLength)..(messagePos % messageLength)]; 
                letter = Message.Substring(messagePos++, 1);
                if (messagePos > messageLength)
                    messagePos = 0;

                fontSize = TextRenderer.MeasureText(RawTextGraphics, letter, ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);

                TextRenderer.DrawText(RawTextGraphics, letter , ScrollerFont, new Point((int)renderOffset, FontCenteringOffset), ScrollerColours[colourPos++]);
                if (colourPos == ScrollerColours.Length)
                    colourPos = 0;

                renderOffset += fontSize.Width;
            }

            IntPtr hdc = MiddleGraphics.GetHdc();
            IntPtr src = NativeMethods.CreateCompatibleDC(hdc);
            IntPtr srcHBitmap = RawTextBitmap.GetHbitmap();
            IntPtr hOldObject = NativeMethods.SelectObject(src, srcHBitmap);

            for (int j = 0; j < ScrollerHeight; j++)
            {
                double offset = (int)(Math.Sin((count * 1.17) + (j * 0.048)) * HorizontalWobbleScale);
                NativeMethods.BitBlt(hdc, (int)offset, j, PaddedWidth, 1, src, 0, j, PatBltType.SrcCopy);
            }

            if (hOldObject != IntPtr.Zero)
                NativeMethods.SelectObject(src, hOldObject);

            NativeMethods.DeleteObject(srcHBitmap);
            NativeMethods.DeleteDC(src);
            MiddleGraphics.ReleaseHdc(hdc);

            hdc = ScrollerGraphics.GetHdc();
            src = NativeMethods.CreateCompatibleDC(hdc);
            srcHBitmap = MiddleBitmap.GetHbitmap();
            hOldObject = NativeMethods.SelectObject(src, srcHBitmap);

            for (int i = 0; i < ScrollerWidth; i++)
            {
                double offset = (int)(Math.Sin(count + (i * 0.02)) * VerticalWobbleScale);
                NativeMethods.BitBlt(hdc, i, (int)offset, 1, ScrollerHeight, src, i + HorizontalWobbleScale, 0, PatBltType.SrcCopy);
            }

            if (hOldObject != IntPtr.Zero)
                NativeMethods.SelectObject(src, hOldObject);

            NativeMethods.DeleteObject(srcHBitmap);
            NativeMethods.DeleteDC(src);
            ScrollerGraphics.ReleaseHdc(hdc);

            // Logo's
            ScrollerGraphics.DrawImage(LogoBitmap, 0,0,LogoBitmap.Width,LogoBitmap.Height);
            ScrollerGraphics.DrawImage(LogoBitmap, ScrollerBitmap.Width - LogoBitmap.Width, 0, LogoBitmap.Width, LogoBitmap.Height);

            Scroller.Refresh();
            // Invalidate(true);

            Frame++;
        }

        private void About_Load(object sender, EventArgs e)
        {
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
