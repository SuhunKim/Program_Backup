namespace PC_BackUp
{


	public sealed class BackupRecord
	{
		public required DateTime CreatedAt { get; init; }
		public required BackupKind Kind { get; init; }
		public required string FullPath { get; init; }
		public required string FileName { get; init; }
		public long SizeBytes { get; init; }
        public Services.BackupManifest? Manifest { get; init; }
        public string MetadataError { get; init; } = string.Empty;
        public string VerificationText => MetadataError.Length > 0 ? "메타데이터 오류" : Manifest is null ? "구버전 · 검증 미확인" : "생성 시 검증 · 복원 전 재검증";

		public string TimeText => CreatedAt.ToString("HH:mm:ss");
		public string KindText => Kind.ToDisplayName();
		public string SizeText => SizeBytes < 0 ? "용량 미확인" : FormatBytes(SizeBytes);

		/// <summary>여러 백업의 크기를 합산해서 요약 카드에 보여줄 때도 같은 단위 규칙을 쓰기 위한 공용 포맷터.</summary>
		public static string FormatBytes(long bytes) => bytes switch
		{
			>= 1024L * 1024L * 1024L => string.Format("{0:N1} GB", bytes / (1024d * 1024d * 1024d)),
			>= 1024L * 1024L => string.Format("{0:N1} MB", bytes / (1024d * 1024d)),
			>= 1024L => string.Format("{0:N1} KB", bytes / 1024d),
			_ => string.Format("{0:N0} B", bytes)
		};

		public override string ToString() => string.Format("{0:yyyy-MM-dd HH:mm:ss}  |  {1}  |  {2}", CreatedAt, KindText, string.Format("{0}  |  {1}", FileName, VerificationText));
	}

}