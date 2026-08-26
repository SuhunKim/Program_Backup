namespace PC_BackUp
{

	/// <summary>
	/// XmlComparisonService가 만들어내는 차이 한 건의 성격. 그리드를 두 영역(존재 차이/값 차이)으로
	/// 나눠서 보여주는 데 쓴다 — 파일/폴더가 한쪽에만 있는 "구조적" 차이와, 양쪽에 다 있는 파일의
	/// 특정 설정값이 다른 "내용" 차이는 사용자에게 보여줘야 할 정보의 모양이 서로 다르다.
	/// </summary>
	public enum XmlDifferenceKind
	{
		/// <summary>양쪽에 파일이 다 있고, 그 안의 특정 설정값(XmlPath)이 다르다.</summary>
		ValueChanged,
		/// <summary>이 파일이 현재(Source) 쪽에만 있고 백업(Destination) 쪽에는 없다.</summary>
		ExistsOnlyInCurrent,
		/// <summary>이 파일이 백업(Destination) 쪽에만 있고 현재(Source) 쪽에는 없다.</summary>
		ExistsOnlyInBackup,
	}

	public sealed class XmlDifference
	{
		public bool Apply { get; set; }
		public required XmlDifferenceKind Kind { get; init; }
		public required string RelativeFilePath { get; init; }
		public string XmlPath { get; init; } = string.Empty;
		public string CurrentValue { get; init; } = string.Empty;
		public string BackupValue { get; init; } = string.Empty;

		public string ItemName => string.Format("{0}  ·  {1}", RelativeFilePath, XmlPath);
	}

}
