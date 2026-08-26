namespace PC_BackUp
{

	public enum BackupKind
	{
		FullZip,
		SelectiveFolders
	}

	public static class BackupKindExtensions
	{
		public static string ToDisplayName(this BackupKind kind) => kind switch
		{
			BackupKind.FullZip => ".zip",
			BackupKind.SelectiveFolders => "폴더",
			_ => kind.ToString()
		};
	}

}
