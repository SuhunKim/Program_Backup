namespace PC_BackUp;

/// <summary>
/// 콘텐츠 패널에 선택된 메뉴 화면을 표시하는 역할만 담당한다.
/// (구 MainForm.ChangeMenu 로직 추출)
/// </summary>
internal static class ControlPresenter
{
    public static void Show(Panel host, UserControl page)
    {
        host.SuspendLayout();
        try
        {
            page.Dock = DockStyle.Fill;
            host.Controls.Clear();
            host.Controls.Add(page);
            if (page is IMenuPage menuPage)
                menuPage.OnMenuSelected();
        }
        catch (Exception exception)
        {
            MessageBox.Show(host.FindForm(), exception.Message, "메뉴 전환 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            host.ResumeLayout(true);
        }
    }

    /// <summary>
    /// 이미 표시 중인(활성) 메뉴를 다시 클릭했을 때 화면 재구성 없이 새로고침만 한다.
    /// Show()와 동일하게 예외를 잡아 메시지로 보여주므로, 새로고침 중 오류가 나도 앱이 죽지 않는다.
    /// </summary>
    public static void Refresh(Panel host, UserControl page)
    {
        if (page is not IMenuPage menuPage)
            return;

        try
        {
            menuPage.OnMenuSelected();
        }
        catch (Exception exception)
        {
            MessageBox.Show(host.FindForm(), exception.Message, "메뉴 새로고침 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
