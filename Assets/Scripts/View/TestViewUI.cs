using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TestViewUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI gameStateText;

    [Header("Logic Manager")]
    [SerializeField] private LogicManager logicManager;

    [Header("Input Settings")]
    [SerializeField] private float moveRepeatRate = 0.1f;
    [SerializeField] private float moveInitialDelay = 0.2f;

    private float moveTimer = 0f;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool isMovingDown = false;

    // Input System 관련
    private Keyboard keyboard; void Start()
    {
        Debug.Log("=== TestViewUI Start 시작 ===");

        // Keyboard 입력 초기화
        keyboard = Keyboard.current;
        Debug.Log($"Keyboard 초기화: {(keyboard != null ? "성공" : "실패")}");

        // LogicManager 컴포넌트 가져오기 또는 생성
        logicManager = GetComponent<LogicManager>();
        if (logicManager == null)
        {
            Debug.Log("LogicManager 컴포넌트 없음 - 새로 생성");
            logicManager = gameObject.AddComponent<LogicManager>();
        }
        else
        {
            Debug.Log("기존 LogicManager 컴포넌트 발견");
        }

        // LogicManager 초기화
        Debug.Log("LogicManager.Initialize() 호출");
        logicManager.Initialize();

        // UI 컴포넌트 확인
        if (gameStateText == null)
        {
            Debug.LogError("GameStateText가 할당되지 않았습니다!");
        }
        else
        {
            Debug.Log("GameStateText 할당 확인됨");
        }

        // 초기 UI 업데이트
        UpdateUI();

        Debug.Log("=== TestViewUI 초기화 완료 ===");
    }

    void Update()
    {
        // 프레임 확인용 디버그
        Debug.Log($"[Frame {Time.frameCount}] Update 실행 중 - deltaTime: {Time.deltaTime:F3}");

        // AutoFall 업데이트
        if (logicManager != null)
        {
            Debug.Log("LogicManager 발견됨 - UpdateAutoFall 호출");
            logicManager.UpdateAutoFall(Time.deltaTime);
        }
        else
        {
            Debug.LogError("LogicManager가 null입니다!");
        }

        // 입력 처리
        HandleInput();

        // UI 업데이트
        UpdateUI();
    }
    private void HandleInput()
    {
        if (logicManager == null || keyboard == null) return;

        var gameData = logicManager.GetGameData();
        if (gameData == null || gameData.currentState != GameState.Playing) return;

        bool inputReceived = false;

        // 회전 (한 번 누르기)
        if (keyboard.upArrowKey.wasPressedThisFrame || keyboard.zKey.wasPressedThisFrame)
        {
            logicManager.RotateTetrimino();
            inputReceived = true;
        }

        // 하드 드롭 (한 번 누르기)
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            logicManager.DropTetrimino();
            inputReceived = true;
        }

        // 게임 재시작 (한 번 누르기)
        if (keyboard.rKey.wasPressedThisFrame)
        {
            logicManager.RestartGame();
            inputReceived = true;
        }

        // 연속 이동 처리
        HandleContinuousMovement(ref inputReceived);

        // 입력이 있었으면 UI 즉시 업데이트
        if (inputReceived)
        {
            UpdateUI();
        }
    }
    private void HandleContinuousMovement(ref bool inputReceived)
    {
        if (keyboard == null) return;

        // 현재 프레임에서 눌린 키 확인
        bool leftPressed = keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed;
        bool rightPressed = keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed;
        bool downPressed = keyboard.downArrowKey.isPressed || keyboard.sKey.isPressed;

        // 키가 눌렸을 때 즉시 한 번 이동 (첫 입력)
        if (keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame)
        {
            logicManager.MoveTetrimino(Vector2Int.left);
            isMovingLeft = true;
            moveTimer = moveInitialDelay;
            inputReceived = true;
        }
        else if (keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame)
        {
            logicManager.MoveTetrimino(Vector2Int.right);
            isMovingRight = true;
            moveTimer = moveInitialDelay;
            inputReceived = true;
        }
        else if (keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame)
        {
            logicManager.SoftDrop();
            isMovingDown = true;
            moveTimer = moveInitialDelay;
            inputReceived = true;
        }

        // 키가 떼어졌을 때 연속 이동 중지
        if (!leftPressed) isMovingLeft = false;
        if (!rightPressed) isMovingRight = false;
        if (!downPressed) isMovingDown = false;

        // 연속 이동 타이머 업데이트
        if (isMovingLeft || isMovingRight || isMovingDown)
        {
            moveTimer -= Time.deltaTime;

            if (moveTimer <= 0f)
            {
                if (isMovingLeft && leftPressed)
                {
                    logicManager.MoveTetrimino(Vector2Int.left);
                    inputReceived = true;
                }
                else if (isMovingRight && rightPressed)
                {
                    logicManager.MoveTetrimino(Vector2Int.right);
                    inputReceived = true;
                }
                else if (isMovingDown && downPressed)
                {
                    logicManager.SoftDrop();
                    inputReceived = true;
                }

                moveTimer = moveRepeatRate;
            }
        }
    }
    private void UpdateUI()
    {
        if (logicManager == null || gameStateText == null) return;

        var gameData = logicManager.GetGameData();
        if (gameData == null) return;

        // 간단한 정보 + 보드만 표시
        string gameInfo = $"Score: {gameData.currentScore}/{gameData.targetScore} | State: {gameData.currentState}\n\n";
        string boardInfo = GameLogger.GetBoardString(gameData.board, gameData.currentTetrimino);

        // UI 텍스트 업데이트
        gameStateText.text = gameInfo + boardInfo;

        // 게임 오버나 승리 시 추가 정보 표시
        if (gameData.currentState == GameState.GameOver)
        {
            gameStateText.text += "\n\n=== 게임 오버 ===\n게임을 재시작하려면 R키를 누르세요";
        }
        else if (gameData.currentState == GameState.Victory)
        {
            gameStateText.text += "\n\n=== 승리! ===\n게임을 재시작하려면 R키를 누르세요";
        }
        else if (gameData.currentState == GameState.Playing)
        {
            gameStateText.text += "\n\n=== 조작법 ===\n" +
                                  "← → : 좌우 이동\n" +
                                  "↓ : 소프트 드롭\n" +
                                  "↑ / Z : 회전\n" +
                                  "스페이스 : 하드 드롭\n" +
                                  "R : 게임 재시작";
        }
    }

    // 외부에서 UI 업데이트를 요청할 수 있는 메서드
    public void ForceUpdateUI()
    {
        UpdateUI();
    }

    // 디버그용 메서드들
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void OnGUI()
    {
        if (logicManager == null) return;

        var gameData = logicManager.GetGameData();
        if (gameData == null) return;

        // 화면 우상단에 간단한 정보 표시
        GUILayout.BeginArea(new Rect(Screen.width - 200, 10, 190, 100));
        GUILayout.Label($"Score: {gameData.currentScore}/{gameData.targetScore}");
        GUILayout.Label($"State: {gameData.currentState}");
        GUILayout.Label($"Currency: {gameData.currency}");
        if (gameData.currentTetrimino != null)
        {
            GUILayout.Label($"Current: {gameData.currentTetrimino.type}");
        }
        if (gameData.nextTetrimino != null)
        {
            GUILayout.Label($"Next: {gameData.nextTetrimino.type}");
        }
        GUILayout.EndArea();
    }
}
