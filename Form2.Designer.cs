namespace WindownformInstallerService
{
    partial class DetailService
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
            this.NameService = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Label();
            this.restart = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.Label();
            this.descriptionValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NameService
            // 
            this.NameService.AutoSize = true;
            this.NameService.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.NameService.Location = new System.Drawing.Point(12, 9);
            this.NameService.MaximumSize = new System.Drawing.Size(100, 40);
            this.NameService.Name = "NameService";
            this.NameService.Size = new System.Drawing.Size(93, 16);
            this.NameService.TabIndex = 0;
            this.NameService.Text = "Name Service";
            // 
            // start
            // 
            this.start.AutoSize = true;
            this.start.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start.Location = new System.Drawing.Point(13, 40);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(34, 16);
            this.start.TabIndex = 1;
            this.start.Text = "Start";
            this.start.Click += new System.EventHandler(this.Start);
            // 
            // restart
            // 
            this.restart.AutoSize = true;
            this.restart.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.restart.Location = new System.Drawing.Point(13, 65);
            this.restart.Name = "restart";
            this.restart.Size = new System.Drawing.Size(50, 16);
            this.restart.TabIndex = 2;
            this.restart.Text = "Restart";
            this.restart.Click += new System.EventHandler(this.Restart);
            // 
            // description
            // 
            this.description.AutoSize = true;
            this.description.Location = new System.Drawing.Point(13, 102);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(78, 16);
            this.description.TabIndex = 3;
            this.description.Text = "Description:";
            // 
            // descriptionValue
            // 
            this.descriptionValue.AutoSize = true;
            this.descriptionValue.Location = new System.Drawing.Point(13, 127);
            this.descriptionValue.MaximumSize = new System.Drawing.Size(170, 300);
            this.descriptionValue.Name = "descriptionValue";
            this.descriptionValue.Size = new System.Drawing.Size(0, 16);
            this.descriptionValue.TabIndex = 4;
            // 
            // DetailService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(170, 220);
            this.Controls.Add(this.descriptionValue);
            this.Controls.Add(this.description);
            this.Controls.Add(this.restart);
            this.Controls.Add(this.start);
            this.Controls.Add(this.NameService);
            this.Name = "DetailService";
            this.Load += new System.EventHandler(this.DetailService_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameService;
        private System.Windows.Forms.Label start;
        private System.Windows.Forms.Label restart;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label descriptionValue;
    }
}