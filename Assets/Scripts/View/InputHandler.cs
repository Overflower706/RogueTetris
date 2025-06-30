using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private LogicManager logicManager;
    private PlayerInput playerInput;

    [Header("Movement Settings")]
    private float moveRepeatRate = 0.1f;
    private float moveTimer = 0f;
    private bool isMoving = false;
    private Vector2 moveInput;

    public void Initialize(LogicManager logic)
    {
        logicManager = logic;
        SetupPlayerInput();
    }

    private void SetupPlayerInput()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        // Input Actions 에셋 로드
        var inputActions = Resources.Load<InputActionAsset>("TetrisInputActions");
        if (inputActions != null)
        {
            playerInput.actions = inputActions;
        }

        // 액션 맵 설정 (실제 게임에서는 GameInitializer에서 수행)
        playerInput.currentActionMap = playerInput.actions.FindActionMap("Gameplay");

        SetupInputCallbacks();
    }

    private void SetupInputCallbacks()
    {
        if (playerInput == null || playerInput.actions == null) return;

        var gameplayMap = playerInput.actions.FindActionMap("Gameplay");
        if (gameplayMap == null) return;

        // 이동 입력 설정
        var moveLeftAction = gameplayMap.FindAction("MoveLeft");
        if (moveLeftAction != null)
        {
            moveLeftAction.performed += OnMoveLeft;
            moveLeftAction.canceled += OnMoveStop;
        }

        var moveRightAction = gameplayMap.FindAction("MoveRight");
        if (moveRightAction != null)
        {
            moveRightAction.performed += OnMoveRight;
            moveRightAction.canceled += OnMoveStop;
        }

        var moveDownAction = gameplayMap.FindAction("MoveDown");
        if (moveDownAction != null)
        {
            moveDownAction.performed += OnMoveDown;
            moveDownAction.canceled += OnMoveStop;
        }

        // 액션 입력 설정
        var rotateAction = gameplayMap.FindAction("Rotate");
        if (rotateAction != null)
        {
            rotateAction.performed += OnRotate;
        }

        var hardDropAction = gameplayMap.FindAction("HardDrop");
        if (hardDropAction != null)
        {
            hardDropAction.performed += OnHardDrop;
        }

        var restartAction = gameplayMap.FindAction("Restart");
        if (restartAction != null)
        {
            restartAction.performed += OnRestart;
        }

        var openShopAction = gameplayMap.FindAction("OpenShop");
        if (openShopAction != null)
        {
            openShopAction.performed += OnOpenShop;
        }
    }

    void Update()
    {
        if (logicManager == null) return;
        HandleContinuousMovement();
    }

    private void HandleContinuousMovement()
    {
        if (!isMoving) return;

        Game gameData = logicManager.GetGameData();
        if (gameData == null || gameData.CurrentState != GameState.Playing) return;

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveRepeatRate)
        {
            moveTimer = 0f; if (moveInput.x < 0)
                logicManager.MoveTetrimino(Vector2Int.left);
            else if (moveInput.x > 0)
                logicManager.MoveTetrimino(Vector2Int.right);
            else if (moveInput.y < 0)
                logicManager.SoftDrop(); // MoveTetrimino 대신 SoftDrop 사용
        }
    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (!CanHandleGameInput()) return;

        moveInput = Vector2.left;
        if (!isMoving)
        {
            isMoving = true;
            moveTimer = 0f;
            logicManager.MoveTetrimino(Vector2Int.left);
        }
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        if (!CanHandleGameInput()) return;

        moveInput = Vector2.right;
        if (!isMoving)
        {
            isMoving = true;
            moveTimer = 0f;
            logicManager.MoveTetrimino(Vector2Int.right);
        }
    }
    private void OnMoveDown(InputAction.CallbackContext context)
    {
        if (!CanHandleGameInput()) return;

        moveInput = Vector2.down;
        if (!isMoving)
        {
            isMoving = true;
            moveTimer = 0f;
            logicManager.SoftDrop(); // MoveTetrimino 대신 SoftDrop 사용
        }
    }

    private void OnMoveStop(InputAction.CallbackContext context)
    {
        isMoving = false;
        moveTimer = 0f;
        moveInput = Vector2.zero;
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        if (!CanHandleGameInput()) return;
        logicManager.RotateTetrimino();
    }

    private void OnHardDrop(InputAction.CallbackContext context)
    {
        if (!CanHandleGameInput()) return;
        logicManager.DropTetrimino();
    }

    private void OnRestart(InputAction.CallbackContext context)
    {
        if (logicManager == null) return;
        logicManager.RestartGame();
    }

    private void OnOpenShop(InputAction.CallbackContext context)
    {
        if (logicManager == null) return;

        Game gameData = logicManager.GetGameData();
        if (gameData != null)
        {
            if (gameData.CurrentState == GameState.Victory)
            {
                logicManager.OpenShop();
            }
            else if (gameData.CurrentState == GameState.Shop)
            {
                logicManager.CloseShop();
            }
        }
    }

    private bool CanHandleGameInput()
    {
        if (logicManager == null) return false;

        Game gameData = logicManager.GetGameData();
        return gameData != null && gameData.CurrentState == GameState.Playing;
    }

    private void OnDestroy()
    {
        // Input Actions 정리
        if (playerInput != null && playerInput.actions != null)
        {
            var gameplayMap = playerInput.actions.FindActionMap("Gameplay");
            if (gameplayMap != null)
            {
                var moveLeftAction = gameplayMap.FindAction("MoveLeft");
                if (moveLeftAction != null)
                {
                    moveLeftAction.performed -= OnMoveLeft;
                    moveLeftAction.canceled -= OnMoveStop;
                }

                var moveRightAction = gameplayMap.FindAction("MoveRight");
                if (moveRightAction != null)
                {
                    moveRightAction.performed -= OnMoveRight;
                    moveRightAction.canceled -= OnMoveStop;
                }

                var moveDownAction = gameplayMap.FindAction("MoveDown");
                if (moveDownAction != null)
                {
                    moveDownAction.performed -= OnMoveDown;
                    moveDownAction.canceled -= OnMoveStop;
                }

                var rotateAction = gameplayMap.FindAction("Rotate");
                if (rotateAction != null)
                {
                    rotateAction.performed -= OnRotate;
                }

                var hardDropAction = gameplayMap.FindAction("HardDrop");
                if (hardDropAction != null)
                {
                    hardDropAction.performed -= OnHardDrop;
                }

                var restartAction = gameplayMap.FindAction("Restart");
                if (restartAction != null)
                {
                    restartAction.performed -= OnRestart;
                }

                var openShopAction = gameplayMap.FindAction("OpenShop");
                if (openShopAction != null)
                {
                    openShopAction.performed -= OnOpenShop;
                }
            }
        }
    }

    // 테스트용 메서드들
    public void SimulateInput(string actionName)
    {
        if (playerInput == null || playerInput.actions == null) return;

        var gameplayMap = playerInput.actions.FindActionMap("Gameplay");
        if (gameplayMap == null) return;

        var action = gameplayMap.FindAction(actionName);
        if (action == null) return;

        // 액션 시뮬레이션
        switch (actionName)
        {
            case "MoveLeft":
                OnMoveLeft(new InputAction.CallbackContext());
                break;
            case "MoveRight":
                OnMoveRight(new InputAction.CallbackContext());
                break;
            case "MoveDown":
                OnMoveDown(new InputAction.CallbackContext());
                break;
            case "Rotate":
                OnRotate(new InputAction.CallbackContext());
                break;
            case "HardDrop":
                OnHardDrop(new InputAction.CallbackContext());
                break;
            case "Restart":
                OnRestart(new InputAction.CallbackContext());
                break;
            case "OpenShop":
                OnOpenShop(new InputAction.CallbackContext());
                break;
        }
    }

    public void SimulateInputStop(string actionName)
    {
        if (actionName.StartsWith("Move"))
        {
            OnMoveStop(new InputAction.CallbackContext());
        }
    }
}
