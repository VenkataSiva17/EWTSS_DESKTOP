namespace EWTSS_DESKTOP.Infrastructure.Services
{
    partial class Stk3DViewControl
    {
        private System.ComponentModel.IContainer components = null;
        private AxAGI.Ui.AxVOCntrl axAgUiAxVOCntrl1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.axAgUiAxVOCntrl1 = new AxAGI.Ui.AxVOCntrl();
            ((System.ComponentModel.ISupportInitialize)(this.axAgUiAxVOCntrl1)).BeginInit();
            this.SuspendLayout();

            this.axAgUiAxVOCntrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axAgUiAxVOCntrl1.Enabled = true;
            this.axAgUiAxVOCntrl1.Location = new System.Drawing.Point(0, 0);
            this.axAgUiAxVOCntrl1.Name = "axAgUiAxVOCntrl1";
            this.axAgUiAxVOCntrl1.Size = new System.Drawing.Size(800, 600);
            this.axAgUiAxVOCntrl1.TabIndex = 0;

            this.Controls.Add(this.axAgUiAxVOCntrl1);
            this.Name = "Stk3DViewControl";
            this.Size = new System.Drawing.Size(800, 600);

            ((System.ComponentModel.ISupportInitialize)(this.axAgUiAxVOCntrl1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}