namespace pwsg_2
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.szerokosc = new System.Windows.Forms.NumericUpDown();
            this.wysokosc = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.szerokosc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wysokosc)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // szerokosc
            // 
            resources.ApplyResources(this.szerokosc, "szerokosc");
            this.szerokosc.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.szerokosc.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.szerokosc.Name = "szerokosc";
            this.szerokosc.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // wysokosc
            // 
            resources.ApplyResources(this.wysokosc, "wysokosc");
            this.wysokosc.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.wysokosc.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.wysokosc.Name = "wysokosc";
            this.wysokosc.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // Form2
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wysokosc);
            this.Controls.Add(this.szerokosc);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.szerokosc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wysokosc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown szerokosc;
        private System.Windows.Forms.NumericUpDown wysokosc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}