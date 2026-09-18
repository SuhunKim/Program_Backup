using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>디자이너에서도 표시되는 원형 아이콘 배지의 종류.</summary>
public enum GlyphBadgeKind
{
    Calendar,
    Archive,
    Check,
    Storage,
    File,
    Button,
    Clock,
    RestoreArrow,
    History,
    Folder
}

/// <summary>
/// 요약 카드와 안내 카드에서 사용하는 원형 아이콘 배지.
/// 일반 컨트롤로 구현해 Visual Studio 디자이너에서 선택, 이동, 크기 조절할 수 있다.
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
public class GlyphBadge : Control
{
    private Color m_oBadgeColor = ColorRGB.SidebarActive;
    private Color m_oGlyphColor = ColorRGB.Primary;
    private GlyphBadgeKind m_eGlyph = GlyphBadgeKind.Calendar;

    public GlyphBadge()
    {
        // [Codex - 2026.09.14] 런타임 생성 배지를 디자이너 편집 가능한 컨트롤로 전환한다.
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                 ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Size = new Size(36, 36);
    }

    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color BadgeColor
    {
        get => m_oBadgeColor;
        set
        {
            m_oBadgeColor = value;
            Invalidate();
        }
    }

    [Category("Appearance")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color GlyphColor
    {
        get => m_oGlyphColor;
        set
        {
            m_oGlyphColor = value;
            Invalidate();
        }
    }

    [Category("Appearance")]
    [DefaultValue(GlyphBadgeKind.Calendar)]
    public GlyphBadgeKind Glyph
    {
        get => m_eGlyph;
        set
        {
            m_eGlyph = value;
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs eventArgs)
    {
        base.OnPaint(eventArgs);

        eventArgs.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var diameter = Math.Max(1, Math.Min(ClientSize.Width, ClientSize.Height) - 1);
        var badgeBounds = new RectangleF(
            (ClientSize.Width - diameter) / 2F,
            (ClientSize.Height - diameter) / 2F,
            diameter,
            diameter);

        using (var brush = new SolidBrush(m_oBadgeColor))
            eventArgs.Graphics.FillEllipse(brush, badgeBounds);

        var inset = diameter * 0.27F;
        var glyphBounds = new RectangleF(
            badgeBounds.X + inset,
            badgeBounds.Y + inset,
            badgeBounds.Width - inset * 2F,
            badgeBounds.Height - inset * 2F);

        switch (m_eGlyph)
        {
            case GlyphBadgeKind.Archive:
                IconGlyphs.Archive(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.Check:
                IconGlyphs.Check(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.Storage:
                IconGlyphs.Storage(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.File:
                IconGlyphs.File(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.Button:
                IconGlyphs.Button(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.Clock:
                IconGlyphs.Clock(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.RestoreArrow:
                IconGlyphs.RestoreArrow(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.History:
                IconGlyphs.History(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            case GlyphBadgeKind.Folder:
                IconGlyphs.Folder(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
            default:
                IconGlyphs.Calendar(eventArgs.Graphics, glyphBounds, m_oGlyphColor);
                break;
        }
    }
}
