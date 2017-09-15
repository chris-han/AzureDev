namespace Test_EventHub
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
            this.Single = new System.Windows.Forms.Button();
            this.btn100Msg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Single
            // 
            this.Single.Location = new System.Drawing.Point(336, 258);
            this.Single.Name = "Single";
            this.Single.Size = new System.Drawing.Size(205, 87);
            this.Single.TabIndex = 0;
            this.Single.Text = "SingleMsg";
            this.Single.UseVisualStyleBackColor = true;
            this.Single.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn100Msg
            // 
            this.btn100Msg.Location = new System.Drawing.Point(336, 415);
            this.btn100Msg.Name = "btn100Msg";
            this.btn100Msg.Size = new System.Drawing.Size(205, 87);
            this.btn100Msg.TabIndex = 1;
            this.btn100Msg.Text = "100 Msg";
            this.btn100Msg.UseVisualStyleBackColor = true;
            this.btn100Msg.Click += new System.EventHandler(this.btn100Msg_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 647);
            this.Controls.Add(this.btn100Msg);
            this.Controls.Add(this.Single);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Single;
        private System.Windows.Forms.Button btn100Msg;
    }
}

