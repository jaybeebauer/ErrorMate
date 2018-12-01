namespace ErrorMate
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dlg_uploadError = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btn_captureError = new System.Windows.Forms.ToolStripButton();
            this.btn_imageError = new System.Windows.Forms.ToolStripButton();
            this.btn_settings = new System.Windows.Forms.ToolStripButton();
            this.lbl_Results = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dlg_uploadError
            // 
            this.dlg_uploadError.Title = "Select captured error image";
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(75, 75);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_captureError,
            this.btn_imageError,
            this.btn_settings});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip.Size = new System.Drawing.Size(621, 82);
            this.toolStrip.TabIndex = 3;
            // 
            // btn_captureError
            // 
            this.btn_captureError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_captureError.Image = ((System.Drawing.Image)(resources.GetObject("btn_captureError.Image")));
            this.btn_captureError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_captureError.Name = "btn_captureError";
            this.btn_captureError.Padding = new System.Windows.Forms.Padding(20, 30, 20, 30);
            this.btn_captureError.Size = new System.Drawing.Size(121, 79);
            this.btn_captureError.Text = "Capture Error";
            this.btn_captureError.Click += new System.EventHandler(this.btn_captureError_Click);
            // 
            // btn_imageError
            // 
            this.btn_imageError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_imageError.Image = ((System.Drawing.Image)(resources.GetObject("btn_imageError.Image")));
            this.btn_imageError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_imageError.Name = "btn_imageError";
            this.btn_imageError.Padding = new System.Windows.Forms.Padding(20, 30, 20, 30);
            this.btn_imageError.Size = new System.Drawing.Size(117, 79);
            this.btn_imageError.Text = "Browse Error";
            this.btn_imageError.Click += new System.EventHandler(this.btn_imageError_Click);
            // 
            // btn_settings
            // 
            this.btn_settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_settings.Image = ((System.Drawing.Image)(resources.GetObject("btn_settings.Image")));
            this.btn_settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_settings.Name = "btn_settings";
            this.btn_settings.Padding = new System.Windows.Forms.Padding(20, 30, 20, 30);
            this.btn_settings.Size = new System.Drawing.Size(93, 79);
            this.btn_settings.Text = "Settings";
            this.btn_settings.Click += new System.EventHandler(this.btn_settings_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowNavigation = true;
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Location = new System.Drawing.Point(12, 85);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(597, 431);
            this.webBrowser1.TabIndex = 7;
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(webBrowser1_Navigating);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 528);
            this.Controls.Add(this.lbl_Results);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.webBrowser1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Error Mate";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog dlg_uploadError;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btn_captureError;
        private System.Windows.Forms.ToolStripButton btn_imageError;
        private System.Windows.Forms.ToolStripButton btn_settings;
        private System.Windows.Forms.Label lbl_Results;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}