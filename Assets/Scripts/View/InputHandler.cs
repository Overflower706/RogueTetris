using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private LogicManager logicManager;

    [Header("Input Settings")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode downKey = KeyCode.S;
    public KeyCode rotateKey = KeyCode.W;
    public KeyCode hardDropKey = KeyCode.Space;
    public KeyCode restartKey = KeyCode.R;

    private float moveRepeatRate = 0.1f; // 키를 누르고 있을 때 반복 속도
    private float moveTimer = 0f;
    private bool isMoving = false;

    public void Initialize(LogicManager logic)
    {
        logicManager = logic;
    }

    void Update()
    {
        if (logicManager == null) return;

        HandleGameInput();
        HandleUIInput();
    }

    private void HandleGameInput()
    {
        Game gameData = logicManager.GetGameData();
        if (gameData == null || gameData.currentState != GameState.Playing) return;

        // 이동 입력 (연속 입력 지원)
        HandleMovementInput();

        // 회전 입력 (한 번만)
        if (Input.GetKeyDown(rotateKey))
        {
            logicManager.RotateTetrimino();
        }

        // 하드 드롭 (한 번만)
        if (Input.GetKeyDown(hardDropKey))
        {
            logicManager.DropTetrimino();
        }

        // 소프트 드롭 (누르고 있는 동안)
        if (Input.GetKey(downKey))
        {
            logicManager.MoveTetrimino(Vector2Int.down);
        }
    }

    private void HandleMovementInput()
    {
        bool leftPressed = Input.GetKey(leftKey);
        bool rightPressed = Input.GetKey(rightKey);

        // 새로운 입력이 시작됨
        if ((leftPressed || rightPressed) && !isMoving)
        {
            isMoving = true;
            moveTimer = 0f;

            // 즉시 이동
            if (leftPressed)
                logicManager.MoveTetrimino(Vector2Int.left);
            else if (rightPressed)
                logicManager.MoveTetrimino(Vector2Int.right);
        }
        // 입력이 계속되고 있음
        else if ((leftPressed || rightPressed) && isMoving)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer >= moveRepeatRate)
            {
                moveTimer = 0f;

                if (leftPressed)
                    logicManager.MoveTetrimino(Vector2Int.left);
                else if (rightPressed)
                    logicManager.MoveTetrimino(Vector2Int.right);
            }
        }
        // 입력이 끝남
        else if (!leftPressed && !rightPressed)
        {
            isMoving = false;
            moveTimer = 0f;
        }
    }

    private void HandleUIInput()
    {
        // 재시작 (언제든지 가능)
        if (Input.GetKeyDown(restartKey))
        {
            logicManager.RestartGame();
        }

        // ESC로 상점 열기/닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Game gameData = logicManager.GetGameData();
            if (gameData != null)
            {
                if (gameData.currentState == GameState.Victory)
                {
                    logicManager.OpenShop();
                }
                else if (gameData.currentState == GameState.Shop)
                {
                    logicManager.CloseShop();
                }
            }
        }
    }
}
