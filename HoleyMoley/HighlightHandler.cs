using System;
using System.Drawing;
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

        public static void SetPosition(IntPtr parent, int x, int y, int w, int h, Color color)
        {
            Highlight.BackColor = color;
            bool result = NativeMethods.SetWindowPos(Highlight.Handle, parent, x, y, w, h, SetWindowPosFlags.NOACTIVATE | SetWindowPosFlags.SHOWWINDOW);
        }

        public static void Hide()
        {
            Highlight.Visible = false;
        }
    }
}
