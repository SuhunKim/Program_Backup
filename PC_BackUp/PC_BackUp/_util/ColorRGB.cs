using System.Drawing.Drawing2D;

namespace PC_BackUp;

/// <summary>
/// 앱 전역에서 쓰는 색상 팔레트와 공용 UI 헬퍼. (구 UiTheme.cs)
/// 예전에는 보안/백신 프로그램(V3, 알약 등)에서 흔히 쓰는 파스텔 블루 라이트 테마였으나,
/// 화면 전체가 블루로 물드는 느낌이 촌스럽다는 피드백에 따라 중립 그레이 캔버스 +
/// 흰색 카드 + 포인트로만 쓰는 블루로 리프레시했다(2026-08-26).
/// 연두/민트 톤은 피하고, "안전함"을 나타내는 안내 영역에만 제한적으로 그린을 쓴다.
/// </summary>
internal static class ColorRGB
{
    public static readonly Color Background = Color.FromArgb(246, 247, 249);
    public static readonly Color Surface = Color.White;
    public static readonly Color Sidebar = Color.FromArgb(240, 241, 244);
    public static readonly Color SidebarHover = Color.FromArgb(231, 233, 237);
    public static readonly Color SidebarActive = Color.FromArgb(224, 231, 241);
    public static readonly Color Primary = Color.FromArgb(30, 100, 199);
    public static readonly Color PrimaryHover = Color.FromArgb(24, 83, 168);
    public static readonly Color PrimaryPressed = Color.FromArgb(19, 68, 140);
    public static readonly Color Text = Color.FromArgb(28, 32, 41);
    public static readonly Color MutedText = Color.FromArgb(108, 115, 128);
    public static readonly Color Border = Color.FromArgb(224, 226, 231);

    // 카드/버튼 모서리 반경. 라운드 사각형을 그리는 모든 컴포넌트가 이 값을 공유해서
    // 반경을 한 군데서만 조정할 수 있게 한다.
    public const int CardRadius = 10;
    public const int ButtonRadius = 8;

    // 보조(Secondary) 버튼류에 쓰는 옅은 블루 틴트 — 이전에는 각 화면에서 개별적으로
    // 하드코딩돼 있던 값들을 여기 하나로 모았다.
    public static readonly Color SecondaryBackground = Color.FromArgb(233, 238, 245);
    public static readonly Color SecondaryHover = Color.FromArgb(220, 228, 239);
    public static readonly Color SecondaryPressed = Color.FromArgb(204, 215, 231);

    // 삭제/위험 동작(예: 선별 폴더 행 제거)에 쓰는 옅은 레드 틴트.
    public static readonly Color DangerBackground = Color.FromArgb(250, 235, 235);

    // 표/목록 헤더 배경.
    public static readonly Color GridHeaderBackground = Color.FromArgb(238, 240, 244);

    // 카드 안의 보조 패널(예: 복원 화면 하단 상세 정보) 배경.
    public static readonly Color DetailPanelBackground = Color.FromArgb(247, 248, 250);

    // "안전하게 복원됩니다" 같은 안내/체크 영역에만 쓰는 그린 — 전체 테마색이 아니라
    // 의미상 "안전/완료"를 나타낼 때만 국소적으로 사용한다.
    public static readonly Color SafetyBackground = Color.FromArgb(232, 246, 237);
    public static readonly Color SafetyBorder = Color.FromArgb(199, 232, 212);
    public static readonly Color SafetyText = Color.FromArgb(27, 122, 76);
    public static readonly Color SafetyIcon = Color.FromArgb(45, 143, 96);

    // 이력 관리 화면의 XML 비교 그리드에서 "현재"/"백업" 값 중 실제로 다른 부분만 형광펜처럼
    // 강조할 때 쓴다(TextDiff 참고) — 빨강 계열: 현재 값에서 달라지는 부분, 노랑 계열: 백업
    // 값으로 바뀌는 부분.
    public static readonly Color DiffRemovedBackground = Color.FromArgb(253, 218, 218);
    public static readonly Color DiffRemovedText = Color.FromArgb(153, 27, 27);
    public static readonly Color DiffAddedBackground = Color.FromArgb(253, 235, 180);
    public static readonly Color DiffAddedText = Color.FromArgb(120, 84, 6);

    /// <summary>
    /// 아이콘 배지 + 캡션 + 값으로 이뤄진 요약 카드(예: "선택된 백업 날짜 · 2026년 8월 20일").
    /// caption/value 라벨은 호출한 쪽에서 만들어 전달하므로, 나중에 값이 바뀌면 그 참조로 갈아 끼우면 된다.
    /// </summary>
    public static Control CreateStatCard(Control icon, Label caption, Label value)
    {
        // Panel.Padding은 Dock/Anchor로 늘어나는 자식에만 적용되고, 아래처럼 Location으로
        // 직접 배치하는 자식에는 적용되지 않는다. 그래서 여백을 Padding이 아니라 좌표 계산에 직접 넣는다.
        const int pad = 16;
        var card = new CardPanel { BackColor = Surface };
        icon.Location = new Point(pad, pad - 2);
        caption.AutoSize = true;
        caption.Font = new Font("맑은 고딕", 9F);
        caption.ForeColor = MutedText;
        caption.Location = new Point(icon.Right + 12, pad - 4);
        value.AutoSize = true;
        value.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
        value.ForeColor = Text;
        value.Location = new Point(icon.Right + 12, pad + 14);
        card.Controls.Add(value);
        card.Controls.Add(caption);
        card.Controls.Add(icon);
        return card;
    }

    /// <summary>
    /// 지정한 사각형을 반경 <paramref name="radius"/>인 둥근 사각형 경로로 만든다.
    /// <see cref="CardPanel"/>과 <see cref="StyledButton"/>이 이 헬퍼를 공유해서,
    /// Region 클리핑(모서리 밖은 부모가 그대로 비쳐 보이는 방식)으로 라운드 모서리를
    /// 표현한다 — 안티앨리어싱은 없지만 반경 8~12px 범위에서는 충분히 매끄럽다.
    /// </summary>
    public static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
    {
        var path = new GraphicsPath();
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return path;

        var diameter = Math.Max(1, radius * 2);
        if (diameter >= bounds.Width || diameter >= bounds.Height)
        {
            path.AddRectangle(bounds);
            return path;
        }

        var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
        path.AddArc(arc, 180, 90);
        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);
        path.CloseFigure();
        return path;
    }
}
