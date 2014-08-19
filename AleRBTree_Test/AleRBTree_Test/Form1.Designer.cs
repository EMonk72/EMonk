namespace AleRBTree_Test
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnBuildTree = new System.Windows.Forms.Button();
            this.btnIterate = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblData = new System.Windows.Forms.Label();
            this.tvRB = new System.Windows.Forms.TreeView();
            this.imglistTreeNodes = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // btnBuildTree
            // 
            this.btnBuildTree.Location = new System.Drawing.Point(12, 12);
            this.btnBuildTree.Name = "btnBuildTree";
            this.btnBuildTree.Size = new System.Drawing.Size(75, 23);
            this.btnBuildTree.TabIndex = 3;
            this.btnBuildTree.Text = "Add 5 nodes";
            this.btnBuildTree.UseVisualStyleBackColor = true;
            this.btnBuildTree.Click += new System.EventHandler(this.btnBuildTree_Click);
            // 
            // btnIterate
            // 
            this.btnIterate.Location = new System.Drawing.Point(183, 12);
            this.btnIterate.Name = "btnIterate";
            this.btnIterate.Size = new System.Drawing.Size(75, 23);
            this.btnIterate.TabIndex = 4;
            this.btnIterate.Text = "Iterate";
            this.btnIterate.UseVisualStyleBackColor = true;
            this.btnIterate.Click += new System.EventHandler(this.btnIterate_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(367, 15);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(102, 20);
            this.txtKey.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Key";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(580, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(93, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(497, 13);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(77, 23);
            this.btnFind.TabIndex = 9;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(336, 50);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(0, 13);
            this.lblData.TabIndex = 10;
            // 
            // tvRB
            // 
            this.tvRB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvRB.ImageIndex = 0;
            this.tvRB.ImageList = this.imglistTreeNodes;
            this.tvRB.Location = new System.Drawing.Point(12, 86);
            this.tvRB.Name = "tvRB";
            this.tvRB.SelectedImageIndex = 0;
            this.tvRB.Size = new System.Drawing.Size(645, 465);
            this.tvRB.TabIndex = 11;
            // 
            // imglistTreeNodes
            // 
            this.imglistTreeNodes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglistTreeNodes.ImageStream")));
            this.imglistTreeNodes.TransparentColor = System.Drawing.Color.Transparent;
            this.imglistTreeNodes.Images.SetKeyName(0, "black.bmp");
            this.imglistTreeNodes.Images.SetKeyName(1, "red.bmp");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 563);
            this.Controls.Add(this.tvRB);
            this.Controls.Add(this.lblData);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.btnIterate);
            this.Controls.Add(this.btnBuildTree);
            this.MinimumSize = new System.Drawing.Size(685, 200);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBuildTree;
        private System.Windows.Forms.Button btnIterate;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TreeView tvRB;
        private System.Windows.Forms.ImageList imglistTreeNodes;
    }
}

