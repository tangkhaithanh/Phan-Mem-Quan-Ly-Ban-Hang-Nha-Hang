namespace DoAnCuoiKiCSDL.Analyst
{
    partial class AnalystForm
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
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnPeakHour = new Guna.UI2.WinForms.Guna2Button();
            this.btnPerform = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.highlightPanel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.guna2Panel1.Controls.Add(this.highlightPanel2);
            this.guna2Panel1.Controls.Add(this.btnPerform);
            this.guna2Panel1.Controls.Add(this.btnPeakHour);
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1482, 45);
            this.guna2Panel1.TabIndex = 0;
            // 
            // btnPeakHour
            // 
            this.btnPeakHour.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPeakHour.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPeakHour.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPeakHour.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPeakHour.FillColor = System.Drawing.Color.Transparent;
            this.btnPeakHour.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPeakHour.ForeColor = System.Drawing.Color.Black;
            this.btnPeakHour.Location = new System.Drawing.Point(12, 3);
            this.btnPeakHour.Name = "btnPeakHour";
            this.btnPeakHour.Size = new System.Drawing.Size(137, 27);
            this.btnPeakHour.TabIndex = 23;
            this.btnPeakHour.Text = "Peak Hour";
            this.btnPeakHour.Click += new System.EventHandler(this.btnPeakHour_Click);
            // 
            // btnPerform
            // 
            this.btnPerform.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPerform.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPerform.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPerform.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPerform.FillColor = System.Drawing.Color.Transparent;
            this.btnPerform.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerform.ForeColor = System.Drawing.Color.Black;
            this.btnPerform.Location = new System.Drawing.Point(181, 3);
            this.btnPerform.Name = "btnPerform";
            this.btnPerform.Size = new System.Drawing.Size(261, 23);
            this.btnPerform.TabIndex = 24;
            this.btnPerform.Text = "Employee\'s Performance";
            this.btnPerform.Click += new System.EventHandler(this.btnPerform_Click);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.Location = new System.Drawing.Point(0, 76);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(1482, 788);
            this.guna2Panel2.TabIndex = 1;
            // 
            // highlightPanel2
            // 
            this.highlightPanel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.highlightPanel2.Location = new System.Drawing.Point(42, 32);
            this.highlightPanel2.Name = "highlightPanel2";
            this.highlightPanel2.Size = new System.Drawing.Size(85, 10);
            this.highlightPanel2.TabIndex = 25;
            this.highlightPanel2.Visible = false;
            // 
            // AnalystForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 876);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.Name = "AnalystForm";
            this.Text = "AnalystForm";
            this.Load += new System.EventHandler(this.AnalystForm_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnPeakHour;
        private Guna.UI2.WinForms.Guna2Button btnPerform;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Panel highlightPanel2;
    }
}