namespace MicroscopeApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.videoPictureBox = new System.Windows.Forms.PictureBox();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.cboDevices = new System.Windows.Forms.ComboBox();
            this.cmbResolution = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnNOK = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.txtcodebare = new System.Windows.Forms.TextBox();
            this.labelBarcode = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.trbBrightness = new System.Windows.Forms.TrackBar();
            this.chkGrayscale = new System.Windows.Forms.CheckBox();

            ((System.ComponentModel.ISupportInitialize)(this.videoPictureBox)).BeginInit();
            this.panelSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbBrightness)).BeginInit();
            this.SuspendLayout();

            //
            // videoPictureBox
            //
            this.videoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPictureBox.BackColor = System.Drawing.Color.Black;
            this.videoPictureBox.Location = new System.Drawing.Point(325, 12);
            this.videoPictureBox.Name = "videoPictureBox";
            this.videoPictureBox.Size = new System.Drawing.Size(1006, 727);
            this.videoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.videoPictureBox.TabIndex = 0;
            this.videoPictureBox.TabStop = false;

            //
            // panelSidebar
            //
            this.panelSidebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.panelSidebar.Controls.Add(this.btnClose);
            this.panelSidebar.Controls.Add(this.btnSettings);
            this.panelSidebar.Controls.Add(this.cboDevices);
            this.panelSidebar.Controls.Add(this.cmbResolution);
            this.panelSidebar.Controls.Add(this.label1);
            this.panelSidebar.Controls.Add(this.btnZoomOut);
            this.panelSidebar.Controls.Add(this.btnZoomIn);
            this.panelSidebar.Controls.Add(this.button3);
            this.panelSidebar.Controls.Add(this.btnNOK);
            this.panelSidebar.Controls.Add(this.btnOK);
            this.panelSidebar.Controls.Add(this.statusLabel);
            this.panelSidebar.Controls.Add(this.txtcodebare);
            this.panelSidebar.Controls.Add(this.labelBarcode);
            this.panelSidebar.Controls.Add(this.BtnStop);
            this.panelSidebar.Controls.Add(this.btnStart);
            this.panelSidebar.Location = new System.Drawing.Point(12, 12);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(307, 727);
            this.panelSidebar.TabIndex = 20;

            //
            // btnClose
            //
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(14, 665);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(279, 45);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Exit Application";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);

            //
            // btnSettings
            //
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(14, 614);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(279, 40);
            this.btnSettings.TabIndex = 23;
            this.btnSettings.Text = "Camera Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);

            //
            // cboDevices
            //
            this.cboDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevices.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboDevices.FormattingEnabled = true;
            this.cboDevices.Location = new System.Drawing.Point(14, 578);
            this.cboDevices.Name = "cboDevices";
            this.cboDevices.Size = new System.Drawing.Size(279, 25);
            this.cboDevices.TabIndex = 22;

            //
            // cmbResolution
            //
            this.cmbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResolution.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbResolution.FormattingEnabled = true;
            this.cmbResolution.Location = new System.Drawing.Point(14, 525);
            this.cmbResolution.Name = "cmbResolution";
            this.cmbResolution.Size = new System.Drawing.Size(279, 25);
            this.cmbResolution.TabIndex = 21;

            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 505);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Camera Resolution";

            //
            // btnZoomOut
            //
            this.btnZoomOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnZoomOut.ForeColor = System.Drawing.Color.White;
            this.btnZoomOut.Location = new System.Drawing.Point(155, 349);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(138, 45);
            this.btnZoomOut.TabIndex = 19;
            this.btnZoomOut.Text = "Zoom -";
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);

            //
            // btnZoomIn
            //
            this.btnZoomIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnZoomIn.ForeColor = System.Drawing.Color.White;
            this.btnZoomIn.Location = new System.Drawing.Point(14, 349);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(135, 45);
            this.btnZoomIn.TabIndex = 18;
            this.btnZoomIn.Text = "Zoom +";
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);

            //
            // button3
            //
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(14, 285);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(279, 50);
            this.button3.TabIndex = 15;
            this.button3.Text = "NEW / RESET SESSION";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);

            //
            // btnNOK
            //
            this.btnNOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnNOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNOK.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnNOK.ForeColor = System.Drawing.Color.White;
            this.btnNOK.Location = new System.Drawing.Point(155, 204);
            this.btnNOK.Name = "btnNOK";
            this.btnNOK.Size = new System.Drawing.Size(138, 75);
            this.btnNOK.TabIndex = 17;
            this.btnNOK.Text = "NOK";
            this.btnNOK.UseVisualStyleBackColor = false;
            this.btnNOK.Click += new System.EventHandler(this.btnNOK_Click);

            //
            // btnOK
            //
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(14, 204);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(135, 75);
            this.btnOK.TabIndex = 16;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

            //
            // statusLabel
            //
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.Location = new System.Drawing.Point(14, 407);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(279, 87);
            this.statusLabel.TabIndex = 10;
            this.statusLabel.Text = "System Ready. Scan Harness Barcode.";

            //
            // txtcodebare
            //
            this.txtcodebare.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtcodebare.Location = new System.Drawing.Point(14, 131);
            this.txtcodebare.Multiline = true;
            this.txtcodebare.Name = "txtcodebare";
            this.txtcodebare.Size = new System.Drawing.Size(279, 60);
            this.txtcodebare.TabIndex = 9;
            this.txtcodebare.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtcodebare_KeyDown);

            //
            // labelBarcode
            //
            this.labelBarcode.AutoSize = true;
            this.labelBarcode.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelBarcode.ForeColor = System.Drawing.Color.White;
            this.labelBarcode.Location = new System.Drawing.Point(14, 107);
            this.labelBarcode.Name = "labelBarcode";
            this.labelBarcode.Size = new System.Drawing.Size(133, 19);
            this.labelBarcode.TabIndex = 13;
            this.labelBarcode.Text = "Scan Barcode Here:";

            //
            // BtnStop
            //
            this.BtnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.BtnStop.ForeColor = System.Drawing.Color.White;
            this.BtnStop.Location = new System.Drawing.Point(155, 14);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(138, 50);
            this.BtnStop.TabIndex = 2;
            this.BtnStop.Text = "Stop Camera";
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.btnStop_Click);

            //
            // btnStart
            //
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(14, 14);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(135, 50);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start Camera";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1343, 751);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.videoPictureBox);
            this.Name = "Form1";
            this.Text = "Sews Morocco - Quality Inspection    By @[A-S]";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.videoPictureBox)).EndInit();
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbBrightness)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox videoPictureBox;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.TrackBar trbBrightness;
        private System.Windows.Forms.ComboBox cboDevices;
        private System.Windows.Forms.CheckBox chkGrayscale;
        private System.Windows.Forms.TextBox txtcodebare;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbResolution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnNOK;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Label labelBarcode;
    }
}

