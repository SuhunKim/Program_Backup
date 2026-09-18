namespace PC_BackUp;

/// <summary>
/// 메뉴 화면과 Main 폼에서 공유하는 레이아웃 기준값이다.
/// </summary>
internal static class PageLayoutMetrics
{
    // [Codex - 2026.09.18] 메뉴 전환 화면의 좌표/여백 기준을 한 곳으로 통일한다.
    public const int FormWidth = 1280;
    public const int FormHeight = 800;
    public const int MinimumFormWidth = 1080;
    public const int MinimumFormHeight = 700;
    public const int SidebarWidth = 220;
    public const int HeaderHeight = 66;
    public const int ContentHorizontalPadding = 30;
    public const int ContentTopPadding = 24;
    public const int ContentBottomPadding = 28;
    public const int CardGap = 16;
    public const int CardPadding = 16;
    public const int FormLabelWidth = 180;
    public const int FormRowHeight = 72;
    public const int ActionBarHeight = 68;
    public const int PrimaryButtonWidth = 160;
    public const int SecondaryButtonWidth = 100;
}
