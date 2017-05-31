namespace Test
{
    partial class FrmTest
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
            this.GeneratePDFButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.GetStatusButton = new System.Windows.Forms.Button();
            this.IDTextBox = new System.Windows.Forms.TextBox();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.PdfLocationButton = new System.Windows.Forms.Button();
            this.PdfLocationLink = new System.Windows.Forms.LinkLabel();
            this.GeneratePDFExistFileButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.WebServiceRadio = new System.Windows.Forms.RadioButton();
            this.LibraryRadio = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Sample4Radio = new System.Windows.Forms.RadioButton();
            this.Sample3Radio = new System.Windows.Forms.RadioButton();
            this.Sample2Radio = new System.Windows.Forms.RadioButton();
            this.Sample1Radio = new System.Windows.Forms.RadioButton();
            this.GenerateErrorButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GeneratePDFButton
            // 
            this.GeneratePDFButton.Location = new System.Drawing.Point(493, 65);
            this.GeneratePDFButton.Name = "GeneratePDFButton";
            this.GeneratePDFButton.Size = new System.Drawing.Size(88, 23);
            this.GeneratePDFButton.TabIndex = 0;
            this.GeneratePDFButton.Text = "Gen -> Content";
            this.GeneratePDFButton.UseVisualStyleBackColor = true;
            this.GeneratePDFButton.Click += new System.EventHandler(this.GeneratePDFButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(493, 7);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(88, 23);
            this.ClearButton.TabIndex = 1;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // GetStatusButton
            // 
            this.GetStatusButton.Location = new System.Drawing.Point(493, 123);
            this.GetStatusButton.Name = "GetStatusButton";
            this.GetStatusButton.Size = new System.Drawing.Size(88, 23);
            this.GetStatusButton.TabIndex = 2;
            this.GetStatusButton.Text = "Get Status";
            this.GetStatusButton.UseVisualStyleBackColor = true;
            this.GetStatusButton.Click += new System.EventHandler(this.GetStatusButton_Click);
            // 
            // IDTextBox
            // 
            this.IDTextBox.Location = new System.Drawing.Point(12, 61);
            this.IDTextBox.Name = "IDTextBox";
            this.IDTextBox.Size = new System.Drawing.Size(475, 20);
            this.IDTextBox.TabIndex = 3;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(9, 93);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.StatusLabel.TabIndex = 4;
            this.StatusLabel.Text = "Status";
            // 
            // PdfLocationButton
            // 
            this.PdfLocationButton.Location = new System.Drawing.Point(493, 152);
            this.PdfLocationButton.Name = "PdfLocationButton";
            this.PdfLocationButton.Size = new System.Drawing.Size(88, 23);
            this.PdfLocationButton.TabIndex = 5;
            this.PdfLocationButton.Text = "Location";
            this.PdfLocationButton.UseVisualStyleBackColor = true;
            this.PdfLocationButton.Click += new System.EventHandler(this.PdfLocationButton_Click);
            // 
            // PdfLocationLink
            // 
            this.PdfLocationLink.AutoSize = true;
            this.PdfLocationLink.Location = new System.Drawing.Point(9, 122);
            this.PdfLocationLink.Name = "PdfLocationLink";
            this.PdfLocationLink.Size = new System.Drawing.Size(55, 13);
            this.PdfLocationLink.TabIndex = 9;
            this.PdfLocationLink.TabStop = true;
            this.PdfLocationLink.Text = "linkLabel1";
            this.PdfLocationLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PdfLocationLink_LinkClicked);
            // 
            // GeneratePDFExistFileButton
            // 
            this.GeneratePDFExistFileButton.Location = new System.Drawing.Point(493, 94);
            this.GeneratePDFExistFileButton.Name = "GeneratePDFExistFileButton";
            this.GeneratePDFExistFileButton.Size = new System.Drawing.Size(88, 23);
            this.GeneratePDFExistFileButton.TabIndex = 10;
            this.GeneratePDFExistFileButton.Text = "Gen -> File";
            this.GeneratePDFExistFileButton.UseVisualStyleBackColor = true;
            this.GeneratePDFExistFileButton.Click += new System.EventHandler(this.GeneratePDFExistFileButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.WebServiceRadio);
            this.groupBox1.Controls.Add(this.LibraryRadio);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 43);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // WebServiceRadio
            // 
            this.WebServiceRadio.AutoSize = true;
            this.WebServiceRadio.Location = new System.Drawing.Point(77, 19);
            this.WebServiceRadio.Name = "WebServiceRadio";
            this.WebServiceRadio.Size = new System.Drawing.Size(87, 17);
            this.WebServiceRadio.TabIndex = 10;
            this.WebServiceRadio.Text = "Web Service";
            this.WebServiceRadio.UseVisualStyleBackColor = true;
            // 
            // LibraryRadio
            // 
            this.LibraryRadio.AutoSize = true;
            this.LibraryRadio.Checked = true;
            this.LibraryRadio.Location = new System.Drawing.Point(15, 19);
            this.LibraryRadio.Name = "LibraryRadio";
            this.LibraryRadio.Size = new System.Drawing.Size(56, 17);
            this.LibraryRadio.TabIndex = 9;
            this.LibraryRadio.TabStop = true;
            this.LibraryRadio.Text = "Library";
            this.LibraryRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Sample4Radio);
            this.groupBox2.Controls.Add(this.Sample3Radio);
            this.groupBox2.Controls.Add(this.Sample2Radio);
            this.groupBox2.Controls.Add(this.Sample1Radio);
            this.groupBox2.Location = new System.Drawing.Point(192, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 43);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sample File";
            // 
            // Sample4Radio
            // 
            this.Sample4Radio.AutoSize = true;
            this.Sample4Radio.Location = new System.Drawing.Point(223, 19);
            this.Sample4Radio.Name = "Sample4Radio";
            this.Sample4Radio.Size = new System.Drawing.Size(66, 17);
            this.Sample4Radio.TabIndex = 3;
            this.Sample4Radio.TabStop = true;
            this.Sample4Radio.Text = "Sample4";
            this.Sample4Radio.UseVisualStyleBackColor = true;
            this.Sample4Radio.Click += new System.EventHandler(this.Sample4Radio_Click);
            // 
            // Sample3Radio
            // 
            this.Sample3Radio.AutoSize = true;
            this.Sample3Radio.Location = new System.Drawing.Point(151, 19);
            this.Sample3Radio.Name = "Sample3Radio";
            this.Sample3Radio.Size = new System.Drawing.Size(66, 17);
            this.Sample3Radio.TabIndex = 2;
            this.Sample3Radio.TabStop = true;
            this.Sample3Radio.Text = "Sample3";
            this.Sample3Radio.UseVisualStyleBackColor = true;
            this.Sample3Radio.Click += new System.EventHandler(this.Sample3Radio_Click);
            // 
            // Sample2Radio
            // 
            this.Sample2Radio.AutoSize = true;
            this.Sample2Radio.Location = new System.Drawing.Point(79, 19);
            this.Sample2Radio.Name = "Sample2Radio";
            this.Sample2Radio.Size = new System.Drawing.Size(66, 17);
            this.Sample2Radio.TabIndex = 1;
            this.Sample2Radio.TabStop = true;
            this.Sample2Radio.Text = "Sample2";
            this.Sample2Radio.UseVisualStyleBackColor = true;
            this.Sample2Radio.Click += new System.EventHandler(this.Sample2Radio_Click);
            // 
            // Sample1Radio
            // 
            this.Sample1Radio.AutoSize = true;
            this.Sample1Radio.Checked = true;
            this.Sample1Radio.Location = new System.Drawing.Point(7, 19);
            this.Sample1Radio.Name = "Sample1Radio";
            this.Sample1Radio.Size = new System.Drawing.Size(66, 17);
            this.Sample1Radio.TabIndex = 0;
            this.Sample1Radio.TabStop = true;
            this.Sample1Radio.Text = "Sample1";
            this.Sample1Radio.UseVisualStyleBackColor = true;
            this.Sample1Radio.Click += new System.EventHandler(this.Sample1Radio_Click);
            // 
            // GenerateErrorButton
            // 
            this.GenerateErrorButton.Location = new System.Drawing.Point(493, 36);
            this.GenerateErrorButton.Name = "GenerateErrorButton";
            this.GenerateErrorButton.Size = new System.Drawing.Size(88, 23);
            this.GenerateErrorButton.TabIndex = 13;
            this.GenerateErrorButton.Text = "Gen - > Error";
            this.GenerateErrorButton.UseVisualStyleBackColor = true;
            this.GenerateErrorButton.Click += new System.EventHandler(this.GenerateErrorButton_Click);
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 182);
            this.Controls.Add(this.GenerateErrorButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.GeneratePDFExistFileButton);
            this.Controls.Add(this.PdfLocationLink);
            this.Controls.Add(this.PdfLocationButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.IDTextBox);
            this.Controls.Add(this.GetStatusButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.GeneratePDFButton);
            this.Name = "FrmTest";
            this.Text = "Test PDF Creator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GeneratePDFButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button GetStatusButton;
        private System.Windows.Forms.TextBox IDTextBox;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button PdfLocationButton;
        private System.Windows.Forms.LinkLabel PdfLocationLink;
        private System.Windows.Forms.Button GeneratePDFExistFileButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton WebServiceRadio;
        private System.Windows.Forms.RadioButton LibraryRadio;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton Sample4Radio;
        private System.Windows.Forms.RadioButton Sample3Radio;
        private System.Windows.Forms.RadioButton Sample2Radio;
        private System.Windows.Forms.RadioButton Sample1Radio;
        private System.Windows.Forms.Button GenerateErrorButton;
    }
}

