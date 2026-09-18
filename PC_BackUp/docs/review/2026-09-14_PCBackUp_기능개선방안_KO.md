# PC Backup Manager 기능 개선/추가 방안

- 이슈: 없음 (정기 점검 요청에 따른 제안)
- 작성: 2026-09-14 / Claude
- 대상: `ARCHITECTURE.md` §7 "후속 개발 권장 순서"에 남아 있는 5개 항목 + §8 미결정 1건
- 관점: 기능 개선/추가 (버그 수정 아님 — 보안/안정성 수정은 `SECURITY_FIXES_TODO.md`에서 이미 4건 모두 완료됨)

## 결론

1. 남은 5개 항목 중 **①백업 전 검증**과 **④용량/남은시간 표시**는 기존 서비스(`BackupService`, `AppSettings`)를 확장하는 수준이라 위험이 낮고 체감 효과가 크다 — 우선 착수 권장.
2. **②보존 정책**과 **⑥복원 후 자동 재실행**(미결정 항목)은 `AppSettings`에 필드를 추가하고 기존 토글 패턴(복원 화면 체크박스, 저장 즉시 반영)을 그대로 따라가면 되므로 난이도가 낮다.
3. **③예약 백업**은 유일하게 새 실행 경로(헤드리스 모드)와 OS 통합(작업 스케줄러/시작프로그램)이 필요해 설계 결정이 먼저 필요하다 — 아래 §3에 방향 제시.
4. **⑤단위 테스트**는 아키텍처 문서가 이미 명시했듯 `Services/`가 UI를 참조하지 않아 준비가 되어 있다. 별도 테스트 프로젝트만 추가하면 된다.

## 확인한 것

- [ARCHITECTURE.md:218-237](../../PC_BackUp/ARCHITECTURE.md) — §7 남은 항목 5개, §8 미결정 1건(복원 후 자동 재실행)이 텍스트로 이미 정리되어 있음.
- [SECURITY_FIXES_TODO.md](../../../SECURITY_FIXES_TODO.md) — 보안 수정 4건 모두 체크 완료. 현재 남은 작업은 순수 기능 확장뿐임을 확인.
- [Kernel/Configuration/Models/AppSettings.cs](../../PC_BackUp/Kernel/Configuration/Models/AppSettings.cs) — 설정 스키마, `Validate()`, `GetSourceRoot()` 구조. 새 옵션은 이 클래스에 속성 추가 + `XmlSettingsService`가 그대로 직렬화하는 방식으로 자연스럽게 확장됨.
- [Services/BackupService.cs](../../PC_BackUp/Services/BackupService.cs) — 파일 잠금/권한 사전 검사 없음(현재는 `CreateZip`/`CopyToFolder` 도중 예외가 나야 실패로 드러남). 진행률은 파일 개수 기준(`(index+1)*100/files.Count`)만 계산하고 바이트/용량 개념이 전혀 없음.
- [Services/BackupCatalogService.cs](../../PC_BackUp/Services/BackupCatalogService.cs) — 보존 정책(개수/기간 제한) 로직 없음. `GetAll()`이 이미 최신순 정렬 리스트를 주므로 정리 대상 선정에 바로 쓸 수 있음.
- [Kernel/GUI/02_Recovery/02_RecoveryControl.cs:148-242](../../PC_BackUp/Kernel/GUI/02_Recovery/02_RecoveryControl.cs) — `AutoBackupBeforeRestore`/`CompareBeforeRestore` 토글이 체크 즉시 `AppSettings`에 저장되는 기존 패턴 확인. 신규 옵션(자동 재실행 등)도 이 패턴을 그대로 재사용 가능.
- `Services/*.cs` 전체 — 생성자에서 UI 컨트롤을 참조하는 서비스가 없음(순수 POCO/파일시스템 로직). 단위 테스트 도입에 구조적 장벽 없음.

## 제안 항목

### ① `_bin`/`Config` 위치·권한 검증 + 백업 전 파일 잠금 검사 (우선순위: 상, 난이도: 하)

- 현재 `AppSettings.Validate()`는 경로 문자열 존재 여부만 확인하고, `BackupService.EnsureSelectiveFoldersExist()`는 폴더 "존재"만 검사한다. 잠긴 파일이나 권한 부족은 ZIP 생성 도중 `IOException`/`UnauthorizedAccessException`으로만 드러나 어떤 파일이 문제였는지 사용자가 알기 어렵다.
- 제안: `BackupService`에 사전 점검 메서드를 추가한다.
  ```csharp
  public static IReadOnlyList<string> CheckFileAccessibility(IEnumerable<string> files)
  {
      var blocked = new List<string>();
      foreach (var file in files)
      {
          try
          {
              using var _ = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
          }
          catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
          {
              blocked.Add(file);
          }
      }
      return blocked;
  }
  ```
  `CreateAsync`에서 파일 목록을 만든 직후(§`EnumerateSourceFiles` 다음) 이 검사를 실행해, 잠긴 파일이 있으면 `OperationResult.Failure`로 목록을 반환한다(전체 실패 대신 "건너뛰고 계속" 옵션은 후속 범위로 남겨도 됨).
- 대상 백업 폴더(`BackupRootPath`)의 쓰기 권한은 `Directory.CreateDirectory` 성공 여부로 간접 확인 중인데, `PathTooLongException`/`UnauthorizedAccessException`을 `OperationResult.Failure`로 명시 변환해 메시지를 사용자에게 그대로 노출하면 충분하다(별도 ACL API까지는 과설계).
- 원본 폴더(`_bin`, `Config`) 위치 검증은 `AppSettings.Validate()`에 "선별 폴더가 실제로 원본 루트 하위에 있는가"를 추가하면 됨 — `EnsureSelectiveFoldersExist`와 로직이 겹치므로 백업 실행 시점이 아니라 환경설정 화면 저장 시점으로 앞당기는 것이 사용자 경험상 낫다(04_SettingControl 저장 버튼에서 `Validate()` 호출 시 이미 걸러짐 확인 필요).

### ② 백업 보존 개수/기간 정책 (우선순위: 상, 난이도: 하)

- `AppSettings`에 옵션 2개 추가:
  ```csharp
  public int? MaxBackupCount { get; set; } // null = 무제한
  public int? MaxBackupAgeDays { get; set; } // null = 무제한
  ```
- `BackupCatalogService.GetAll()`은 이미 최신순 정렬을 제공하므로, 새 `BackupRetentionService`(또는 `BackupCatalogService`에 메서드 추가)에서:
  1. `MaxBackupCount` 초과분(오래된 것부터) 삭제 대상으로 선정
  2. `MaxBackupAgeDays`보다 오래된 것 삭제 대상으로 선정
  3. 두 조건의 합집합에서 **최소 1개(가장 최근 백업)는 항상 제외**(전량 삭제 방지)
  4. `BackupKind`에 따라 `File.Delete` 또는 `Directory.Delete(path, true)` 분기
- 적용 시점: `BackupService.CreateAsync` 성공 후 `m_oCatalogService.Refresh(backupRoot)` 다음 줄에서 호출 — 새 백업이 생긴 직후에만 정리하면 백그라운드 타이머가 필요 없다.
- UI: 04_SettingControl에 "보존 개수 제한"/"보존 기간(일)" 입력 필드 2개 추가(둘 다 빈 값=무제한). 삭제된 백업은 `LogManager`에 기록해 이력 화면 "작업 로그"에서 확인 가능하게 한다.
- 삭제 전 사용자 확인 여부는 결정 필요 — 자동 백업 정리는 보통 무확인으로 하는 것이 실용적이나, 처음 활성화할 때 한 번은 안내 메시지를 보여주는 절충안을 권장한다.

### ③ 예약 백업 + Windows 시작 시 실행 (우선순위: 중, 난이도: 상 — 설계 결정 필요)

이 항목만 유일하게 인프라 추가가 필요하다. 두 방식을 비교한다.

| 방식 | 장점 | 단점 |
|---|---|---|
| **A. Windows 작업 스케줄러(schtasks.exe) 위임** (권장) | 앱이 안 떠 있어도 실행됨, OS 재부팅에도 살아남음, 절전모드 깨우기 등 OS 기능 활용 가능 | `Program.cs`에 헤드리스(무-UI) 백업 모드 인자 처리 필요 |
| B. 앱 내부 타이머 + 트레이 상주 | 구현이 눈에 보임(진행 상황 UI 재사용 가능) | 앱이 항상 떠 있어야 함 — "예약"의 의미가 약해짐, 트레이 아이콘/상주 로직 추가 필요 |

- **A안 세부 방향**:
  1. `Program.cs`에 `Main(string[] args)`로 변경, `--auto-backup` 인자가 있으면 `ApplicationConfiguration.Initialize()`/`Application.Run(new MainForm())` 대신 `AppSettings` 로드 → `BackupService.CreateAsync(..., BackupKind.FullZip)` 실행 후 종료 코드만 반환(창 없음).
  2. 04_SettingControl에 "예약 백업 사용" 체크박스 + 주기(매일/매주 요일) + 시각 UI 추가. 저장 시 `schtasks /create /tn "PC_BackUp_AutoBackup_<TargetProjectName>" /tr "<exe경로> --auto-backup" /sc daily /st HH:mm /f`를 `Process.Start`로 실행(관리자 권한 불필요 범위로 `/ru` 생략, 현재 로그인 사용자 컨텍스트 사용).
  3. "Windows 시작 시 실행"은 별도 옵션으로, `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`에 값 추가/삭제(레지스트리 직접 조작은 관리자 권한 불필요, `Microsoft.Win32.Registry` 사용).
  4. 헤드리스 실행 실패는 `LogManager`에만 기록(사용자가 안 보고 있으므로 `MessageBox` 금지).
- 이 항목은 착수 전에 "관리자 권한 필요 여부", "여러 프로젝트를 백업하는 인스턴스가 각각 스케줄을 만들 때 작업 이름 충돌 방지" 두 가지를 먼저 결정하는 것을 권장한다.

### ④ 대용량 백업 예상 용량 · 남은 시간 표시 (우선순위: 상, 난이도: 하)

- 현재 `BackupService.CreateZip`/`CopyToFolder`의 `progress?.Report(...)`는 파일 **개수** 기준 퍼센트만 준다. 큰 파일 하나가 대부분을 차지하면 퍼센트가 왜곡된다.
- 제안:
  1. 백업 시작 전 `files` 목록을 얻은 직후 `files.Sum(f => new FileInfo(f).Length)`로 총 바이트 계산 → 01_BackupControl에서 다이얼로그 확인 후 "예상 용량: N MB" 라벨로 표시.
  2. 같은 시점에 `new DriveInfo(Path.GetPathRoot(settings.BackupRootPath)).AvailableFreeSpace`와 비교해 여유 공간이 부족하면 시작 전에 경고(§①의 사전 검증과 같은 자리에 추가하면 자연스러움).
  3. `IProgress<int>` 대신 `IProgress<(long BytesDone, long BytesTotal)>`로 바꾸거나 별도 콜백을 추가해 바이트 단위 진행을 보고하도록 `CreateZip`/`CopyToFolder` 수정. UI 쪽에서 경과 시간 대비 처리 속도(`bytesDone / elapsed.TotalSeconds`)로 남은 시간을 추정해 "약 N분 남음"으로 표시.
  4. 기존 호출부(`IProgress<int>`)와의 호환을 위해 새 오버로드를 추가하는 방식을 권장(기존 퍼센트 표시는 유지, 신규 바이트 진행은 선택적으로 구독).

### ⑤ 서비스 계층 단위 테스트 (우선순위: 중, 난이도: 하)

- `PC_BackUp.sln`에 `PC_BackUp.Tests` (xUnit 권장 — .NET 생태계 표준, `net7.0` 대상 프로젝트와 궁합 좋음) 프로젝트를 새로 추가하고 `Services`, `_util` 프로젝트를 참조.
- 우선 커버할 대상(전부 임시 디렉터리 기반 통합 테스트로 충분):
  - `FileNaming` — 폴더명 유효성 검사, 접미사 정리 규칙.
  - `BackupCatalogService.TryParseBackupFileName` — 신규/구버전 파일명 패턴 각각, 잘못된 이름 거부.
  - `BackupService.CreateAsync` — 임시 폴더에 더미 원본을 만들고 전체/선별 백업 각각 실행 → 산출물 파일 목록·이름 규칙·백업 폴더 자기 제외 검증.
  - `RecoveryService`의 안전 압축 해제(`SafeExtractToDirectory`, 이미 구현됨) — zip-slip 시도용 조작된 zip으로 예외 발생 확인(보안 회귀 테스트 겸함).
  - `XmlComparisonService` — 리프 값/특성 비교, 선택 적용 후 원본 XML이 백업 값으로 정확히 바뀌는지.
- 각 테스트는 `IDisposable` 픽스처로 임시 디렉터리를 만들고 `Dispose()`에서 정리하는 패턴을 권장(테스트 간 파일 잔존 방지).

### ⑥ (미결정 항목) 복원 후 대상 프로그램 자동 재실행 (우선순위: 중, 난이도: 하)

- `ARCHITECTURE.md` §8에 미결정으로 남아 있던 항목. 기존 `AutoBackupBeforeRestore`/`CompareBeforeRestore`와 동일한 패턴을 그대로 따르면 구현 자체는 단순하다.
- 제안: `AppSettings.RestartTargetAfterRestore` (bool, 기본 꺼짐) 추가 → 02_RecoveryControl 상세 패널에 기존 두 체크박스와 나란히 배치, 체크 즉시 저장(§145 문서 표현과 일치).
- 동작: `RecoveryService`가 복원 전 대상 프로세스를 종료시킨 경우에 한해서만(사용자가 직접 꺼둔 상태였다면 복원 후 임의로 새로 띄우지 않음) 복원 성공 시 `Process.Start(settings.ExecutablePath)` 호출. 실행 실패는 복원 자체 실패로 취급하지 않고 로그+안내 메시지로만 알림.

## 확인하지 못한 것

- 실제 대상 프로그램(백업 대상이 되는 다른 사내 프로그램들)에서 `_bin`/`Config` 폴더 권한이 실제로 문제가 된 사례가 있는지는 이 저장소만으로 확인 불가 — 필요 시 실사용 환경에서 재현 사례 확인 권장.
- 예약 백업(③)의 "관리자 권한 필요 여부"는 `schtasks`를 일반 사용자 컨텍스트로 등록하는 경우 대부분 불필요하지만, 사내 PC의 그룹 정책(GPO)이 작업 스케줄러 등록을 제한하는지는 이 세션에서 확인할 수 없음.
- 위 항목들의 공수(개발 시간) 추정치는 포함하지 않았다 — 필요하면 항목별로 별도 산정 요청.
- 빌드/실행 검증은 수행하지 않았다(문서 작성만 진행, 코드 변경 없음).

## 다음 단계 제안

1. 우선순위 상(①④)부터 착수 여부를 결정하고, 착수 시 이 문서의 해당 절을 그대로 작업 지시서로 사용.
2. ③(예약 백업)은 착수 전 "관리자 권한", "다중 인스턴스 작업 이름 충돌 방지" 두 결정 사항에 대해 사용자 확인 먼저 받기.
3. 결정된 항목은 `SECURITY_FIXES_TODO.md`와 같은 형식으로 `docs/review/`에 실행용 작업 목록을 별도로 뽑아내거나, 이 문서에 체크박스를 추가해 진행 상황을 추적.
