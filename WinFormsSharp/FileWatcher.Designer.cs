
namespace WinFormsSharp
{
    partial class FileWatcher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // fileList
            // 
            this.fileList.FormattingEnabled = true;
            this.fileList.ItemHeight = 31;
            this.fileList.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.fileList.Location = new System.Drawing.Point(-3, 2);
            this.fileList.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(1294, 686);
            this.fileList.TabIndex = 0;
            // 
            // FileWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 698);
            this.Controls.Add(this.fileList);
            this.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FileWatcher";
            this.Text = "FileWatcher";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox fileList;
    }
}