namespace PC_BackUp
{

	public sealed class LogEntry
	{
		public required DateTime Timestamp { get; init; }
		public required string Level { get; init; }
		public required string Message { get; init; }

		public string TimestampText => Timestamp.ToString("yyyy-MM-dd HH:mm:ss");

		public override string ToString() => string.Format("{0}  |  {1}  |  {2}", TimestampText, Level, Message);
	}

}
