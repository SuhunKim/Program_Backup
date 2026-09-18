using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>
/// 이미지 리소스 없이 GDI+로 직접 그리는 아주 단순한 선(line) 아이콘 모음.
/// 요약 카드의 동그란 배지 안에 들어가는 아이콘을 제공한다.
/// 아이콘 폰트나 이미지 파일 대신 GDI+ 선으로 그린다.
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

    public static void Storage(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f);
        using var brush = new SolidBrush(color);
        var rowHeight = bounds.Height / 3f;
        for (var row = 0; row < 3; row++)
        {
            var y = bounds.Y + row * rowHeight;
            g.DrawRectangle(pen, bounds.X, y, bounds.Width, rowHeight);
            var indicator = rowHeight * 0.22f;
            g.FillEllipse(brush, bounds.Right - indicator * 1.8f, y + (rowHeight - indicator) / 2f, indicator, indicator);
        }
    }

    public static void File(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f) { LineJoin = LineJoin.Round };
        var fold = bounds.Width * 0.32f;
        g.DrawLines(pen, new[]
        {
            new PointF(bounds.X, bounds.Y), new PointF(bounds.Right - fold, bounds.Y),
            new PointF(bounds.Right, bounds.Y + fold), new PointF(bounds.Right, bounds.Bottom),
            new PointF(bounds.X, bounds.Bottom), new PointF(bounds.X, bounds.Y),
        });
        g.DrawLines(pen, new[]
        {
            new PointF(bounds.Right - fold, bounds.Y), new PointF(bounds.Right - fold, bounds.Y + fold),
            new PointF(bounds.Right, bounds.Y + fold),
        });
        var lineY = bounds.Y + bounds.Height * 0.62f;
        g.DrawLine(pen, bounds.X + bounds.Width * 0.22f, lineY, bounds.Right - bounds.Width * 0.22f, lineY);
        g.DrawLine(pen, bounds.X + bounds.Width * 0.22f, lineY + bounds.Height * 0.16f, bounds.Right - bounds.Width * 0.35f, lineY + bounds.Height * 0.16f);
    }

    public static void Button(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f) { LineJoin = LineJoin.Round };
        using var brush = new SolidBrush(color);
        var button = new RectangleF(bounds.X, bounds.Y + bounds.Height * 0.18f, bounds.Width, bounds.Height * 0.64f);
        g.DrawRectangle(pen, button.X, button.Y, button.Width, button.Height);
        var dotSize = bounds.Height * 0.16f;
        g.FillEllipse(brush, button.X + button.Width * 0.5f - dotSize / 2f, button.Y + button.Height * 0.5f - dotSize / 2f, dotSize, dotSize);
    }

    public static void Clock(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        g.DrawEllipse(pen, bounds);
        var center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
        g.DrawLine(pen, center, new PointF(center.X, bounds.Y + bounds.Height * 0.27f));
        g.DrawLine(pen, center, new PointF(bounds.Right - bounds.Width * 0.23f, center.Y + bounds.Height * 0.15f));
    }

    public static void RestoreArrow(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        g.DrawArc(pen, bounds.X + bounds.Width * 0.08f, bounds.Y + bounds.Height * 0.08f, bounds.Width * 0.84f, bounds.Height * 0.84f, -40f, 270f);
        g.DrawLines(pen, new[]
        {
            new PointF(bounds.X + bounds.Width * 0.33f, bounds.Y + bounds.Height * 0.13f),
            new PointF(bounds.X + bounds.Width * 0.03f, bounds.Y + bounds.Height * 0.42f),
            new PointF(bounds.X + bounds.Width * 0.38f, bounds.Y + bounds.Height * 0.67f),
        });
    }

    public static void History(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        var arcBounds = new RectangleF(bounds.X + bounds.Width * 0.1f, bounds.Y + bounds.Height * 0.1f, bounds.Width * 0.8f, bounds.Height * 0.8f);
        g.DrawArc(pen, arcBounds, -30f, 285f);
        g.DrawLines(pen, new[]
        {
            new PointF(bounds.X + bounds.Width * 0.28f, bounds.Y + bounds.Height * 0.1f),
            new PointF(bounds.X, bounds.Y + bounds.Height * 0.36f),
            new PointF(bounds.X + bounds.Width * 0.31f, bounds.Y + bounds.Height * 0.59f),
        });
        var center = new PointF(bounds.X + bounds.Width / 2f, bounds.Y + bounds.Height / 2f);
        g.DrawLine(pen, center, new PointF(center.X, bounds.Y + bounds.Height * 0.31f));
        g.DrawLine(pen, center, new PointF(bounds.Right - bounds.Width * 0.28f, center.Y));
    }

    public static void Folder(Graphics g, RectangleF bounds, Color color)
    {
        using var pen = new Pen(color, 1.6f) { LineJoin = LineJoin.Round };
        var tabHeight = bounds.Height * 0.28f;
        var tabWidth = bounds.Width * 0.42f;
        g.DrawLines(pen, new[]
        {
            new PointF(bounds.X, bounds.Bottom), new PointF(bounds.X, bounds.Y + tabHeight),
            new PointF(bounds.X + tabWidth, bounds.Y + tabHeight), new PointF(bounds.X + tabWidth + bounds.Width * 0.1f, bounds.Y),
            new PointF(bounds.Right, bounds.Y), new PointF(bounds.Right, bounds.Bottom), new PointF(bounds.X, bounds.Bottom),
        });
    }
}
