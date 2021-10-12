using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        public About()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double offsetx = (double)(System.Environment.TickCount % 2000) / 2000D;
            double offsety = (double)(System.Environment.TickCount % 3000) / 3000D;

           // offsetx *= 360;
           // offsety *= 360;

            Info.Left = x + (int)(Math.Sin(offsetx * rad) * 10D);
            Info.Top = y + (int)(Math.Sin(offsety * rad) * 10D);
        }

        private void About_Load(object sender, EventArgs e)
        {
            x = Info.Left;
            y = Info.Top;

            timer1.Start();
        }
    }
}
