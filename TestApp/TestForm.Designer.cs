namespace MediaSync
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
			this.button8 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.button9 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.button10 = new System.Windows.Forms.Button();
			this.button11 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.AllowUserToDeleteRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Location = new System.Drawing.Point(12, 122);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(1116, 314);
			this.grid.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(6, 23);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(129, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Generate Test Data";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.GenerateTestData_Click);
			// 
			// btnTestInsert
			// 
			this.btnTestInsert.Location = new System.Drawing.Point(141, 23);
			this.btnTestInsert.Name = "btnTestInsert";
			this.btnTestInsert.Size = new System.Drawing.Size(129, 23);
			this.btnTestInsert.TabIndex = 2;
			this.btnTestInsert.Text = "Insert";
			this.btnTestInsert.UseVisualStyleBackColor = true;
			this.btnTestInsert.Click += new System.EventHandler(this.Insert_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(141, 52);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(129, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Update";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Update_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(6, 52);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(129, 23);
			this.button3.TabIndex = 4;
			this.button3.Text = "Test Code";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.TestCode_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(276, 22);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(129, 23);
			this.button4.TabIndex = 5;
			this.button4.Text = "Select";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.Select_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(6, 80);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(129, 23);
			this.button5.TabIndex = 6;
			this.button5.Text = "Test Expression";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.Expression_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(276, 52);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(129, 23);
			this.button6.TabIndex = 7;
			this.button6.Text = "Select No Cache";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.SelectNoCache_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(276, 81);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(129, 23);
			this.button7.TabIndex = 8;
			this.button7.Text = "Select Where";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.SelectWhere_Click);
			// 
			// txtWhere
			// 
			this.txtWhere.Location = new System.Drawing.Point(411, 83);
			this.txtWhere.Name = "txtWhere";
			this.txtWhere.Size = new System.Drawing.Size(137, 20);
			this.txtWhere.TabIndex = 9;
			this.txtWhere.Text = "Where clause...";
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(6, 19);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(129, 23);
			this.button8.TabIndex = 10;
			this.button8.Text = "Select";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += new System.EventHandler(this.DomainSelect_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.button9);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.txtWhere);
			this.groupBox1.Controls.Add(this.btnTestInsert);
			this.groupBox1.Controls.Add(this.button7);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button6);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.button5);
			this.groupBox1.Location = new System.Drawing.Point(12, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(554, 114);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Core";
			// 
			// button9
			// 
			this.button9.Location = new System.Drawing.Point(141, 80);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(129, 23);
			this.button9.TabIndex = 10;
			this.button9.Text = "Debug Assemblies";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += new System.EventHandler(this.DebugAssemblies_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.button11);
			this.groupBox2.Controls.Add(this.button10);
			this.groupBox2.Controls.Add(this.button8);
			this.groupBox2.Location = new System.Drawing.Point(572, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(292, 110);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Domain";
			// 
			// button10
			// 
			this.button10.Location = new System.Drawing.Point(141, 19);
			this.button10.Name = "button10";
			this.button10.Size = new System.Drawing.Size(129, 23);
			this.button10.TabIndex = 11;
			this.button10.Text = "Benchmark";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += new System.EventHandler(this.Benchmark_Click);
			// 
			// button11
			// 
			this.button11.Location = new System.Drawing.Point(6, 48);
			this.button11.Name = "button11";
			this.button11.Size = new System.Drawing.Size(129, 23);
			this.button11.TabIndex = 12;
			this.button11.Text = "Website Select";
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += new System.EventHandler(this.WebsiteSelect_Click);
			// 
			// TestForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1140, 448);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grid);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.Shown += new System.EventHandler(this.TestForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

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
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button button10;
		private System.Windows.Forms.Button button11;
	}
}