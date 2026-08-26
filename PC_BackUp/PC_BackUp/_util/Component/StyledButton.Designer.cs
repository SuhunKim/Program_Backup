namespace PC_BackUp
{

	partial class StyledButton
	{
		private System.ComponentModel.IContainer? components = null;
		private Label captionLabel = null!;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			captionLabel = new Label();
			SuspendLayout();
			// 
			// captionLabel
			// 
			captionLabel.BackColor = Color.Transparent;
			captionLabel.Dock = DockStyle.Fill;
			captionLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
			captionLabel.ForeColor = Color.White;
			captionLabel.Location = new Point(0, 0);
			captionLabel.Name = "captionLabel";
			captionLabel.Size = new Size(140, 42);
			captionLabel.TabIndex = 0;
			captionLabel.Text = "Styled Button";
			captionLabel.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// StyledButton
			// 
			AutoScaleDimensions = new SizeF(8F, 17F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = ColorRGB.Primary;
			Controls.Add(captionLabel);
			Cursor = Cursors.Hand;
			Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
			ForeColor = Color.White;
			Name = "StyledButton";
			Size = new Size(140, 42);
			ResumeLayout(false);
		}
	}
}
