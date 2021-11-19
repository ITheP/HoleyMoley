namespace HoleyMoley
{
    partial class UI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
            this.BorderColour = new System.Windows.Forms.ColorDialog();
            this.EnableHole = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ScreenCrossHairs = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.Move = new System.Windows.Forms.Button();
            this.Centre = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.MarginDepth = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.HoleH = new System.Windows.Forms.ComboBox();
            this.HoleW = new System.Windows.Forms.ComboBox();
            this.EnableMargin = new System.Windows.Forms.CheckBox();
            this.HoleSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OpacityLevel = new System.Windows.Forms.TrackBar();
            this.ColourPicker = new System.Windows.Forms.Button();
            this.About = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.MouseTimer = new System.Windows.Forms.Timer(this.components);
            this.HolePanel = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.HoleControls = new System.Windows.Forms.CheckBox();
            this.ControlPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.HighlightingPanel = new System.Windows.Forms.Panel();
            this.TitleSearch4Colour = new System.Windows.Forms.Button();
            this.TitleSearch3Colour = new System.Windows.Forms.Button();
            this.TitleSearch2Colour = new System.Windows.Forms.Button();
            this.TitleSearch1Colour = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RestoreApp = new System.Windows.Forms.Button();
            this.HighlightAppNames = new System.Windows.Forms.TextBox();
            this.HighlightingTitle = new System.Windows.Forms.Label();
            this.TitleSearchNotFound = new System.Windows.Forms.TextBox();
            this.TitleSearch3 = new System.Windows.Forms.TextBox();
            this.TitleSearch2 = new System.Windows.Forms.TextBox();
            this.TitleSearch1 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.MoveAppToHole = new System.Windows.Forms.Button();
            this.HighlightIncludePopUps = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.EnableHilighting = new System.Windows.Forms.CheckBox();
            this.HighlightDepth = new System.Windows.Forms.TrackBar();
            this.ZoomPanel = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.EnableZoom = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CrossHairV = new System.Windows.Forms.Panel();
            this.ConstantUpdate = new System.Windows.Forms.CheckBox();
            this.CrossHairH = new System.Windows.Forms.Panel();
            this.Zoom = new HoleyMoley.CustomPictureBox();
            this.FasterRefresh = new System.Windows.Forms.CheckBox();
            this.CrossHair = new System.Windows.Forms.CheckBox();
            this.FasterRefreshL = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ZoomLevel = new System.Windows.Forms.TrackBar();
            this.InfoPanel = new System.Windows.Forms.Panel();
            this.MousePosition = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.MouseMeasure = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MarginDepth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.HolePanel.SuspendLayout();
            this.ControlPanel.SuspendLayout();
            this.HighlightingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HighlightDepth)).BeginInit();
            this.ZoomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomLevel)).BeginInit();
            this.InfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BorderColour
            // 
            this.BorderColour.Color = System.Drawing.Color.Lime;
            // 
            // EnableHole
            // 
            this.EnableHole.Appearance = System.Windows.Forms.Appearance.Button;
            this.EnableHole.Checked = true;
            this.EnableHole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableHole.Location = new System.Drawing.Point(406, 8);
            this.EnableHole.Margin = new System.Windows.Forms.Padding(4, 3, 8, 3);
            this.EnableHole.Name = "EnableHole";
            this.EnableHole.Size = new System.Drawing.Size(37, 27);
            this.EnableHole.TabIndex = 39;
            this.EnableHole.Text = "Off";
            this.EnableHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip.SetToolTip(this.EnableHole, "Enable and disable the Hole");
            this.EnableHole.UseVisualStyleBackColor = true;
            this.EnableHole.CheckedChanged += new System.EventHandler(this.EnableHole_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(4, 10);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 20);
            this.label13.TabIndex = 38;
            this.label13.Text = "Hole";
            // 
            // ScreenCrossHairs
            // 
            this.ScreenCrossHairs.AutoSize = true;
            this.ScreenCrossHairs.Checked = true;
            this.ScreenCrossHairs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ScreenCrossHairs.Location = new System.Drawing.Point(172, 145);
            this.ScreenCrossHairs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ScreenCrossHairs.Name = "ScreenCrossHairs";
            this.ScreenCrossHairs.Size = new System.Drawing.Size(15, 14);
            this.ScreenCrossHairs.TabIndex = 36;
            this.ScreenCrossHairs.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.ScreenCrossHairs, "Display crosshairs within the Hole");
            this.ScreenCrossHairs.UseVisualStyleBackColor = true;
            this.ScreenCrossHairs.CheckedChanged += new System.EventHandler(this.ScreenCrossHairs_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(101, 144);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 13);
            this.label11.TabIndex = 35;
            this.label11.Tag = "HoleVisibility";
            this.label11.Text = "Cross hairs";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Move
            // 
            this.Move.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Move.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Move.Location = new System.Drawing.Point(345, 41);
            this.Move.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Move.Name = "Move";
            this.Move.Size = new System.Drawing.Size(98, 27);
            this.Move.TabIndex = 20;
            this.Move.Tag = "HoleVisibility";
            this.Move.Text = "Move";
            this.ToolTip.SetToolTip(this.Move, "Click and drag to move the Hole around the screen");
            this.Move.UseVisualStyleBackColor = true;
            this.Move.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Move_MouseDown);
            // 
            // Centre
            // 
            this.Centre.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Centre.Location = new System.Drawing.Point(282, 41);
            this.Centre.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Centre.Name = "Centre";
            this.Centre.Size = new System.Drawing.Size(57, 27);
            this.Centre.TabIndex = 11;
            this.Centre.Tag = "HoleVisibility";
            this.Centre.Text = "Centre";
            this.ToolTip.SetToolTip(this.Centre, "Centre the hole on your display");
            this.Centre.UseVisualStyleBackColor = false;
            this.Centre.Click += new System.EventHandler(this.Centre_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(230, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 18;
            this.label6.Tag = "HoleVisibility";
            this.label6.Text = "Position";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(4, 117);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.label9.TabIndex = 30;
            this.label9.Tag = "HoleVisibility";
            this.label9.Text = "Depth";
            // 
            // MarginDepth
            // 
            this.MarginDepth.AccessibleDescription = "";
            this.MarginDepth.Location = new System.Drawing.Point(62, 107);
            this.MarginDepth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MarginDepth.Name = "MarginDepth";
            this.MarginDepth.Size = new System.Drawing.Size(162, 45);
            this.MarginDepth.TabIndex = 29;
            this.MarginDepth.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.MarginDepth, "Hole depth/thickness");
            this.MarginDepth.Value = 2;
            this.MarginDepth.ValueChanged += new System.EventHandler(this.MarginDepth_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(138, 78);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 15);
            this.label5.TabIndex = 23;
            this.label5.Tag = "HoleVisibility";
            this.label5.Text = "x";
            // 
            // HoleH
            // 
            this.HoleH.DisplayMember = "800x600";
            this.HoleH.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.HoleH.Location = new System.Drawing.Point(155, 75);
            this.HoleH.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HoleH.MaxDropDownItems = 16;
            this.HoleH.Name = "HoleH";
            this.HoleH.Size = new System.Drawing.Size(67, 21);
            this.HoleH.TabIndex = 22;
            this.HoleH.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.HoleH, "Height of the Hole");
            this.HoleH.SelectedIndexChanged += new System.EventHandler(this.HoleH_SelectedIndexChanged);
            this.HoleH.Validated += new System.EventHandler(this.HoleH_Validated);
            // 
            // HoleW
            // 
            this.HoleW.DisplayMember = "800x600";
            this.HoleW.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HoleW.FormattingEnabled = true;
            this.HoleW.Items.AddRange(new object[] {
            "0",
            "800",
            "1024",
            "1280",
            "1600",
            "1680",
            "1920"});
            this.HoleW.Location = new System.Drawing.Point(62, 75);
            this.HoleW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HoleW.MaxDropDownItems = 16;
            this.HoleW.Name = "HoleW";
            this.HoleW.Size = new System.Drawing.Size(67, 21);
            this.HoleW.TabIndex = 21;
            this.HoleW.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.HoleW, "Width of the Hole");
            this.HoleW.SelectedIndexChanged += new System.EventHandler(this.HoleW_SelectedIndexChanged);
            this.HoleW.Validated += new System.EventHandler(this.HoleW_Validated);
            // 
            // EnableMargin
            // 
            this.EnableMargin.AutoSize = true;
            this.EnableMargin.Checked = true;
            this.EnableMargin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableMargin.Location = new System.Drawing.Point(62, 145);
            this.EnableMargin.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.EnableMargin.Name = "EnableMargin";
            this.EnableMargin.Size = new System.Drawing.Size(15, 14);
            this.EnableMargin.TabIndex = 14;
            this.EnableMargin.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.EnableMargin, "Show a margin around the Hole, rather than highlighting to the edges of the scree" +
        "n");
            this.EnableMargin.UseVisualStyleBackColor = true;
            this.EnableMargin.CheckedChanged += new System.EventHandler(this.EnableMargin_CheckedChanged);
            // 
            // HoleSize
            // 
            this.HoleSize.DisplayMember = "800x600";
            this.HoleSize.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            this.HoleSize.Location = new System.Drawing.Point(62, 44);
            this.HoleSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HoleSize.MaxDropDownItems = 16;
            this.HoleSize.Name = "HoleSize";
            this.HoleSize.Size = new System.Drawing.Size(160, 21);
            this.HoleSize.TabIndex = 12;
            this.HoleSize.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.HoleSize, "Size of Hole");
            this.HoleSize.SelectedIndexChanged += new System.EventHandler(this.HoleSize_SelectedIndexChanged);
            this.HoleSize.Validated += new System.EventHandler(this.HoleSize_Validated);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 15);
            this.label2.TabIndex = 11;
            this.label2.Tag = "HoleVisibility";
            this.label2.Text = "Size";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(4, 144);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 8;
            this.label4.Tag = "HoleVisibility";
            this.label4.Text = "Margin";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(230, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 10;
            this.label1.Tag = "HoleVisibility";
            this.label1.Text = "Opacity";
            // 
            // OpacityLevel
            // 
            this.OpacityLevel.AccessibleDescription = "";
            this.OpacityLevel.Location = new System.Drawing.Point(284, 107);
            this.OpacityLevel.Margin = new System.Windows.Forms.Padding(4, 3, 5, 3);
            this.OpacityLevel.Minimum = 1;
            this.OpacityLevel.Name = "OpacityLevel";
            this.OpacityLevel.Size = new System.Drawing.Size(162, 45);
            this.OpacityLevel.TabIndex = 7;
            this.OpacityLevel.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.OpacityLevel, "How transparent the Holes margin is");
            this.OpacityLevel.Value = 9;
            this.OpacityLevel.ValueChanged += new System.EventHandler(this.OpacityLevel_ValueChanged);
            // 
            // ColourPicker
            // 
            this.ColourPicker.BackColor = System.Drawing.Color.Lime;
            this.ColourPicker.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ColourPicker.Location = new System.Drawing.Point(8, 75);
            this.ColourPicker.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ColourPicker.Name = "ColourPicker";
            this.ColourPicker.Size = new System.Drawing.Size(26, 23);
            this.ColourPicker.TabIndex = 43;
            this.ColourPicker.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.ColourPicker, "Colour of the Holes border. Click to change.");
            this.ColourPicker.UseVisualStyleBackColor = false;
            this.ColourPicker.Click += new System.EventHandler(this.ColourPicker_Click);
            // 
            // About
            // 
            this.About.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.About.Location = new System.Drawing.Point(381, 6);
            this.About.Margin = new System.Windows.Forms.Padding(4, 3, 8, 6);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(62, 27);
            this.About.TabIndex = 10;
            this.About.Text = "About";
            this.ToolTip.SetToolTip(this.About, "Extra information about Holey Moley");
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // Logo
            // 
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(0, 0);
            this.Logo.Margin = new System.Windows.Forms.Padding(0);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(64, 64);
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
            // HolePanel
            // 
            this.HolePanel.AutoSize = true;
            this.HolePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.HolePanel.BackColor = System.Drawing.Color.White;
            this.HolePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HolePanel.Controls.Add(this.label20);
            this.HolePanel.Controls.Add(this.HoleControls);
            this.HolePanel.Controls.Add(this.OpacityLevel);
            this.HolePanel.Controls.Add(this.label1);
            this.HolePanel.Controls.Add(this.label13);
            this.HolePanel.Controls.Add(this.EnableHole);
            this.HolePanel.Controls.Add(this.label4);
            this.HolePanel.Controls.Add(this.label2);
            this.HolePanel.Controls.Add(this.ScreenCrossHairs);
            this.HolePanel.Controls.Add(this.HoleSize);
            this.HolePanel.Controls.Add(this.label11);
            this.HolePanel.Controls.Add(this.EnableMargin);
            this.HolePanel.Controls.Add(this.HoleW);
            this.HolePanel.Controls.Add(this.HoleH);
            this.HolePanel.Controls.Add(this.label5);
            this.HolePanel.Controls.Add(this.label9);
            this.HolePanel.Controls.Add(this.label6);
            this.HolePanel.Controls.Add(this.Move);
            this.HolePanel.Controls.Add(this.Centre);
            this.HolePanel.Controls.Add(this.ColourPicker);
            this.HolePanel.Controls.Add(this.MarginDepth);
            this.HolePanel.Location = new System.Drawing.Point(2, 3);
            this.HolePanel.Margin = new System.Windows.Forms.Padding(0);
            this.HolePanel.Name = "HolePanel";
            this.HolePanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.HolePanel.Size = new System.Drawing.Size(453, 168);
            this.HolePanel.TabIndex = 14;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label20.Location = new System.Drawing.Point(211, 144);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(51, 13);
            this.label20.TabIndex = 41;
            this.label20.Tag = "HoleVisibility";
            this.label20.Text = "Controls";
            // 
            // HoleControls
            // 
            this.HoleControls.AutoSize = true;
            this.HoleControls.Checked = true;
            this.HoleControls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HoleControls.Location = new System.Drawing.Point(270, 145);
            this.HoleControls.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HoleControls.Name = "HoleControls";
            this.HoleControls.Size = new System.Drawing.Size(15, 14);
            this.HoleControls.TabIndex = 42;
            this.HoleControls.Tag = "HoleVisibility";
            this.ToolTip.SetToolTip(this.HoleControls, "Show Move/App move and restore/etc. controls on the Hole");
            this.HoleControls.UseVisualStyleBackColor = true;
            this.HoleControls.Click += new System.EventHandler(this.HoleControls_Click);
            // 
            // ControlPanel
            // 
            this.ControlPanel.AutoSize = true;
            this.ControlPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ControlPanel.BackColor = System.Drawing.Color.Transparent;
            this.ControlPanel.Controls.Add(this.HolePanel);
            this.ControlPanel.Controls.Add(this.HighlightingPanel);
            this.ControlPanel.Controls.Add(this.ZoomPanel);
            this.ControlPanel.Controls.Add(this.InfoPanel);
            this.ControlPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ControlPanel.Location = new System.Drawing.Point(0, 0);
            this.ControlPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Padding = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.ControlPanel.Size = new System.Drawing.Size(457, 962);
            this.ControlPanel.TabIndex = 15;
            // 
            // HighlightingPanel
            // 
            this.HighlightingPanel.AutoSize = true;
            this.HighlightingPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.HighlightingPanel.BackColor = System.Drawing.Color.White;
            this.HighlightingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HighlightingPanel.Controls.Add(this.TitleSearch4Colour);
            this.HighlightingPanel.Controls.Add(this.TitleSearch3Colour);
            this.HighlightingPanel.Controls.Add(this.TitleSearch2Colour);
            this.HighlightingPanel.Controls.Add(this.TitleSearch1Colour);
            this.HighlightingPanel.Controls.Add(this.label21);
            this.HighlightingPanel.Controls.Add(this.label3);
            this.HighlightingPanel.Controls.Add(this.RestoreApp);
            this.HighlightingPanel.Controls.Add(this.HighlightAppNames);
            this.HighlightingPanel.Controls.Add(this.HighlightingTitle);
            this.HighlightingPanel.Controls.Add(this.TitleSearchNotFound);
            this.HighlightingPanel.Controls.Add(this.TitleSearch3);
            this.HighlightingPanel.Controls.Add(this.TitleSearch2);
            this.HighlightingPanel.Controls.Add(this.TitleSearch1);
            this.HighlightingPanel.Controls.Add(this.label17);
            this.HighlightingPanel.Controls.Add(this.MoveAppToHole);
            this.HighlightingPanel.Controls.Add(this.HighlightIncludePopUps);
            this.HighlightingPanel.Controls.Add(this.label15);
            this.HighlightingPanel.Controls.Add(this.label16);
            this.HighlightingPanel.Controls.Add(this.EnableHilighting);
            this.HighlightingPanel.Controls.Add(this.HighlightDepth);
            this.HighlightingPanel.Location = new System.Drawing.Point(2, 173);
            this.HighlightingPanel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.HighlightingPanel.Name = "HighlightingPanel";
            this.HighlightingPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.HighlightingPanel.Size = new System.Drawing.Size(453, 201);
            this.HighlightingPanel.TabIndex = 17;
            // 
            // TitleSearch4Colour
            // 
            this.TitleSearch4Colour.BackColor = System.Drawing.Color.Gold;
            this.TitleSearch4Colour.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TitleSearch4Colour.Location = new System.Drawing.Point(340, 145);
            this.TitleSearch4Colour.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch4Colour.Name = "TitleSearch4Colour";
            this.TitleSearch4Colour.Size = new System.Drawing.Size(26, 23);
            this.TitleSearch4Colour.TabIndex = 55;
            this.TitleSearch4Colour.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.TitleSearch4Colour, "Colour of the Holes border. Click to change.");
            this.TitleSearch4Colour.UseVisualStyleBackColor = false;
            // 
            // TitleSearch3Colour
            // 
            this.TitleSearch3Colour.BackColor = System.Drawing.Color.DodgerBlue;
            this.TitleSearch3Colour.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TitleSearch3Colour.Location = new System.Drawing.Point(229, 145);
            this.TitleSearch3Colour.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch3Colour.Name = "TitleSearch3Colour";
            this.TitleSearch3Colour.Size = new System.Drawing.Size(26, 23);
            this.TitleSearch3Colour.TabIndex = 54;
            this.TitleSearch3Colour.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.TitleSearch3Colour, "Colour of the Holes border. Click to change.");
            this.TitleSearch3Colour.UseVisualStyleBackColor = false;
            // 
            // TitleSearch2Colour
            // 
            this.TitleSearch2Colour.BackColor = System.Drawing.Color.LimeGreen;
            this.TitleSearch2Colour.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TitleSearch2Colour.Location = new System.Drawing.Point(119, 145);
            this.TitleSearch2Colour.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch2Colour.Name = "TitleSearch2Colour";
            this.TitleSearch2Colour.Size = new System.Drawing.Size(26, 23);
            this.TitleSearch2Colour.TabIndex = 53;
            this.TitleSearch2Colour.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.TitleSearch2Colour, "Colour of the Holes border. Click to change.");
            this.TitleSearch2Colour.UseVisualStyleBackColor = false;
            // 
            // TitleSearch1Colour
            // 
            this.TitleSearch1Colour.BackColor = System.Drawing.Color.Red;
            this.TitleSearch1Colour.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TitleSearch1Colour.Location = new System.Drawing.Point(8, 145);
            this.TitleSearch1Colour.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch1Colour.Name = "TitleSearch1Colour";
            this.TitleSearch1Colour.Size = new System.Drawing.Size(26, 23);
            this.TitleSearch1Colour.TabIndex = 52;
            this.TitleSearch1Colour.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.TitleSearch1Colour, "Colour of the Holes border. Click to change.");
            this.TitleSearch1Colour.UseVisualStyleBackColor = false;
            this.TitleSearch1Colour.Click += new System.EventHandler(this.TitleSearch1Colour_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label21.Location = new System.Drawing.Point(4, 126);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(253, 13);
            this.label21.TabIndex = 51;
            this.label21.Tag = "HilightingVisibility";
            this.label21.Text = "Highlight colours to titles matching (; delimited)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(4, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(348, 13);
            this.label3.TabIndex = 50;
            this.label3.Tag = "HilightingVisibility";
            this.label3.Text = "Only highlight apps with the following in their names (; delimited)";
            // 
            // RestoreApp
            // 
            this.RestoreApp.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RestoreApp.Location = new System.Drawing.Point(378, 41);
            this.RestoreApp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.RestoreApp.Name = "RestoreApp";
            this.RestoreApp.Size = new System.Drawing.Size(65, 27);
            this.RestoreApp.TabIndex = 49;
            this.RestoreApp.Tag = "HilightingVisibility";
            this.RestoreApp.Text = "Restore";
            this.ToolTip.SetToolTip(this.RestoreApp, "Restore any window moved to the Hole back to its previous position");
            this.RestoreApp.UseVisualStyleBackColor = true;
            this.RestoreApp.Click += new System.EventHandler(this.RestoreApp_Click);
            // 
            // HighlightAppNames
            // 
            this.HighlightAppNames.BackColor = System.Drawing.SystemColors.Window;
            this.HighlightAppNames.Location = new System.Drawing.Point(8, 96);
            this.HighlightAppNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HighlightAppNames.Name = "HighlightAppNames";
            this.HighlightAppNames.Size = new System.Drawing.Size(435, 23);
            this.HighlightAppNames.TabIndex = 48;
            this.HighlightAppNames.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.HighlightAppNames, "; delimited list of application names - only these will ever be highlighted. Leav" +
        "e blank to highlight all applications.");
            // 
            // HighlightingTitle
            // 
            this.HighlightingTitle.Font = new System.Drawing.Font("Arial Narrow", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HighlightingTitle.Location = new System.Drawing.Point(104, 12);
            this.HighlightingTitle.Margin = new System.Windows.Forms.Padding(4, 0, 9, 0);
            this.HighlightingTitle.Name = "HighlightingTitle";
            this.HighlightingTitle.Size = new System.Drawing.Size(299, 21);
            this.HighlightingTitle.TabIndex = 41;
            this.HighlightingTitle.Tag = "HilightingVisibility";
            this.HighlightingTitle.Text = "Title";
            // 
            // TitleSearchNotFound
            // 
            this.TitleSearchNotFound.BackColor = System.Drawing.Color.White;
            this.TitleSearchNotFound.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TitleSearchNotFound.Enabled = false;
            this.TitleSearchNotFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.TitleSearchNotFound.Location = new System.Drawing.Point(366, 149);
            this.TitleSearchNotFound.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearchNotFound.Name = "TitleSearchNotFound";
            this.TitleSearchNotFound.Size = new System.Drawing.Size(76, 13);
            this.TitleSearchNotFound.TabIndex = 47;
            this.TitleSearchNotFound.Tag = "HilightingVisibility";
            this.TitleSearchNotFound.Text = "not specified";
            this.TitleSearchNotFound.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TitleSearch3
            // 
            this.TitleSearch3.BackColor = System.Drawing.Color.White;
            this.TitleSearch3.Location = new System.Drawing.Point(255, 145);
            this.TitleSearch3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch3.Name = "TitleSearch3";
            this.TitleSearch3.Size = new System.Drawing.Size(76, 23);
            this.TitleSearch3.TabIndex = 46;
            this.TitleSearch3.Tag = "HilightingVisibility";
            this.TitleSearch3.Text = "Dev";
            // 
            // TitleSearch2
            // 
            this.TitleSearch2.BackColor = System.Drawing.Color.White;
            this.TitleSearch2.Location = new System.Drawing.Point(145, 145);
            this.TitleSearch2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch2.Name = "TitleSearch2";
            this.TitleSearch2.Size = new System.Drawing.Size(76, 23);
            this.TitleSearch2.TabIndex = 45;
            this.TitleSearch2.Tag = "HilightingVisibility";
            this.TitleSearch2.Text = "Test";
            // 
            // TitleSearch1
            // 
            this.TitleSearch1.BackColor = System.Drawing.Color.White;
            this.TitleSearch1.Location = new System.Drawing.Point(34, 145);
            this.TitleSearch1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TitleSearch1.Name = "TitleSearch1";
            this.TitleSearch1.Size = new System.Drawing.Size(76, 23);
            this.TitleSearch1.TabIndex = 44;
            this.TitleSearch1.Tag = "HilightingVisibility";
            this.TitleSearch1.Text = "Live";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label17.Location = new System.Drawing.Point(4, 48);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 13);
            this.label17.TabIndex = 41;
            this.label17.Tag = "HilightingVisibility";
            this.label17.Text = "Depth";
            // 
            // MoveAppToHole
            // 
            this.MoveAppToHole.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MoveAppToHole.Location = new System.Drawing.Point(230, 41);
            this.MoveAppToHole.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MoveAppToHole.Name = "MoveAppToHole";
            this.MoveAppToHole.Size = new System.Drawing.Size(142, 27);
            this.MoveAppToHole.TabIndex = 40;
            this.MoveAppToHole.Tag = "HilightingVisibility";
            this.MoveAppToHole.Text = "Move App to Hole";
            this.ToolTip.SetToolTip(this.MoveAppToHole, "Move currently highlighted app to fit inside Hole");
            this.MoveAppToHole.UseVisualStyleBackColor = true;
            this.MoveAppToHole.Click += new System.EventHandler(this.MoveAppToHole_Click);
            // 
            // HighlightIncludePopUps
            // 
            this.HighlightIncludePopUps.AutoSize = true;
            this.HighlightIncludePopUps.Checked = true;
            this.HighlightIncludePopUps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HighlightIncludePopUps.Location = new System.Drawing.Point(119, 178);
            this.HighlightIncludePopUps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HighlightIncludePopUps.Name = "HighlightIncludePopUps";
            this.HighlightIncludePopUps.Size = new System.Drawing.Size(15, 14);
            this.HighlightIncludePopUps.TabIndex = 43;
            this.HighlightIncludePopUps.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.HighlightIncludePopUps, "Highlight pop up windows as well as main windows");
            this.HighlightIncludePopUps.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label15.Location = new System.Drawing.Point(4, 10);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(92, 20);
            this.label15.TabIndex = 40;
            this.label15.Text = "Highlighting";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label16.Location = new System.Drawing.Point(4, 177);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(99, 13);
            this.label16.TabIndex = 42;
            this.label16.Tag = "HilightingVisibility";
            this.label16.Text = "Highlight PopUps";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EnableHilighting
            // 
            this.EnableHilighting.Appearance = System.Windows.Forms.Appearance.Button;
            this.EnableHilighting.Checked = true;
            this.EnableHilighting.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableHilighting.Location = new System.Drawing.Point(406, 8);
            this.EnableHilighting.Margin = new System.Windows.Forms.Padding(4, 3, 8, 3);
            this.EnableHilighting.Name = "EnableHilighting";
            this.EnableHilighting.Size = new System.Drawing.Size(37, 27);
            this.EnableHilighting.TabIndex = 41;
            this.EnableHilighting.Text = "Off";
            this.EnableHilighting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip.SetToolTip(this.EnableHilighting, "Enable and disable app/window highlighting");
            this.EnableHilighting.UseVisualStyleBackColor = true;
            this.EnableHilighting.CheckedChanged += new System.EventHandler(this.EnableHilighting_CheckedChanged);
            // 
            // HighlightDepth
            // 
            this.HighlightDepth.AccessibleDescription = "";
            this.HighlightDepth.Location = new System.Drawing.Point(62, 37);
            this.HighlightDepth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.HighlightDepth.Maximum = 42;
            this.HighlightDepth.Minimum = 2;
            this.HighlightDepth.Name = "HighlightDepth";
            this.HighlightDepth.Size = new System.Drawing.Size(162, 45);
            this.HighlightDepth.TabIndex = 40;
            this.HighlightDepth.Tag = "HilightingVisibility";
            this.ToolTip.SetToolTip(this.HighlightDepth, "Depth/thickness of border around highlighted window");
            this.HighlightDepth.Value = 2;
            this.HighlightDepth.ValueChanged += new System.EventHandler(this.HighlightDepth_ValueChanged);
            // 
            // ZoomPanel
            // 
            this.ZoomPanel.AutoSize = true;
            this.ZoomPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ZoomPanel.BackColor = System.Drawing.Color.White;
            this.ZoomPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ZoomPanel.Controls.Add(this.label14);
            this.ZoomPanel.Controls.Add(this.label8);
            this.ZoomPanel.Controls.Add(this.EnableZoom);
            this.ZoomPanel.Controls.Add(this.label7);
            this.ZoomPanel.Controls.Add(this.CrossHairV);
            this.ZoomPanel.Controls.Add(this.ConstantUpdate);
            this.ZoomPanel.Controls.Add(this.CrossHairH);
            this.ZoomPanel.Controls.Add(this.Zoom);
            this.ZoomPanel.Controls.Add(this.FasterRefresh);
            this.ZoomPanel.Controls.Add(this.CrossHair);
            this.ZoomPanel.Controls.Add(this.FasterRefreshL);
            this.ZoomPanel.Controls.Add(this.label10);
            this.ZoomPanel.Controls.Add(this.ZoomLevel);
            this.ZoomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoomPanel.Location = new System.Drawing.Point(2, 376);
            this.ZoomPanel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ZoomPanel.Name = "ZoomPanel";
            this.ZoomPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.ZoomPanel.Size = new System.Drawing.Size(453, 541);
            this.ZoomPanel.TabIndex = 16;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(4, 47);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 13);
            this.label14.TabIndex = 41;
            this.label14.Tag = "ZoomVisibility";
            this.label14.Text = "Scale";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(4, 8);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 21);
            this.label8.TabIndex = 27;
            this.label8.Text = "Zoom";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EnableZoom
            // 
            this.EnableZoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.EnableZoom.Checked = true;
            this.EnableZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnableZoom.Location = new System.Drawing.Point(406, 8);
            this.EnableZoom.Margin = new System.Windows.Forms.Padding(4, 3, 8, 3);
            this.EnableZoom.Name = "EnableZoom";
            this.EnableZoom.Size = new System.Drawing.Size(37, 27);
            this.EnableZoom.TabIndex = 40;
            this.EnableZoom.Text = "Off";
            this.EnableZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ToolTip.SetToolTip(this.EnableZoom, "Enable and disable magnified view of screen");
            this.EnableZoom.UseVisualStyleBackColor = true;
            this.EnableZoom.CheckedChanged += new System.EventHandler(this.EnableZoom_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(4, 73);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 26;
            this.label7.Tag = "ZoomVisibility";
            this.label7.Text = "Live";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CrossHairV
            // 
            this.CrossHairV.BackColor = System.Drawing.Color.Black;
            this.CrossHairV.Location = new System.Drawing.Point(9, 96);
            this.CrossHairV.Margin = new System.Windows.Forms.Padding(0);
            this.CrossHairV.Name = "CrossHairV";
            this.CrossHairV.Size = new System.Drawing.Size(54, 115);
            this.CrossHairV.TabIndex = 34;
            this.CrossHairV.Tag = "";
            this.CrossHairV.Visible = false;
            // 
            // ConstantUpdate
            // 
            this.ConstantUpdate.AutoSize = true;
            this.ConstantUpdate.Checked = true;
            this.ConstantUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ConstantUpdate.Location = new System.Drawing.Point(59, 73);
            this.ConstantUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ConstantUpdate.Name = "ConstantUpdate";
            this.ConstantUpdate.Size = new System.Drawing.Size(15, 14);
            this.ConstantUpdate.TabIndex = 26;
            this.ConstantUpdate.Tag = "ZoomVisibility";
            this.ToolTip.SetToolTip(this.ConstantUpdate, "Zoom updates constantly (so e.g. animation will appear within it). When disabled," +
        " only updates on pointer movement.");
            this.ConstantUpdate.UseVisualStyleBackColor = true;
            // 
            // CrossHairH
            // 
            this.CrossHairH.BackColor = System.Drawing.Color.Black;
            this.CrossHairH.Location = new System.Drawing.Point(9, 244);
            this.CrossHairH.Margin = new System.Windows.Forms.Padding(0);
            this.CrossHairH.Name = "CrossHairH";
            this.CrossHairH.Size = new System.Drawing.Size(54, 115);
            this.CrossHairH.TabIndex = 33;
            this.CrossHairH.Tag = "";
            this.CrossHairH.Visible = false;
            // 
            // Zoom
            // 
            this.Zoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Zoom.Location = new System.Drawing.Point(8, 96);
            this.Zoom.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Zoom.Name = "Zoom";
            this.Zoom.Size = new System.Drawing.Size(435, 435);
            this.Zoom.TabIndex = 28;
            this.Zoom.TabStop = false;
            this.Zoom.Tag = "ZoomVisibility";
            this.Zoom.Click += new System.EventHandler(this.Zoom_Click);
            // 
            // FasterRefresh
            // 
            this.FasterRefresh.AutoSize = true;
            this.FasterRefresh.Checked = true;
            this.FasterRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FasterRefresh.Location = new System.Drawing.Point(296, 73);
            this.FasterRefresh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.FasterRefresh.Name = "FasterRefresh";
            this.FasterRefresh.Size = new System.Drawing.Size(15, 14);
            this.FasterRefresh.TabIndex = 31;
            this.FasterRefresh.Tag = "ZoomVisibility";
            this.ToolTip.SetToolTip(this.FasterRefresh, "Refresh zoom view more frequently - smoother bit slightly more resources required" +
        " on your machine");
            this.FasterRefresh.UseVisualStyleBackColor = true;
            this.FasterRefresh.CheckedChanged += new System.EventHandler(this.FasterRefresh_CheckedChanged);
            // 
            // CrossHair
            // 
            this.CrossHair.AutoSize = true;
            this.CrossHair.Checked = true;
            this.CrossHair.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CrossHair.Location = new System.Drawing.Point(172, 73);
            this.CrossHair.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CrossHair.Name = "CrossHair";
            this.CrossHair.Size = new System.Drawing.Size(15, 14);
            this.CrossHair.TabIndex = 31;
            this.CrossHair.Tag = "ZoomVisibility";
            this.ToolTip.SetToolTip(this.CrossHair, "Show cross hairs in zoom view");
            this.CrossHair.UseVisualStyleBackColor = true;
            this.CrossHair.CheckedChanged += new System.EventHandler(this.CrossHair_CheckedChanged);
            // 
            // FasterRefreshL
            // 
            this.FasterRefreshL.AutoSize = true;
            this.FasterRefreshL.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FasterRefreshL.Location = new System.Drawing.Point(211, 72);
            this.FasterRefreshL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FasterRefreshL.Name = "FasterRefreshL";
            this.FasterRefreshL.Size = new System.Drawing.Size(77, 13);
            this.FasterRefreshL.TabIndex = 32;
            this.FasterRefreshL.Tag = "ZoomVisibility";
            this.FasterRefreshL.Text = "Faster refresh";
            this.FasterRefreshL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(101, 73);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 32;
            this.label10.Tag = "ZoomVisibility";
            this.label10.Text = "Cross hairs";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ZoomLevel
            // 
            this.ZoomLevel.AccessibleDescription = "";
            this.ZoomLevel.Location = new System.Drawing.Point(62, 37);
            this.ZoomLevel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ZoomLevel.Minimum = 1;
            this.ZoomLevel.Name = "ZoomLevel";
            this.ZoomLevel.Size = new System.Drawing.Size(162, 45);
            this.ZoomLevel.TabIndex = 25;
            this.ZoomLevel.Tag = "ZoomVisibility";
            this.ToolTip.SetToolTip(this.ZoomLevel, "Zoom scale");
            this.ZoomLevel.Value = 3;
            this.ZoomLevel.ValueChanged += new System.EventHandler(this.ZoomLevel_ValueChanged);
            // 
            // InfoPanel
            // 
            this.InfoPanel.AutoSize = true;
            this.InfoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.InfoPanel.BackColor = System.Drawing.Color.White;
            this.InfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoPanel.Controls.Add(this.MousePosition);
            this.InfoPanel.Controls.Add(this.label18);
            this.InfoPanel.Controls.Add(this.About);
            this.InfoPanel.Controls.Add(this.MouseMeasure);
            this.InfoPanel.Controls.Add(this.label12);
            this.InfoPanel.Location = new System.Drawing.Point(2, 919);
            this.InfoPanel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.InfoPanel.Name = "InfoPanel";
            this.InfoPanel.Size = new System.Drawing.Size(453, 41);
            this.InfoPanel.TabIndex = 16;
            // 
            // MousePosition
            // 
            this.MousePosition.AutoSize = true;
            this.MousePosition.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MousePosition.Location = new System.Drawing.Point(39, 12);
            this.MousePosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MousePosition.Name = "MousePosition";
            this.MousePosition.Size = new System.Drawing.Size(64, 13);
            this.MousePosition.TabIndex = 24;
            this.MousePosition.Text = "Mouse pos";
            this.MousePosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label18.Location = new System.Drawing.Point(112, 11);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(33, 17);
            this.label18.TabIndex = 42;
            this.label18.Text = "Dist.";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MouseMeasure
            // 
            this.MouseMeasure.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MouseMeasure.Location = new System.Drawing.Point(138, 12);
            this.MouseMeasure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MouseMeasure.Name = "MouseMeasure";
            this.MouseMeasure.Size = new System.Drawing.Size(75, 15);
            this.MouseMeasure.TabIndex = 24;
            this.MouseMeasure.Text = "Mouse pos";
            this.MouseMeasure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(10, 11);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 15);
            this.label12.TabIndex = 24;
            this.label12.Text = "Pos.";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(951, 1005);
            this.Controls.Add(this.ControlPanel);
            this.Controls.Add(this.Logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "UI";
            this.Text = "Holey Moley";
            this.Activated += new System.EventHandler(this.Controller_Activated);
            this.Deactivate += new System.EventHandler(this.Controller_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Controller_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Controller_FormClosed);
            this.Load += new System.EventHandler(this.UI_Load);
            this.MouseEnter += new System.EventHandler(this.Controller_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Controller_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.MarginDepth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpacityLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.HolePanel.ResumeLayout(false);
            this.HolePanel.PerformLayout();
            this.ControlPanel.ResumeLayout(false);
            this.ControlPanel.PerformLayout();
            this.HighlightingPanel.ResumeLayout(false);
            this.HighlightingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HighlightDepth)).EndInit();
            this.ZoomPanel.ResumeLayout(false);
            this.ZoomPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Zoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomLevel)).EndInit();
            this.InfoPanel.ResumeLayout(false);
            this.InfoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog BorderColour;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.CheckBox EnableMargin;
        private System.Windows.Forms.ComboBox HoleSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar OpacityLevel;
        private System.Windows.Forms.Button ColourPicker;
        private System.Windows.Forms.Button Centre;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Move;
        private System.Windows.Forms.ComboBox HoleH;
        private System.Windows.Forms.ComboBox HoleW;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer MouseTimer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TrackBar MarginDepth;
        private System.Windows.Forms.CheckBox ScreenCrossHairs;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox EnableHole;
        private System.Windows.Forms.Panel HolePanel;
        private System.Windows.Forms.FlowLayoutPanel ControlPanel;
        private System.Windows.Forms.Panel HighlightingPanel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox EnableHilighting;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button MoveAppToHole;
        private System.Windows.Forms.CheckBox HighlightIncludePopUps;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TrackBar HighlightDepth;
        private System.Windows.Forms.Panel InfoPanel;
        private System.Windows.Forms.TextBox TitleSearchNotFound;
        private System.Windows.Forms.TextBox TitleSearch3;
        private System.Windows.Forms.TextBox TitleSearch2;
        private System.Windows.Forms.TextBox TitleSearch1;
        private System.Windows.Forms.Label HighlightingTitle;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox HoleControls;
        private System.Windows.Forms.TextBox HighlightAppNames;
        private System.Windows.Forms.Button RestoreApp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel ZoomPanel;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox EnableZoom;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel CrossHairV;
        private System.Windows.Forms.CheckBox ConstantUpdate;
        private System.Windows.Forms.Panel CrossHairH;
        private CustomPictureBox Zoom;
        private System.Windows.Forms.CheckBox FasterRefresh;
        private System.Windows.Forms.Label MousePosition;
        private System.Windows.Forms.CheckBox CrossHair;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label FasterRefreshL;
        private System.Windows.Forms.Label MouseMeasure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TrackBar ZoomLevel;
        private System.Windows.Forms.ToolTip ToolTip;
        private System.Windows.Forms.Button TitleSearch4Colour;
        private System.Windows.Forms.Button TitleSearch3Colour;
        private System.Windows.Forms.Button TitleSearch2Colour;
        private System.Windows.Forms.Button TitleSearch1Colour;
    }
}