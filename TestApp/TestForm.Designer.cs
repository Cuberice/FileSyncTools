﻿namespace MediaSync
{
	partial class TestForm
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
			this.grid = new System.Windows.Forms.DataGridView();
			this.button1 = new System.Windows.Forms.Button();
			this.btnTestInsert = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.txtWhere = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.AllowUserToDeleteRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Location = new System.Drawing.Point(12, 100);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(1116, 336);
			this.grid.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(13, 13);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(129, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Generate Test Data";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.GenerateTestData_Click);
			// 
			// btnTestInsert
			// 
			this.btnTestInsert.Location = new System.Drawing.Point(148, 13);
			this.btnTestInsert.Name = "btnTestInsert";
			this.btnTestInsert.Size = new System.Drawing.Size(129, 23);
			this.btnTestInsert.TabIndex = 2;
			this.btnTestInsert.Text = "Insert";
			this.btnTestInsert.UseVisualStyleBackColor = true;
			this.btnTestInsert.Click += new System.EventHandler(this.Insert_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(148, 42);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(129, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Update";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Update_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(13, 42);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(129, 23);
			this.button3.TabIndex = 4;
			this.button3.Text = "Test Code";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.TestCode_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(283, 12);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(129, 23);
			this.button4.TabIndex = 5;
			this.button4.Text = "Select";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.Select_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(13, 70);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(129, 23);
			this.button5.TabIndex = 6;
			this.button5.Text = "Test Expression";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.Expression_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(283, 42);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(129, 23);
			this.button6.TabIndex = 7;
			this.button6.Text = "Select Cache";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.SelectCache_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(283, 71);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(129, 23);
			this.button7.TabIndex = 8;
			this.button7.Text = "Select Where";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.SelectWhere_Click);
			// 
			// txtWhere
			// 
			this.txtWhere.Location = new System.Drawing.Point(148, 72);
			this.txtWhere.Name = "txtWhere";
			this.txtWhere.Size = new System.Drawing.Size(129, 20);
			this.txtWhere.TabIndex = 9;
			this.txtWhere.Text = "Where clause...";
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1140, 448);
			this.Controls.Add(this.txtWhere);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.btnTestInsert);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.grid);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.Shown += new System.EventHandler(this.TestForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnTestInsert;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.TextBox txtWhere;
	}
}