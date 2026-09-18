using System.ComponentModel;

namespace PC_BackUp;

/// <summary>
/// 좌측 사이드바(로고 + 4개 메뉴 버튼 + 버전 표시)를 캡슐화한 재사용 가능 컴포넌트.
/// MainForm은 이 컨트롤을 도킹만 하고, 실제 메뉴 버튼 구성/강조 로직은 여기서 관리한다.
/// </summary>
[ToolboxItem(true)]
[DesignTimeVisible(true)]
public partial class MainMenuControl : UserControl
{
    public event EventHandler<MainMenuId>? MenuItemClicked;

    /// <summary>
    /// 향후 메뉴 그룹이나 보조 메뉴를 배치할 수 있는 사이드바 확장 영역이다.
    /// </summary>
    internal Panel ExtensionMenuPanel => extensionMenuPanel;

    private readonly Dictionary<MainMenuId, StyledButton> m_oButtons = new();
    private StyledButton? m_oActiveButton;

    public MainMenuControl()
    {
        InitializeComponent();

        Register(MainMenuId.Dashboard, m_menuDashboard);
        Register(MainMenuId.Backup, m_menuBackup);
        Register(MainMenuId.Recovery, m_menuRecovery);
        Register(MainMenuId.History, m_menuHistory);
        Register(MainMenuId.Settings, m_menuSettings);
    }

    public void SetActive(MainMenuId id)
    {
        if (m_oActiveButton is not null)
            m_oActiveButton.IsActive = false;

        if (m_oButtons.TryGetValue(id, out var button))
        {
            button.IsActive = true;
            m_oActiveButton = button;
        }
    }

    private void Register(MainMenuId id, StyledButton button)
    {
        m_oButtons[id] = button;
        button.Tag = id;
        button.ButtonType = StyledButtonType.Sidebar;
        button.Click += (_, _) => MenuItemClicked?.Invoke(this, id);
    }
}
