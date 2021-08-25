namespace MLA.APPS
{
    partial class MLASettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MLASettings));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.treevwLevels = new System.Windows.Forms.TreeView();
            this.treevwUsers = new System.Windows.Forms.TreeView();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDownUsers = new System.Windows.Forms.Button();
            this.btnUpUsers = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imagelist = new System.Windows.Forms.ImageList(this.components);
            this.treevwNodes = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.treevwLevels);
            this.groupBox1.Controls.Add(this.treevwUsers);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnDownUsers);
            this.groupBox1.Controls.Add(this.btnUpUsers);
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.treevwNodes);
            this.groupBox1.Location = new System.Drawing.Point(9, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 421);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Level Settings  ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(570, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "label4";
            // 
            // treevwLevels
            // 
            this.treevwLevels.AllowDrop = true;
            this.treevwLevels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treevwLevels.CheckBoxes = true;
            this.treevwLevels.Location = new System.Drawing.Point(468, 66);
            this.treevwLevels.Name = "treevwLevels";
            this.treevwLevels.Size = new System.Drawing.Size(233, 304);
            this.treevwLevels.TabIndex = 34;
            this.treevwLevels.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treevwUsers_AfterCheck);
            this.treevwLevels.DragDrop += new System.Windows.Forms.DragEventHandler(this.treevwLevels_DragDrop);
            this.treevwLevels.DragEnter += new System.Windows.Forms.DragEventHandler(this.treevwLevels_DragEnter);
            this.treevwLevels.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treevwLevels_ItemDrag);
            // 
            // treevwUsers
            // 
            this.treevwUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treevwUsers.CheckBoxes = true;
            this.treevwUsers.Location = new System.Drawing.Point(208, 66);
            this.treevwUsers.Name = "treevwUsers";
            this.treevwUsers.Size = new System.Drawing.Size(206, 304);
            this.treevwUsers.TabIndex = 33;
            this.treevwUsers.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treevwUsers_AfterCheck);
            this.treevwUsers.DragEnter += new System.Windows.Forms.DragEventHandler(this.treevwUsers_DragEnter);
            this.treevwUsers.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treevwUsers_ItemDrag);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(534, 389);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 32;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(437, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 31;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnDownUsers
            // 
            this.btnDownUsers.Image = global::MLA.APPS.Properties.Resources.Arrows_Left_3_icon;
            this.btnDownUsers.Location = new System.Drawing.Point(420, 217);
            this.btnDownUsers.Name = "btnDownUsers";
            this.btnDownUsers.Size = new System.Drawing.Size(32, 36);
            this.btnDownUsers.TabIndex = 30;
            this.btnDownUsers.UseVisualStyleBackColor = true;
            this.btnDownUsers.Click += new System.EventHandler(this.btnDownUsers_Click);
            // 
            // btnUpUsers
            // 
            this.btnUpUsers.Image = global::MLA.APPS.Properties.Resources.Arrows_Right_3_icon;
            this.btnUpUsers.Location = new System.Drawing.Point(420, 162);
            this.btnUpUsers.Name = "btnUpUsers";
            this.btnUpUsers.Size = new System.Drawing.Size(32, 36);
            this.btnUpUsers.TabIndex = 29;
            this.btnUpUsers.UseVisualStyleBackColor = true;
            this.btnUpUsers.Click += new System.EventHandler(this.btnUpUsers_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::MLA.APPS.Properties.Resources.menu_item_icon;
            this.pictureBox3.Location = new System.Drawing.Point(104, 19);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(24, 25);
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::MLA.APPS.Properties.Resources.Increase_icon;
            this.pictureBox2.Location = new System.Drawing.Point(507, 38);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 21);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MLA.APPS.Properties.Resources.users_icon;
            this.pictureBox1.Location = new System.Drawing.Point(254, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 21);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(455, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Levels";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(205, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Users";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Transactions";
            // 
            // imagelist
            // 
            this.imagelist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagelist.ImageStream")));
            this.imagelist.TransparentColor = System.Drawing.Color.Transparent;
            this.imagelist.Images.SetKeyName(0, "user-icon.png");
            this.imagelist.Images.SetKeyName(1, "menu-item-icon16.png");
            this.imagelist.Images.SetKeyName(2, "users-icon.png");
            this.imagelist.Images.SetKeyName(3, "Increase-icon.png");
            // 
            // treevwNodes
            // 
            this.treevwNodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treevwNodes.Location = new System.Drawing.Point(10, 49);
            this.treevwNodes.Name = "treevwNodes";
            this.treevwNodes.Size = new System.Drawing.Size(174, 321);
            this.treevwNodes.TabIndex = 1;
            // 
            // MLASettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 438);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MLASettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MLASettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treevwNodes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.ImageList imagelist;
        private System.Windows.Forms.Button btnDownUsers;
        private System.Windows.Forms.Button btnUpUsers;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treevwUsers;
        private System.Windows.Forms.TreeView treevwLevels;
        private System.Windows.Forms.Label label4;

    }
}

