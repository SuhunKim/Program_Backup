namespace PC_BackUp;

/// <summary>
/// 사이드바 메뉴로 전환되는 화면(01~04 Control)들의 공통 베이스.
/// IMenuPage 구현과 공통 배경색 적용을 여기서 한 번만 처리한다.
/// </summary>
public class UserControlBase : UserControl, IMenuPage
{
    protected UserControlBase()
    {
        BackColor = ColorRGB.Background;
    }

    public virtual void OnMenuSelected()
    {
    }
}
