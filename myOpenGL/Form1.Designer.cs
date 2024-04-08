using System;
using System.Windows.Forms;

namespace myOpenGL
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar4 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar5 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar6 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar9 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar8 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar7 = new System.Windows.Forms.HScrollBar();
            this.slider1 = new System.Windows.Forms.HScrollBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.slider2 = new System.Windows.Forms.HScrollBar();
            this.slider4 = new System.Windows.Forms.HScrollBar();
            this.slider3 = new System.Windows.Forms.HScrollBar();
            this.sliderLablel3 = new System.Windows.Forms.Label();
            this.sliderLablel2 = new System.Windows.Forms.Label();
            this.sliderLablel1 = new System.Windows.Forms.Label();
            this.sliderLablel4 = new System.Windows.Forms.Label();
            this.LightingMaterialChoises = new System.Windows.Forms.GroupBox();
            this.LightPosition = new System.Windows.Forms.RadioButton();
            this.LightSpecular = new System.Windows.Forms.RadioButton();
            this.LightDiffuse = new System.Windows.Forms.RadioButton();
            this.LightAmbient = new System.Windows.Forms.RadioButton();
            this.MatShininess = new System.Windows.Forms.RadioButton();
            this.MatSpecular = new System.Windows.Forms.RadioButton();
            this.MatDiffuse = new System.Windows.Forms.RadioButton();
            this.MatAmbient = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            this.LightingMaterialChoises.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Location = new System.Drawing.Point(12, 16);
            this.panel1.MinimumSize = new System.Drawing.Size(515, 500);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(515, 500);
            this.panel1.TabIndex = 6;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseEnter += new System.EventHandler(this.panel1_MouseEnter);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseWheel);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(549, 16);
            this.hScrollBar1.Maximum = 200;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar1.TabIndex = 7;
            this.hScrollBar1.Value = 100;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar2.Location = new System.Drawing.Point(549, 31);
            this.hScrollBar2.Maximum = 200;
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar2.TabIndex = 8;
            this.hScrollBar2.Value = 100;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar3.Location = new System.Drawing.Point(549, 46);
            this.hScrollBar3.Maximum = 200;
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar3.TabIndex = 9;
            this.hScrollBar3.Value = 200;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar4
            // 
            this.hScrollBar4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar4.Location = new System.Drawing.Point(549, 61);
            this.hScrollBar4.Maximum = 200;
            this.hScrollBar4.Name = "hScrollBar4";
            this.hScrollBar4.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar4.TabIndex = 10;
            this.hScrollBar4.Value = 100;
            this.hScrollBar4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar5
            // 
            this.hScrollBar5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar5.Location = new System.Drawing.Point(549, 76);
            this.hScrollBar5.Maximum = 200;
            this.hScrollBar5.Name = "hScrollBar5";
            this.hScrollBar5.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar5.TabIndex = 8;
            this.hScrollBar5.Value = 100;
            this.hScrollBar5.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar6
            // 
            this.hScrollBar6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar6.Location = new System.Drawing.Point(609, 16);
            this.hScrollBar6.Maximum = 200;
            this.hScrollBar6.Name = "hScrollBar6";
            this.hScrollBar6.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar6.TabIndex = 11;
            this.hScrollBar6.Value = 100;
            this.hScrollBar6.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar9
            // 
            this.hScrollBar9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar9.Location = new System.Drawing.Point(609, 61);
            this.hScrollBar9.Maximum = 200;
            this.hScrollBar9.Name = "hScrollBar9";
            this.hScrollBar9.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar9.TabIndex = 14;
            this.hScrollBar9.Value = 100;
            this.hScrollBar9.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar8
            // 
            this.hScrollBar8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar8.Location = new System.Drawing.Point(609, 46);
            this.hScrollBar8.Maximum = 200;
            this.hScrollBar8.Name = "hScrollBar8";
            this.hScrollBar8.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar8.TabIndex = 12;
            this.hScrollBar8.Value = 110;
            this.hScrollBar8.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // hScrollBar7
            // 
            this.hScrollBar7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar7.Location = new System.Drawing.Point(609, 31);
            this.hScrollBar7.Maximum = 200;
            this.hScrollBar7.Name = "hScrollBar7";
            this.hScrollBar7.Size = new System.Drawing.Size(60, 15);
            this.hScrollBar7.TabIndex = 13;
            this.hScrollBar7.Value = 100;
            this.hScrollBar7.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // slider1
            // 
            this.slider1.Location = new System.Drawing.Point(21, 212);
            this.slider1.Name = "slider1";
            this.slider1.Size = new System.Drawing.Size(100, 20);
            this.slider1.TabIndex = 19;
            this.slider1.Value = 50;
            this.slider1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown3);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(549, 245);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 66);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Translate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(88, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "y";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(84, 21);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(48, 21);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(10, 21);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDown4);
            this.groupBox2.Controls.Add(this.numericUpDown5);
            this.groupBox2.Controls.Add(this.numericUpDown6);
            this.groupBox2.Location = new System.Drawing.Point(549, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(118, 66);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rotate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "y";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "x";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(10, 21);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown4.TabIndex = 2;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(48, 21);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown5.TabIndex = 1;
            this.numericUpDown5.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(84, 21);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(19, 20);
            this.numericUpDown6.TabIndex = 0;
            this.numericUpDown6.ValueChanged += new System.EventHandler(this.numericUpDownValueChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(542, 403);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(124, 82);
            this.listBox1.TabIndex = 17;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(738, 399);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(124, 117);
            this.textBox1.TabIndex = 18;
            // 
            // slider2
            // 
            this.slider2.Location = new System.Drawing.Point(21, 235);
            this.slider2.Name = "slider2";
            this.slider2.Size = new System.Drawing.Size(100, 20);
            this.slider2.TabIndex = 20;
            this.slider2.Value = 50;
            this.slider2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // slider4
            // 
            this.slider4.Location = new System.Drawing.Point(21, 275);
            this.slider4.Name = "slider4";
            this.slider4.Size = new System.Drawing.Size(100, 20);
            this.slider4.TabIndex = 21;
            this.slider4.Value = 50;
            this.slider4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // slider3
            // 
            this.slider3.Location = new System.Drawing.Point(21, 255);
            this.slider3.Name = "slider3";
            this.slider3.Size = new System.Drawing.Size(100, 20);
            this.slider3.TabIndex = 22;
            this.slider3.Value = 50;
            this.slider3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarScroll);
            // 
            // sliderLablel3
            // 
            this.sliderLablel3.AutoSize = true;
            this.sliderLablel3.Location = new System.Drawing.Point(1, 257);
            this.sliderLablel3.Name = "sliderLablel3";
            this.sliderLablel3.Size = new System.Drawing.Size(14, 13);
            this.sliderLablel3.TabIndex = 8;
            this.sliderLablel3.Text = "B";
            // 
            // sliderLablel2
            // 
            this.sliderLablel2.AutoSize = true;
            this.sliderLablel2.Location = new System.Drawing.Point(1, 236);
            this.sliderLablel2.Name = "sliderLablel2";
            this.sliderLablel2.Size = new System.Drawing.Size(15, 13);
            this.sliderLablel2.TabIndex = 7;
            this.sliderLablel2.Text = "G";
            // 
            // sliderLablel1
            // 
            this.sliderLablel1.AutoSize = true;
            this.sliderLablel1.Location = new System.Drawing.Point(1, 216);
            this.sliderLablel1.Name = "sliderLablel1";
            this.sliderLablel1.Size = new System.Drawing.Size(15, 13);
            this.sliderLablel1.TabIndex = 6;
            this.sliderLablel1.Text = "R";
            // 
            // sliderLablel4
            // 
            this.sliderLablel4.AutoSize = true;
            this.sliderLablel4.Location = new System.Drawing.Point(1, 278);
            this.sliderLablel4.Name = "sliderLablel4";
            this.sliderLablel4.Size = new System.Drawing.Size(14, 13);
            this.sliderLablel4.TabIndex = 23;
            this.sliderLablel4.Text = "A";
            // 
            // LightingMaterialChoises
            // 
            this.LightingMaterialChoises.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LightingMaterialChoises.Controls.Add(this.LightPosition);
            this.LightingMaterialChoises.Controls.Add(this.sliderLablel4);
            this.LightingMaterialChoises.Controls.Add(this.LightSpecular);
            this.LightingMaterialChoises.Controls.Add(this.sliderLablel3);
            this.LightingMaterialChoises.Controls.Add(this.LightDiffuse);
            this.LightingMaterialChoises.Controls.Add(this.slider3);
            this.LightingMaterialChoises.Controls.Add(this.LightAmbient);
            this.LightingMaterialChoises.Controls.Add(this.slider1);
            this.LightingMaterialChoises.Controls.Add(this.MatShininess);
            this.LightingMaterialChoises.Controls.Add(this.slider4);
            this.LightingMaterialChoises.Controls.Add(this.MatSpecular);
            this.LightingMaterialChoises.Controls.Add(this.sliderLablel2);
            this.LightingMaterialChoises.Controls.Add(this.MatDiffuse);
            this.LightingMaterialChoises.Controls.Add(this.slider2);
            this.LightingMaterialChoises.Controls.Add(this.MatAmbient);
            this.LightingMaterialChoises.Controls.Add(this.sliderLablel1);
            this.LightingMaterialChoises.Location = new System.Drawing.Point(711, 16);
            this.LightingMaterialChoises.Name = "LightingMaterialChoises";
            this.LightingMaterialChoises.Size = new System.Drawing.Size(140, 322);
            this.LightingMaterialChoises.TabIndex = 24;
            this.LightingMaterialChoises.TabStop = false;
            this.LightingMaterialChoises.Text = "Lighting and Material";
            // 
            // LightPosition
            // 
            this.LightPosition.AutoSize = true;
            this.LightPosition.Location = new System.Drawing.Point(7, 181);
            this.LightPosition.Name = "LightPosition";
            this.LightPosition.Size = new System.Drawing.Size(85, 17);
            this.LightPosition.TabIndex = 7;
            this.LightPosition.Text = "LightPosition";
            this.LightPosition.UseVisualStyleBackColor = true;
            this.LightPosition.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // LightSpecular
            // 
            this.LightSpecular.AutoSize = true;
            this.LightSpecular.Location = new System.Drawing.Point(7, 158);
            this.LightSpecular.Name = "LightSpecular";
            this.LightSpecular.Size = new System.Drawing.Size(90, 17);
            this.LightSpecular.TabIndex = 6;
            this.LightSpecular.Text = "LightSpecular";
            this.LightSpecular.UseVisualStyleBackColor = true;
            this.LightSpecular.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // LightDiffuse
            // 
            this.LightDiffuse.AutoSize = true;
            this.LightDiffuse.Location = new System.Drawing.Point(7, 135);
            this.LightDiffuse.Name = "LightDiffuse";
            this.LightDiffuse.Size = new System.Drawing.Size(81, 17);
            this.LightDiffuse.TabIndex = 5;
            this.LightDiffuse.Text = "LightDiffuse";
            this.LightDiffuse.UseVisualStyleBackColor = true;
            this.LightDiffuse.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // LightAmbient
            // 
            this.LightAmbient.AutoSize = true;
            this.LightAmbient.Location = new System.Drawing.Point(7, 112);
            this.LightAmbient.Name = "LightAmbient";
            this.LightAmbient.Size = new System.Drawing.Size(86, 17);
            this.LightAmbient.TabIndex = 4;
            this.LightAmbient.Text = "LightAmbient";
            this.LightAmbient.UseVisualStyleBackColor = true;
            this.LightAmbient.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // MatShininess
            // 
            this.MatShininess.AutoSize = true;
            this.MatShininess.Location = new System.Drawing.Point(7, 89);
            this.MatShininess.Name = "MatShininess";
            this.MatShininess.Size = new System.Drawing.Size(88, 17);
            this.MatShininess.TabIndex = 3;
            this.MatShininess.Text = "MatShininess";
            this.MatShininess.UseVisualStyleBackColor = true;
            this.MatShininess.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // MatSpecular
            // 
            this.MatSpecular.AutoSize = true;
            this.MatSpecular.Location = new System.Drawing.Point(7, 66);
            this.MatSpecular.Name = "MatSpecular";
            this.MatSpecular.Size = new System.Drawing.Size(85, 17);
            this.MatSpecular.TabIndex = 2;
            this.MatSpecular.Text = "MatSpecular";
            this.MatSpecular.UseVisualStyleBackColor = true;
            this.MatSpecular.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // MatDiffuse
            // 
            this.MatDiffuse.AutoSize = true;
            this.MatDiffuse.Location = new System.Drawing.Point(7, 43);
            this.MatDiffuse.Name = "MatDiffuse";
            this.MatDiffuse.Size = new System.Drawing.Size(76, 17);
            this.MatDiffuse.TabIndex = 1;
            this.MatDiffuse.Text = "MatDiffuse";
            this.MatDiffuse.UseVisualStyleBackColor = true;
            this.MatDiffuse.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // MatAmbient
            // 
            this.MatAmbient.AutoSize = true;
            this.MatAmbient.Checked = true;
            this.MatAmbient.Location = new System.Drawing.Point(7, 20);
            this.MatAmbient.Name = "MatAmbient";
            this.MatAmbient.Size = new System.Drawing.Size(81, 17);
            this.MatAmbient.TabIndex = 0;
            this.MatAmbient.TabStop = true;
            this.MatAmbient.Text = "MatAmbient";
            this.MatAmbient.UseVisualStyleBackColor = true;
            this.MatAmbient.CheckedChanged += new System.EventHandler(this.lightOrMaterialRaddioChange);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 528);
            this.Controls.Add(this.LightingMaterialChoises);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.hScrollBar9);
            this.Controls.Add(this.hScrollBar8);
            this.Controls.Add(this.hScrollBar7);
            this.Controls.Add(this.hScrollBar6);
            this.Controls.Add(this.hScrollBar5);
            this.Controls.Add(this.hScrollBar4);
            this.Controls.Add(this.hScrollBar3);
            this.Controls.Add(this.hScrollBar2);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(879, 567);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            this.LightingMaterialChoises.ResumeLayout(false);
            this.LightingMaterialChoises.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.HScrollBar hScrollBar3;
        private System.Windows.Forms.HScrollBar hScrollBar4;
        private System.Windows.Forms.HScrollBar hScrollBar5;
        private System.Windows.Forms.HScrollBar hScrollBar6;
        private System.Windows.Forms.HScrollBar hScrollBar9;
        private System.Windows.Forms.HScrollBar hScrollBar8;
        private System.Windows.Forms.HScrollBar hScrollBar7;
        private System.Windows.Forms.HScrollBar slider1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox listBox1;
        private TextBox textBox1;
        private HScrollBar slider2;
        private HScrollBar slider4;
        private HScrollBar slider3;
        private Label sliderLablel3;
        private Label sliderLablel2;
        private Label sliderLablel1;
        private Label sliderLablel4;
        private GroupBox LightingMaterialChoises;
        private RadioButton MatAmbient;
        private RadioButton LightPosition;
        private RadioButton LightSpecular;
        private RadioButton LightDiffuse;
        private RadioButton LightAmbient;
        private RadioButton MatShininess;
        private RadioButton MatSpecular;
        private RadioButton MatDiffuse;
    }
}

