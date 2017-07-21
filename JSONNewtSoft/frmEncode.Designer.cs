namespace JSONNewtSoft
{
    partial class frmEncode
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabNewtonsoft = new System.Windows.Forms.TabPage();
            this.txtJSON = new System.Windows.Forms.TextBox();
            this.btnCreateJson = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnToClass = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstFriends = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtfirstName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtlastName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.tabJavaScriptSerializer = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.btnDeSerializeJSS = new System.Windows.Forms.Button();
            this.btnSerializeJSS = new System.Windows.Forms.Button();
            this.tabDataContractJso = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.btnDeSerialize = new System.Windows.Forms.Button();
            this.btnSerialize = new System.Windows.Forms.Button();
            this.tabJSONNET = new System.Windows.Forms.TabPage();
            this.txtJsonNet = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDeSerializeJSONnet = new System.Windows.Forms.Button();
            this.btnSerializeJSONnet = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnShopping = new System.Windows.Forms.Button();
            this.tabNewtonsoft.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.tabJavaScriptSerializer.SuspendLayout();
            this.tabDataContractJso.SuspendLayout();
            this.tabJSONNET.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabNewtonsoft
            // 
            this.tabNewtonsoft.Controls.Add(this.btnShopping);
            this.tabNewtonsoft.Controls.Add(this.txtJSON);
            this.tabNewtonsoft.Controls.Add(this.btnCreateJson);
            this.tabNewtonsoft.Controls.Add(this.label6);
            this.tabNewtonsoft.Controls.Add(this.btnToClass);
            this.tabNewtonsoft.Controls.Add(this.label1);
            this.tabNewtonsoft.Controls.Add(this.lstFriends);
            this.tabNewtonsoft.Controls.Add(this.label2);
            this.tabNewtonsoft.Controls.Add(this.txtPhone);
            this.tabNewtonsoft.Controls.Add(this.txtfirstName);
            this.tabNewtonsoft.Controls.Add(this.txtEmail);
            this.tabNewtonsoft.Controls.Add(this.txtlastName);
            this.tabNewtonsoft.Controls.Add(this.label3);
            this.tabNewtonsoft.Controls.Add(this.label4);
            this.tabNewtonsoft.Controls.Add(this.label5);
            this.tabNewtonsoft.Location = new System.Drawing.Point(4, 22);
            this.tabNewtonsoft.Name = "tabNewtonsoft";
            this.tabNewtonsoft.Padding = new System.Windows.Forms.Padding(3);
            this.tabNewtonsoft.Size = new System.Drawing.Size(740, 354);
            this.tabNewtonsoft.TabIndex = 0;
            this.tabNewtonsoft.Text = "Newtonsoft";
            this.tabNewtonsoft.UseVisualStyleBackColor = true;
            // 
            // txtJSON
            // 
            this.txtJSON.Location = new System.Drawing.Point(72, 26);
            this.txtJSON.Name = "txtJSON";
            this.txtJSON.Size = new System.Drawing.Size(647, 20);
            this.txtJSON.TabIndex = 12;
            // 
            // btnCreateJson
            // 
            this.btnCreateJson.Location = new System.Drawing.Point(331, 165);
            this.btnCreateJson.Name = "btnCreateJson";
            this.btnCreateJson.Size = new System.Drawing.Size(107, 23);
            this.btnCreateJson.TabIndex = 6;
            this.btnCreateJson.Text = "Create JSON";
            this.btnCreateJson.UseVisualStyleBackColor = true;
            this.btnCreateJson.Click += new System.EventHandler(this.btnCreateJson_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(69, 175);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Use: Newtonsoft.Json ";
            // 
            // btnToClass
            // 
            this.btnToClass.Location = new System.Drawing.Point(207, 165);
            this.btnToClass.Name = "btnToClass";
            this.btnToClass.Size = new System.Drawing.Size(107, 23);
            this.btnToClass.TabIndex = 5;
            this.btnToClass.Text = "DecodeJSON";
            this.btnToClass.UseVisualStyleBackColor = true;
            this.btnToClass.Click += new System.EventHandler(this.btnToClass_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "JSON";
            // 
            // lstFriends
            // 
            this.lstFriends.FormattingEnabled = true;
            this.lstFriends.Location = new System.Drawing.Point(320, 51);
            this.lstFriends.Name = "lstFriends";
            this.lstFriends.Size = new System.Drawing.Size(399, 108);
            this.lstFriends.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "First Name";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(72, 139);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(242, 20);
            this.txtPhone.TabIndex = 11;
            // 
            // txtfirstName
            // 
            this.txtfirstName.Location = new System.Drawing.Point(72, 53);
            this.txtfirstName.Name = "txtfirstName";
            this.txtfirstName.Size = new System.Drawing.Size(242, 20);
            this.txtfirstName.TabIndex = 8;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(72, 109);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(242, 20);
            this.txtEmail.TabIndex = 10;
            // 
            // txtlastName
            // 
            this.txtlastName.Location = new System.Drawing.Point(72, 79);
            this.txtlastName.Name = "txtlastName";
            this.txtlastName.Size = new System.Drawing.Size(242, 20);
            this.txtlastName.TabIndex = 9;
            this.txtlastName.Text = " ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Email";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Phone";
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.tabNewtonsoft);
            this.TabControl.Controls.Add(this.tabJavaScriptSerializer);
            this.TabControl.Controls.Add(this.tabDataContractJso);
            this.TabControl.Controls.Add(this.tabJSONNET);
            this.TabControl.Location = new System.Drawing.Point(2, 12);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(748, 380);
            this.TabControl.TabIndex = 15;
            // 
            // tabJavaScriptSerializer
            // 
            this.tabJavaScriptSerializer.Controls.Add(this.label13);
            this.tabJavaScriptSerializer.Controls.Add(this.label11);
            this.tabJavaScriptSerializer.Controls.Add(this.btnDeSerializeJSS);
            this.tabJavaScriptSerializer.Controls.Add(this.btnSerializeJSS);
            this.tabJavaScriptSerializer.Location = new System.Drawing.Point(4, 22);
            this.tabJavaScriptSerializer.Name = "tabJavaScriptSerializer";
            this.tabJavaScriptSerializer.Padding = new System.Windows.Forms.Padding(3);
            this.tabJavaScriptSerializer.Size = new System.Drawing.Size(740, 354);
            this.tabJavaScriptSerializer.TabIndex = 1;
            this.tabJavaScriptSerializer.Text = "JavaScriptSerializer";
            this.tabJavaScriptSerializer.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label11.Location = new System.Drawing.Point(156, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(226, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Add Refernece system.web.Script.Serialization";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // btnDeSerializeJSS
            // 
            this.btnDeSerializeJSS.Location = new System.Drawing.Point(379, 166);
            this.btnDeSerializeJSS.Name = "btnDeSerializeJSS";
            this.btnDeSerializeJSS.Size = new System.Drawing.Size(107, 23);
            this.btnDeSerializeJSS.TabIndex = 10;
            this.btnDeSerializeJSS.Text = "DeserializationJSON";
            this.btnDeSerializeJSS.UseVisualStyleBackColor = true;
            this.btnDeSerializeJSS.Click += new System.EventHandler(this.btnDeSerializeJSS_Click);
            // 
            // btnSerializeJSS
            // 
            this.btnSerializeJSS.Location = new System.Drawing.Point(255, 166);
            this.btnSerializeJSS.Name = "btnSerializeJSS";
            this.btnSerializeJSS.Size = new System.Drawing.Size(107, 23);
            this.btnSerializeJSS.TabIndex = 9;
            this.btnSerializeJSS.Text = "SerializeJSON";
            this.btnSerializeJSS.UseVisualStyleBackColor = true;
            this.btnSerializeJSS.Click += new System.EventHandler(this.btnSerializeJSS_Click);
            // 
            // tabDataContractJso
            // 
            this.tabDataContractJso.Controls.Add(this.label12);
            this.tabDataContractJso.Controls.Add(this.btnDeSerialize);
            this.tabDataContractJso.Controls.Add(this.btnSerialize);
            this.tabDataContractJso.Location = new System.Drawing.Point(4, 22);
            this.tabDataContractJso.Name = "tabDataContractJso";
            this.tabDataContractJso.Size = new System.Drawing.Size(740, 354);
            this.tabDataContractJso.TabIndex = 2;
            this.tabDataContractJso.Text = "DataContractJsonSerializer ";
            this.tabDataContractJso.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label12.Location = new System.Drawing.Point(177, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(395, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "DataContractJsonSerializer  was also not included in  Core ..use Newtonsoft.Jason" +
    "";
            // 
            // btnDeSerialize
            // 
            this.btnDeSerialize.Location = new System.Drawing.Point(379, 166);
            this.btnDeSerialize.Name = "btnDeSerialize";
            this.btnDeSerialize.Size = new System.Drawing.Size(107, 23);
            this.btnDeSerialize.TabIndex = 8;
            this.btnDeSerialize.Text = "DeserializationJSON";
            this.btnDeSerialize.UseVisualStyleBackColor = true;
            this.btnDeSerialize.Click += new System.EventHandler(this.btnDeSerialize_Click);
            // 
            // btnSerialize
            // 
            this.btnSerialize.Location = new System.Drawing.Point(255, 166);
            this.btnSerialize.Name = "btnSerialize";
            this.btnSerialize.Size = new System.Drawing.Size(107, 23);
            this.btnSerialize.TabIndex = 7;
            this.btnSerialize.Text = "SerializeJSON";
            this.btnSerialize.UseVisualStyleBackColor = true;
            this.btnSerialize.Click += new System.EventHandler(this.btnSerialize_Click);
            // 
            // tabJSONNET
            // 
            this.tabJSONNET.Controls.Add(this.txtJsonNet);
            this.tabJSONNET.Controls.Add(this.label10);
            this.tabJSONNET.Controls.Add(this.label9);
            this.tabJSONNET.Controls.Add(this.label8);
            this.tabJSONNET.Controls.Add(this.txtDescription);
            this.tabJSONNET.Controls.Add(this.txtName);
            this.tabJSONNET.Controls.Add(this.label7);
            this.tabJSONNET.Controls.Add(this.btnDeSerializeJSONnet);
            this.tabJSONNET.Controls.Add(this.btnSerializeJSONnet);
            this.tabJSONNET.Location = new System.Drawing.Point(4, 22);
            this.tabJSONNET.Name = "tabJSONNET";
            this.tabJSONNET.Size = new System.Drawing.Size(740, 354);
            this.tabJSONNET.TabIndex = 3;
            this.tabJSONNET.Text = "JSON.NET";
            this.tabJSONNET.UseVisualStyleBackColor = true;
            // 
            // txtJsonNet
            // 
            this.txtJsonNet.Location = new System.Drawing.Point(90, 33);
            this.txtJsonNet.Name = "txtJsonNet";
            this.txtJsonNet.Size = new System.Drawing.Size(647, 20);
            this.txtJsonNet.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "JSON";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Description";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Name";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(90, 93);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(214, 20);
            this.txtDescription.TabIndex = 13;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(90, 63);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(214, 20);
            this.txtName.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(187, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "PM> Install-Package Newtonsoft.Json";
            // 
            // btnDeSerializeJSONnet
            // 
            this.btnDeSerializeJSONnet.Location = new System.Drawing.Point(379, 166);
            this.btnDeSerializeJSONnet.Name = "btnDeSerializeJSONnet";
            this.btnDeSerializeJSONnet.Size = new System.Drawing.Size(107, 23);
            this.btnDeSerializeJSONnet.TabIndex = 10;
            this.btnDeSerializeJSONnet.Text = "DeserializationJSON";
            this.btnDeSerializeJSONnet.UseVisualStyleBackColor = true;
            // 
            // btnSerializeJSONnet
            // 
            this.btnSerializeJSONnet.Location = new System.Drawing.Point(255, 166);
            this.btnSerializeJSONnet.Name = "btnSerializeJSONnet";
            this.btnSerializeJSONnet.Size = new System.Drawing.Size(107, 23);
            this.btnSerializeJSONnet.TabIndex = 9;
            this.btnSerializeJSONnet.Text = "SerializeJSON";
            this.btnSerializeJSONnet.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label13.Location = new System.Drawing.Point(156, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(357, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "JavaScriptSerializer was also not included in  Core ..use Newtonsoft.Jason";
            // 
            // btnShopping
            // 
            this.btnShopping.Location = new System.Drawing.Point(23, 221);
            this.btnShopping.Name = "btnShopping";
            this.btnShopping.Size = new System.Drawing.Size(182, 23);
            this.btnShopping.TabIndex = 16;
            this.btnShopping.Text = "Run Shopping Console";
            this.btnShopping.UseVisualStyleBackColor = true;
            this.btnShopping.Click += new System.EventHandler(this.btnShopping_Click_1);
            // 
            // frmEncode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 414);
            this.Controls.Add(this.TabControl);
            this.Name = "frmEncode";
            this.Text = "JavaScript Object Notation; serialization/deserialization ";
            this.tabNewtonsoft.ResumeLayout(false);
            this.tabNewtonsoft.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.tabJavaScriptSerializer.ResumeLayout(false);
            this.tabJavaScriptSerializer.PerformLayout();
            this.tabDataContractJso.ResumeLayout(false);
            this.tabDataContractJso.PerformLayout();
            this.tabJSONNET.ResumeLayout(false);
            this.tabJSONNET.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabNewtonsoft;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage tabJavaScriptSerializer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnToClass;
        private System.Windows.Forms.Button btnCreateJson;
        private System.Windows.Forms.TextBox txtfirstName;
        private System.Windows.Forms.TextBox txtlastName;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtJSON;
        private System.Windows.Forms.ListBox lstFriends;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabDataContractJso;
        private System.Windows.Forms.TabPage tabJSONNET;
        private System.Windows.Forms.Button btnDeSerialize;
        private System.Windows.Forms.Button btnSerialize;
        private System.Windows.Forms.Button btnDeSerializeJSS;
        private System.Windows.Forms.Button btnSerializeJSS;
        private System.Windows.Forms.Button btnDeSerializeJSONnet;
        private System.Windows.Forms.Button btnSerializeJSONnet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtJsonNet;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnShopping;
    }
}

