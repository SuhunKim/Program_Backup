# Project Code Style Rules

> **이 파일은 실제 작성 사례입니다.**
> 자동화 파트의 레거시 C# (.NET Framework 4.8.1 WinForms/WPF) 프로젝트에 적용 중인 규칙입니다.
> 다른 파트/프로젝트는 이 내용을 그대로 쓰지 말고, `templates/RULES.md.template` 로
> 직접 작성하세요. 레거시 C# 프로젝트라면 이 파일을 복사해서 시작하는 것이 빠릅니다.

이 문서는 이 저장소의 코딩 규칙을 정의합니다.
**모든 개발자와 AI 에이전트(Claude, Codex 포함)는 예외 없이 이 규칙을 따릅니다.**

적용 범위: 자동화 파트 / 레거시 C# 장비 제어 프로젝트
최종 갱신: 2026-08-06

## 1. 변수 명명 규칙 (헝가리안 표기법)

- **멤버 변수**: `m_` 접두어 + 소문자 타입 약자 + 이름
  - 형식: `m_[타입약자][Name]`
- **지역 변수 및 함수 인자**: 소문자 타입 약자로 시작 (`m_` 없음)
  - 형식: `[타입약자][Name]`

### 타입 약자

| 타입 | 약자 | 예 |
| --- | --- | --- |
| `string` | `s` | `sName` |
| `int` | `i` | `iCount` |
| `double` | `d` | `dValue` |
| `long` | `l` | `lIndex` |
| `bool` | `b` | `bSuccess` |
| `string[]` | `sary` | `saryConfigs` |
| `int[]` | `iary` | `iaryCodes` |
| 객체 | `o` | `m_oRobotManager` |

- **Correct**: `m_iReady`, `bSuccess`, `saryConfigs`, `m_oRobotManager`
- **Incorrect**: `_ready`, `ready`, `isSuccess`, `configArray`

> 왜: 기존 코드 전체가 이 표기법으로 작성되어 있습니다. 최신 C# 관례와는
> 다르지만, 한 파일 안에 두 스타일이 섞이면 리뷰 비용이 계속 늘어납니다.

## 2. 메서드 명명 규칙

- **일반 메서드**: 동사 + 명사, PascalCase
  - 예: `ExecuteAutoRun()`, `CheckInterlock()`
- **일반 이벤트 핸들러**: `On` 접두어
  - 예: `OnDataReceived()`
- **UI 컨트롤 이벤트 핸들러**: `Ui` + 동작 + `_` + 설명
  - 예: `UiClick_AutoRun()`, `UiChange_Mode()`

## 3. 클래스 명명 규칙

- **`cl` 접두어를 새로 사용하지 마세요.**
  - Correct: `BufferManager`, `RobotManager`
  - Incorrect: `clBufferManager`, `clRobotManager`
- 클래스명은 PascalCase, 간결한 동사+명사 또는 명사 형태.
- 해당 클래스를 담는 인스턴스 변수는 `o` / `m_o` 접두어.
  - 예: `private RobotManager m_oRobotManager;`

## 4. 문자열 처리 규칙

- **문자열 보간(`$"..."`) 금지**
- **문자열 연결(`+`) 금지**
- 모든 문자열 구성에 **`string.Format()`** 을 사용합니다.
  - 예: `string.Format("{0}_{1} ms", sName, sValue);`

> 왜: 로그/알람/예외 메시지가 런타임에 조립되는 지점이 많고, 포맷 문자열을
> 분리해두면 문제 발생 시 검색과 추적이 쉽습니다.

## 5. 주석 및 변경 이력 규칙

- 새 메서드, 주요 리팩터링 블록, AI가 삽입한 수정은 개발자 서명으로 시작합니다.
  - `// [이름 - YYYY.MM.DD] <간단한 설명>`

### 5.1 XML Summary 형식

- 신규/수정 메서드의 XML `summary` 본문은 비워둡니다. (기존 솔루션 스타일)
- 실제 목적이나 변경 내용은 `</summary>` 바로 뒤에 날짜 주석으로 기록합니다.
- `/// <summary>` 바로 위에 빈 줄을 두지 마세요.

```csharp
/// <summary>
///
/// </summary>
/// [2026.07.24] BF01 Slot 1~3 순서로 자동 이송하고 BIB/SEM 공정을 수행합니다.
public async Task<bool> RunRobotAsync(CancellationToken ct)
```

## 6. 안전 규칙 (타협 불가)

> 이 섹션의 클래스명은 실제 존재하는 것만 적습니다.
> 코드에서 이름이 바뀌거나 삭제되면 이 문서도 같이 고치세요.

### Law #1 — 중단 토큰 우선 확인

`RobotManager`, `LoadlockManager`, `InterlockManager` 의 모든 하드웨어 폴링/드라이버
`while` 루프는 **물리 I/O 읽기 전에** `ct.IsCancellationRequested` 확인
(또는 `ct.ThrowIfCancellationRequested()`)을 루프 본문의 첫 조건으로 두어야 합니다.

작업자가 중단을 눌렀을 때 하드웨어 스캔 한 바퀴를 끝까지 기다리지 않게 하기 위함입니다.

- `MainControl.CheckAbort` (구 bool 플래그)는 2026-07-10 전 저장소에서 제거되었습니다.
  Main Abort만 반영하고 Semi Abort는 반영하지 않아 반응이 느렸습니다.
- **이 플래그나 동등한 bool 플래그를 다시 만들지 마세요.**
- 루프에 `CancellationToken` 이 없으면 `MainControl.pMain.EnsureRunToken()` 또는
  `EnsurePumpingVentToken()` 으로 얻으세요.

### Law #2 — 자동화 루프 무한 재시도 금지

자동 시퀀스 루프는 예외를 잡고 무한 재시도해서는 안 됩니다.
연속 실패 횟수를 세고, 기준치에 도달하면 자동 실행을 해제하세요.

- 일시적 예외 한 번으로 중단되면 안 됩니다.
- 지속적인 실패가 영원히 재시도되어서도 안 됩니다.

### Law #3 — 문자열 보간 금지 (재확인)

활성 또는 수정 중인 로그·알람·예외 추적 코드에 `$"..."` 를 쓰지 마세요.
`string.Format()` 필수. (§4 재확인)

## 7. 파일 위생

- `.bak` 파일을 수정하지 마세요.
- `_backup`, `_임시`, `복사본` 등 백업/사본 표시가 붙은 파일은 명시적 지시가 없으면 수정하지 마세요.
- 활성 상태인 비백업 파일을 사용하세요.
- `git add .` 를 사용하지 마세요. 의도한 파일만 선별 스테이징합니다.

---

> **저작권 안내**
> © AP Systems — Smart Engineering PC Control Team. All rights reserved.
> 이 문서는 사내 업무용으로 작성되었습니다. 사전 승인 없이 사외로 배포·전달·게시하지 마세요.
