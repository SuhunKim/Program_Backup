namespace PC_BackUp;

partial class BackupControl
{
    private System.ComponentModel.IContainer? components;

    // 화면의 고정 골격(패널/카드/제목) — 값이 바뀌지 않는 부분이라 전부 필드로 선언해
    // Visual Studio 디자이너에서도 그대로 열어서 확인할 수 있게 한다.
    private Panel body = null!;
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel statusCard = null!;
    private TableLayoutPanel table = null!;
    private Label projectCaptionLabel = null!, sourceCaptionLabel = null!, destinationCaptionLabel = null!;
    private CardPanel optionCard = null!;
    private Label optionTitle = null!;
    private Panel action = null!;
    // 카드끼리 맞닿지 않도록 사이에 끼우는 여백용 스페이서(배경색으로만 채운 빈 패널).
    private Panel actionOptionGap = null!;
    private Panel optionStatusGap = null!;

    private Label projectValueLabel = null!, sourceValueLabel = null!, destinationValueLabel = null!, statusLabel = null!;
    private RadioButton fullZipRadioButton = null!, selectiveRadioButton = null!;
    private PC_BackUp.StyledButton backupButton = null!;
    private PC_BackUp.StyledButton cancelButton = null!;
    private ProgressBar progressBar = null!;

    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        body = new Panel();
        headerPanel = new Panel();
        headerTitleLabel = new Label();
        headerDescriptionLabel = new Label();
        statusCard = new CardPanel();
        table = new TableLayoutPanel();
        projectCaptionLabel = new Label();
        sourceCaptionLabel = new Label();
        destinationCaptionLabel = new Label();
        optionCard = new CardPanel();
        optionTitle = new Label();
        action = new Panel();
        actionOptionGap = new Panel();
        optionStatusGap = new Panel();
        projectValueLabel = new Label();
        sourceValueLabel = new Label();
        destinationValueLabel = new Label();
        statusLabel = new Label();
        fullZipRadioButton = new RadioButton();
        selectiveRadioButton = new RadioButton();
        backupButton = new PC_BackUp.StyledButton();
        cancelButton = new PC_BackUp.StyledButton();
        progressBar = new ProgressBar();
        SuspendLayout();
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 66;
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerTitleLabel.Text = "백업";
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Height = 31;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = ColorRGB.Text;
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        headerDescriptionLabel.Text = "현재 프로그램 상태를 ZIP 또는 선택 폴더로 안전하게 보관합니다.";
        headerDescriptionLabel.Dock = DockStyle.Top;
        headerDescriptionLabel.Height = 24;
        headerDescriptionLabel.Font = new Font("맑은 고딕", 10F);
        headerDescriptionLabel.ForeColor = ColorRGB.MutedText;
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        //
        // body
        //
        body.Dock = DockStyle.Fill;
        body.AutoScroll = true;
        body.BackColor = ColorRGB.Background;
        //
        // statusCard (프로젝트/원본 위치/백업 위치 요약)
        //
        statusCard.Dock = DockStyle.Top;
        statusCard.Height = 132;
        statusCard.BackColor = ColorRGB.Surface;
        table.Dock = DockStyle.Fill;
        table.Padding = new Padding(22, 12, 22, 12);
        table.ColumnCount = 2;
        table.RowCount = 3;
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        projectCaptionLabel.Text = "프로젝트";
        projectCaptionLabel.Dock = DockStyle.Fill;
        projectCaptionLabel.ForeColor = ColorRGB.MutedText;
        projectCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        sourceCaptionLabel.Text = "원본 위치";
        sourceCaptionLabel.Dock = DockStyle.Fill;
        sourceCaptionLabel.ForeColor = ColorRGB.MutedText;
        sourceCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        destinationCaptionLabel.Text = "백업 위치";
        destinationCaptionLabel.Dock = DockStyle.Fill;
        destinationCaptionLabel.ForeColor = ColorRGB.MutedText;
        destinationCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        projectValueLabel.Dock = DockStyle.Fill;
        projectValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        projectValueLabel.AutoEllipsis = true;
        sourceValueLabel.Dock = DockStyle.Fill;
        sourceValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        sourceValueLabel.AutoEllipsis = true;
        destinationValueLabel.Dock = DockStyle.Fill;
        destinationValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        destinationValueLabel.AutoEllipsis = true;
        table.Controls.Add(projectCaptionLabel, 0, 0);
        table.Controls.Add(projectValueLabel, 1, 0);
        table.Controls.Add(sourceCaptionLabel, 0, 1);
        table.Controls.Add(sourceValueLabel, 1, 1);
        table.Controls.Add(destinationCaptionLabel, 0, 2);
        table.Controls.Add(destinationValueLabel, 1, 2);
        statusCard.Controls.Add(table);
        //
        // optionCard (백업 방식 선택)
        //
        optionCard.Dock = DockStyle.Top;
        optionCard.Height = 132;
        optionCard.BackColor = ColorRGB.Surface;
        optionCard.Padding = new Padding(22, 12, 22, 12);
        optionTitle.Text = "백업 방식";
        optionTitle.Dock = DockStyle.Top;
        optionTitle.Height = 26;
        optionTitle.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        fullZipRadioButton.Text = "전체 백업 — ZIP 압축 파일로 저장";
        fullZipRadioButton.Dock = DockStyle.Top;
        fullZipRadioButton.Height = 36;
        fullZipRadioButton.Checked = true;
        selectiveRadioButton.Text = "지정 폴더 선별 백업    ·    설정한 두 폴더 전체 복사";
        selectiveRadioButton.Dock = DockStyle.Top;
        selectiveRadioButton.Height = 36;
        optionCard.Controls.Add(selectiveRadioButton);
        optionCard.Controls.Add(fullZipRadioButton);
        optionCard.Controls.Add(optionTitle);
        //
        // action (백업 시작/취소 버튼 + 진행 바)
        //
        action.Dock = DockStyle.Top;
        action.Height = 72;
        action.Padding = new Padding(0, 12, 0, 0);
        progressBar.Dock = DockStyle.Top;
        progressBar.Height = 8;
        backupButton.Text = "백업 시작";
        backupButton.Dock = DockStyle.Right;
        backupButton.Width = 160;
        backupButton.Click += UiClick_Backup;
        cancelButton.ButtonType = StyledButtonType.Secondary;
        cancelButton.Text = "취소";
        cancelButton.Dock = DockStyle.Right;
        cancelButton.Width = 100;
        cancelButton.Margin = new Padding(0, 0, 10, 0);
        cancelButton.Visible = false;
        cancelButton.Click += UiClick_Cancel;
        statusLabel.Text = "환경 설정을 확인한 뒤 백업을 시작하세요.";
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.ForeColor = ColorRGB.MutedText;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        action.Controls.Add(statusLabel);
        action.Controls.Add(cancelButton);
        action.Controls.Add(backupButton);
        action.Controls.Add(progressBar);
        //
        // 카드 사이 여백 스페이서 — 배경색으로 채운 16px 빈 패널이라 카드끼리 맞닿지 않는다.
        //
        actionOptionGap.Dock = DockStyle.Top;
        actionOptionGap.Height = 16;
        actionOptionGap.BackColor = ColorRGB.Background;
        optionStatusGap.Dock = DockStyle.Top;
        optionStatusGap.Height = 16;
        optionStatusGap.BackColor = ColorRGB.Background;
        //
        // body에 순서대로 쌓기 (Dock=Top 세 개가 위에서부터 action → optionCard → statusCard 순으로 보이도록
        // 마지막에 추가한 게 제일 위로 온다)
        //
        body.Controls.Add(action);
        body.Controls.Add(actionOptionGap);
        body.Controls.Add(optionCard);
        body.Controls.Add(optionStatusGap);
        body.Controls.Add(statusCard);
        //
        // BackupControl
        //
        Controls.Add(body);
        Controls.Add(headerPanel);
        Name = "BackupControl";
        Size = new Size(1000, 700);
        ResumeLayout(false);
    }
}
