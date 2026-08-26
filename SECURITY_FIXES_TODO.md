# 보안/안정성 수정 계획 (2026-08-24 분석)

> 이 파일은 다음 세션에서 바로 1→2→3→4 순서로 수정을 이어갈 수 있도록 만든 작업 목록입니다.
> 각 항목은 "지금 코드가 이렇다 → 이렇게 바꾼다"까지 구체적으로 적어뒀으니, 다시 분석할 필요 없이
> 바로 Edit부터 시작하면 됩니다. 완료한 항목은 체크박스에 표시하세요.

- [x] 1. 전역 미처리 예외 핸들러 없음 (크래시 + 스택트레이스 노출) — 완료
- [x] 2. 복원 시 대상 프로세스가 "이름만" 보고 강제종료됨 — 완료
- [x] 3. zip 압축 해제 경로 검증이 프레임워크 암묵 동작에만 의존 — 완료 (`_util/ZipExtraction.cs`)
- [x] 4. obj/bin에 net7.0-windows / net10.0-windows 산출물 혼재 (클린 빌드) — 완료, 클린 빌드 확인(오류 0개)

---

## 1. 전역 미처리 예외 핸들러 추가 + 개별 try 범위 정리

### 1-A. `Program.cs`에 전역 핸들러 추가
파일: `PC_BackUp/PC_BackUp/Program.cs`

현재:
```csharp
[STAThread]
static void Main()
{
    ApplicationConfiguration.Initialize();
    Application.Run(new MainForm());
}
```

변경 방향:
```csharp
[STAThread]
static void Main()
{
    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
    Application.ThreadException += (_, e) => HandleUnhandledException(e.Exception);
    AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        HandleUnhandledException(e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject?.ToString()));

    ApplicationConfiguration.Initialize();
    Application.Run(new MainForm());
}

private static void HandleUnhandledException(Exception exception)
{
    // LogManager는 static이 아니므로, Program.cs 전용으로 파일 경로를 직접 잡아
    // 별도 LogManager 인스턴스를 만들거나(권장) 최소 로그만 남긴다.
    // 사용자에게는 스택트레이스 없이 일반화된 메시지만 보여준다.
    try { new LogManager().LogError("처리되지 않은 예외", exception); } catch { }
    MessageBox.Show(
        "예상치 못한 오류가 발생했습니다. 작업 로그에 자세한 내용이 기록되었습니다.",
        "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```
- `LogManager`가 이미 예외를 삼키는 안전한 로거이므로 재사용.
- 이 핸들러는 "최후 방어선"이다 — 1-B가 우선이고, 여기까지 오는 예외는 줄여야 정상.

### 1-B. try 블록 밖에서 던질 수 있는 코드를 try 안으로 이동

세 곳 모두 패턴이 같다: `try` 진입 전에 `.Load()`, `new XxxDialog(...)`, `.ShowDialog()`, 또는 다른 서비스 호출이 있고 여기서 던지면 안 잡힘.

1. **`PC_BackUp/PC_BackUp/Kernel/GUI/03_History/03_HistoryControl.cs:253`** `CreateInstantBackupAsync`
   - 현재: `m_oSettingsService.Load()` / `new BackupNameDialog(...)` / `nameDialog.ShowDialog(this)`가 `try` 밖.
   - 수정: 이 세 줄을 통째로 `try` 블록 안으로 옮기고, `catch` 블록에서 기존과 동일하게 `MessageBox.Show` + `LogError` 처리. (다이얼로그 취소로 `return`하는 경우는 그대로 유지 — 예외가 아니므로 catch에 안 걸림)

2. **`PC_BackUp/PC_BackUp/Kernel/GUI/01_Backup/01_BackupControl.cs:39`** `BackupButton_Click`
   - 현재: `m_oSettingsService.Load()` / `new BackupNameDialog(...)` / `nameDialog.ShowDialog(this)`가 `try` 밖 (43~48행).
   - 수정: 위와 동일한 패턴으로 try 블록을 앞으로 확장.

3. **`PC_BackUp/PC_BackUp/Kernel/GUI/02_Recovery/02_RecoveryControl.cs:255`** `RestoreButton_Click`
   - 현재 구조: `try/finally`(비교) → `MessageBox.Show`(복원 확인) → `m_oRecoveryService.GetRunningTargetProcesses(settings)` → `MessageBox.Show`(프로세스 종료 확인) → `await StopProcessesAsync` → `try/catch`(실제 복원). **두 try 블록 사이의 확인 다이얼로그 + `GetRunningTargetProcesses` 호출이 무방비.**
   - 수정: `GetRunningTargetProcesses` 호출을 `try/catch`로 감싸거나, 두 번째 `try` 블록을 이 코드까지 앞당겨 포함시킨다. `MessageBox.Show` 자체는 거의 던지지 않지만, `GetRunningTargetProcesses`는 `Process.GetProcessesByName` 호출이라 이론상 `Win32Exception` 가능성이 있음.

---

## 2. 복원 시 프로세스 이름만으로 매칭되는 문제 수정

파일: `PC_BackUp/PC_BackUp/Services/RecoveryService.cs:8` `GetRunningTargetProcesses`

현재:
```csharp
public IReadOnlyList<Process> GetRunningTargetProcesses(AppSettings settings)
{
    var processName = Path.GetFileNameWithoutExtension(settings.ExecutablePath);
    if (string.IsNullOrWhiteSpace(processName))
        return Array.Empty<Process>();

    return Process.GetProcessesByName(processName)
        .Where(process => process.Id != Environment.ProcessId)
        .ToList();
}
```

수정 방향: 이름으로 후보를 고른 뒤, 실제 실행 파일 경로가 `settings.ExecutablePath`와 같은 것만 남긴다.
```csharp
public IReadOnlyList<Process> GetRunningTargetProcesses(AppSettings settings)
{
    var processName = Path.GetFileNameWithoutExtension(settings.ExecutablePath);
    if (string.IsNullOrWhiteSpace(processName))
        return Array.Empty<Process>();

    var targetPath = Path.GetFullPath(settings.ExecutablePath);
    var matched = new List<Process>();
    foreach (var process in Process.GetProcessesByName(processName))
    {
        if (process.Id == Environment.ProcessId)
        {
            process.Dispose();
            continue;
        }

        bool isTarget;
        try
        {
            isTarget = string.Equals(process.MainModule?.FileName, targetPath, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            // 접근 거부 등으로 MainModule을 못 읽으면 대상이 맞는지 확인 불가 — 안전하게 제외.
            isTarget = false;
        }

        if (isTarget) matched.Add(process);
        else process.Dispose();
    }
    return matched;
}
```
- `Process.Dispose()` 호출을 빠뜨리지 않도록 주의 (기존 코드는 `.ToList()`로 걸러진 것만 호출자가 `Dispose()`함 — 걸러져서 버려지는 것들도 여기서 직접 정리해야 핸들 누수 없음).
- 호출부([02_RecoveryControl.cs:309](PC_BackUp/PC_BackUp/Kernel/GUI/02_Recovery/02_RecoveryControl.cs:309))는 변경 불필요 (반환 타입 동일).

---

## 3. zip 압축 해제 경로 검증 명시화

### 3-A. `RecoveryService.RestoreAsync`
파일: `PC_BackUp/PC_BackUp/Services/RecoveryService.cs:74`

현재:
```csharp
Directory.CreateDirectory(temporaryRoot);
ZipFile.ExtractToDirectory(record.FullPath, temporaryRoot, true);
restoreSource = temporaryRoot;
```

수정 방향: 압축 해제 전에 각 엔트리 이름을 직접 검증하는 헬퍼로 교체(엔트리별 순회 + 안전한 경로만 추출). 아래처럼 `ExtractToDirectory`를 대체하는 명시적 루프를 추가:
```csharp
private static void SafeExtractToDirectory(string zipPath, string destinationRoot)
{
    var normalizedRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(destinationRoot));
    using var archive = ZipFile.OpenRead(zipPath);
    foreach (var entry in archive.Entries)
    {
        var targetPath = Path.GetFullPath(Path.Combine(normalizedRoot, entry.FullName));
        if (!targetPath.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase) &&
            !targetPath.StartsWith(normalizedRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException(string.Format("백업 zip에 대상 폴더를 벗어나는 항목이 있습니다: {0}", entry.FullName));

        if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
        {
            Directory.CreateDirectory(targetPath);
            continue;
        }
        Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
        entry.ExtractToFile(targetPath, overwrite: true);
    }
}
```
그리고 호출부를 `ZipFile.ExtractToDirectory(record.FullPath, temporaryRoot, true);` → `SafeExtractToDirectory(record.FullPath, temporaryRoot);`로 교체.

### 3-B. `BackupComparisonService.ReadBackupFiles`
파일: `PC_BackUp/PC_BackUp/Services/BackupComparisonService.cs:63`

현재:
```csharp
var sTemporaryRoot = Path.Combine(Path.GetTempPath(), "PC_BackUp", Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(sTemporaryRoot);
ZipFile.ExtractToDirectory(oRecord.FullPath, sTemporaryRoot);
```
수정: 3-A에서 만든 `SafeExtractToDirectory`를 두 서비스에서 공유할 수 있는 곳(예: 새 internal static 클래스 `_util/ZipExtraction.cs` 또는 기존 `FileNaming.cs` 옆에 새 파일)으로 옮기고, 여기서도 동일하게 교체.

> 참고: `.NET`이 이미 `..` zip-slip을 내부적으로 막아주므로 동작상 변화는 없어야 함(회귀 테스트 필요: 정상 백업 zip 복원이 여전히 잘 되는지).

---

## 4. 빌드 산출물 정리 (net7.0-windows / net10.0-windows 혼재)

`.csproj`의 `TargetFramework`는 `net7.0-windows`로 고정돼 있음 ([PC_BackUp.csproj:5](PC_BackUp/PC_BackUp/PC_BackUp.csproj:5)). 그런데 `obj/Debug/net7.0-windows`와 `bin/Debug/net10.0-windows`가 동시에 존재 — 다른 SDK로 빌드했던 잔재.

실행 순서:
```bash
rm -rf "D:/Server/Git_PCBackUp/PC_BackUp/PC_BackUp/PC_BackUp/obj"
rm -rf "D:/Server/Git_PCBackUp/PC_BackUp/PC_BackUp/PC_BackUp/bin"
rm -rf "D:/Server/Git_PCBackUp/PC_BackUp/PC_BackUp/_bin"
```
그 다음 `PC_BackUp.exe`가 실행 중이 아닌 상태에서 `dotnet build PC_BackUp.sln` 한 번 클린 빌드.
(코드 변경 아님 — 폴더 정리 + 재빌드만)

---

## 진행 순서 제안
1 → 2 → 3 → 4. 각 항목은 서로 독립적이라 순서를 바꿔도 무방하지만, 1(전역 핸들러)을 가장 먼저 넣어두면
이후 2/3 수정 중 실수해도 앱이 죽지 않고 로그로 남으므로 디버깅이 쉬워짐.

빌드 확인은 매번 `PC_BackUp.exe`를 먼저 종료한 뒤 `dotnet build`로. (Design 뷰는 절대 열지 않음 — 코드 뷰 + F5 실행으로만 확인)
