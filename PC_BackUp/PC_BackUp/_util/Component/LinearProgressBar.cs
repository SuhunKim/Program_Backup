using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>
/// 직접 그리는 선형 진행바. 표준 <see cref="ProgressBar"/>는 Windows 비주얼 스타일이 색을
/// 정해서 <c>ForeColor</c>를 바꿔도 초록색 고정이고 얇아서 잘 보이지 않는다.
/// 이 컨트롤은 둥근 트랙 + 그라데이션 채움 + 가운데 퍼센트 글자로 진행 상황을 분명하게 보여준다.
/// <c>Value</c>/<c>Maximum</c> 사용법은 표준 ProgressBar와 같다.
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
[DefaultProperty(nameof(Value))]
public class LinearProgressBar : Control
{
    private int m_iValue;
    private int m_iMaximum = 100;
    private Color m_oTrackColor = Color.FromArgb(222, 228, 238);
    private Color m_oProgressStartColor = Color.FromArgb(0, 168, 120);
    private Color m_oProgressEndColor = Color.FromArgb(34, 197, 94);

    public LinearProgressBar()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        Size = new Size(200, 20);
        Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
    }

    [Category("Linear Progress Bar")]
    [DefaultValue(0)]
    public int Value
    {
        get => m_iValue;
        set { m_iValue = Math.Clamp(value, 0, m_iMaximum); Invalidate(); }
    }

    [Category("Linear Progress Bar")]
    [DefaultValue(100)]
    public int Maximum
    {
        get => m_iMaximum;
        set { m_iMaximum = Math.Max(1, value); Value = Math.Min(m_iValue, m_iMaximum); Invalidate(); }
    }

    // 기본값이 코드에서 정한 색이라 디자이너 직렬화 대상에서 뺀다(CardPanel/CircularProgressBar와 동일).
    [Category("Linear Progress Bar")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color TrackColor
    {
        get => m_oTrackColor;
        set { m_oTrackColor = value; Invalidate(); }
    }

    [Category("Linear Progress Bar")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ProgressStartColor
    {
        get => m_oProgressStartColor;
        set { m_oProgressStartColor = value; Invalidate(); }
    }

    [Category("Linear Progress Bar")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ProgressEndColor
    {
        get => m_oProgressEndColor;
        set { m_oProgressEndColor = value; Invalidate(); }
    }

    /// <summary>
    /// 트랙(바탕) → 진행 구간(그라데이션) → 퍼센트 글자 순으로 그린다.
    /// 진행 구간은 트랙 모양으로 클리핑해서 양 끝이 항상 둥글게 보이게 한다.
    /// </summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        // [Claude - 2026.10.08] 선형 진행바 직접 그리기
        var graphics = e.Graphics;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.Clear(Parent?.BackColor ?? ColorRGB.Background);

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        if (bounds.Width <= 0 || bounds.Height <= 0) return;
        var radius = bounds.Height / 2;

        using var trackPath = ColorRGB.CreateRoundedPath(bounds, radius);
        using (var trackBrush = new SolidBrush(m_oTrackColor))
            graphics.FillPath(trackBrush, trackPath);

        var fillWidth = (int)Math.Round(bounds.Width * (double)m_iValue / m_iMaximum);
        if (fillWidth > 0)
        {
            var fillRect = new Rectangle(bounds.X, bounds.Y, Math.Max(fillWidth, bounds.Height), bounds.Height);
            var state = graphics.Save();
            graphics.SetClip(trackPath, CombineMode.Intersect);
            using (var fillBrush = new LinearGradientBrush(fillRect, m_oProgressStartColor, m_oProgressEndColor, LinearGradientMode.Horizontal))
            using (var fillPath = ColorRGB.CreateRoundedPath(fillRect, radius))
                graphics.FillPath(fillBrush, fillPath);
            graphics.Restore(state);
        }

        var percent = (int)Math.Round(100.0 * m_iValue / m_iMaximum);
        var text = string.Format("{0}%", percent);
        // 채움이 글자 중앙을 넘으면 흰 글자, 아니면 어두운 글자로 항상 읽히게 한다.
        var textColor = fillWidth > bounds.Width / 2 ? Color.White : ColorRGB.Text;
        TextRenderer.DrawText(graphics, text, Font, bounds, textColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        using var borderPen = new Pen(Color.FromArgb(200, 208, 220), 1);
        graphics.DrawPath(borderPen, trackPath);
    }
}
