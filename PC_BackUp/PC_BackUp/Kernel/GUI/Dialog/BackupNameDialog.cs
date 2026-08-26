using PC_BackUp.Services;

namespace PC_BackUp;

/// <summary>
/// 백업 실행 전 파일명을 확인하는 모달 창. 기본 파일명은 "yyyyMMdd - HHmm"이며,
/// 체크박스를 켤 때만 뒤에 프로젝트 이름 등을 덧붙일 수 있다(기본은 미포함).
/// Windows 파일명에 쓸 수 없는 문자는 입력하는 즉시 자동으로 제거된다(막지 않고 조용히 정리).
/// 같은 분에 두 번째 이후로 만들 때 붙는 "- 01" 같은 순번은 실제 저장 시점에 결정되므로 미리보기에는 표시하지 않는다.
/// 01_BackupControl(전체 백업)과 03_HistoryControl(즉시 백업) 양쪽에서 재사용한다.
/// </summary>
public sealed class BackupNameDialog : Form
{
    private readonly CheckBox m_oIncludeSuffixCheck = new();
    private readonly TextBox m_oSuffixText = new();
    private readonly Label m_oPreviewLabel = new();
    private readonly StyledButton m_oConfirmButton = new() { Text = "백업 시작" };
    private readonly BackupKind m_eKind;

    public string Suffix { get; private set; } = string.Empty;

    public BackupNameDialog(string suggestedSuffix, BackupKind kind)
    {
        m_eKind = kind;
        Text = "백업 파일명 확인";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(440, 208);
        BackColor = ColorRGB.Surface;
        Font = new Font("맑은 고딕", 9F);

        var titleLabel = new Label
        {
            Text = "백업 파일명은 날짜와 시간(yyyyMMdd - HHmm)으로 자동 지정됩니다.",
            Dock = DockStyle.Top,
            Height = 44,
            Padding = new Padding(16, 12, 16, 0),
            ForeColor = ColorRGB.Text
        };

        var body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 4, 16, 0) };

        m_oIncludeSuffixCheck.Text = "파일명에 프로젝트명 추가";
        m_oIncludeSuffixCheck.Dock = DockStyle.Top;
        m_oIncludeSuffixCheck.Height = 26;
        m_oIncludeSuffixCheck.ForeColor = ColorRGB.Text;
        m_oIncludeSuffixCheck.Checked = false;
        m_oIncludeSuffixCheck.CheckedChanged += (_, _) =>
        {
            m_oSuffixText.Enabled = m_oIncludeSuffixCheck.Checked;
            UpdatePreview();
        };

        m_oSuffixText.Dock = DockStyle.Top;
        m_oSuffixText.Font = new Font("맑은 고딕", 11F);
        m_oSuffixText.Text = suggestedSuffix;
        m_oSuffixText.Enabled = false;
        m_oSuffixText.TextChanged += (_, _) => UpdatePreview();

        m_oPreviewLabel.Dock = DockStyle.Top;
        m_oPreviewLabel.Height = 28;
        m_oPreviewLabel.ForeColor = ColorRGB.MutedText;
        m_oPreviewLabel.Padding = new Padding(0, 10, 0, 0);

        body.Controls.Add(m_oPreviewLabel);
        body.Controls.Add(m_oSuffixText);
        body.Controls.Add(m_oIncludeSuffixCheck);

        var actionPanel = new Panel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(16, 10, 16, 14) };
        var cancelButton = new StyledButton
        {
            ButtonType = StyledButtonType.Secondary,
            Text = "취소",
            Dock = DockStyle.Right,
            Width = 100
        };
        m_oConfirmButton.Dock = DockStyle.Right;
        m_oConfirmButton.Width = 120;
        m_oConfirmButton.Margin = new Padding(0, 0, 10, 0);
        cancelButton.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
        m_oConfirmButton.Click += UiClick_Confirm;
        actionPanel.Controls.Add(m_oConfirmButton);
        actionPanel.Controls.Add(cancelButton);

        Controls.Add(body);
        Controls.Add(actionPanel);
        Controls.Add(titleLabel);

        AcceptButton = m_oConfirmButton;
        CancelButton = cancelButton;

        UpdatePreview();
    }

    private void UiClick_Confirm(object? sender, EventArgs e)
    {
        Suffix = m_oIncludeSuffixCheck.Checked ? FileNaming.SanitizeSuffix(m_oSuffixText.Text) : string.Empty;
        DialogResult = DialogResult.OK;
        Close();
    }

    private void UpdatePreview()
    {
        var suffix = m_oIncludeSuffixCheck.Checked ? FileNaming.SanitizeSuffix(m_oSuffixText.Text) : string.Empty;
        m_oPreviewLabel.Text = string.Format("저장 이름: {0}", BackupService.PreviewFileName(suffix, m_eKind));
    }
}
