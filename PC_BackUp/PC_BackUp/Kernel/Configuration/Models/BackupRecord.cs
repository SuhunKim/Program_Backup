namespace PC_BackUp
{


	public sealed class BackupRecord
	{
		public required DateTime CreatedAt { get; init; }
		public required BackupKind Kind { get; init; }
		public required string FullPath { get; init; }
		public required string FileName { get; init; }
		public long SizeBytes { get; init; }

		public string TimeText => CreatedAt.ToString("HH:mm:ss");
		public string KindText => Kind.ToDisplayName();
		public string SizeText => FormatBytes(SizeBytes);

		/// <summary>여러 백업의 크기를 합산해서 요약 카드에 보여줄 때도 같은 단위 규칙을 쓰기 위한 공용 포맷터.</summary>
		public static string FormatBytes(long bytes) => bytes switch
		{
			>= 1024L * 1024L * 1024L => string.Format("{0:N1} GB", bytes / (1024d * 1024d * 1024d)),
			>= 1024L * 1024L => string.Format("{0:N1} MB", bytes / (1024d * 1024d)),
			>= 1024L => string.Format("{0:N1} KB", bytes / 1024d),
			_ => string.Format("{0:N0} B", bytes)
		};

		public override string ToString() => string.Format("{0:yyyy-MM-dd HH:mm:ss}  |  {1}  |  {2}", CreatedAt, KindText, FileName);
	}

}