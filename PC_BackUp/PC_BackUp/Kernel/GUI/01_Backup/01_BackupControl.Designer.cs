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
        action = new Panel();
        statusLabel = new Label();
        cancelButton = new StyledButton();
        backupButton = new StyledButton();
        progressBar = new ProgressBar();
        actionOptionGap = new Panel();
        optionCard = new CardPanel();
        selectiveRadioButton = new RadioButton();
        fullZipRadioButton = new RadioButton();
        optionTitle = new Label();
        optionStatusGap = new Panel();
        statusCard = new CardPanel();
        table = new TableLayoutPanel();
        projectCaptionLabel = new Label();
        projectValueLabel = new Label();
        sourceCaptionLabel = new Label();
        sourceValueLabel = new Label();
        destinationCaptionLabel = new Label();
        destinationValueLabel = new Label();
        headerPanel = new Panel();
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        body.SuspendLayout();
        action.SuspendLayout();
        optionCard.SuspendLayout();
        statusCard.SuspendLayout();
        table.SuspendLayout();
        headerPanel.SuspendLayout();
        SuspendLayout();
        // 
        // body
        // 
        body.AutoScroll = true;
        body.BackColor = Color.White;
        body.Controls.Add(action);
        body.Controls.Add(actionOptionGap);
        body.Controls.Add(optionCard);
        body.Controls.Add(optionStatusGap);
        body.Controls.Add(statusCard);
        body.Dock = DockStyle.Fill;
        body.Location = new Point(0, 66);
        body.Name = "body";
        body.Size = new Size(1000, 634);
        body.TabIndex = 0;
        // 
        // action
        // 
        action.Controls.Add(statusLabel);
        action.Controls.Add(cancelButton);
        action.Controls.Add(backupButton);
        action.Controls.Add(progressBar);
        action.Dock = DockStyle.Top;
        action.Location = new Point(0, 296);
        action.Name = "action";
        action.Padding = new Padding(0, 12, 0, 0);
        action.Size = new Size(1000, 72);
        action.TabIndex = 0;
        // 
        // statusLabel
        // 
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.ForeColor = Color.FromArgb(108, 115, 128);
        statusLabel.Location = new Point(0, 20);
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(740, 52);
        statusLabel.TabIndex = 0;
        statusLabel.Text = "환경 설정을 확인한 뒤 백업을 시작하세요.";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cancelButton
        // 
        cancelButton.BackColor = Color.FromArgb(233, 238, 245);
        cancelButton.BorderThickness = 1;
        cancelButton.ButtonType = StyledButtonType.Secondary;
        cancelButton.Dock = DockStyle.Right;
        cancelButton.Font = new Font("맑은 고딕", 9F);
        cancelButton.ForeColor = Color.FromArgb(28, 32, 41);
        cancelButton.Location = new Point(740, 20);
        cancelButton.Margin = new Padding(0, 0, 10, 0);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(100, 52);
        cancelButton.TabIndex = 1;
        cancelButton.Text = "취소";
        cancelButton.Visible = false;
        cancelButton.Click += UiClick_Cancel;
        // 
        // backupButton
        // 
        backupButton.BackColor = Color.FromArgb(30, 100, 199);
        backupButton.Dock = DockStyle.Right;
        backupButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        backupButton.ForeColor = Color.White;
        backupButton.Location = new Point(840, 20);
        backupButton.Name = "backupButton";
        backupButton.Size = new Size(160, 52);
        backupButton.TabIndex = 2;
        backupButton.Text = "백업 시작";
        backupButton.Click += UiClick_Backup;
        // 
        // progressBar
        // 
        progressBar.Dock = DockStyle.Top;
        progressBar.Location = new Point(0, 12);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(1000, 8);
        progressBar.TabIndex = 3;
        // 
        // actionOptionGap
        // 
        actionOptionGap.BackColor = Color.White;
        actionOptionGap.Dock = DockStyle.Top;
        actionOptionGap.Location = new Point(0, 280);
        actionOptionGap.Name = "actionOptionGap";
        actionOptionGap.Size = new Size(1000, 16);
        actionOptionGap.TabIndex = 1;
        // 
        // optionCard
        // 
        optionCard.BackColor = Color.White;
        optionCard.Controls.Add(selectiveRadioButton);
        optionCard.Controls.Add(fullZipRadioButton);
        optionCard.Controls.Add(optionTitle);
        optionCard.Dock = DockStyle.Top;
        optionCard.Location = new Point(0, 148);
        optionCard.Name = "optionCard";
        optionCard.Padding = new Padding(22, 12, 22, 12);
        optionCard.Size = new Size(1000, 132);
        optionCard.TabIndex = 2;
        // 
        // selectiveRadioButton
        // 
        selectiveRadioButton.Dock = DockStyle.Top;
        selectiveRadioButton.Location = new Point(22, 74);
        selectiveRadioButton.Name = "selectiveRadioButton";
        selectiveRadioButton.Size = new Size(956, 36);
        selectiveRadioButton.TabIndex = 0;
        selectiveRadioButton.Text = "지정 폴더 선별 백업    ·    설정한 두 폴더 전체 복사";
        // 
        // fullZipRadioButton
        // 
        fullZipRadioButton.Checked = true;
        fullZipRadioButton.Dock = DockStyle.Top;
        fullZipRadioButton.Location = new Point(22, 38);
        fullZipRadioButton.Name = "fullZipRadioButton";
        fullZipRadioButton.Size = new Size(956, 36);
        fullZipRadioButton.TabIndex = 1;
        fullZipRadioButton.TabStop = true;
        fullZipRadioButton.Text = "전체 백업 — ZIP 압축 파일로 저장";
        // 
        // optionTitle
        // 
        optionTitle.Dock = DockStyle.Top;
        optionTitle.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        optionTitle.Location = new Point(22, 12);
        optionTitle.Name = "optionTitle";
        optionTitle.Size = new Size(956, 26);
        optionTitle.TabIndex = 2;
        optionTitle.Text = "백업 방식";
        // 
        // optionStatusGap
        // 
        optionStatusGap.BackColor = Color.White;
        optionStatusGap.Dock = DockStyle.Top;
        optionStatusGap.Location = new Point(0, 132);
        optionStatusGap.Name = "optionStatusGap";
        optionStatusGap.Size = new Size(1000, 16);
        optionStatusGap.TabIndex = 3;
        // 
        // statusCard
        // 
        statusCard.BackColor = Color.White;
        statusCard.Controls.Add(table);
        statusCard.Dock = DockStyle.Top;
        statusCard.Location = new Point(0, 0);
        statusCard.Name = "statusCard";
        statusCard.Padding = new Padding(3);
        statusCard.Size = new Size(1000, 132);
        statusCard.TabIndex = 4;
        // 
        // table
        // 
        table.ColumnCount = 2;
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        table.Controls.Add(projectCaptionLabel, 0, 0);
        table.Controls.Add(projectValueLabel, 1, 0);
        table.Controls.Add(sourceCaptionLabel, 0, 1);
        table.Controls.Add(sourceValueLabel, 1, 1);
        table.Controls.Add(destinationCaptionLabel, 0, 2);
        table.Controls.Add(destinationValueLabel, 1, 2);
        table.Dock = DockStyle.Fill;
        table.Location = new Point(3, 3);
        table.Name = "table";
        table.Padding = new Padding(22, 12, 22, 12);
        table.RowCount = 3;
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        table.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        table.Size = new Size(994, 126);
        table.TabIndex = 0;
        // 
        // projectCaptionLabel
        // 
        projectCaptionLabel.Dock = DockStyle.Fill;
        projectCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        projectCaptionLabel.Location = new Point(25, 12);
        projectCaptionLabel.Name = "projectCaptionLabel";
        projectCaptionLabel.Size = new Size(139, 34);
        projectCaptionLabel.TabIndex = 0;
        projectCaptionLabel.Text = "프로젝트";
        projectCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // projectValueLabel
        // 
        projectValueLabel.AutoEllipsis = true;
        projectValueLabel.Dock = DockStyle.Fill;
        projectValueLabel.Location = new Point(170, 12);
        projectValueLabel.Name = "projectValueLabel";
        projectValueLabel.Size = new Size(799, 34);
        projectValueLabel.TabIndex = 1;
        projectValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // sourceCaptionLabel
        // 
        sourceCaptionLabel.Dock = DockStyle.Fill;
        sourceCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        sourceCaptionLabel.Location = new Point(25, 46);
        sourceCaptionLabel.Name = "sourceCaptionLabel";
        sourceCaptionLabel.Size = new Size(139, 34);
        sourceCaptionLabel.TabIndex = 2;
        sourceCaptionLabel.Text = "원본 위치";
        sourceCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // sourceValueLabel
        // 
        sourceValueLabel.AutoEllipsis = true;
        sourceValueLabel.Dock = DockStyle.Fill;
        sourceValueLabel.Location = new Point(170, 46);
        sourceValueLabel.Name = "sourceValueLabel";
        sourceValueLabel.Size = new Size(799, 34);
        sourceValueLabel.TabIndex = 3;
        sourceValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // destinationCaptionLabel
        // 
        destinationCaptionLabel.Dock = DockStyle.Fill;
        destinationCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        destinationCaptionLabel.Location = new Point(25, 80);
        destinationCaptionLabel.Name = "destinationCaptionLabel";
        destinationCaptionLabel.Size = new Size(139, 34);
        destinationCaptionLabel.TabIndex = 4;
        destinationCaptionLabel.Text = "백업 위치";
        destinationCaptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // destinationValueLabel
        // 
        destinationValueLabel.AutoEllipsis = true;
        destinationValueLabel.Dock = DockStyle.Fill;
        destinationValueLabel.Location = new Point(170, 80);
        destinationValueLabel.Name = "destinationValueLabel";
        destinationValueLabel.Size = new Size(799, 34);
        destinationValueLabel.TabIndex = 5;
        destinationValueLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // headerPanel
        // 
        headerPanel.BackColor = Color.White;
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Location = new Point(0, 0);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerPanel.Size = new Size(1000, 66);
        headerPanel.TabIndex = 1;
        // 
        // headerDescriptionLabel
        // 
        headerDescriptionLabel.Dock = DockStyle.Top;
        headerDescriptionLabel.Font = new Font("맑은 고딕", 10F);
        headerDescriptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        headerDescriptionLabel.Location = new Point(8, 33);
        headerDescriptionLabel.Name = "headerDescriptionLabel";
        headerDescriptionLabel.Size = new Size(992, 24);
        headerDescriptionLabel.TabIndex = 0;
        headerDescriptionLabel.Text = "현재 프로그램 상태를 ZIP 또는 선택 폴더로 안전하게 보관합니다.";
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // headerTitleLabel
        // 
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        headerTitleLabel.Location = new Point(8, 2);
        headerTitleLabel.Name = "headerTitleLabel";
        headerTitleLabel.Size = new Size(992, 31);
        headerTitleLabel.TabIndex = 1;
        headerTitleLabel.Text = "백업";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // BackupControl
        // 
        Controls.Add(body);
        Controls.Add(headerPanel);
        Name = "BackupControl";
        Size = new Size(1000, 700);
        body.ResumeLayout(false);
        action.ResumeLayout(false);
        optionCard.ResumeLayout(false);
        statusCard.ResumeLayout(false);
        table.ResumeLayout(false);
        headerPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
