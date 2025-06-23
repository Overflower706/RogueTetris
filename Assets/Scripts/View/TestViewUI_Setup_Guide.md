# TestViewUI 설정 가이드

## Unity에서 TestViewUI 설정하는 방법

### 1. GameObject 생성
1. Unity 에디터에서 `Create Empty` GameObject를 생성합니다.
2. 이름을 "GameTester"로 변경합니다.

### 2. 컴포넌트 추가
1. GameTester GameObject를 선택합니다.
2. `Add Component` → `TestViewUI` 스크립트를 추가합니다.

### 3. UI Canvas 설정
1. `Create` → `UI` → `Canvas`를 생성합니다.
2. Canvas 하위에 `Create` → `UI` → `Text - TextMeshPro`를 생성합니다.
3. TextMeshPro 오브젝트의 이름을 "GameStateText"로 변경합니다.

### 4. TextMeshPro 설정
1. GameStateText를 선택합니다.
2. RectTransform에서:
   - Anchor: Top Left
   - Position: X=10, Y=-10
   - Width: 800, Height: 600
3. TextMeshPro - Text (UI) 컴포넌트에서:
   - Font Size: 12
   - Color: White
   - Alignment: Top Left
   - Overflow: Page (텍스트가 길면 스크롤)
   - Font Asset: LiberationSans SDF (기본)

### 5. TestViewUI 연결
1. GameTester GameObject를 선택합니다.
2. TestViewUI 컴포넌트의 `Game State Text` 필드에 GameStateText를 드래그&드롭합니다.

### 6. 실행
1. Play 버튼을 누르면 게임이 시작됩니다.
2. 조작법:
   - ← → : 좌우 이동
   - ↓ : 소프트 드롭
   - ↑ / Z : 회전
   - 스페이스바 : 하드 드롭 (즉시 바닥까지)
   - R : 게임 재시작

### 7. 추가 설정 (선택사항)
TestViewUI 컴포넌트에서 조정 가능한 값들:
- `Move Repeat Rate`: 키를 계속 누를 때 이동 간격 (기본: 0.1초)
- `Move Initial Delay`: 키를 누른 후 연속 이동 시작까지의 지연 (기본: 0.2초)

## 주요 기능

### 실시간 게임 상태 표시
- 현재 점수 및 목표 점수
- 게임 상태 (Playing, GameOver, Victory)
- 현재 테트리미노 타입 및 위치
- 다음 테트리미노 정보
- ASCII 아트로 표현된 보드 상태

### 입력 처리
- 키보드 입력으로 테트리미노 조작
- 연속 입력 지원 (키를 누르고 있으면 계속 이동)
- 게임 재시작 기능

### 디버그 정보
- 게임 에디터에서 실행 시 화면 우상단에 간단한 정보 표시
- 점수, 상태, 통화, 현재/다음 테트리미노 정보

## 문제 해결

### TextMeshPro 관련 오류
- "The type or namespace name 'TextMeshProUGUI' could not be found" 오류가 발생하면:
  1. Window → Package Manager 열기
  2. "TextMeshPro" 검색 후 Import
  3. TMP Essentials 설치

### 텍스트가 잘리는 경우
- GameStateText의 RectTransform Width/Height 값을 늘려주세요
- TextMeshPro 컴포넌트의 Overflow를 "Scroll Rect"로 설정하면 스크롤 가능합니다

### 성능 이슈
- Update에서 매 프레임 UI를 업데이트하므로, 필요시 업데이트 주기를 조절할 수 있습니다
- 실제 게임에서는 상태 변경 시에만 UI를 업데이트하는 것이 좋습니다
