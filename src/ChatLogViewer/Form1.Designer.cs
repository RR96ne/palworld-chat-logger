namespace ChatLogViewer
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.checkBoxPopSound = new System.Windows.Forms.CheckBox();
			this.checkBoxTopMost = new System.Windows.Forms.CheckBox();
			this.checkBoxScroll = new System.Windows.Forms.CheckBox();
			this.buttonClear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// webView
			// 
			this.webView.AllowExternalDrop = true;
			this.webView.CreationProperties = null;
			this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
			this.webView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webView.Location = new System.Drawing.Point(0, 0);
			this.webView.Name = "webView";
			this.webView.Size = new System.Drawing.Size(624, 399);
			this.webView.TabIndex = 0;
			this.webView.ZoomFactor = 1D;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.checkBoxPopSound);
			this.splitContainer1.Panel1.Controls.Add(this.checkBoxTopMost);
			this.splitContainer1.Panel1.Controls.Add(this.checkBoxScroll);
			this.splitContainer1.Panel1.Controls.Add(this.buttonClear);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.webView);
			this.splitContainer1.Size = new System.Drawing.Size(624, 441);
			this.splitContainer1.SplitterDistance = 40;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 1;
			// 
			// checkBoxPopSound
			// 
			this.checkBoxPopSound.AutoSize = true;
			this.checkBoxPopSound.Location = new System.Drawing.Point(270, 16);
			this.checkBoxPopSound.Name = "checkBoxPopSound";
			this.checkBoxPopSound.Size = new System.Drawing.Size(92, 16);
			this.checkBoxPopSound.TabIndex = 3;
			this.checkBoxPopSound.Text = "Poping_sound";
			this.checkBoxPopSound.UseVisualStyleBackColor = true;
			// 
			// checkBoxTopMost
			// 
			this.checkBoxTopMost.AutoSize = true;
			this.checkBoxTopMost.Location = new System.Drawing.Point(141, 16);
			this.checkBoxTopMost.Name = "checkBoxTopMost";
			this.checkBoxTopMost.Size = new System.Drawing.Size(123, 16);
			this.checkBoxTopMost.TabIndex = 2;
			this.checkBoxTopMost.Text = "Display_on_topmost";
			this.checkBoxTopMost.UseVisualStyleBackColor = true;
			this.checkBoxTopMost.CheckedChanged += new System.EventHandler(this.checkBoxTopMost_CheckedChanged);
			// 
			// checkBoxScroll
			// 
			this.checkBoxScroll.AutoSize = true;
			this.checkBoxScroll.Checked = true;
			this.checkBoxScroll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxScroll.Location = new System.Drawing.Point(12, 16);
			this.checkBoxScroll.Name = "checkBoxScroll";
			this.checkBoxScroll.Size = new System.Drawing.Size(123, 16);
			this.checkBoxScroll.TabIndex = 1;
			this.checkBoxScroll.Text = "Automatic_scrolling";
			this.checkBoxScroll.UseVisualStyleBackColor = true;
			this.checkBoxScroll.CheckedChanged += new System.EventHandler(this.checkBoxScroll_CheckedChanged);
			// 
			// buttonClear
			// 
			this.buttonClear.Enabled = false;
			this.buttonClear.Location = new System.Drawing.Point(399, 13);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(75, 23);
			this.buttonClear.TabIndex = 0;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 441);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.ShowIcon = false;
			this.Text = "PalWorld Chat Log Viewer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.CheckBox checkBoxScroll;
        private System.Windows.Forms.CheckBox checkBoxTopMost;
		private System.Windows.Forms.CheckBox checkBoxPopSound;
	}
}

