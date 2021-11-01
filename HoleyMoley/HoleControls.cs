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
    public partial class HoleControls : Form
    {
        public Controller Controller { get; set; }

        public HoleControls()
        {
            InitializeComponent();
        }

        public void EnableDisableMoveAppToHole(bool enable)
        {
            MoveAppToHole.Enabled = enable;
            RestoreApp.Enabled = enable;
        }

        private void Move_MouseDown(object sender, MouseEventArgs e)
        {
            Controller.Move_MouseDown(sender, e);
        }

        private void MoveAppToHole_Click(object sender, EventArgs e)
        {
            Controller.MoveApptoHole();
        }

        private void RestoreApp_Click(object sender, EventArgs e)
        {
            Controller.RestoreAppPos();
        }
    }
}
