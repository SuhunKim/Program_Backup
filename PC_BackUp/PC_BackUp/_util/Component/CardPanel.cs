using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>
/// 화면 곳곳의 "카드"로 쓰는 흰색 패널. 기존에는 각 화면이 평범한 <see cref="Panel"/>에
/// BackColor만 흰색으로 칠해서 썼는데(모서리가 직각, 테두리/여백 없음), 그러면 카드끼리
/// 서로 맞닿아 배경과 경계가 흐릿하게 보인다. 이 컨트롤은 <see cref="ColorRGB.CardRadius"/>
/// 반경의 둥근 사각형으로 Region을 클리핑하고, 같은 경로로 1px 테두리를 그려 카드 경계를
/// 분명하게 만든다.
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
public class CardPanel : Panel
{
    public CardPanel()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = ColorRGB.Surface;
    }

    /// <summary>카드 테두리 색. 배경과 구분이 필요 없는 곳(예: 요약 카드)에서는 Transparent로 끌 수 있다.</summary>
    [Category("Card Panel")]
    public Color BorderColor { get; set; } = ColorRGB.Border;

    [Category("Card Panel")]
    [DefaultValue(ColorRGB.CardRadius)]
    public int Radius { get; set; } = ColorRGB.CardRadius;

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        ApplyRegion();
    }

    private void ApplyRegion()
    {
        using var path = ColorRGB.CreateRoundedPath(new Rectangle(0, 0, Width, Height), Radius);
        Region?.Dispose();
        Region = new Region(path);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
        using var path = ColorRGB.CreateRoundedPath(bounds, Radius);
        using (var brush = new SolidBrush(BackColor))
            e.Graphics.FillPath(brush, path);

        if (BorderColor != Color.Transparent)
        {
            using var pen = new Pen(BorderColor, 1);
            e.Graphics.DrawPath(pen, path);
        }

        base.OnPaint(e);
    }
}
