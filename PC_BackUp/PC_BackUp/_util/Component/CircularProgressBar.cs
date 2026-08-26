using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>
/// 재사용 가능한 원형 진행률 표시 컴포넌트. 현재는 부품만 준비해 두고 화면에는
/// 기존 방식(선형 ProgressBar)을 계속 사용한다 — 필요해지면 바로 가져다 쓸 수 있다.
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
[DefaultProperty(nameof(Value))]
public partial class CircularProgressBar : UserControl
{
    private int m_iValue;
    private int m_iMaximum = 100;
    private int m_iThickness = 8;
    private Color m_oTrackColor = ColorRGB.Border;
    private Color m_oProgressColor = ColorRGB.Primary;

    public CircularProgressBar()
    {
        InitializeComponent();
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
    }

    [Category("Circular Progress Bar")]
    [DefaultValue(0)]
    public int Value
    {
        get => m_iValue;
        set { m_iValue = Math.Clamp(value, 0, m_iMaximum); Invalidate(); }
    }

    [Category("Circular Progress Bar")]
    [DefaultValue(100)]
    public int Maximum
    {
        get => m_iMaximum;
        set { m_iMaximum = Math.Max(1, value); Value = Math.Min(m_iValue, m_iMaximum); Invalidate(); }
    }

    [Category("Circular Progress Bar")]
    [DefaultValue(8)]
    public int Thickness
    {
        get => m_iThickness;
        set { m_iThickness = Math.Max(1, value); Invalidate(); }
    }

    // 기본값이 ColorRGB 테마 색상이라 디자이너 속성 창에서 개별 편집/직렬화 대상으로 삼지
    // 않는다(WFO1000 대응) — 필요하면 코드에서 그대로 값을 설정할 수 있다.
    [Category("Circular Progress Bar")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color TrackColor
    {
        get => m_oTrackColor;
        set { m_oTrackColor = value; Invalidate(); }
    }

    [Category("Circular Progress Bar")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ProgressColor
    {
        get => m_oProgressColor;
        set { m_oProgressColor = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var rect = new Rectangle(m_iThickness / 2, m_iThickness / 2, Width - m_iThickness, Height - m_iThickness);
        if (rect.Width <= 0 || rect.Height <= 0)
            return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using var trackPen = new Pen(m_oTrackColor, m_iThickness) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        e.Graphics.DrawEllipse(trackPen, rect);

        var sweep = m_iMaximum == 0 ? 0 : 360f * m_iValue / m_iMaximum;
        if (sweep <= 0)
            return;

        using var progressPen = new Pen(m_oProgressColor, m_iThickness) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        e.Graphics.DrawArc(progressPen, rect, -90f, sweep);
    }
}
