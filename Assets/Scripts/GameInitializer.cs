using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviour
{
    [Header("Scene Setup")]
    public GameObject logicManagerPrefab;
    public GameObject viewManagerPrefab;

    [Header("Auto-Create Basic UI")]
    public bool createBasicUI = true;
    public Canvas mainCanvas;

    void Awake()
    {
        // LogicManager 생성
        if (FindFirstObjectByType<LogicManager>() == null)
        {
            if (logicManagerPrefab != null)
            {
                Instantiate(logicManagerPrefab);
            }
            else
            {
                // 기본 LogicManager 생성
                GameObject logicObj = new GameObject("LogicManager");
                logicObj.AddComponent<LogicManager>();
            }
        }

        // ViewManager 생성
        if (FindFirstObjectByType<ViewManager>() == null)
        {
            if (viewManagerPrefab != null)
            {
                Instantiate(viewManagerPrefab);
            }
            else
            {
                CreateDefaultViewManager();
            }
        }

        // 기본 UI 생성
        if (createBasicUI)
        {
            CreateBasicUI();
        }
    }

    private void CreateDefaultViewManager()
    {
        GameObject viewObj = new GameObject("ViewManager");
        ViewManager viewManager = viewObj.AddComponent<ViewManager>();

        // InputHandler 생성
        GameObject inputObj = new GameObject("InputHandler");
        inputObj.transform.SetParent(viewObj.transform);
        InputHandler inputHandler = inputObj.AddComponent<InputHandler>();

        viewManager.inputHandler = inputHandler;
    }

    private void CreateBasicUI()
    {
        if (mainCanvas == null)
        {
            // 캔버스 생성
            GameObject canvasObj = new GameObject("MainCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            mainCanvas = canvas;
        }

        // 기본 게임 UI 생성
        CreateGameUI();
        CreateShopUI();
        CreateMenuUI();
    }

    private void CreateGameUI()
    {
        GameObject gameUIObj = new GameObject("GameView");
        gameUIObj.transform.SetParent(mainCanvas.transform, false);

        GameView gameView = gameUIObj.AddComponent<GameView>();

        // 기본 UI 요소들 생성
        CreateBasicGameElements(gameView);

        // ViewManager에 연결
        ViewManager viewManager = FindFirstObjectByType<ViewManager>();
        if (viewManager != null)
        {
            viewManager.gameView = gameView;
        }
    }

    private void CreateBasicGameElements(GameView gameView)
    {
        // 점수 표시 UI
        GameObject scoreObj = new GameObject("ScoreText");
        scoreObj.transform.SetParent(gameView.transform, false);
        TextMeshProUGUI scoreText = scoreObj.AddComponent<TextMeshProUGUI>();
        scoreText.text = "점수: 0";
        scoreText.fontSize = 24;
        scoreText.color = Color.white;

        // 위치 설정
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0, 1);
        scoreRect.anchorMax = new Vector2(0, 1);
        scoreRect.anchoredPosition = new Vector2(10, -10);
        scoreRect.sizeDelta = new Vector2(200, 30);

        gameView.scoreText = scoreText;

        // 목표 점수 표시
        GameObject targetObj = new GameObject("TargetScoreText");
        targetObj.transform.SetParent(gameView.transform, false);
        TextMeshProUGUI targetText = targetObj.AddComponent<TextMeshProUGUI>();
        targetText.text = "목표: 1000";
        targetText.fontSize = 24;
        targetText.color = Color.yellow;

        RectTransform targetRect = targetObj.GetComponent<RectTransform>();
        targetRect.anchorMin = new Vector2(0, 1);
        targetRect.anchorMax = new Vector2(0, 1);
        targetRect.anchoredPosition = new Vector2(10, -50);
        targetRect.sizeDelta = new Vector2(200, 30);

        gameView.targetScoreText = targetText;
    }

    private void CreateShopUI()
    {
        GameObject shopUIObj = new GameObject("ShopView");
        shopUIObj.transform.SetParent(mainCanvas.transform, false);
        shopUIObj.SetActive(false); // 기본적으로 비활성화

        ShopView shopView = shopUIObj.AddComponent<ShopView>();

        // ViewManager에 연결
        ViewManager viewManager = FindFirstObjectByType<ViewManager>();
        if (viewManager != null)
        {
            viewManager.shopView = shopView;
        }
    }

    private void CreateMenuUI()
    {
        GameObject menuUIObj = new GameObject("MenuView");
        menuUIObj.transform.SetParent(mainCanvas.transform, false);
        menuUIObj.SetActive(false); // 기본적으로 비활성화

        MenuView menuView = menuUIObj.AddComponent<MenuView>();

        // ViewManager에 연결
        ViewManager viewManager = FindFirstObjectByType<ViewManager>();
        if (viewManager != null)
        {
            viewManager.menuView = menuView;
        }
    }
}
