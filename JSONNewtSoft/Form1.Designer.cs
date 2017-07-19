namespace JSONNewtSoft
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnToClass = new System.Windows.Forms.Button();
            this.btnCreateJson = new System.Windows.Forms.Button();
            this.lstFriends = new System.Windows.Forms.TextBox();
            this.txtfirstName = new System.Windows.Forms.TextBox();
            this.txtlastName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtJSON = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "JSON";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "First Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Email";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Phone";
            // 
            // btnToClass
            // 
            this.btnToClass.Location = new System.Drawing.Point(227, 242);
            this.btnToClass.Name = "btnToClass";
            this.btnToClass.Size = new System.Drawing.Size(107, 23);
            this.btnToClass.TabIndex = 5;
            this.btnToClass.Text = "DecodeJSON";
            this.btnToClass.UseVisualStyleBackColor = true;
            this.btnToClass.Click += new System.EventHandler(this.btnToClass_Click);
            // 
            // btnCreateJson
            // 
            this.btnCreateJson.Location = new System.Drawing.Point(444, 242);
            this.btnCreateJson.Name = "btnCreateJson";
            this.btnCreateJson.Size = new System.Drawing.Size(107, 23);
            this.btnCreateJson.TabIndex = 6;
            this.btnCreateJson.Text = "Create JSON";
            this.btnCreateJson.UseVisualStyleBackColor = true;
            // 
            // lstFriends
            // 
            this.lstFriends.Location = new System.Drawing.Point(359, 65);
            this.lstFriends.Multiline = true;
            this.lstFriends.Name = "lstFriends";
            this.lstFriends.Size = new System.Drawing.Size(380, 115);
            this.lstFriends.TabIndex = 7;
            // 
            // txtfirstName
            // 
            this.txtfirstName.Location = new System.Drawing.Point(92, 65);
            this.txtfirstName.Name = "txtfirstName";
            this.txtfirstName.Size = new System.Drawing.Size(242, 20);
            this.txtfirstName.TabIndex = 8;
            // 
            // txtlastName
            // 
            this.txtlastName.Location = new System.Drawing.Point(92, 95);
            this.txtlastName.Name = "txtlastName";
            this.txtlastName.Size = new System.Drawing.Size(242, 20);
            this.txtlastName.TabIndex = 9;
            this.txtlastName.Text = " ";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(92, 125);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(242, 20);
            this.txtEmail.TabIndex = 10;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(92, 155);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(242, 20);
            this.txtPhone.TabIndex = 11;
            // 
            // txtJSON
            // 
            this.txtJSON.Location = new System.Drawing.Point(92, 28);
            this.txtJSON.Name = "txtJSON";
            this.txtJSON.Size = new System.Drawing.Size(647, 20);
            this.txtJSON.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 393);
            this.Controls.Add(this.txtJSON);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtlastName);
            this.Controls.Add(this.txtfirstName);
            this.Controls.Add(this.lstFriends);
            this.Controls.Add(this.btnCreateJson);
            this.Controls.Add(this.btnToClass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnToClass;
        private System.Windows.Forms.Button btnCreateJson;
        private System.Windows.Forms.TextBox lstFriends;
        private System.Windows.Forms.TextBox txtfirstName;
        private System.Windows.Forms.TextBox txtlastName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtJSON;
    }
}

