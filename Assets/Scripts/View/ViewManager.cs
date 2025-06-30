using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    [Header("UI References")]
    public GameView gameView;
    public ShopView shopView;
    public MenuView menuView;

    [Header("Input")]
    public InputHandler inputHandler;

    private LogicManager logicManager;
    private Game currentGameData;

    void Start()
    {
        // // LogicManager 참조 획득
        // logicManager = LogicManager.Instance;
        // if (logicManager == null)
        // {
        //     Debug.LogError("LogicManager를 찾을 수 없습니다!");
        //     return;
        // }

        // // 입력 핸들러 초기화
        // if (inputHandler != null)
        // {
        //     inputHandler.Initialize(logicManager);
        // }

        // // 초기 상태 설정
        // ShowGameView();
    }

    void Update()
    {
        if (logicManager == null) return;

        // 게임 데이터 갱신
        currentGameData = logicManager.GetGameData();

        // 게임 상태에 따른 UI 전환
        UpdateViewState();

        // 현재 활성화된 뷰 업데이트
        UpdateActiveView();
    }

    private void UpdateViewState()
    {
        if (currentGameData == null) return;

        switch (currentGameData.CurrentState)
        {
            case GameState.Playing:
            case GameState.LineClearAnimation:
                ShowGameView();
                break;
            case GameState.Shop:
                ShowShopView();
                break;
            case GameState.GameOver:
            case GameState.Victory:
                ShowMenuView();
                break;
        }
    }

    private void UpdateActiveView()
    {
        if (currentGameData == null) return;

        // 게임 뷰 업데이트
        if (gameView != null && gameView.gameObject.activeInHierarchy)
        {
            gameView.UpdateView(currentGameData);
        }

        // 상점 뷰 업데이트
        if (shopView != null && shopView.gameObject.activeInHierarchy)
        {
            shopView.UpdateView(currentGameData);
        }

        // 메뉴 뷰 업데이트
        if (menuView != null && menuView.gameObject.activeInHierarchy)
        {
            menuView.UpdateView(currentGameData);
        }
    }

    public void ShowGameView()
    {
        SetActiveView(gameView);
    }

    public void ShowShopView()
    {
        SetActiveView(shopView);
    }

    public void ShowMenuView()
    {
        SetActiveView(menuView);
    }

    private void SetActiveView(MonoBehaviour targetView)
    {
        // 모든 뷰 비활성화
        if (gameView != null) gameView.gameObject.SetActive(false);
        if (shopView != null) shopView.gameObject.SetActive(false);
        if (menuView != null) menuView.gameObject.SetActive(false);

        // 대상 뷰 활성화
        if (targetView != null)
        {
            targetView.gameObject.SetActive(true);
        }
    }
}
