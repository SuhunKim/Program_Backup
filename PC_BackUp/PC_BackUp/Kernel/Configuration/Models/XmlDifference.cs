namespace PC_BackUp
{

	public sealed class XmlDifference
	{
		public bool Apply { get; set; }
		public required string RelativeFilePath { get; init; }
		public required string XmlPath { get; init; }
		public string CurrentValue { get; init; } = string.Empty;
		public string BackupValue { get; init; } = string.Empty;

		public string ItemName => string.Format("{0}  ·  {1}", RelativeFilePath, XmlPath);
	}

}