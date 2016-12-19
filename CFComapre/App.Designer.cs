namespace CFComapre
{
    partial class App
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem_Options = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_CompareRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_CompareHighlight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Go1_BTN = new System.Windows.Forms.Button();
            this.templateOrStack1_TB = new System.Windows.Forms.TextBox();
            this.templateOrStack1_LB = new System.Windows.Forms.Label();
            this.validation1_LB = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.profile1_LB = new System.Windows.Forms.Label();
            this.source1_LB = new System.Windows.Forms.Label();
            this.profile1_CB = new System.Windows.Forms.ComboBox();
            this.source1_CB = new System.Windows.Forms.ComboBox();
            this.Go2_BTN = new System.Windows.Forms.Button();
            this.templateOrStack2_TB = new System.Windows.Forms.TextBox();
            this.templateOrStack2_LB = new System.Windows.Forms.Label();
            this.validation2_LB = new System.Windows.Forms.Label();
            this.profile2_LB = new System.Windows.Forms.Label();
            this.source2_LB = new System.Windows.Forms.Label();
            this.profile2_CB = new System.Windows.Forms.ComboBox();
            this.source2_CB = new System.Windows.Forms.ComboBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.compare_BTN = new System.Windows.Forms.ToolStripButton();
            this.SwitchView_BTN = new System.Windows.Forms.ToolStripButton();
            this.validateTemplate_CB = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(18, 18);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Options,
            this.toolStripMenuItem_Help});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1384, 28);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem_Options
            // 
            this.toolStripMenuItem_Options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_CompareRemove,
            this.toolStripMenuItem_CompareHighlight});
            this.toolStripMenuItem_Options.Name = "toolStripMenuItem_Options";
            this.toolStripMenuItem_Options.Size = new System.Drawing.Size(73, 24);
            this.toolStripMenuItem_Options.Text = "Options";
            // 
            // toolStripMenuItem_CompareRemove
            // 
            this.toolStripMenuItem_CompareRemove.Checked = true;
            this.toolStripMenuItem_CompareRemove.CheckOnClick = true;
            this.toolStripMenuItem_CompareRemove.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem_CompareRemove.Name = "toolStripMenuItem_CompareRemove";
            this.toolStripMenuItem_CompareRemove.Size = new System.Drawing.Size(237, 24);
            this.toolStripMenuItem_CompareRemove.Text = "Remove on Comparison";
            this.toolStripMenuItem_CompareRemove.ToolTipText = "Removes entries that are the same on both sides during the compare operation.";
            this.toolStripMenuItem_CompareRemove.Click += new System.EventHandler(this.toolStripMenuItem_CompareRemove_Click);
            // 
            // toolStripMenuItem_CompareHighlight
            // 
            this.toolStripMenuItem_CompareHighlight.CheckOnClick = true;
            this.toolStripMenuItem_CompareHighlight.Name = "toolStripMenuItem_CompareHighlight";
            this.toolStripMenuItem_CompareHighlight.Size = new System.Drawing.Size(237, 24);
            this.toolStripMenuItem_CompareHighlight.Text = "Highlight on Compare";
            this.toolStripMenuItem_CompareHighlight.ToolTipText = "Highlights entries that are the same on both sides during the compare operation.";
            this.toolStripMenuItem_CompareHighlight.Click += new System.EventHandler(this.toolStripMenuItem_CompareHighlight_Click);
            // 
            // toolStripMenuItem_Help
            // 
            this.toolStripMenuItem_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.versionInfoToolStripMenuItem});
            this.toolStripMenuItem_Help.ForeColor = System.Drawing.SystemColors.ControlText;
            this.toolStripMenuItem_Help.Name = "toolStripMenuItem_Help";
            this.toolStripMenuItem_Help.Size = new System.Drawing.Size(53, 24);
            this.toolStripMenuItem_Help.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // versionInfoToolStripMenuItem
            // 
            this.versionInfoToolStripMenuItem.Name = "versionInfoToolStripMenuItem";
            this.versionInfoToolStripMenuItem.Size = new System.Drawing.Size(157, 24);
            this.versionInfoToolStripMenuItem.Text = "Version Info";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 58);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.Go1_BTN);
            this.splitContainer1.Panel1.Controls.Add(this.templateOrStack1_TB);
            this.splitContainer1.Panel1.Controls.Add(this.templateOrStack1_LB);
            this.splitContainer1.Panel1.Controls.Add(this.validation1_LB);
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
            this.splitContainer1.Panel1.Controls.Add(this.profile1_LB);
            this.splitContainer1.Panel1.Controls.Add(this.source1_LB);
            this.splitContainer1.Panel1.Controls.Add(this.profile1_CB);
            this.splitContainer1.Panel1.Controls.Add(this.source1_CB);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Go2_BTN);
            this.splitContainer1.Panel2.Controls.Add(this.templateOrStack2_TB);
            this.splitContainer1.Panel2.Controls.Add(this.templateOrStack2_LB);
            this.splitContainer1.Panel2.Controls.Add(this.validation2_LB);
            this.splitContainer1.Panel2.Controls.Add(this.profile2_LB);
            this.splitContainer1.Panel2.Controls.Add(this.source2_LB);
            this.splitContainer1.Panel2.Controls.Add(this.profile2_CB);
            this.splitContainer1.Panel2.Controls.Add(this.source2_CB);
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox2);
            this.splitContainer1.Size = new System.Drawing.Size(1384, 674);
            this.splitContainer1.SplitterDistance = 701;
            this.splitContainer1.TabIndex = 6;
            // 
            // Go1_BTN
            // 
            this.Go1_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Go1_BTN.Location = new System.Drawing.Point(611, 6);
            this.Go1_BTN.Name = "Go1_BTN";
            this.Go1_BTN.Size = new System.Drawing.Size(75, 23);
            this.Go1_BTN.TabIndex = 8;
            this.Go1_BTN.Text = "Go";
            this.Go1_BTN.UseVisualStyleBackColor = true;
            this.Go1_BTN.Click += new System.EventHandler(this.Go1_BTN_Click);
            // 
            // templateOrStack1_TB
            // 
            this.templateOrStack1_TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templateOrStack1_TB.Location = new System.Drawing.Point(90, 65);
            this.templateOrStack1_TB.Name = "templateOrStack1_TB";
            this.templateOrStack1_TB.Size = new System.Drawing.Size(596, 22);
            this.templateOrStack1_TB.TabIndex = 7;
            // 
            // templateOrStack1_LB
            // 
            this.templateOrStack1_LB.AutoSize = true;
            this.templateOrStack1_LB.Location = new System.Drawing.Point(12, 65);
            this.templateOrStack1_LB.Name = "templateOrStack1_LB";
            this.templateOrStack1_LB.Size = new System.Drawing.Size(71, 17);
            this.templateOrStack1_LB.TabIndex = 6;
            this.templateOrStack1_LB.Text = "Template:";
            // 
            // validation1_LB
            // 
            this.validation1_LB.AutoSize = true;
            this.validation1_LB.Location = new System.Drawing.Point(260, 9);
            this.validation1_LB.Name = "validation1_LB";
            this.validation1_LB.Size = new System.Drawing.Size(59, 17);
            this.validation1_LB.TabIndex = 5;
            this.validation1_LB.Text = "Validate";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(15, 101);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(671, 562);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // profile1_LB
            // 
            this.profile1_LB.AutoSize = true;
            this.profile1_LB.Location = new System.Drawing.Point(12, 37);
            this.profile1_LB.Name = "profile1_LB";
            this.profile1_LB.Size = new System.Drawing.Size(52, 17);
            this.profile1_LB.TabIndex = 3;
            this.profile1_LB.Text = "Profile:";
            // 
            // source1_LB
            // 
            this.source1_LB.AutoSize = true;
            this.source1_LB.Location = new System.Drawing.Point(12, 9);
            this.source1_LB.Name = "source1_LB";
            this.source1_LB.Size = new System.Drawing.Size(57, 17);
            this.source1_LB.TabIndex = 2;
            this.source1_LB.Text = "Source:";
            // 
            // profile1_CB
            // 
            this.profile1_CB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profile1_CB.FormattingEnabled = true;
            this.profile1_CB.Location = new System.Drawing.Point(90, 34);
            this.profile1_CB.Name = "profile1_CB";
            this.profile1_CB.Size = new System.Drawing.Size(288, 24);
            this.profile1_CB.TabIndex = 1;
            this.profile1_CB.Text = "select";
            // 
            // source1_CB
            // 
            this.source1_CB.FormattingEnabled = true;
            this.source1_CB.Items.AddRange(new object[] {
            "AWS",
            "Template"});
            this.source1_CB.Location = new System.Drawing.Point(90, 6);
            this.source1_CB.Name = "source1_CB";
            this.source1_CB.Size = new System.Drawing.Size(153, 24);
            this.source1_CB.TabIndex = 0;
            this.source1_CB.Text = "select";
            this.source1_CB.SelectedIndexChanged += new System.EventHandler(this.source1_CB_SelectedIndexChanged);
            // 
            // Go2_BTN
            // 
            this.Go2_BTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Go2_BTN.Location = new System.Drawing.Point(592, 6);
            this.Go2_BTN.Name = "Go2_BTN";
            this.Go2_BTN.Size = new System.Drawing.Size(75, 23);
            this.Go2_BTN.TabIndex = 15;
            this.Go2_BTN.Text = "Go";
            this.Go2_BTN.UseVisualStyleBackColor = true;
            this.Go2_BTN.Click += new System.EventHandler(this.Go2_BTN_Click);
            // 
            // templateOrStack2_TB
            // 
            this.templateOrStack2_TB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.templateOrStack2_TB.Location = new System.Drawing.Point(92, 65);
            this.templateOrStack2_TB.Name = "templateOrStack2_TB";
            this.templateOrStack2_TB.Size = new System.Drawing.Size(575, 22);
            this.templateOrStack2_TB.TabIndex = 14;
            // 
            // templateOrStack2_LB
            // 
            this.templateOrStack2_LB.AutoSize = true;
            this.templateOrStack2_LB.Location = new System.Drawing.Point(14, 65);
            this.templateOrStack2_LB.Name = "templateOrStack2_LB";
            this.templateOrStack2_LB.Size = new System.Drawing.Size(71, 17);
            this.templateOrStack2_LB.TabIndex = 13;
            this.templateOrStack2_LB.Text = "Template:";
            // 
            // validation2_LB
            // 
            this.validation2_LB.AutoSize = true;
            this.validation2_LB.Location = new System.Drawing.Point(262, 9);
            this.validation2_LB.Name = "validation2_LB";
            this.validation2_LB.Size = new System.Drawing.Size(59, 17);
            this.validation2_LB.TabIndex = 12;
            this.validation2_LB.Text = "Validate";
            // 
            // profile2_LB
            // 
            this.profile2_LB.AutoSize = true;
            this.profile2_LB.Location = new System.Drawing.Point(14, 37);
            this.profile2_LB.Name = "profile2_LB";
            this.profile2_LB.Size = new System.Drawing.Size(52, 17);
            this.profile2_LB.TabIndex = 11;
            this.profile2_LB.Text = "Profile:";
            // 
            // source2_LB
            // 
            this.source2_LB.AutoSize = true;
            this.source2_LB.Location = new System.Drawing.Point(14, 9);
            this.source2_LB.Name = "source2_LB";
            this.source2_LB.Size = new System.Drawing.Size(57, 17);
            this.source2_LB.TabIndex = 10;
            this.source2_LB.Text = "Source:";
            // 
            // profile2_CB
            // 
            this.profile2_CB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profile2_CB.FormattingEnabled = true;
            this.profile2_CB.Location = new System.Drawing.Point(92, 34);
            this.profile2_CB.Name = "profile2_CB";
            this.profile2_CB.Size = new System.Drawing.Size(288, 24);
            this.profile2_CB.TabIndex = 9;
            this.profile2_CB.Text = "select";
            // 
            // source2_CB
            // 
            this.source2_CB.FormattingEnabled = true;
            this.source2_CB.Items.AddRange(new object[] {
            "AWS",
            "Template"});
            this.source2_CB.Location = new System.Drawing.Point(92, 6);
            this.source2_CB.Name = "source2_CB";
            this.source2_CB.Size = new System.Drawing.Size(153, 24);
            this.source2_CB.TabIndex = 8;
            this.source2_CB.Text = "select";
            this.source2_CB.SelectedIndexChanged += new System.EventHandler(this.source2_CB_SelectedIndexChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.Location = new System.Drawing.Point(17, 100);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(650, 562);
            this.richTextBox2.TabIndex = 5;
            this.richTextBox2.Text = "";
            this.richTextBox2.WordWrap = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compare_BTN,
            this.SwitchView_BTN});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1384, 27);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // compare_BTN
            // 
            this.compare_BTN.Image = ((System.Drawing.Image)(resources.GetObject("compare_BTN.Image")));
            this.compare_BTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.compare_BTN.Name = "compare_BTN";
            this.compare_BTN.Size = new System.Drawing.Size(90, 24);
            this.compare_BTN.Text = "Compare";
            this.compare_BTN.ToolTipText = "Compares the two stacks.";
            this.compare_BTN.Click += new System.EventHandler(this.compare_BTN_Click_1);
            // 
            // SwitchView_BTN
            // 
            this.SwitchView_BTN.Image = ((System.Drawing.Image)(resources.GetObject("SwitchView_BTN.Image")));
            this.SwitchView_BTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SwitchView_BTN.Name = "SwitchView_BTN";
            this.SwitchView_BTN.Size = new System.Drawing.Size(108, 24);
            this.SwitchView_BTN.Text = "Switch View";
            this.SwitchView_BTN.ToolTipText = "Switches between compared and non-compared view.";
            this.SwitchView_BTN.Click += new System.EventHandler(this.SwitchView_BTN_Click);
            // 
            // validateTemplate_CB
            // 
            this.validateTemplate_CB.AutoSize = true;
            this.validateTemplate_CB.Location = new System.Drawing.Point(217, 31);
            this.validateTemplate_CB.Name = "validateTemplate_CB";
            this.validateTemplate_CB.Size = new System.Drawing.Size(161, 21);
            this.validateTemplate_CB.TabIndex = 8;
            this.validateTemplate_CB.Text = "Validate Template(s)";
            this.validateTemplate_CB.UseVisualStyleBackColor = true;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 732);
            this.Controls.Add(this.validateTemplate_CB);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "App";
            this.Text = "CFCompare";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Help;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionInfoToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox profile1_CB;
        private System.Windows.Forms.ComboBox source1_CB;
        private System.Windows.Forms.Label profile1_LB;
        private System.Windows.Forms.Label source1_LB;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Label validation1_LB;
        private System.Windows.Forms.Label templateOrStack1_LB;
        private System.Windows.Forms.TextBox templateOrStack1_TB;
        private System.Windows.Forms.CheckBox validateTemplate_CB;
        private System.Windows.Forms.TextBox templateOrStack2_TB;
        private System.Windows.Forms.Label templateOrStack2_LB;
        private System.Windows.Forms.Label validation2_LB;
        private System.Windows.Forms.Label profile2_LB;
        private System.Windows.Forms.Label source2_LB;
        private System.Windows.Forms.ComboBox profile2_CB;
        private System.Windows.Forms.ComboBox source2_CB;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button Go1_BTN;
        private System.Windows.Forms.Button Go2_BTN;
        private System.Windows.Forms.ToolStripButton compare_BTN;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Options;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_CompareRemove;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_CompareHighlight;
        private System.Windows.Forms.ToolStripButton SwitchView_BTN;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

