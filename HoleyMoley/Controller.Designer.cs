namespace HoleyMoley
{
    partial class Controller
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Controller));
            this.BorderColour = new System.Windows.Forms.ColorDialog();
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.ZoomSeparator = new System.Windows.Forms.Panel();
            this.ScreenCrossHairs = new System.Windows.Forms.CheckBox();
            this.About = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.CrossHairV = new System.Windows.Forms.Panel();
            this.CrossHairH = new System.Windows.Forms.Panel();
            this.FasterRefresh = new System.Windows.Forms.CheckBox();
            this.CrossHair = new System.Windows.Forms.CheckBox();
            this.FasterRefreshL = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Move = new System.Windows.Forms.Button();
            this.Centre = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.MouseMeasure = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.MousePosition = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.MarginDepth = new System.Windows.Forms.TrackBar();
            this.Zoom = new HoleyMoley.CustomPictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ConstantUpdate = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.HoleH = new System.Windows.Forms.ComboBox();
            this.HoleW = new System.Windows.Forms.ComboBox();
            this.Margin = new System.Windows.Forms.CheckBox();
            this.Toggle = new System.Windows.Forms.Button();
            this.HoleSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OpacityLevel = new System.Windows.Forms.TrackBar();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.ColourPicker = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.ZoomLevel = new System.Windows.Forms.TrackBar();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.MouseTimer = new System.Windows.Forms.Timer(this.components);
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MarginDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Zoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // BorderColour
            // 
            this.BorderColour.Color = System.Drawing.Color.Lime;
            // 
            // ControlPanel
            // 
            this.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ControlPanel.Controls.Add(this.ZoomSeparator);
            this.ControlPanel.Controls.Add(this.ScreenCrossHairs);
            this.ControlPanel.Controls.Add(this.About);
            this.ControlPanel.Controls.Add(this.label11);
            this.ControlPanel.Controls.Add(this.CrossHairV);
            this.ControlPanel.Controls.Add(this.CrossHairH);
            this.ControlPanel.Controls.Add(this.FasterRefresh);
            this.ControlPanel.Controls.Add(this.CrossHair);
            this.ControlPanel.Controls.Add(this.FasterRefreshL);
            this.ControlPanel.Controls.Add(this.label10);
            this.ControlPanel.Controls.Add(this.Move);
            this.ControlPanel.Controls.Add(this.Centre);
            this.ControlPanel.Controls.Add(this.label6);
            this.ControlPanel.Controls.Add(this.MouseMeasure);
            this.ControlPanel.Controls.Add(this.label12);
            this.ControlPanel.Controls.Add(this.MousePosition);
            this.ControlPanel.Controls.Add(this.label9);
            this.ControlPanel.Controls.Add(this.MarginDepth);
            this.ControlPanel.Controls.Add(this.Zoom);
            this.ControlPanel.Controls.Add(this.label8);
            this.ControlPanel.Controls.Add(this.ConstantUpdate);
            this.ControlPanel.Controls.Add(this.label7);
            this.ControlPanel.Controls.Add(this.label5);
            this.ControlPanel.Controls.Add(this.HoleH);
            this.ControlPanel.Controls.Add(this.HoleW);
            this.ControlPanel.Controls.Add(this.Margin);
            this.ControlPanel.Controls.Add(this.Toggle);
            this.ControlPanel.Controls.Add(this.HoleSize);
            this.ControlPanel.Controls.Add(this.label2);
            this.ControlPanel.Controls.Add(this.label4);
            this.ControlPanel.Controls.Add(this.label3);
            this.ControlPanel.Controls.Add(this.label1);
            this.ControlPanel.Controls.Add(this.OpacityLevel);
            this.ControlPanel.Controls.Add(this.shapeContainer1);
            this.ControlPanel.Controls.Add(this.ZoomLevel);
            this.ControlPanel.Location = new System.Drawing.Point(3, 3);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(196, 501);
            this.ControlPanel.TabIndex = 9;
            // 
            // ZoomSeparator
            // 
            this.ZoomSeparator.BackColor = System.Drawing.Color.Maroon;
            this.ZoomSeparator.Location = new System.Drawing.Point(-1, 208);
            this.ZoomSeparator.Name = "ZoomSeparator";
            this.ZoomSeparator.Size = new System.Drawing.Size(400, 1);
            this.ZoomSeparator.TabIndex = 37;
            // 
            // ScreenCrossHairs
            // 
            this.ScreenCrossHairs.AutoSize = true;
            this.ScreenCrossHairs.Checked = true;
            this.ScreenCrossHairs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ScreenCrossHairs.Location = new System.Drawing.Point(174, 123);
            this.ScreenCrossHairs.Name = "ScreenCrossHairs";
            this.ScreenCrossHairs.Size = new System.Drawing.Size(15, 14);
            this.ScreenCrossHairs.TabIndex = 36;
            this.ScreenCrossHairs.UseVisualStyleBackColor = true;
            this.ScreenCrossHairs.CheckedChanged += new System.EventHandler(this.ScreenCrossHairs_CheckedChanged);
            // 
            // About
            // 
            this.About.Font = new System.Drawing.Font("Segoe UI Symbol", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.About.Location = new System.Drawing.Point(197, 5);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(53, 16);
            this.About.TabIndex = 10;
            this.About.Text = "About";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(72, 123);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "Screen cross hairs";
            // 
            // CrossHairV
            // 
            this.CrossHairV.BackColor = System.Drawing.Color.Black;
            this.CrossHairV.Location = new System.Drawing.Point(97, 328);
            this.CrossHairV.Name = "CrossHairV";
            this.CrossHairV.Size = new System.Drawing.Size(46, 100);
            this.CrossHairV.TabIndex = 34;
            this.CrossHairV.Visible = false;
            // 
            // CrossHairH
            // 
            this.CrossHairH.BackColor = System.Drawing.Color.Black;
            this.CrossHairH.Location = new System.Drawing.Point(20, 328);
            this.CrossHairH.Name = "CrossHairH";
            this.CrossHairH.Size = new System.Drawing.Size(46, 100);
            this.CrossHairH.TabIndex = 33;
            this.CrossHairH.Visible = false;
            // 
            // FasterRefresh
            // 
            this.FasterRefresh.AutoSize = true;
            this.FasterRefresh.Checked = true;
            this.FasterRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FasterRefresh.Location = new System.Drawing.Point(105, 289);
            this.FasterRefresh.Name = "FasterRefresh";
            this.FasterRefresh.Size = new System.Drawing.Size(15, 14);
            this.FasterRefresh.TabIndex = 31;
            this.FasterRefresh.UseVisualStyleBackColor = true;
            this.FasterRefresh.CheckedChanged += new System.EventHandler(this.FasterRefresh_CheckedChanged);
            // 
            // CrossHair
            // 
            this.CrossHair.AutoSize = true;
            this.CrossHair.Checked = true;
            this.CrossHair.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CrossHair.Location = new System.Drawing.Point(105, 269);
            this.CrossHair.Name = "CrossHair";
            this.CrossHair.Size = new System.Drawing.Size(15, 14);
            this.CrossHair.TabIndex = 31;
            this.CrossHair.UseVisualStyleBackColor = true;
            this.CrossHair.CheckedChanged += new System.EventHandler(this.CrossHair_CheckedChanged);
            // 
            // FasterRefreshL
            // 
            this.FasterRefreshL.AutoSize = true;
            this.FasterRefreshL.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FasterRefreshL.Location = new System.Drawing.Point(4, 289);
            this.FasterRefreshL.Name = "FasterRefreshL";
            this.FasterRefreshL.Size = new System.Drawing.Size(96, 13);
            this.FasterRefreshL.TabIndex = 32;
            this.FasterRefreshL.Text = "Smoother refresh";
            this.FasterRefreshL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(4, 269);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Cross hairs";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Move
            // 
            this.Move.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Move.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Move.Location = new System.Drawing.Point(105, 180);
            this.Move.Name = "Move";
            this.Move.Size = new System.Drawing.Size(84, 22);
            this.Move.TabIndex = 20;
            this.Move.Text = "Move";
            this.Move.UseVisualStyleBackColor = true;
            this.Move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Move_MouseDown);
            // 
            // Centre
            // 
            this.Centre.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Centre.Location = new System.Drawing.Point(51, 179);
            this.Centre.Name = "Centre";
            this.Centre.Size = new System.Drawing.Size(49, 23);
            this.Centre.TabIndex = 11;
            this.Centre.Text = "Centre";
            this.Centre.UseVisualStyleBackColor = true;
            this.Centre.Click += new System.EventHandler(this.Centre_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(2, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Position";
            // 
            // MouseMeasure
            // 
            this.MouseMeasure.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MouseMeasure.Location = new System.Drawing.Point(128, 289);
            this.MouseMeasure.Name = "MouseMeasure";
            this.MouseMeasure.Size = new System.Drawing.Size(64, 13);
            this.MouseMeasure.TabIndex = 24;
            this.MouseMeasure.Text = "Mouse pos";
            this.MouseMeasure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(128, 269);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Pos⭡ ⭣Dist";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MousePosition
            // 
            this.MousePosition.AutoSize = true;
            this.MousePosition.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MousePosition.Location = new System.Drawing.Point(128, 249);
            this.MousePosition.Name = "MousePosition";
            this.MousePosition.Size = new System.Drawing.Size(64, 13);
            this.MousePosition.TabIndex = 24;
            this.MousePosition.Text = "Mouse pos";
            this.MousePosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(2, 152);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Depth";
            // 
            // MarginDepth
            // 
            this.MarginDepth.AccessibleDescription = "";
            this.MarginDepth.Location = new System.Drawing.Point(51, 143);
            this.MarginDepth.Name = "MarginDepth";
            this.MarginDepth.Size = new System.Drawing.Size(139, 45);
            this.MarginDepth.TabIndex = 29;
            this.MarginDepth.Value = 2;
            this.MarginDepth.ValueChanged += new System.EventHandler(this.MarginDepth_ValueChanged);
            // 
            // Zoom
            // 
            this.Zoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Zoom.Location = new System.Drawing.Point(5, 310);
            this.Zoom.Name = "Zoom";
            this.Zoom.Size = new System.Drawing.Size(184, 184);
            this.Zoom.TabIndex = 28;
            this.Zoom.TabStop = false;
            this.Zoom.Click += new System.EventHandler(this.Zoom_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 221);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Zoom";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConstantUpdate
            // 
            this.ConstantUpdate.AutoSize = true;
            this.ConstantUpdate.Checked = true;
            this.ConstantUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ConstantUpdate.Location = new System.Drawing.Point(105, 249);
            this.ConstantUpdate.Name = "ConstantUpdate";
            this.ConstantUpdate.Size = new System.Drawing.Size(15, 14);
            this.ConstantUpdate.TabIndex = 26;
            this.ConstantUpdate.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Live update";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(116, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "x";
            // 
            // HoleH
            // 
            this.HoleH.DisplayMember = "800x600";
            this.HoleH.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoleH.FormattingEnabled = true;
            this.HoleH.Items.AddRange(new object[] {
            "0",
            "600",
            "768",
            "800",
            "900",
            "1024",
            "1050",
            "1080",
            "1200"});
            this.HoleH.Location = new System.Drawing.Point(131, 32);
            this.HoleH.MaxDropDownItems = 16;
            this.HoleH.Name = "HoleH";
            this.HoleH.Size = new System.Drawing.Size(58, 21);
            this.HoleH.TabIndex = 22;
            this.HoleH.SelectedIndexChanged += new System.EventHandler(this.HoleH_SelectedIndexChanged);
            this.HoleH.Validated += new System.EventHandler(this.HoleH_Validated);
            // 
            // HoleW
            // 
            this.HoleW.DisplayMember = "800x600";
            this.HoleW.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoleW.FormattingEnabled = true;
            this.HoleW.Items.AddRange(new object[] {
            "0",
            "800",
            "1024",
            "1280",
            "1600",
            "1680",
            "1920"});
            this.HoleW.Location = new System.Drawing.Point(51, 32);
            this.HoleW.MaxDropDownItems = 16;
            this.HoleW.Name = "HoleW";
            this.HoleW.Size = new System.Drawing.Size(58, 21);
            this.HoleW.TabIndex = 21;
            this.HoleW.SelectedIndexChanged += new System.EventHandler(this.HoleW_SelectedIndexChanged);
            this.HoleW.Validated += new System.EventHandler(this.HoleW_Validated);
            // 
            // Margin
            // 
            this.Margin.AutoSize = true;
            this.Margin.Checked = true;
            this.Margin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Margin.Location = new System.Drawing.Point(51, 123);
            this.Margin.Name = "Margin";
            this.Margin.Size = new System.Drawing.Size(15, 14);
            this.Margin.TabIndex = 14;
            this.Margin.UseVisualStyleBackColor = true;
            this.Margin.CheckedChanged += new System.EventHandler(this.Margin_CheckedChanged);
            // 
            // Toggle
            // 
            this.Toggle.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Toggle.Location = new System.Drawing.Point(51, 94);
            this.Toggle.Name = "Toggle";
            this.Toggle.Size = new System.Drawing.Size(138, 23);
            this.Toggle.TabIndex = 13;
            this.Toggle.Text = "Toggle Visibility";
            this.Toggle.UseVisualStyleBackColor = true;
            this.Toggle.Click += new System.EventHandler(this.Toggle_Click);
            // 
            // HoleSize
            // 
            this.HoleSize.DisplayMember = "800x600";
            this.HoleSize.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HoleSize.FormattingEnabled = true;
            this.HoleSize.Items.AddRange(new object[] {
            "800x600",
            "1024x768",
            "1280x800",
            "1280x1024",
            "1600x900",
            "1680x1050",
            "1920x1080",
            "1920x1200",
            "800x0",
            "0x600",
            "Size to screen"});
            this.HoleSize.Location = new System.Drawing.Point(51, 5);
            this.HoleSize.MaxDropDownItems = 16;
            this.HoleSize.Name = "HoleSize";
            this.HoleSize.Size = new System.Drawing.Size(111, 21);
            this.HoleSize.TabIndex = 12;
            this.HoleSize.SelectedIndexChanged += new System.EventHandler(this.HoleSize_SelectedIndexChanged);
            this.HoleSize.Validated += new System.EventHandler(this.HoleSize_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Hole";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(2, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Margin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(2, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Overlay";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Opacity";
            // 
            // OpacityLevel
            // 
            this.OpacityLevel.AccessibleDescription = "";
            this.OpacityLevel.Location = new System.Drawing.Point(51, 59);
            this.OpacityLevel.Minimum = 1;
            this.OpacityLevel.Name = "OpacityLevel";
            this.OpacityLevel.Size = new System.Drawing.Size(139, 45);
            this.OpacityLevel.TabIndex = 7;
            this.OpacityLevel.Value = 9;
            this.OpacityLevel.ValueChanged += new System.EventHandler(this.OpacityLevel_ValueChanged);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.ColourPicker});
            this.shapeContainer1.Size = new System.Drawing.Size(194, 499);
            this.shapeContainer1.TabIndex = 15;
            this.shapeContainer1.TabStop = false;
            // 
            // ColourPicker
            // 
            this.ColourPicker.FillColor = System.Drawing.Color.Lime;
            this.ColourPicker.FillGradientStyle = Microsoft.VisualBasic.PowerPacks.FillGradientStyle.Vertical;
            this.ColourPicker.FillStyle = Microsoft.VisualBasic.PowerPacks.FillStyle.Solid;
            this.ColourPicker.Location = new System.Drawing.Point(166, 5);
            this.ColourPicker.Name = "ColourPicker";
            this.ColourPicker.Size = new System.Drawing.Size(22, 20);
            this.ColourPicker.Click += new System.EventHandler(this.ColourPicker_Click);
            // 
            // ZoomLevel
            // 
            this.ZoomLevel.AccessibleDescription = "";
            this.ZoomLevel.Location = new System.Drawing.Point(53, 217);
            this.ZoomLevel.Minimum = 1;
            this.ZoomLevel.Name = "ZoomLevel";
            this.ZoomLevel.Size = new System.Drawing.Size(139, 45);
            this.ZoomLevel.TabIndex = 25;
            this.ZoomLevel.Value = 3;
            this.ZoomLevel.ValueChanged += new System.EventHandler(this.ZoomLevel_ValueChanged);
            // 
            // Logo
            // 
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(7, 4);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(74, 72);
            this.Logo.TabIndex = 8;
            this.Logo.TabStop = false;
            this.Logo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Logo_MouseClick);
            // 
            // MouseTimer
            // 
            this.MouseTimer.Enabled = true;
            this.MouseTimer.Interval = 30;
            this.MouseTimer.Tick += new System.EventHandler(this.MouseTimer_Tick);
            // 
            // Controller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(205, 508);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.ControlPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Controller";
            this.Text = "Holey Moley";
            this.Activated += new System.EventHandler(this.Controller_Activated);
            this.Deactivate += new System.EventHandler(this.Controller_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Controller_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Controller_FormClosed);
            this.Load += new System.EventHandler(this.Controller_Load);
            this.MouseEnter += new System.EventHandler(this.Controller_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Controller_MouseLeave);
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MarginDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Zoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog BorderColour;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.CheckBox Margin;
        private System.Windows.Forms.Button Toggle;
        private System.Windows.Forms.ComboBox HoleSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar OpacityLevel;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape ColourPicker;
        private System.Windows.Forms.Button Centre;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Move;
        private System.Windows.Forms.ComboBox HoleH;
        private System.Windows.Forms.ComboBox HoleW;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer MouseTimer;
        private System.Windows.Forms.TrackBar ZoomLevel;
        private System.Windows.Forms.Label label8;
        private CustomPictureBox Zoom;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar MarginDepth;
        private System.Windows.Forms.Panel CrossHairV;
        private System.Windows.Forms.Panel CrossHairH;
        private System.Windows.Forms.CheckBox ScreenCrossHairs;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel ZoomSeparator;
        private System.Windows.Forms.CheckBox CrossHair;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.Label MousePosition;
        private System.Windows.Forms.CheckBox ConstantUpdate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label MouseMeasure;
        private System.Windows.Forms.CheckBox FasterRefresh;
        private System.Windows.Forms.Label FasterRefreshL;
        private System.Windows.Forms.Label label12;
    }
}