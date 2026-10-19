namespace PC_BackUp
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// 개별 화면의 try/catch를 뚫고 올라오는 예외까지 마지막에 한 번 더 잡는다.
			// 이게 없으면 OS 기본 "처리되지 않은 예외" 다이얼로그가 뜨면서 앱이 죽고,
			// 그 다이얼로그에 내부 파일 경로가 포함된 전체 스택트레이스가 그대로 노출된다.
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			Application.ThreadException += (_, e) => HandleUnhandledException(e.Exception);
			AppDomain.CurrentDomain.UnhandledException += (_, e) =>
				HandleUnhandledException(e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString()));

			ApplicationConfiguration.Initialize();
            try { Application.Run(new MainForm()); }
            catch (Exception exception)
            {
                try { new LogManager().LogError("초기화 실패", exception); } catch { }
                MessageBox.Show(string.Format("프로그램을 시작하지 못했습니다.\n{0}\n\n설정·데이터 경로의 읽기/쓰기 권한을 확인하세요.", exception.Message),
                    "시작 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
		}

		private static void HandleUnhandledException(Exception exception)
		{
			try { new LogManager().LogError("처리되지 않은 예외", exception); } catch { /* 로깅 실패는 무시 */ }

			MessageBox.Show(
				"예상치 못한 오류가 발생했습니다. 작업 로그에 자세한 내용이 기록되었습니다.",
				"오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
