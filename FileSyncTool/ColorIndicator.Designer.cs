namespace FileSyncTool
{
	partial class ColorIndicator
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlColor = new System.Windows.Forms.Panel();
			this.lblText = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pnlError
			// 
			this.pnlColor.BackColor = System.Drawing.Color.Red;
			this.pnlColor.Location = new System.Drawing.Point(3, 2);
			this.pnlColor.Name = "pnlColor";
			this.pnlColor.Size = new System.Drawing.Size(14, 15);
			this.pnlColor.TabIndex = 14;
			// 
			// lblText
			// 
			this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblText.AutoSize = true;
			this.lblText.Location = new System.Drawing.Point(23, 3);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(35, 13);
			this.lblText.TabIndex = 15;
			this.lblText.Text = "label1";
			// 
			// ColorIndicator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblText);
			this.Controls.Add(this.pnlColor);
			this.Name = "ColorIndicator";
			this.Size = new System.Drawing.Size(98, 19);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlColor;
		private System.Windows.Forms.Label lblText;

	}
}
