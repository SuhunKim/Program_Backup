using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PC_BackUp
{


	public enum StyledButtonType
	{
		Primary,
		Secondary,
		Sidebar
	}

	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	[DefaultProperty(nameof(Text))]
	[DefaultEvent(nameof(Click))]
	public partial class StyledButton : UserControl, IButtonControl
	{
		private StyledButtonType m_eButtonType = StyledButtonType.Primary;
		private bool m_bIsActive;
		private bool m_bPointerOver;
		private bool m_bPointerDown;
		private Color m_oNormalBackColor;
		private Color m_oHoverBackColor;
		private Color m_oPressedBackColor;
		private Color m_oActiveBackColor;
		private Color m_oBorderColor;
		private int m_iBorderThickness;

		public StyledButton()
		{
			InitializeComponent();

			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
							 ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			WirePointerEvents(this);
			WirePointerEvents(captionLabel);
			captionLabel.Click += (_, _) => OnClick(EventArgs.Empty);
			ApplyButtonType();
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get => base.Text;
			set
			{
				base.Text = value;
				if (captionLabel is not null)
					captionLabel.Text = value;
			}
		}

		[Category("Styled Button")]
		[DefaultValue(StyledButtonType.Primary)]
		public StyledButtonType ButtonType
		{
			get => m_eButtonType;
			set
			{
				m_eButtonType = value;
				ApplyButtonType();
			}
		}

		[Category("Styled Button")]
		[DefaultValue(false)]
		public bool IsActive
		{
			get => m_bIsActive;
			set
			{
				m_bIsActive = value;
				RefreshVisualState();
			}
		}

		[Category("Styled Button")]
		[DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
		public ContentAlignment TextAlign
		{
			get => captionLabel.TextAlign;
			set => captionLabel.TextAlign = value;
		}

		// 이 색상들은 ButtonType에 따라 ApplyButtonType()이 계산해서 채우는 값이라 고정된 기본값이
		// 없다 — 디자이너 속성 창에서 개별 편집/직렬화 대상으로 삼지 않는다(WFO1000 대응).
		[Category("Styled Button")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color NormalBackColor
		{
			get => m_oNormalBackColor;
			set { m_oNormalBackColor = value; RefreshVisualState(); }
		}

		[Category("Styled Button")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color HoverBackColor
		{
			get => m_oHoverBackColor;
			set { m_oHoverBackColor = value; RefreshVisualState(); }
		}

		[Category("Styled Button")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color PressedBackColor
		{
			get => m_oPressedBackColor;
			set { m_oPressedBackColor = value; RefreshVisualState(); }
		}

		[Category("Styled Button")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color ActiveBackColor
		{
			get => m_oActiveBackColor;
			set { m_oActiveBackColor = value; RefreshVisualState(); }
		}

		[Category("Styled Button")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color BorderColor
		{
			get => m_oBorderColor;
			set { m_oBorderColor = value; Invalidate(); }
		}

		[Category("Styled Button")]
		[DefaultValue(0)]
		public int BorderThickness
		{
			get => m_iBorderThickness;
			set { m_iBorderThickness = Math.Max(0, value); Invalidate(); }
		}

		public void PerformClick()
		{
			if (Enabled)
				OnClick(EventArgs.Empty);
		}

		// Form.AcceptButton/CancelButton에 지정할 수 있도록 IButtonControl을 구현한다.
		[Category("Styled Button")]
		[DefaultValue(typeof(DialogResult), "None")]
		public DialogResult DialogResult { get; set; } = DialogResult.None;

		public void NotifyDefault(bool value)
		{
			// 기본 버튼 강조 표시는 현재 별도로 처리하지 않는다.
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (captionLabel is not null)
				captionLabel.Font = Font;
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			base.OnForeColorChanged(e);
			if (captionLabel is not null)
				captionLabel.ForeColor = ForeColor;
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			RefreshVisualState();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (BorderThickness <= 0 || BorderColor == Color.Transparent)
				return;

			using var pen = new Pen(BorderColor, BorderThickness) { Alignment = PenAlignment.Inset };
			e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (Enabled && (keyData == Keys.Enter || keyData == Keys.Space))
			{
				PerformClick();
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		private void ApplyButtonType()
		{
			switch (ButtonType)
			{
				case StyledButtonType.Sidebar:
					Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
					ForeColor = ColorRGB.Text;
					NormalBackColor = ColorRGB.Sidebar;
					HoverBackColor = ColorRGB.SidebarHover;
					PressedBackColor = ColorRGB.SidebarActive;
					ActiveBackColor = ColorRGB.SidebarActive;
					BorderColor = Color.Transparent;
					BorderThickness = 0;
					Padding = new Padding(16, 0, 0, 0);
					TextAlign = ContentAlignment.MiddleLeft;
					Size = new Size(190, 50);
					break;

				case StyledButtonType.Secondary:
					Font = new Font("맑은 고딕", 9F);
					ForeColor = ColorRGB.Text;
					NormalBackColor = ColorRGB.SecondaryBackground;
					HoverBackColor = ColorRGB.SecondaryHover;
					PressedBackColor = ColorRGB.SecondaryPressed;
					ActiveBackColor = NormalBackColor;
					BorderColor = ColorRGB.Border;
					BorderThickness = 1;
					Padding = System.Windows.Forms.Padding.Empty;
					TextAlign = ContentAlignment.MiddleCenter;
					Size = new Size(105, 42);
					break;

				default:
					Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
					ForeColor = Color.White;
					NormalBackColor = ColorRGB.Primary;
					HoverBackColor = ColorRGB.PrimaryHover;
					PressedBackColor = ColorRGB.PrimaryPressed;
					ActiveBackColor = NormalBackColor;
					BorderColor = Color.Transparent;
					BorderThickness = 0;
					Padding = System.Windows.Forms.Padding.Empty;
					TextAlign = ContentAlignment.MiddleCenter;
					Size = new Size(140, 42);
					break;
			}
			RefreshVisualState();
		}

		private void RefreshVisualState()
		{
			var color = !Enabled
					? Color.FromArgb(190, 195, 203)
					: IsActive
							? ActiveBackColor
							: m_bPointerDown
									? PressedBackColor
									: m_bPointerOver ? HoverBackColor : NormalBackColor;

			base.BackColor = color;
			if (captionLabel is not null)
			{
				captionLabel.BackColor = Color.Transparent;
				captionLabel.ForeColor = Enabled ? ForeColor : Color.FromArgb(115, 120, 128);
			}
			Invalidate();
		}

		private void WirePointerEvents(Control control)
		{
			control.MouseEnter += (_, _) =>
			{
				m_bPointerOver = true;
				RefreshVisualState();
			};
			control.MouseLeave += (_, _) =>
			{
				if (!ClientRectangle.Contains(PointToClient(System.Windows.Forms.Cursor.Position)))
				{
					m_bPointerOver = false;
					m_bPointerDown = false;
					RefreshVisualState();
				}
			};
			control.MouseDown += (_, e) =>
			{
				if (e.Button == MouseButtons.Left)
				{
					m_bPointerDown = true;
					RefreshVisualState();
				}
			};
			control.MouseUp += (_, e) =>
			{
				if (e.Button == MouseButtons.Left)
				{
					m_bPointerDown = false;
					RefreshVisualState();
				}
			};
		}
	}
}
