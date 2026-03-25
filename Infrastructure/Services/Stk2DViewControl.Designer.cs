using AGI.STKObjects;
using AGI.STKUtil;
using AGI.STKX;


namespace EWTSS_DESKTOP.Infrastructure.Services

{
    partial class Stk2DViewControl
    {
        private System.ComponentModel.IContainer components = null;

        private AGI.STKX.AgUiAx2DCntrl axAgUiAx2DCntrl1;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.axAgUiAx2DCntrl1 = new AGI.STKX.AgUiAx2DCntrlClass();
            ((System.ComponentModel.ISupportInitialize)(this.axAgUiAx2DCntrl1)).BeginInit();
            this.SuspendLayout();

            // this.axAgUiAx2DCntrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            // this.axAgUiAx2DCntrl1.Enabled = true;
            // this.axAgUiAx2DCntrl1.Location = new System.Drawing.Point(0, 0);
            // this.axAgUiAx2DCntrl1.Name = "axAgUiAx2DCntrl1";
            // this.axAgUiAx2DCntrl1.Size = new System.Drawing.Size(800, 600);
            // this.axAgUiAx2DCntrl1.TabIndex = 0;

            // this.Controls.Add(this.axAgUiAx2DCntrl1);
            this.Name = "Stk2DViewControl";
            this.Size = new System.Drawing.Size(800, 600);

            ((System.ComponentModel.ISupportInitialize)(this.axAgUiAx2DCntrl1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}