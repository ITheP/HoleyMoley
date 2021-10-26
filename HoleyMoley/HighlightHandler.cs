using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HoleyMoley
{
    public static class HighlightHandler
    {
        public static Highlight Highlight { get; set; } = null;

        public static void InitHighlight()
        {
            if (Highlight == null)
            {
                Highlight = new Highlight();
                Highlight.Show();
            }
        }

        public static void SetPosition(IntPtr parent, int x, int y, int w, int h)
        {
            bool result = NativeMethods.SetWindowPos(Highlight.Handle, parent, x, y, w, h, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
            //Highlight.Left = x;
            //Highlight.Top = y;
            //Highlight.Width = w;
            //Highlight.Height = h;
        }
    }
}
