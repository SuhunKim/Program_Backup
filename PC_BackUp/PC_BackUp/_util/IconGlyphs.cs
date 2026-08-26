using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>
/// 이미지 리소스 없이 GDI+로 직접 그리는 아주 단순한 선(line) 아이콘 모음.
/// 요약 카드의 동그란 배지 안에 들어가는 캘린더/보관함/체크 표시 정도만 필요해서
/// 아이콘 폰트나 이미지 파일을 추가하는 대신 이 정도로 충분하다고 판단했다.
/// </summary>
internal static class IconGlyphs
{
    /// <summary>지정한 크기의 원형 배지를 만들고, 그 안에 glyph를 그린다.</summary>
    public static Control CreateBadge(int size, Color background, Color foreground, Action<Graphics, RectangleF, Color> glyph)
    {
        var panel = new Panel { Size = new Size(size, size) };
        panel.Paint += (_, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(background);
            e.Graphics.FillEllipse(brush, 0, 0, size - 1, size - 1);

            var inset = size * 0.27f;
            var bounds = new RectangleF(inset, inset, size - inset * 2, size - inset * 2);
            glyph(e.Graphics, bounds, foreground);
        };
        return panel;
    }

    public static void Calendar(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f);
        var body = new RectangleF(bounds.X, bounds.Y + bounds.Height * 0.15f, bounds.Width, bounds.Height * 0.75f);
        g.DrawRectangle(pen, body.X, body.Y, body.Width, body.Height);
        g.DrawLine(pen, body.X, body.Y + body.Height * 0.4f, body.Right, body.Y + body.Height * 0.4f);
        g.DrawLine(pen, bounds.X + bounds.Width * 0.25f, bounds.Y, bounds.X + bounds.Width * 0.25f, body.Y + body.Height * 0.25f);
        g.DrawLine(pen, bounds.X + bounds.Width * 0.75f, bounds.Y, bounds.X + bounds.Width * 0.75f, body.Y + body.Height * 0.25f);
    }

    public static void Archive(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f);
        var lidHeight = bounds.Height * 0.28f;
        g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, lidHeight);
        var body = new RectangleF(bounds.X + bounds.Width * 0.08f, bounds.Y + lidHeight, bounds.Width * 0.84f, bounds.Height - lidHeight);
        g.DrawRectangle(pen, body.X, body.Y, body.Width, body.Height);
        var slotY = body.Y + body.Height * 0.32f;
        g.DrawLine(pen, bounds.X + bounds.Width * 0.35f, slotY, bounds.X + bounds.Width * 0.65f, slotY);
    }

    public static void Check(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        g.DrawLines(pen, new[]
        {
            new PointF(bounds.X, bounds.Y + bounds.Height * 0.55f),
            new PointF(bounds.X + bounds.Width * 0.38f, bounds.Bottom),
            new PointF(bounds.Right, bounds.Y + bounds.Height * 0.15f),
        });
    }
}
