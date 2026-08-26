namespace PC_BackUp;

/// <summary>
/// 어떤 메뉴가 현재 활성 상태인지 추적하고, 사이드바 강조 표시 + 화면 전환을 조율한다.
/// (구 MainForm._pagesByTag / OnMainMenuClicked 로직 추출)
/// </summary>
public sealed class ActiveMenuManager
{
    private readonly Panel m_oContentPanel;
    private readonly MainMenuControl m_oMenu;
    private readonly Dictionary<MainMenuId, UserControl> m_oPages = new();
    private MainMenuId? m_eCurrentId;

    public ActiveMenuManager(Panel contentPanel, MainMenuControl menu)
    {
        m_oContentPanel = contentPanel;
        m_oMenu = menu;
        m_oMenu.MenuItemClicked += (_, id) => Select(id);
    }

    public void Register(MainMenuId id, UserControl page) => m_oPages[id] = page;

    public void Select(MainMenuId id)
    {
        if (!m_oPages.TryGetValue(id, out var page))
            return;

        m_oMenu.SetActive(id);

        if (m_eCurrentId == id)
        {
            ControlPresenter.Refresh(m_oContentPanel, page);
            return;
        }

        m_eCurrentId = id;
        ControlPresenter.Show(m_oContentPanel, page);
    }
}
