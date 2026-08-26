namespace PC_BackUp;

/// <summary>
/// 앱 전역에서 쓰는 색상 팔레트와 공용 UI 헬퍼. (구 UiTheme.cs)
/// 보안/백신 프로그램(V3, 알약 등)에서 흔히 쓰는 차분한 블루 계열 라이트 테마.
/// 연두/민트 톤은 피하고, "안전함"을 나타내는 안내 영역에만 제한적으로 그린을 쓴다.
/// </summary>
internal static class ColorRGB
{
    public static readonly Color Background = Color.FromArgb(240, 244, 249);
    public static readonly Color Surface = Color.White;
    public static readonly Color Sidebar = Color.FromArgb(230, 238, 247);
    public static readonly Color SidebarHover = Color.FromArgb(219, 230, 244);
    public static readonly Color SidebarActive = Color.FromArgb(207, 224, 245);
    public static readonly Color Primary = Color.FromArgb(33, 111, 189);
    public static readonly Color PrimaryHover = Color.FromArgb(26, 92, 161);
    public static readonly Color PrimaryPressed = Color.FromArgb(20, 75, 133);
    public static readonly Color Text = Color.FromArgb(30, 41, 59);
    public static readonly Color MutedText = Color.FromArgb(100, 116, 139);
    public static readonly Color Border = Color.FromArgb(214, 224, 236);

    // 보조(Secondary) 버튼류에 쓰는 옅은 블루 틴트 — 이전에는 각 화면에서 개별적으로
    // 하드코딩돼 있던 값들을 여기 하나로 모았다.
    public static readonly Color SecondaryBackground = Color.FromArgb(228, 236, 246);
    public static readonly Color SecondaryHover = Color.FromArgb(213, 226, 242);
    public static readonly Color SecondaryPressed = Color.FromArgb(196, 213, 235);

    // 삭제/위험 동작(예: 선별 폴더 행 제거)에 쓰는 옅은 레드 틴트.
    public static readonly Color DangerBackground = Color.FromArgb(250, 235, 235);

    // 표/목록 헤더 배경.
    public static readonly Color GridHeaderBackground = Color.FromArgb(224, 234, 246);

    // 카드 안의 보조 패널(예: 복원 화면 하단 상세 정보) 배경.
    public static readonly Color DetailPanelBackground = Color.FromArgb(244, 248, 252);

    // "안전하게 복원됩니다" 같은 안내/체크 영역에만 쓰는 그린 — 전체 테마색이 아니라
    // 의미상 "안전/완료"를 나타낼 때만 국소적으로 사용한다.
    public static readonly Color SafetyBackground = Color.FromArgb(232, 246, 237);
    public static readonly Color SafetyBorder = Color.FromArgb(199, 232, 212);
    public static readonly Color SafetyText = Color.FromArgb(27, 122, 76);
    public static readonly Color SafetyIcon = Color.FromArgb(45, 143, 96);

    /// <summary>
    /// 아이콘 배지 + 캡션 + 값으로 이뤄진 요약 카드(예: "선택된 백업 날짜 · 2026년 8월 20일").
    /// caption/value 라벨은 호출한 쪽에서 만들어 전달하므로, 나중에 값이 바뀌면 그 참조로 갈아 끼우면 된다.
    /// </summary>
    public static Control CreateStatCard(Control icon, Label caption, Label value)
    {
        // Panel.Padding은 Dock/Anchor로 늘어나는 자식에만 적용되고, 아래처럼 Location으로
        // 직접 배치하는 자식에는 적용되지 않는다. 그래서 여백을 Padding이 아니라 좌표 계산에 직접 넣는다.
        const int pad = 16;
        var card = new Panel { BackColor = Surface };
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
}
