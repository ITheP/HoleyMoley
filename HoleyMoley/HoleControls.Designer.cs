
namespace HoleyMoley
{
    partial class HoleControls
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Move = new System.Windows.Forms.Button();
            this.MoveAppToHole = new System.Windows.Forms.Button();
            this.RestoreApp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Move
            // 
            this.Move.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Move.Location = new System.Drawing.Point(0, 0);
            this.Move.Margin = new System.Windows.Forms.Padding(0);
            this.Move.Name = "Move";
            this.Move.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.Move.Size = new System.Drawing.Size(25, 25);
            this.Move.TabIndex = 0;
            this.Move.Text = "";
            this.Move.UseVisualStyleBackColor = true;
            this.Move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Move_MouseDown);
            // 
            // MoveAppToHole
            // 
            this.MoveAppToHole.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoveAppToHole.Location = new System.Drawing.Point(25, 0);
            this.MoveAppToHole.Margin = new System.Windows.Forms.Padding(0);
            this.MoveAppToHole.Name = "MoveAppToHole";
            this.MoveAppToHole.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.MoveAppToHole.Size = new System.Drawing.Size(25, 25);
            this.MoveAppToHole.TabIndex = 1;
            this.MoveAppToHole.Text = "";
            this.MoveAppToHole.UseVisualStyleBackColor = true;
            this.MoveAppToHole.Click += new System.EventHandler(this.MoveAppToHole_Click);
            // 
            // RestoreApp
            // 
            this.RestoreApp.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestoreApp.Location = new System.Drawing.Point(50, 0);
            this.RestoreApp.Margin = new System.Windows.Forms.Padding(0);
            this.RestoreApp.Name = "RestoreApp";
            this.RestoreApp.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.RestoreApp.Size = new System.Drawing.Size(25, 25);
            this.RestoreApp.TabIndex = 2;
            this.RestoreApp.Text = "";
            this.RestoreApp.UseVisualStyleBackColor = true;
            this.RestoreApp.Click += new System.EventHandler(this.RestoreApp_Click);
            // 
            // HoleControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(497, 64);
            this.ControlBox = false;
            this.Controls.Add(this.RestoreApp);
            this.Controls.Add(this.MoveAppToHole);
            this.Controls.Add(this.Move);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HoleControls";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Move;
        private System.Windows.Forms.Button MoveAppToHole;
        private System.Windows.Forms.Button RestoreApp;
    }
}