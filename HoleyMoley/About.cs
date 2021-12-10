using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Windows.Media;

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
        private int VerticalWobbleScale { get; set; } = 48;
        private string Message { get; set; } = "►Holey Moley◄ ☺ ► ©2010-2021 I-The-P ◄ ☺ ►Shout-outs go to my kitty cats, IRC and bored lunchtimes◄ ☺ ";
        private int MessagePos { get; set; } = 0;
        private double RenderOffset { get; set; } = 0;
        private double ScrollSpeed { get; set; } = 9.0d;

        private Color[] ScrollerColours { get; set; } = { Color.Pink, Color.Moccasin, Color.LightGoldenrodYellow, Color.PaleGreen, Color.LightCyan, Color.PowderBlue, Color.Thistle };

        private Color[] MetalStartColors { get; set; } = new[] { Color.DarkBlue, Color.LightBlue, Color.White, Color.FromArgb(255, 12, 6, 0), Color.Gold, Color.AntiqueWhite };
        private float[] MetalGradientPositions { get; set; } = new[] { 0.0f, 0.54f, 0.57f, 0.5701f, 0.7f, 1.0f };
        private Color[] MetalAnimatedFirstColours { get; set; } = { Color.DarkBlue, Color.DarkRed, Color.Gold, Color.DarkGreen, Color.Maroon, Color.SaddleBrown, Color.DarkGreen, Color.Purple };
        private Color[] MetalAnimatedSecondColours { get; set; } = { Color.LightBlue, Color.Pink, Color.LightGoldenrodYellow, Color.PaleGreen, Color.Firebrick, Color.Orange, Color.LimeGreen, Color.Plum };

        private AnimatedColor FirstAnimatedColor { get; set; }
        private AnimatedColor SecondAnimatedColor { get; set; }
        private int MetalAnimationRate { get; set; } = 300;     // number of frames to blend between colours

        Rectangle TopFadeRec { get; set; }
        Rectangle BottomFadeRec { get; set; }
        Rectangle FontRec { get; set; }

        LinearGradientBrush TopFadeBrush { get; set; }
        LinearGradientBrush BottomFadeBrush { get; set; }
        LinearGradientBrush FontBrush { get; set; }

        ColorBlend FontColorBlend { get; set; }

        //private const int WM_NCHITTEST = 0x84;
        //private const int HTCLIENT = 0x1;
        //private const int HTCAPTION = 0x2;

        //protected override void WndProc(ref Message message)
        //{
        //    base.WndProc(ref message);

        //    if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
        //        message.Result = (IntPtr)HTCAPTION;
        //}

        public About()
        {
            InitializeComponent();

            this.ControlBox = false;
            this.Text = String.Empty;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

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

            // G = Graphics.FromImage(ScrollerBitmap);
            // G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            // G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            ScrollerFont = new Font("Impact", 96, FontStyle.Regular, GraphicsUnit.Pixel);

            Scroller.Image = ScrollerBitmap;
            this.DoubleBuffered = true;

            // Approximate font height based on a couple of letters (ascending/decending content, plus some padding)
            //var fontSize = TextRenderer.MeasureText(RawTextGraphics, "Ay", ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue));
            var fontSize = RawTextGraphics.MeasureString("Ay", ScrollerFont, new System.Drawing.PointF(int.MaxValue, int.MaxValue), StringFormat.GenericTypographic);
            ApproxFontHeight = (int)fontSize.Height;
            FontCenteringOffset = (int)((ScrollerHeight - ApproxFontHeight) * 0.5);

            TopFadeRec = new() { X = 0, Y = 0, Width = ScrollerBitmap.Width, Height = (int)(ScrollerBitmap.Height * 0.2) };
            BottomFadeRec = new() { X = 0, Y = ScrollerBitmap.Height - (int)(ScrollerBitmap.Height * 0.2), Width = ScrollerBitmap.Width, Height = (int)(ScrollerBitmap.Height * 0.2) + 1 }; // The +1 overcomes a gdi bug where extra 1 pixel black line can be drawn https://bytes.com/topic/c-sharp/answers/587091-known-problem-lineargradientbrush

            TopFadeBrush = new(TopFadeRec, Color.Black, Color.Transparent, LinearGradientMode.Vertical);
            BottomFadeBrush = new(BottomFadeRec, Color.Transparent, Color.Black, LinearGradientMode.Vertical);

            FontRec = new(0, 0, 1, ApproxFontHeight);
            FontBrush = new(FontRec, Color.Black, Color.Black, LinearGradientMode.Vertical);
            FontColorBlend = new();
            FontColorBlend.Positions = MetalGradientPositions;
            FontColorBlend.Colors = MetalStartColors;
            FontBrush.InterpolationColors = FontColorBlend;

            FirstAnimatedColor = new(MetalAnimatedFirstColours, MetalAnimationRate);
            SecondAnimatedColor = new(MetalAnimatedSecondColours, MetalAnimationRate);

            Timer.Start();
        }

        //private int ColourPos = 0;
        //private int Frame = 0;
        private Stopwatch Timer = new();
        private long LastTicks { get; set; } = 0;
        private double CurrentTime { get; set; } = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Movement/positioning related to elapsed time, to try and make as smooth as we can

            long currentTicks = Timer.ElapsedTicks;
            double frameLength = (currentTicks - LastTicks) * 0.0000033d; // 10,000 ticks in a millisecond
            LastTicks = currentTicks;
            CurrentTime = currentTicks * 0.0000001d;   // 30ms timer, 0.0001 = ms -> seconds, so we should end up with an approx fractional second we are on

            RawTextGraphics.Clear(Color.Black);

            // Whole scroller re-renders each frame (rather than shifting content) - so we can animate colours and stuff :)

            //  Get width of first character
            int messageLength = Message.Length - 1;
            string letter = Message.Substring(MessagePos, 1); //Message[(MessagePos % messageLength)..1]; // Message.Substring(MessageOffset);
            //var fontSize = TextRenderer.MeasureText(RawTextGraphics, letter, ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
            var fontSize = RawTextGraphics.MeasureString(letter, ScrollerFont, new System.Drawing.PointF(int.MaxValue, int.MaxValue), StringFormat.GenericTypographic);

            //  Move everything to the left
            RenderOffset -= ScrollSpeed;

            // We check to see if the first letter is no longer visible (scrolled too far to the left)
            // If this is the case, we scan next letters until we are within content appearing on screen
            while (RenderOffset + fontSize.Width < 0)
            {
                // left offset += width of current 1st character (result should never be > 0)
                RenderOffset += fontSize.Width;

                // byebye to current 1st character...
                MessagePos++;
                if (MessagePos > messageLength)
                    MessagePos = 0;

                //letter = Message[(messagePos++ % messageLength)..(messagePos % messageLength)];
                letter = Message.Substring(MessagePos, 1);

                //fontSize = TextRenderer.MeasureText(RawTextGraphics, letter, ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                fontSize = RawTextGraphics.MeasureString(letter, ScrollerFont, new System.Drawing.PointF(int.MaxValue, int.MaxValue), StringFormat.GenericTypographic);

                // And around we go for another check!                
            }

            // Get to here? Nice! Render out message
            double renderOffset = RenderOffset;
            int messagePos = MessagePos;
            //int colourPos = ColourPos;
            //if (Frame % 10 == 0)
            //{
            //    if (--ColourPos < 0)
            //        ColourPos = ScrollerColours.Length - 1;
            //}

            FontColorBlend.Colors[0] = FirstAnimatedColor.Next(frameLength);
            FontColorBlend.Colors[1] = SecondAnimatedColor.Next(frameLength);
            FontBrush.InterpolationColors = FontColorBlend;

            while (renderOffset < PaddedWidth)
            {
                //letter = Message[(messagePos++ % messageLength)..(messagePos % messageLength)]; 
                letter = Message.Substring(messagePos++, 1);
                if (messagePos > messageLength)
                    messagePos = 0;

                // MeasureText gives us better spacing, MeasureString returns zero for spaces.
                fontSize = TextRenderer.MeasureText(RawTextGraphics, letter, ScrollerFont, new System.Drawing.Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                //fontSize = RawTextGraphics.MeasureString(letter, ScrollerFont, new System.Drawing.PointF(int.MaxValue, int.MaxValue), StringFormat.GenericTypographic);

                if (letter != " ")
                    //TextRenderer.DrawText(RawTextGraphics, letter, ScrollerFont, new Point((int)renderOffset, FontCenteringOffset), FontBrush); // ScrollerColours[colourPos++]);
                    RawTextGraphics.DrawString(letter, ScrollerFont, FontBrush, new Point((int)renderOffset, FontCenteringOffset)); // ScrollerColours[colourPos++]);


                renderOffset += fontSize.Width;
            }

            //if (colourPos == ScrollerColours.Length)
            //    colourPos = 0;

            IntPtr hdc = MiddleGraphics.GetHdc();
            IntPtr src = NativeMethods.CreateCompatibleDC(hdc);
            IntPtr srcHBitmap = RawTextBitmap.GetHbitmap();
            IntPtr hOldObject = NativeMethods.SelectObject(src, srcHBitmap);

            for (int j = 0; j < ScrollerHeight; j++)
            {
                double offset = (int)(Math.Sin((CurrentTime * 11.7d) + (j * 0.048)) * HorizontalWobbleScale);
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
                double offset = (int)(Math.Sin((CurrentTime * 8.19d) + (i * 0.01117)) * VerticalWobbleScale);
                NativeMethods.BitBlt(hdc, i, (int)offset, 1, ScrollerHeight, src, i + HorizontalWobbleScale, 0, PatBltType.SrcCopy);
            }

            if (hOldObject != IntPtr.Zero)
                NativeMethods.SelectObject(src, hOldObject);

            NativeMethods.DeleteObject(srcHBitmap);
            NativeMethods.DeleteDC(src);
            ScrollerGraphics.ReleaseHdc(hdc);

            // Logo's
            ScrollerGraphics.DrawImage(LogoBitmap, 32, 32, LogoBitmap.Width, LogoBitmap.Height);
            ScrollerGraphics.DrawImage(LogoBitmap, ScrollerBitmap.Width - LogoBitmap.Width - 32, 32, LogoBitmap.Width, LogoBitmap.Height);

            // Fading
            ScrollerGraphics.FillRectangle(TopFadeBrush, TopFadeRec);
            ScrollerGraphics.FillRectangle(BottomFadeBrush, BottomFadeRec);


            Scroller.Refresh();
            // Invalidate(true);

            //Frame++;
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

        private void Scroller_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    // This is not coded for accuracy, as it doesn't matter that much
    public class AnimatedColor
    {
        private double R { get; set; }
        private double G { get; set; }
        private double B { get; set; }

        private double RChange { get; set; }
        private double GChange { get; set; }
        private double BChange { get; set; }

        private Color[] Colors { get; set; }

        private int ColorPos { get; set; } = 0;
        private double Counter { get; set; } = 0;
        private int ChangeRate { get; set; }

        public AnimatedColor(Color[] colors, int changeRate)
        {
            ChangeRate = changeRate;

            Colors = colors;
            R = Colors[0].R;
            G = Colors[0].G;
            B = Colors[0].B;

            InitNextColour();
        }

        private void InitNextColour()
        {
            ColorPos++;
            if (ColorPos == Colors.Length)
                ColorPos = 0;

            RChange = (Colors[ColorPos].R - R) / ChangeRate;
            GChange = (Colors[ColorPos].G - G) / ChangeRate;
            BChange = (Colors[ColorPos].B - B) / ChangeRate;
        }

        public Color Next(double frameLength)
        {
            R += RChange * frameLength;
            G += GChange * frameLength;
            B += BChange * frameLength;

            // Just incase, even though it shouldnt strictly speaking be a thing...
            if (R > 255)
                R = 255;
            else if (R < 0)
                R = 0;

            if (G > 255)
                G = 255;
            else if (G < 0)
                G = 0;

            if (B > 255)
                B = 255;
            else if (B < 0)
                B = 0;

            Counter += frameLength;
            if (Counter >= ChangeRate)
            {
                Counter -= ChangeRate;

                ColorPos++;
                if (ColorPos == Colors.Length)
                    ColorPos = 0;

                double rRange = Colors[ColorPos].R - R;
                double gRange = Colors[ColorPos].G - G;
                double bRange = Colors[ColorPos].B - B;

                RChange = rRange / ChangeRate;
                GChange = gRange / ChangeRate;
                BChange = bRange / ChangeRate;

                // And account for any small initial change, cause, why not :)
                // Counter has a possible tiny fractional change amount already in it
                R += RChange * Counter;
                G += GChange * Counter;
                B += BChange * Counter;
            }

            return Color.FromArgb(255, (int)R, (int)G, (int)B);
        }

    }
}
