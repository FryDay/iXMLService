namespace Tester
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
            this.btnClTest = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnKillService = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClTest
            // 
            this.btnClTest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClTest.Location = new System.Drawing.Point(131, 102);
            this.btnClTest.Name = "btnClTest";
            this.btnClTest.Size = new System.Drawing.Size(75, 23);
            this.btnClTest.TabIndex = 2;
            this.btnClTest.Text = "Test CL";
            this.btnClTest.UseVisualStyleBackColor = true;
            this.btnClTest.Click += new System.EventHandler(this.btnClTest_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtOutput.Location = new System.Drawing.Point(0, 228);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(674, 262);
            this.txtOutput.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnClTest, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnKillService, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(674, 228);
            this.tableLayoutPanel2.TabIndex = 9;
            // 
            // btnKillService
            // 
            this.btnKillService.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnKillService.Location = new System.Drawing.Point(468, 102);
            this.btnKillService.Name = "btnKillService";
            this.btnKillService.Size = new System.Drawing.Size(75, 23);
            this.btnKillService.TabIndex = 3;
            this.btnKillService.Text = "Kill Service";
            this.btnKillService.UseVisualStyleBackColor = true;
            this.btnKillService.Click += new System.EventHandler(this.btnKillService_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 490);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.txtOutput);
            this.Name = "Form1";
            this.Text = "iXMLService Tester";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClTest;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnKillService;
    }
}

