namespace Remote_Desktop
{
    partial class HostInfoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HostInfoForm));
            this.lbl_Host = new System.Windows.Forms.Label();
            this.lbl_Type = new System.Windows.Forms.Label();
            this.lbl_Group = new System.Windows.Forms.Label();
            this.cbo_PcName = new System.Windows.Forms.ComboBox();
            this.cbo_EquipmentType = new System.Windows.Forms.ComboBox();
            this.cbo_Factory = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.txt_IPAddress = new System.Windows.Forms.TextBox();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.txt_PcName = new System.Windows.Forms.TextBox();
            this.lbl_PcName = new System.Windows.Forms.Label();
            this.btn_Save = new System.Windows.Forms.Button();
            this.txt_Factory = new System.Windows.Forms.TextBox();
            this.txt_EquipmentType = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lbl_Host
            // 
            this.lbl_Host.AutoSize = true;
            this.lbl_Host.Location = new System.Drawing.Point(282, -4);
            this.lbl_Host.Name = "lbl_Host";
            this.lbl_Host.Size = new System.Drawing.Size(38, 17);
            this.lbl_Host.TabIndex = 19;
            this.lbl_Host.Text = "Host:";
            // 
            // lbl_Type
            // 
            this.lbl_Type.AutoSize = true;
            this.lbl_Type.Location = new System.Drawing.Point(218, 25);
            this.lbl_Type.Name = "lbl_Type";
            this.lbl_Type.Size = new System.Drawing.Size(39, 17);
            this.lbl_Type.TabIndex = 18;
            this.lbl_Type.Text = "Type:";
            // 
            // lbl_Group
            // 
            this.lbl_Group.AutoSize = true;
            this.lbl_Group.Location = new System.Drawing.Point(11, 25);
            this.lbl_Group.Name = "lbl_Group";
            this.lbl_Group.Size = new System.Drawing.Size(48, 17);
            this.lbl_Group.TabIndex = 16;
            this.lbl_Group.Text = "Group:";
            // 
            // cbo_PcName
            // 
            this.cbo_PcName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_PcName.FormattingEnabled = true;
            this.cbo_PcName.Location = new System.Drawing.Point(326, -7);
            this.cbo_PcName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbo_PcName.Name = "cbo_PcName";
            this.cbo_PcName.Size = new System.Drawing.Size(140, 25);
            this.cbo_PcName.TabIndex = 14;
            // 
            // cbo_EquipmentType
            // 
            this.cbo_EquipmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_EquipmentType.FormattingEnabled = true;
            this.cbo_EquipmentType.Location = new System.Drawing.Point(261, 21);
            this.cbo_EquipmentType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbo_EquipmentType.Name = "cbo_EquipmentType";
            this.cbo_EquipmentType.Size = new System.Drawing.Size(140, 25);
            this.cbo_EquipmentType.TabIndex = 1;
            this.cbo_EquipmentType.SelectedIndexChanged += new System.EventHandler(this.cbo_EquipmentType_SelectedIndexChanged);
            // 
            // cbo_Factory
            // 
            this.cbo_Factory.BackColor = System.Drawing.SystemColors.Window;
            this.cbo_Factory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Factory.FormattingEnabled = true;
            this.cbo_Factory.Location = new System.Drawing.Point(60, 21);
            this.cbo_Factory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbo_Factory.Name = "cbo_Factory";
            this.cbo_Factory.Size = new System.Drawing.Size(140, 25);
            this.cbo_Factory.TabIndex = 0;
            this.cbo_Factory.SelectedIndexChanged += new System.EventHandler(this.cbo_Factory_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(218, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 17);
            this.label10.TabIndex = 24;
            this.label10.Text = "Port:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 151);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 17);
            this.label9.TabIndex = 23;
            this.label9.Text = "IPAddress:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 121);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 17);
            this.label8.TabIndex = 22;
            this.label8.Text = "Password:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "UserName:";
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(285, 118);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Size = new System.Drawing.Size(116, 23);
            this.txt_Password.TabIndex = 6;
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(285, 148);
            this.txt_Port.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(116, 23);
            this.txt_Port.TabIndex = 8;
            // 
            // txt_IPAddress
            // 
            this.txt_IPAddress.Location = new System.Drawing.Point(84, 148);
            this.txt_IPAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_IPAddress.Name = "txt_IPAddress";
            this.txt_IPAddress.Size = new System.Drawing.Size(116, 23);
            this.txt_IPAddress.TabIndex = 7;
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(84, 118);
            this.txt_UserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(116, 23);
            this.txt_UserName.TabIndex = 5;
            // 
            // txt_PcName
            // 
            this.txt_PcName.Location = new System.Drawing.Point(84, 88);
            this.txt_PcName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_PcName.Name = "txt_PcName";
            this.txt_PcName.Size = new System.Drawing.Size(116, 23);
            this.txt_PcName.TabIndex = 4;
            // 
            // lbl_PcName
            // 
            this.lbl_PcName.AutoSize = true;
            this.lbl_PcName.Location = new System.Drawing.Point(11, 92);
            this.lbl_PcName.Name = "lbl_PcName";
            this.lbl_PcName.Size = new System.Drawing.Size(38, 17);
            this.lbl_PcName.TabIndex = 25;
            this.lbl_PcName.Text = "Host:";
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_Save.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_Save.FlatAppearance.BorderSize = 0;
            this.btn_Save.Location = new System.Drawing.Point(298, 189);
            this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(87, 33);
            this.btn_Save.TabIndex = 9;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // txt_Factory
            // 
            this.txt_Factory.Location = new System.Drawing.Point(60, 46);
            this.txt_Factory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Factory.Name = "txt_Factory";
            this.txt_Factory.Size = new System.Drawing.Size(140, 23);
            this.txt_Factory.TabIndex = 2;
            // 
            // txt_EquipmentType
            // 
            this.txt_EquipmentType.Location = new System.Drawing.Point(261, 46);
            this.txt_EquipmentType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_EquipmentType.Name = "txt_EquipmentType";
            this.txt_EquipmentType.Size = new System.Drawing.Size(140, 23);
            this.txt_EquipmentType.TabIndex = 3;
            // 
            // HostInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(414, 241);
            this.Controls.Add(this.txt_EquipmentType);
            this.Controls.Add(this.txt_Factory);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txt_PcName);
            this.Controls.Add(this.lbl_PcName);
            this.Controls.Add(this.lbl_Host);
            this.Controls.Add(this.lbl_Type);
            this.Controls.Add(this.lbl_Group);
            this.Controls.Add(this.cbo_PcName);
            this.Controls.Add(this.cbo_EquipmentType);
            this.Controls.Add(this.cbo_Factory);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_Port);
            this.Controls.Add(this.txt_IPAddress);
            this.Controls.Add(this.txt_UserName);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "HostInfoForm";
            this.Text = "HostInfoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Host;
        private System.Windows.Forms.Label lbl_Type;
        private System.Windows.Forms.Label lbl_Group;
        private System.Windows.Forms.ComboBox cbo_PcName;
        private System.Windows.Forms.ComboBox cbo_EquipmentType;
        private System.Windows.Forms.ComboBox cbo_Factory;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.TextBox txt_IPAddress;
        private System.Windows.Forms.TextBox txt_UserName;
        private System.Windows.Forms.TextBox txt_PcName;
        private System.Windows.Forms.Label lbl_PcName;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.TextBox txt_Factory;
        private System.Windows.Forms.TextBox txt_EquipmentType;
    }
}