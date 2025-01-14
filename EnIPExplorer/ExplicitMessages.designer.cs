﻿namespace EnIPExplorer
{
    partial class ExplicitMessages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplicitMessages));
            this.l_device = new System.Windows.Forms.Label();
            this.l_service = new System.Windows.Forms.Label();
            this.l_Class = new System.Windows.Forms.Label();
            this.l_instance = new System.Windows.Forms.Label();
            this.l_attribute = new System.Windows.Forms.Label();
            this.l_data = new System.Windows.Forms.Label();
            this.cb_service = new System.Windows.Forms.ComboBox();
            this.cb_class = new System.Windows.Forms.ComboBox();
            this.tb_data = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tb_send = new System.Windows.Forms.TextBox();
            this.tb_received = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cb_device = new System.Windows.Forms.ComboBox();
            this.b_refresh = new System.Windows.Forms.Button();
            this.tb_instance = new System.Windows.Forms.TextBox();
            this.tb_attribute = new System.Windows.Forms.TextBox();
            this.b_send = new System.Windows.Forms.Button();
            this.l_send = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.symbolicName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // l_device
            // 
            this.l_device.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_device.AutoSize = true;
            this.l_device.Location = new System.Drawing.Point(33, 13);
            this.l_device.Name = "l_device";
            this.l_device.Size = new System.Drawing.Size(47, 13);
            this.l_device.TabIndex = 99;
            this.l_device.Text = "Device: ";
            // 
            // l_service
            // 
            this.l_service.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_service.AutoSize = true;
            this.l_service.Location = new System.Drawing.Point(32, 41);
            this.l_service.Name = "l_service";
            this.l_service.Size = new System.Drawing.Size(49, 13);
            this.l_service.TabIndex = 99;
            this.l_service.Text = "Service: ";
            // 
            // l_Class
            // 
            this.l_Class.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_Class.AutoSize = true;
            this.l_Class.Location = new System.Drawing.Point(39, 66);
            this.l_Class.Name = "l_Class";
            this.l_Class.Size = new System.Drawing.Size(35, 13);
            this.l_Class.TabIndex = 99;
            this.l_Class.Text = "Class:";
            // 
            // l_instance
            // 
            this.l_instance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_instance.AutoSize = true;
            this.l_instance.Location = new System.Drawing.Point(31, 91);
            this.l_instance.Name = "l_instance";
            this.l_instance.Size = new System.Drawing.Size(51, 13);
            this.l_instance.TabIndex = 99;
            this.l_instance.Text = "Instance:";
            // 
            // l_attribute
            // 
            this.l_attribute.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_attribute.AutoSize = true;
            this.l_attribute.Location = new System.Drawing.Point(32, 116);
            this.l_attribute.Name = "l_attribute";
            this.l_attribute.Size = new System.Drawing.Size(49, 13);
            this.l_attribute.TabIndex = 99;
            this.l_attribute.Text = "Attribute:";
            // 
            // l_data
            // 
            this.l_data.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_data.AutoSize = true;
            this.l_data.Location = new System.Drawing.Point(20, 167);
            this.l_data.Name = "l_data";
            this.l_data.Size = new System.Drawing.Size(74, 13);
            this.l_data.TabIndex = 99;
            this.l_data.Text = "Symbolic Tag:";
            this.l_data.Click += new System.EventHandler(this.l_data_Click);
            // 
            // cb_service
            // 
            this.cb_service.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_service.FormattingEnabled = true;
            this.cb_service.Location = new System.Drawing.Point(112, 38);
            this.cb_service.Name = "cb_service";
            this.cb_service.Size = new System.Drawing.Size(703, 21);
            this.cb_service.TabIndex = 7;
            this.cb_service.Leave += new System.EventHandler(this.cb_service_Leave);
            // 
            // cb_class
            // 
            this.cb_class.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_class.FormattingEnabled = true;
            this.cb_class.Location = new System.Drawing.Point(112, 63);
            this.cb_class.Name = "cb_class";
            this.cb_class.Size = new System.Drawing.Size(703, 21);
            this.cb_class.TabIndex = 8;
            this.cb_class.SelectedIndexChanged += new System.EventHandler(this.cb_class_SelectedIndexChanged);
            this.cb_class.Leave += new System.EventHandler(this.cb_class_Leave);
            // 
            // tb_data
            // 
            this.tb_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_data.Location = new System.Drawing.Point(112, 138);
            this.tb_data.Name = "tb_data";
            this.tb_data.Size = new System.Drawing.Size(703, 20);
            this.tb_data.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.Controls.Add(this.symbolicName, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.l_device, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.l_service, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.l_Class, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.l_instance, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.l_attribute, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cb_service, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cb_class, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tb_data, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.b_send, 2, 11);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tb_instance, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.tb_attribute, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.tb_send, 2, 10);
            this.tableLayoutPanel1.Controls.Add(this.tb_received, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.l_send, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.l_data, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 2, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(823, 531);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tb_send
            // 
            this.tb_send.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tb_send.Location = new System.Drawing.Point(112, 420);
            this.tb_send.Multiline = true;
            this.tb_send.Name = "tb_send";
            this.tb_send.ReadOnly = true;
            this.tb_send.Size = new System.Drawing.Size(703, 76);
            this.tb_send.TabIndex = 13;
            // 
            // tb_received
            // 
            this.tb_received.Location = new System.Drawing.Point(112, 224);
            this.tb_received.Multiline = true;
            this.tb_received.Name = "tb_received";
            this.tb_received.ReadOnly = true;
            this.tb_received.Size = new System.Drawing.Size(703, 190);
            this.tb_received.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel2.Controls.Add(this.cb_device, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.b_refresh, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(112, 8);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(703, 24);
            this.tableLayoutPanel2.TabIndex = 101;
            // 
            // cb_device
            // 
            this.cb_device.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_device.FormattingEnabled = true;
            this.cb_device.Location = new System.Drawing.Point(0, 0);
            this.cb_device.Margin = new System.Windows.Forms.Padding(0);
            this.cb_device.Name = "cb_device";
            this.cb_device.Size = new System.Drawing.Size(651, 21);
            this.cb_device.TabIndex = 7;
            this.cb_device.Leave += new System.EventHandler(this.cb_device_Leave);
            // 
            // b_refresh
            // 
            this.b_refresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b_refresh.Location = new System.Drawing.Point(651, 0);
            this.b_refresh.Margin = new System.Windows.Forms.Padding(0);
            this.b_refresh.Name = "b_refresh";
            this.b_refresh.Size = new System.Drawing.Size(52, 24);
            this.b_refresh.TabIndex = 8;
            this.b_refresh.Text = "Refresh";
            this.b_refresh.UseVisualStyleBackColor = true;
            this.b_refresh.Click += new System.EventHandler(this.b_refresh_Click);
            // 
            // tb_instance
            // 
            this.tb_instance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_instance.Location = new System.Drawing.Point(112, 88);
            this.tb_instance.Name = "tb_instance";
            this.tb_instance.Size = new System.Drawing.Size(703, 20);
            this.tb_instance.TabIndex = 102;
            this.tb_instance.Leave += new System.EventHandler(this.tb_instance_Leave);
            // 
            // tb_attribute
            // 
            this.tb_attribute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_attribute.Location = new System.Drawing.Point(112, 113);
            this.tb_attribute.Name = "tb_attribute";
            this.tb_attribute.Size = new System.Drawing.Size(703, 20);
            this.tb_attribute.TabIndex = 103;
            this.tb_attribute.Leave += new System.EventHandler(this.tb_attribute_Leave);
            // 
            // b_send
            // 
            this.b_send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.b_send.Location = new System.Drawing.Point(109, 499);
            this.b_send.Margin = new System.Windows.Forms.Padding(0);
            this.b_send.Name = "b_send";
            this.b_send.Size = new System.Drawing.Size(709, 32);
            this.b_send.TabIndex = 14;
            this.b_send.Text = "Send";
            this.b_send.UseVisualStyleBackColor = true;
            this.b_send.Click += new System.EventHandler(this.b_send_Click);
            // 
            // l_send
            // 
            this.l_send.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l_send.AutoSize = true;
            this.l_send.Location = new System.Drawing.Point(11, 312);
            this.l_send.Name = "l_send";
            this.l_send.Size = new System.Drawing.Size(92, 13);
            this.l_send.TabIndex = 99;
            this.l_send.Text = "Send/    Receive:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1, 5);
            this.label1.TabIndex = 104;
            this.label1.Text = "Data:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 105;
            this.label2.Text = "Data:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(112, 163);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 106;
            this.checkBox1.Text = "enable";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // symbolicName
            // 
            this.symbolicName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.symbolicName.Enabled = false;
            this.symbolicName.Location = new System.Drawing.Point(112, 196);
            this.symbolicName.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.symbolicName.Name = "symbolicName";
            this.symbolicName.Size = new System.Drawing.Size(703, 20);
            this.symbolicName.TabIndex = 107;
            // 
            // ExplicitMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 531);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExplicitMessages";
            this.Text = "ExplicitMessages";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExplicitMessages_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label l_device;
        private System.Windows.Forms.Label l_service;
        private System.Windows.Forms.Label l_Class;
        private System.Windows.Forms.Label l_instance;
        private System.Windows.Forms.Label l_attribute;
        private System.Windows.Forms.Label l_data;
        private System.Windows.Forms.ComboBox cb_service;
        private System.Windows.Forms.ComboBox cb_class;
        private System.Windows.Forms.TextBox tb_data;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tb_received;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ComboBox cb_device;
        private System.Windows.Forms.Button b_refresh;
        private System.Windows.Forms.TextBox tb_instance;
        private System.Windows.Forms.TextBox tb_attribute;
        private System.Windows.Forms.TextBox tb_send;
        private System.Windows.Forms.Button b_send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_send;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox symbolicName;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}