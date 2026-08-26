# Project Code Style Rules

이 문서는 이 저장소(PC_BackUp)의 코딩 규칙을 정의합니다.
**모든 개발자와 AI 에이전트(Claude, Codex 포함)는 예외 없이 이 규칙을 따릅니다.**

적용 범위: PC_BackUp (WinForms 백업 도구, .NET 10 / C#)
작성자: Claude (사용자 검토 후 확정)
최종 갱신: 2026-08-26

> 참고: `docs/rules/RULES_자동화파트_CSharp_Legacy.md`는 **다른** 프로젝트(자동화 파트,
> .NET Framework 4.8.1 하드웨어 제어)를 위한 작성 예시이며, 이 프로젝트에 그대로 적용하지
> 않습니다. 아래 각 항목에 "왜 다른가"를 적어둔 이유입니다.

## 1. 변수 명명 규칙

- **멤버 변수**: `m_` 접두어 + 소문자 타입 약자 + 이름. 형식: `m_[타입약자][Name]`
  - `string` → `s`, `int` → `i`, `double` → `d`, `long` → `l`, `bool` → `b`, 객체 → `o`
  - 예: `m_oSettingsService`, `m_bIsBusy`, `m_oAllRecords`
  - Correct: `m_oCatalogService`, `m_bIsLogView` / Incorrect: `_catalogService`, `isLogView`
- **지역 변수 및 함수 인자**: **일반 camelCase**를 씁니다(`m_` 없음, 타입 약자 접두어 없음).
  - 예: `var settings = m_oSettingsService.Load();`, `var dayRecords = ...`
  - **왜 자동화 파트 예시와 다른가**: 자동화 파트 예시는 지역변수도 타입약자를 붙이지만,
    이 프로젝트는 처음부터 지역변수 전체(200개 이상)가 이미 일관되게 camelCase로 작성돼
    있습니다. 기존 코드를 소급 변경하지 않기로 했으므로(2026-08-26 결정), camelCase를
    프로젝트 표준으로 채택합니다. **새 코드도 camelCase로 작성**하세요 — 기존 지역변수를
    타입약자 접두어로 바꾸는 리팩터링은 하지 않습니다.

## 2. 메서드 명명 규칙

- **일반 메서드**: 동사 + 명사, PascalCase. 예: `RefreshBackupList()`, `ApplyCalendarSizing()`
- **일반 이벤트 핸들러(비-UI)**: `On` 접두어. 예: `OnMenuSelected()`
- **UI 컨트롤 이벤트 핸들러**: `Ui` + 동작 + `_` + 설명.
  - 예: `UiClick_Backup()`, `UiChange_SourceIsCurrent()`
  - Correct: `UiClick_Compare`, `UiChange_AutoSafetyBackup`
  - Incorrect: `CompareButton_Click`, `AutoSafetyBackupCheckBox_CheckedChanged`
  - 등록부(`xxx.Click += UiClick_...;`)가 `.Designer.cs`에 있는 경우, 이름을 바꿀 때
    **정의부와 등록부를 반드시 함께** 바꾸세요. 하나만 바꾸면 컴파일 에러로 바로 드러납니다.
  - 이름 없는 인라인 람다 핸들러(`button.Click += (_, _) => Foo();`)는 이 규칙의 적용
    대상이 아닙니다. 이름을 붙일지는 별도로 판단하세요.

## 3. 클래스 명명 규칙

- PascalCase, 간결한 동사+명사 또는 명사 형태.
- **`cl` 접두어를 새로 사용하지 마세요.** Correct: `BackupService` / Incorrect: `clBackupService`
- 해당 클래스를 담는 인스턴스 변수는 `o` / `m_o` 접두어. 예: `private BackupService m_oBackupService;`

## 4. 문자열 처리 규칙

- **문자열 보간(`$"..."`) 금지**
- **문자열 연결(`+`) 금지**
- 모든 문자열 구성에 **`string.Format()`**을 사용합니다.
  - 예: `string.Format("백업 {0}건  ·  로그 {1}건", dayRecords.Count, dayLogCount);`

> 왜: 로그/알람/예외 메시지가 런타임에 조립되는 지점이 많고, 포맷 문자열을 분리해두면 문제
> 발생 시 검색과 추적이 쉽습니다. 이 프로젝트는 이미 대부분(67곳 이상) `string.Format()`을
> 쓰고 있어 그대로 표준으로 채택했습니다.

## 5. 주석 및 변경 이력 규칙

- 새 메서드, 주요 리팩터링 블록, AI가 삽입한 수정은 개발자 서명으로 시작합니다.
  - `// [이름 - YYYY.MM.DD] <간단한 설명>`
  - 기존 코드에는 소급 적용하지 않습니다 — 새로 추가/수정하는 코드부터 적용합니다.

### 5.1 XML Summary 형식

- **`<summary>` 안에 실제 설명을 그대로 씁니다.** (자동화 파트 예시의 "빈 칸 + 날짜주석"
  스타일은 채택하지 않습니다.)
  - 예:
    ```csharp
    /// <summary>
    /// 달력에서 날짜를 고르면 그날의 백업 개수/로그 개수를 요약하고, 비교 대상 콤보박스를
    /// 그날의 백업으로만 채운다.
    /// </summary>
    private void SelectDate(DateTime date)
    ```
  - **왜 자동화 파트 예시와 다른가**: 이 프로젝트는 이미 46곳의 `<summary>`가 전부 실제
    설명을 담고 있고, IDE 툴팁에서 바로 설명이 보이는 쪽이 더 유용하다고 판단했습니다
    (2026-08-26 결정).

## 6. 안전 규칙

이 프로젝트에는 `RobotManager`, `LoadlockManager`, `InterlockManager` 같은 하드웨어 제어
클래스가 **존재하지 않습니다**. 자동화 파트 예시의 Law #1(중단 토큰 우선 확인)/Law #2(자동화
루프 무한 재시도 금지)는 **해당 없음**입니다. 코드에 해당 클래스가 새로 생기면 이 섹션을
다시 채우세요.

일반적인 취소 처리(`CancellationTokenSource`/`CancellationToken`)는 `Services\`와 각 화면
Control의 백업/복원/비교 작업에 이미 쓰이고 있으며, 이는 하드웨어 Law와 무관한 일반적인
비동기 취소 패턴입니다.

## 7. 파일 위생

- `.bak` 파일을 수정하지 마세요.
- `_backup`, `_임시`, `복사본` 등 백업/사본 표시가 붙은 파일은 명시적 지시가 없으면 수정하지 마세요.
- 활성 상태인 비백업 파일을 사용하세요.
- `git add .` 를 사용하지 마세요. 의도한 파일만 선별 스테이징합니다.

---

> **저작권 안내**
> © AP Systems — Smart Engineering PC Control Team. All rights reserved.
> 이 문서는 사내 업무용으로 작성되었습니다. 사전 승인 없이 사외로 배포·전달·게시하지 마세요.
