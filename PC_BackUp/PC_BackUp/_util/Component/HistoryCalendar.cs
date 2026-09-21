using System.Drawing.Drawing2D;
using System.Globalization;

namespace PC_BackUp;

/// <summary>이력 유무를 날짜별 색상과 점으로 구분하는 월간 달력.</summary>
internal sealed class HistoryCalendar : Control
{
    private readonly HashSet<DateTime> m_oHistoryDates = new();
    private DateTime m_oSelectedDate = DateTime.Today;
    private DateTime m_oDisplayedMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);
    private readonly Font m_oBoldFont;
    private const int HeaderHeight = 36;
    private const int WeekdayHeight = 24;
    private const int RowHeight = 29;
    private const int ColumnWidth = 32;
    private const int SidePadding = 8;

    public HistoryCalendar()
    {
        // [Codex - 2026.09.21] 날짜별 상태를 직접 그려 기본 MonthCalendar의 스타일 제약을 해소한다.
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.Selectable, true);
        Font = new Font("맑은 고딕", 9F);
        m_oBoldFont = new Font(Font, FontStyle.Bold);
        Size = PreferredSize;
        TabStop = true;
    }

    public override Size GetPreferredSize(Size proposedSize) => new(240, HeaderHeight + WeekdayHeight + RowHeight * 6 + 8);

    public DateTime SelectedDate
    {
        get => m_oSelectedDate;
        set
        {
            m_oSelectedDate = value.Date;
            m_oDisplayedMonth = new DateTime(value.Year, value.Month, 1);
            Invalidate();
        }
    }

    public DateTime DisplayedMonth => m_oDisplayedMonth;
    public event Action<object?, DateTime>? DateSelected;
    public event Action<object?, DateTime>? DisplayedMonthChanged;

    public void SetHistoryDates(IEnumerable<DateTime> dates)
    {
        m_oHistoryDates.Clear();
        m_oHistoryDates.UnionWith(dates.Select(date => date.Date));
        Invalidate();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) m_oBoldFont.Dispose();
        base.Dispose(disposing);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.Clear(ColorRGB.Surface);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var width = ColumnWidth * 7;
        var left = (ClientSize.Width - width) / 2;
        var title = string.Format("{0:yyyy년 M월}", m_oDisplayedMonth);
        TextRenderer.DrawText(e.Graphics, title, m_oBoldFont,
            new Rectangle(left + ColumnWidth, 0, width - ColumnWidth * 2, HeaderHeight),
            ColorRGB.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        TextRenderer.DrawText(e.Graphics, "‹", m_oBoldFont, new Rectangle(left, 0, ColumnWidth, HeaderHeight),
            ColorRGB.Primary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        TextRenderer.DrawText(e.Graphics, "›", m_oBoldFont,
            new Rectangle(left + ColumnWidth * 6, 0, ColumnWidth, HeaderHeight),
            ColorRGB.Primary, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        var weekdays = new[] { "일", "월", "화", "수", "목", "금", "토" };
        for (var column = 0; column < 7; column++)
            TextRenderer.DrawText(e.Graphics, weekdays[column], Font,
                new Rectangle(left + column * ColumnWidth, HeaderHeight, ColumnWidth, WeekdayHeight),
                ColorRGB.MutedText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

        var firstDate = m_oDisplayedMonth.AddDays(-(int)m_oDisplayedMonth.DayOfWeek);
        for (var index = 0; index < 42; index++)
        {
            var date = firstDate.AddDays(index);
            var bounds = new Rectangle(left + index % 7 * ColumnWidth,
                HeaderHeight + WeekdayHeight + index / 7 * RowHeight, ColumnWidth, RowHeight);
            var hasHistory = m_oHistoryDates.Contains(date);
            var selected = date == m_oSelectedDate;
            if (selected)
            {
                using var selectionBrush = new SolidBrush(ColorRGB.Primary);
                e.Graphics.FillEllipse(selectionBrush, bounds.Left + 3, bounds.Top + 1, bounds.Width - 6, bounds.Height - 4);
            }
            var textColor = selected ? Color.White : date.Month != m_oDisplayedMonth.Month
                ? Color.FromArgb(190, 195, 204) : hasHistory ? ColorRGB.Text : Color.FromArgb(151, 157, 168);
            TextRenderer.DrawText(e.Graphics, date.Day.ToString(CultureInfo.CurrentCulture),
                hasHistory ? m_oBoldFont : Font, new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height - 5),
                textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            if (hasHistory)
            {
                using var dotBrush = new SolidBrush(selected ? Color.White : ColorRGB.Primary);
                e.Graphics.FillEllipse(dotBrush, bounds.X + bounds.Width / 2 - 2, bounds.Bottom - 7, 4, 4);
            }
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button != MouseButtons.Left) return;
        Focus();
        var left = (ClientSize.Width - ColumnWidth * 7) / 2;
        if (e.Y < HeaderHeight)
        {
            if (e.X >= left && e.X < left + ColumnWidth) ChangeMonth(-1);
            else if (e.X >= left + ColumnWidth * 6 && e.X < left + ColumnWidth * 7) ChangeMonth(1);
            return;
        }
        var column = (e.X - left) / ColumnWidth;
        var row = (e.Y - HeaderHeight - WeekdayHeight) / RowHeight;
        if (e.X < left || column is < 0 or > 6 || e.Y < HeaderHeight + WeekdayHeight || row is < 0 or > 5) return;
        var firstDate = m_oDisplayedMonth.AddDays(-(int)m_oDisplayedMonth.DayOfWeek);
        SelectDate(firstDate.AddDays(row * 7 + column));
    }

    protected override bool IsInputKey(Keys keyData) => keyData is Keys.Left or Keys.Right or Keys.Up or Keys.Down || base.IsInputKey(keyData);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        var days = e.KeyCode switch { Keys.Left => -1, Keys.Right => 1, Keys.Up => -7, Keys.Down => 7, _ => 0 };
        if (days == 0) return;
        SelectDate(m_oSelectedDate.AddDays(days));
        e.Handled = true;
    }

    private void ChangeMonth(int months)
    {
        m_oDisplayedMonth = m_oDisplayedMonth.AddMonths(months);
        Invalidate();
        DisplayedMonthChanged?.Invoke(this, m_oDisplayedMonth);
    }

    private void SelectDate(DateTime date)
    {
        SelectedDate = date;
        DateSelected?.Invoke(this, date);
        DisplayedMonthChanged?.Invoke(this, m_oDisplayedMonth);
    }
}
