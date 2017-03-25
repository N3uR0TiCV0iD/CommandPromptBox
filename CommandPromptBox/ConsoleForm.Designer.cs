namespace HiT.CommandPromptBox
{
    partial class ConsoleForm
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
            this.commandPromptBox = new HiT.CommandPromptBox.CommandPromptBox();
            this.SuspendLayout();
            // 
            // commandPromptBox
            // 
            this.commandPromptBox.Columns = 80;
            this.commandPromptBox.CursorHeight = 3;
            this.commandPromptBox.CursorVisible = false;
            this.commandPromptBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandPromptBox.Location = new System.Drawing.Point(0, 0);
            this.commandPromptBox.Name = "commandPromptBox";
            this.commandPromptBox.Rows = 300;
            this.commandPromptBox.Size = new System.Drawing.Size(116, 12);
            this.commandPromptBox.TabIndex = 0;
            this.commandPromptBox.TreatControlCAsInput = false;
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 12);
            this.Controls.Add(this.commandPromptBox);
            this.Name = "ConsoleForm";
            this.Text = "Console";
            this.ResumeLayout(false);

        }

        #endregion

        private CommandPromptBox commandPromptBox;
    }
}