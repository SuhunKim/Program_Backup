namespace PC_BackUp;

partial class CircularProgressBar
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        SuspendLayout();
        //
        // CircularProgressBar
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Transparent;
        DoubleBuffered = true;
        Name = "CircularProgressBar";
        Size = new Size(48, 48);
        ResumeLayout(false);
    }
}
