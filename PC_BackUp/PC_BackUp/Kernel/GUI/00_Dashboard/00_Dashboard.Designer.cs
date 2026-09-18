namespace PC_BackUp.Kernel.GUI._00_Dashboard
{
    partial class _00_Dashboard
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel m_oHeaderPanel = null!;
        private Label m_oHeaderTitleLabel = null!;
        private Label m_oHeaderDescriptionLabel = null!;
        private CardPanel m_oLastBackupCard = null!;
        private GlyphBadge m_oLastBackupBadge = null!;
        private Label m_oLastBackupCaptionLabel = null!;
        private Label _summaryDateValue = null!;
        private CardPanel m_oBackupCountCard = null!;
        private GlyphBadge m_oBackupCountBadge = null!;
        private Label m_oBackupCountCaptionLabel = null!;
        private Label _summaryCountValue = null!;
        private CardPanel m_oBackupSizeCard = null!;
        private GlyphBadge m_oBackupSizeBadge = null!;
        private Label m_oBackupSizeCaptionLabel = null!;
        private Label _summarySizeValue = null!;
        private CardPanel m_oConfigurationCard = null!;
        private GlyphBadge m_oConfigurationBadge = null!;
        private Label m_oConfigurationCaptionLabel = null!;
        private Label _summaryStatusValue = null!;
        private CardPanel m_oRecentCard = null!;
        private Label m_oRecentTitleLabel = null!;
        private StyledButton m_oHistoryButton = null!;
        private DataGridView m_oRecentGrid = null!;
        private DataGridViewTextBoxColumn m_oCreatedAtColumn = null!;
        private DataGridViewTextBoxColumn m_oKindColumn = null!;
        private DataGridViewTextBoxColumn m_oFileNameColumn = null!;
        private DataGridViewTextBoxColumn m_oSizeColumn = null!;
        private CardPanel m_oQuickActionCard = null!;
        private Label m_oQuickActionTitleLabel = null!;
        private StyledButton m_oBackupButton = null!;
        private StyledButton m_oRecoveryButton = null!;
        private StyledButton m_oSettingsButton = null!;
        private CardPanel m_oWarningCard = null!;
        private Label m_oWarningLabel = null!;
        private StyledButton m_oWarningSettingsButton = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components is not null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            m_oHeaderPanel = new Panel();
            m_oHeaderDescriptionLabel = new Label();
            m_oHeaderTitleLabel = new Label();
            m_oLastBackupCard = new CardPanel();
            _summaryDateValue = new Label();
            m_oLastBackupCaptionLabel = new Label();
            m_oLastBackupBadge = new GlyphBadge();
            m_oBackupCountCard = new CardPanel();
            _summaryCountValue = new Label();
            m_oBackupCountCaptionLabel = new Label();
            m_oBackupCountBadge = new GlyphBadge();
            m_oBackupSizeCard = new CardPanel();
            _summarySizeValue = new Label();
            m_oBackupSizeCaptionLabel = new Label();
            m_oBackupSizeBadge = new GlyphBadge();
            m_oConfigurationCard = new CardPanel();
            _summaryStatusValue = new Label();
            m_oConfigurationCaptionLabel = new Label();
            m_oConfigurationBadge = new GlyphBadge();
            m_oRecentCard = new CardPanel();
            m_oRecentGrid = new DataGridView();
            m_oHistoryButton = new StyledButton();
            m_oRecentTitleLabel = new Label();
            m_oQuickActionCard = new CardPanel();
            m_oSettingsButton = new StyledButton();
            m_oRecoveryButton = new StyledButton();
            m_oBackupButton = new StyledButton();
            m_oQuickActionTitleLabel = new Label();
            m_oWarningCard = new CardPanel();
            m_oWarningSettingsButton = new StyledButton();
            m_oWarningLabel = new Label();
            m_oHeaderPanel.SuspendLayout();
            m_oLastBackupCard.SuspendLayout();
            m_oBackupCountCard.SuspendLayout();
            m_oBackupSizeCard.SuspendLayout();
            m_oConfigurationCard.SuspendLayout();
            m_oRecentCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)m_oRecentGrid).BeginInit();
            m_oQuickActionCard.SuspendLayout();
            m_oWarningCard.SuspendLayout();
            SuspendLayout();
            // 
            // m_oHeaderPanel
            // 
            m_oHeaderPanel.Controls.Add(m_oHeaderDescriptionLabel);
            m_oHeaderPanel.Controls.Add(m_oHeaderTitleLabel);
            m_oHeaderPanel.Location = new Point(20, 16);
            m_oHeaderPanel.Name = "m_oHeaderPanel";
            m_oHeaderPanel.Size = new Size(960, 62);
            m_oHeaderPanel.TabIndex = 0;
            // 
            // m_oHeaderDescriptionLabel
            // 
            m_oHeaderDescriptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
            m_oHeaderDescriptionLabel.Location = new Point(2, 36);
            m_oHeaderDescriptionLabel.Name = "m_oHeaderDescriptionLabel";
            m_oHeaderDescriptionLabel.Size = new Size(500, 20);
            m_oHeaderDescriptionLabel.TabIndex = 1;
            m_oHeaderDescriptionLabel.Text = "백업 상태와 최근 이력을 확인합니다.";
            // 
            // m_oHeaderTitleLabel
            // 
            m_oHeaderTitleLabel.Dock = DockStyle.Top;
            m_oHeaderTitleLabel.Font = new Font("맑은 고딕", 18F, FontStyle.Bold);
            m_oHeaderTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
            m_oHeaderTitleLabel.Location = new Point(0, 0);
            m_oHeaderTitleLabel.Name = "m_oHeaderTitleLabel";
            m_oHeaderTitleLabel.Size = new Size(960, 32);
            m_oHeaderTitleLabel.TabIndex = 0;
            m_oHeaderTitleLabel.Text = "대시보드";
            m_oHeaderTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // m_oLastBackupCard
            // 
            m_oLastBackupCard.BackColor = Color.White;
            m_oLastBackupCard.Controls.Add(_summaryDateValue);
            m_oLastBackupCard.Controls.Add(m_oLastBackupCaptionLabel);
            m_oLastBackupCard.Controls.Add(m_oLastBackupBadge);
            m_oLastBackupCard.Location = new Point(20, 94);
            m_oLastBackupCard.Name = "m_oLastBackupCard";
            m_oLastBackupCard.Padding = new Padding(3);
            m_oLastBackupCard.Size = new Size(220, 98);
            m_oLastBackupCard.TabIndex = 1;
            // 
            // _summaryDateValue
            // 
            _summaryDateValue.Font = new Font("맑은 고딕", 12F, FontStyle.Bold);
            _summaryDateValue.ForeColor = Color.FromArgb(28, 32, 41);
            _summaryDateValue.Location = new Point(64, 42);
            _summaryDateValue.Name = "_summaryDateValue";
            _summaryDateValue.Size = new Size(145, 26);
            _summaryDateValue.TabIndex = 2;
            _summaryDateValue.Text = "-";
            // 
            // m_oLastBackupCaptionLabel
            // 
            m_oLastBackupCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
            m_oLastBackupCaptionLabel.Location = new Point(64, 16);
            m_oLastBackupCaptionLabel.Name = "m_oLastBackupCaptionLabel";
            m_oLastBackupCaptionLabel.Size = new Size(140, 20);
            m_oLastBackupCaptionLabel.TabIndex = 1;
            m_oLastBackupCaptionLabel.Text = "마지막 백업";
            // 
            // m_oLastBackupBadge
            // 
            m_oLastBackupBadge.BackColor = Color.Transparent;
            m_oLastBackupBadge.Glyph = GlyphBadgeKind.Clock;
            m_oLastBackupBadge.Location = new Point(16, 16);
            m_oLastBackupBadge.Name = "m_oLastBackupBadge";
            m_oLastBackupBadge.Size = new Size(36, 36);
            m_oLastBackupBadge.TabIndex = 0;
            // 
            // m_oBackupCountCard
            // 
            m_oBackupCountCard.BackColor = Color.White;
            m_oBackupCountCard.Controls.Add(_summaryCountValue);
            m_oBackupCountCard.Controls.Add(m_oBackupCountCaptionLabel);
            m_oBackupCountCard.Controls.Add(m_oBackupCountBadge);
            m_oBackupCountCard.Location = new Point(260, 94);
            m_oBackupCountCard.Name = "m_oBackupCountCard";
            m_oBackupCountCard.Padding = new Padding(3);
            m_oBackupCountCard.Size = new Size(220, 98);
            m_oBackupCountCard.TabIndex = 2;
            // 
            // _summaryCountValue
            // 
            _summaryCountValue.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            _summaryCountValue.ForeColor = Color.FromArgb(28, 32, 41);
            _summaryCountValue.Location = new Point(64, 42);
            _summaryCountValue.Name = "_summaryCountValue";
            _summaryCountValue.Size = new Size(140, 26);
            _summaryCountValue.TabIndex = 2;
            _summaryCountValue.Text = "-";
            // 
            // m_oBackupCountCaptionLabel
            // 
            m_oBackupCountCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
            m_oBackupCountCaptionLabel.Location = new Point(64, 16);
            m_oBackupCountCaptionLabel.Name = "m_oBackupCountCaptionLabel";
            m_oBackupCountCaptionLabel.Size = new Size(140, 20);
            m_oBackupCountCaptionLabel.TabIndex = 1;
            m_oBackupCountCaptionLabel.Text = "백업 보관 수량";
            // 
            // m_oBackupCountBadge
            // 
            m_oBackupCountBadge.BackColor = Color.Transparent;
            m_oBackupCountBadge.Glyph = GlyphBadgeKind.Archive;
            m_oBackupCountBadge.Location = new Point(16, 16);
            m_oBackupCountBadge.Name = "m_oBackupCountBadge";
            m_oBackupCountBadge.Size = new Size(36, 36);
            m_oBackupCountBadge.TabIndex = 0;
            // 
            // m_oBackupSizeCard
            // 
            m_oBackupSizeCard.BackColor = Color.White;
            m_oBackupSizeCard.Controls.Add(_summarySizeValue);
            m_oBackupSizeCard.Controls.Add(m_oBackupSizeCaptionLabel);
            m_oBackupSizeCard.Controls.Add(m_oBackupSizeBadge);
            m_oBackupSizeCard.Location = new Point(500, 94);
            m_oBackupSizeCard.Name = "m_oBackupSizeCard";
            m_oBackupSizeCard.Padding = new Padding(3);
            m_oBackupSizeCard.Size = new Size(220, 98);
            m_oBackupSizeCard.TabIndex = 3;
            // 
            // _summarySizeValue
            // 
            _summarySizeValue.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            _summarySizeValue.ForeColor = Color.FromArgb(28, 32, 41);
            _summarySizeValue.Location = new Point(64, 42);
            _summarySizeValue.Name = "_summarySizeValue";
            _summarySizeValue.Size = new Size(140, 26);
            _summarySizeValue.TabIndex = 2;
            _summarySizeValue.Text = "-";
            // 
            // m_oBackupSizeCaptionLabel
            // 
            m_oBackupSizeCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
            m_oBackupSizeCaptionLabel.Location = new Point(64, 16);
            m_oBackupSizeCaptionLabel.Name = "m_oBackupSizeCaptionLabel";
            m_oBackupSizeCaptionLabel.Size = new Size(140, 20);
            m_oBackupSizeCaptionLabel.TabIndex = 1;
            m_oBackupSizeCaptionLabel.Text = "전체 백업 용량";
            // 
            // m_oBackupSizeBadge
            // 
            m_oBackupSizeBadge.BackColor = Color.Transparent;
            m_oBackupSizeBadge.Glyph = GlyphBadgeKind.Storage;
            m_oBackupSizeBadge.Location = new Point(16, 16);
            m_oBackupSizeBadge.Name = "m_oBackupSizeBadge";
            m_oBackupSizeBadge.Size = new Size(36, 36);
            m_oBackupSizeBadge.TabIndex = 0;
            // 
            // m_oConfigurationCard
            // 
            m_oConfigurationCard.BackColor = Color.White;
            m_oConfigurationCard.Controls.Add(_summaryStatusValue);
            m_oConfigurationCard.Controls.Add(m_oConfigurationCaptionLabel);
            m_oConfigurationCard.Controls.Add(m_oConfigurationBadge);
            m_oConfigurationCard.Location = new Point(740, 94);
            m_oConfigurationCard.Name = "m_oConfigurationCard";
            m_oConfigurationCard.Padding = new Padding(3);
            m_oConfigurationCard.Size = new Size(240, 98);
            m_oConfigurationCard.TabIndex = 4;
            // 
            // _summaryStatusValue
            // 
            _summaryStatusValue.Font = new Font("맑은 고딕", 14F, FontStyle.Bold);
            _summaryStatusValue.ForeColor = Color.FromArgb(28, 32, 41);
            _summaryStatusValue.Location = new Point(64, 42);
            _summaryStatusValue.Name = "_summaryStatusValue";
            _summaryStatusValue.Size = new Size(150, 26);
            _summaryStatusValue.TabIndex = 2;
            _summaryStatusValue.Text = "-";
            // 
            // m_oConfigurationCaptionLabel
            // 
            m_oConfigurationCaptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
            m_oConfigurationCaptionLabel.Location = new Point(64, 16);
            m_oConfigurationCaptionLabel.Name = "m_oConfigurationCaptionLabel";
            m_oConfigurationCaptionLabel.Size = new Size(150, 20);
            m_oConfigurationCaptionLabel.TabIndex = 1;
            m_oConfigurationCaptionLabel.Text = "구성 상태";
            // 
            // m_oConfigurationBadge
            // 
            m_oConfigurationBadge.BackColor = Color.Transparent;
            m_oConfigurationBadge.Glyph = GlyphBadgeKind.Check;
            m_oConfigurationBadge.Location = new Point(16, 16);
            m_oConfigurationBadge.Name = "m_oConfigurationBadge";
            m_oConfigurationBadge.Size = new Size(36, 36);
            m_oConfigurationBadge.TabIndex = 0;
            // 
            // m_oRecentCard
            // 
            m_oRecentCard.BackColor = Color.White;
            m_oRecentCard.Controls.Add(m_oRecentGrid);
            m_oRecentCard.Controls.Add(m_oHistoryButton);
            m_oRecentCard.Controls.Add(m_oRecentTitleLabel);
            m_oRecentCard.Location = new Point(20, 208);
            m_oRecentCard.Name = "m_oRecentCard";
            m_oRecentCard.Padding = new Padding(3);
            m_oRecentCard.Size = new Size(620, 296);
            m_oRecentCard.TabIndex = 5;
            // 
            // m_oRecentGrid
            // 
            m_oRecentGrid.AllowUserToAddRows = false;
            m_oRecentGrid.AllowUserToDeleteRows = false;
            m_oRecentGrid.AllowUserToResizeRows = false;
            m_oRecentGrid.BackgroundColor = Color.White;
            m_oRecentGrid.BorderStyle = BorderStyle.None;
            m_oRecentGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            m_oRecentGrid.EnableHeadersVisualStyles = false;
            m_oRecentGrid.Location = new Point(16, 52);
            m_oRecentGrid.Name = "m_oRecentGrid";
            m_oRecentGrid.ReadOnly = true;
            m_oRecentGrid.RowHeadersVisible = false;
            m_oRecentGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_oRecentGrid.Size = new Size(588, 226);
            m_oRecentGrid.TabIndex = 2;
            // 
            // m_oHistoryButton
            // 
            m_oHistoryButton.BackColor = Color.FromArgb(233, 238, 245);
            m_oHistoryButton.BorderThickness = 1;
            m_oHistoryButton.ButtonType = StyledButtonType.Secondary;
            m_oHistoryButton.Font = new Font("맑은 고딕", 9F);
            m_oHistoryButton.ForeColor = Color.FromArgb(28, 32, 41);
            m_oHistoryButton.Location = new Point(480, 10);
            m_oHistoryButton.Name = "m_oHistoryButton";
            m_oHistoryButton.Size = new Size(124, 32);
            m_oHistoryButton.TabIndex = 1;
            m_oHistoryButton.Text = "전체 이력 보기";
            m_oHistoryButton.Click += UiClick_History;
            // 
            // m_oRecentTitleLabel
            // 
            m_oRecentTitleLabel.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            m_oRecentTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
            m_oRecentTitleLabel.Location = new Point(16, 13);
            m_oRecentTitleLabel.Name = "m_oRecentTitleLabel";
            m_oRecentTitleLabel.Size = new Size(180, 28);
            m_oRecentTitleLabel.TabIndex = 0;
            m_oRecentTitleLabel.Text = "최근 백업 이력";
            // 
            // m_oQuickActionCard
            // 
            m_oQuickActionCard.BackColor = Color.White;
            m_oQuickActionCard.Controls.Add(m_oSettingsButton);
            m_oQuickActionCard.Controls.Add(m_oRecoveryButton);
            m_oQuickActionCard.Controls.Add(m_oBackupButton);
            m_oQuickActionCard.Controls.Add(m_oQuickActionTitleLabel);
            m_oQuickActionCard.Location = new Point(660, 208);
            m_oQuickActionCard.Name = "m_oQuickActionCard";
            m_oQuickActionCard.Padding = new Padding(3);
            m_oQuickActionCard.Size = new Size(320, 296);
            m_oQuickActionCard.TabIndex = 6;
            // 
            // m_oSettingsButton
            // 
            m_oSettingsButton.BackColor = Color.FromArgb(233, 238, 245);
            m_oSettingsButton.BorderThickness = 1;
            m_oSettingsButton.ButtonType = StyledButtonType.Secondary;
            m_oSettingsButton.Font = new Font("맑은 고딕", 9F);
            m_oSettingsButton.ForeColor = Color.FromArgb(28, 32, 41);
            m_oSettingsButton.Location = new Point(16, 166);
            m_oSettingsButton.Name = "m_oSettingsButton";
            m_oSettingsButton.Size = new Size(288, 42);
            m_oSettingsButton.TabIndex = 3;
            m_oSettingsButton.Text = "환경 설정";
            m_oSettingsButton.Click += UiClick_Settings;
            // 
            // m_oRecoveryButton
            // 
            m_oRecoveryButton.BackColor = Color.FromArgb(233, 238, 245);
            m_oRecoveryButton.BorderThickness = 1;
            m_oRecoveryButton.ButtonType = StyledButtonType.Secondary;
            m_oRecoveryButton.Font = new Font("맑은 고딕", 9F);
            m_oRecoveryButton.ForeColor = Color.FromArgb(28, 32, 41);
            m_oRecoveryButton.Location = new Point(16, 112);
            m_oRecoveryButton.Name = "m_oRecoveryButton";
            m_oRecoveryButton.Size = new Size(288, 42);
            m_oRecoveryButton.TabIndex = 2;
            m_oRecoveryButton.Text = "백업 복원";
            m_oRecoveryButton.Click += UiClick_Recovery;
            // 
            // m_oBackupButton
            // 
            m_oBackupButton.BackColor = Color.FromArgb(30, 100, 199);
            m_oBackupButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
            m_oBackupButton.ForeColor = Color.White;
            m_oBackupButton.Location = new Point(16, 54);
            m_oBackupButton.Name = "m_oBackupButton";
            m_oBackupButton.Size = new Size(288, 46);
            m_oBackupButton.TabIndex = 1;
            m_oBackupButton.Text = "새 백업 만들기";
            m_oBackupButton.Click += UiClick_Backup;
            // 
            // m_oQuickActionTitleLabel
            // 
            m_oQuickActionTitleLabel.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
            m_oQuickActionTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
            m_oQuickActionTitleLabel.Location = new Point(16, 13);
            m_oQuickActionTitleLabel.Name = "m_oQuickActionTitleLabel";
            m_oQuickActionTitleLabel.Size = new Size(180, 28);
            m_oQuickActionTitleLabel.TabIndex = 0;
            m_oQuickActionTitleLabel.Text = "빠른 작업";
            // 
            // m_oWarningCard
            // 
            m_oWarningCard.BackColor = Color.FromArgb(247, 248, 250);
            m_oWarningCard.Controls.Add(m_oWarningSettingsButton);
            m_oWarningCard.Controls.Add(m_oWarningLabel);
            m_oWarningCard.Location = new Point(20, 522);
            m_oWarningCard.Name = "m_oWarningCard";
            m_oWarningCard.Padding = new Padding(3);
            m_oWarningCard.Size = new Size(960, 74);
            m_oWarningCard.TabIndex = 7;
            m_oWarningCard.Visible = false;
            // 
            // m_oWarningSettingsButton
            // 
            m_oWarningSettingsButton.BackColor = Color.FromArgb(233, 238, 245);
            m_oWarningSettingsButton.BorderThickness = 1;
            m_oWarningSettingsButton.ButtonType = StyledButtonType.Secondary;
            m_oWarningSettingsButton.Font = new Font("맑은 고딕", 9F);
            m_oWarningSettingsButton.ForeColor = Color.FromArgb(28, 32, 41);
            m_oWarningSettingsButton.Location = new Point(808, 16);
            m_oWarningSettingsButton.Name = "m_oWarningSettingsButton";
            m_oWarningSettingsButton.Size = new Size(136, 40);
            m_oWarningSettingsButton.TabIndex = 1;
            m_oWarningSettingsButton.Text = "환경 설정 열기";
            m_oWarningSettingsButton.Click += UiClick_Settings;
            // 
            // m_oWarningLabel
            // 
            m_oWarningLabel.ForeColor = Color.FromArgb(108, 115, 128);
            m_oWarningLabel.Location = new Point(16, 16);
            m_oWarningLabel.Name = "m_oWarningLabel";
            m_oWarningLabel.Size = new Size(760, 42);
            m_oWarningLabel.TabIndex = 0;
            m_oWarningLabel.Text = "환경 설정을 확인하세요.";
            // 
            // _00_Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.FromArgb(255, 255, 255);
            Controls.Add(m_oWarningCard);
            Controls.Add(m_oQuickActionCard);
            Controls.Add(m_oRecentCard);
            Controls.Add(m_oConfigurationCard);
            Controls.Add(m_oBackupSizeCard);
            Controls.Add(m_oBackupCountCard);
            Controls.Add(m_oLastBackupCard);
            Controls.Add(m_oHeaderPanel);
            Name = "_00_Dashboard";
            Size = new Size(1000, 700);
            m_oHeaderPanel.ResumeLayout(false);
            m_oLastBackupCard.ResumeLayout(false);
            m_oBackupCountCard.ResumeLayout(false);
            m_oBackupSizeCard.ResumeLayout(false);
            m_oConfigurationCard.ResumeLayout(false);
            m_oRecentCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)m_oRecentGrid).EndInit();
            m_oQuickActionCard.ResumeLayout(false);
            m_oWarningCard.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
