namespace JSONNewtSoft
{
    partial class frmShop
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
            this.btnShopping = new System.Windows.Forms.Button();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnShopping
            // 
            this.btnShopping.Location = new System.Drawing.Point(133, 144);
            this.btnShopping.Name = "btnShopping";
            this.btnShopping.Size = new System.Drawing.Size(182, 23);
            this.btnShopping.TabIndex = 16;
            this.btnShopping.Text = "Run Shopping Console";
            this.btnShopping.UseVisualStyleBackColor = true;
            this.btnShopping.Click += new System.EventHandler(this.btnShopping_Click);
            // 
            // txtAction
            // 
            this.txtAction.Location = new System.Drawing.Point(133, 118);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(100, 20);
            this.txtAction.TabIndex = 17;
            this.txtAction.Text = "a";
            // 
            // frmShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 311);
            this.Controls.Add(this.txtAction);
            this.Controls.Add(this.btnShopping);
            this.Name = "frmShop";
            this.Text = "frmShop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShopping;
        private System.Windows.Forms.TextBox txtAction;
    }
}