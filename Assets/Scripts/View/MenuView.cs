using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuView : MonoBehaviour
{
    [Header("Menu UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;
    public TextMeshProUGUI targetScoreText;

    [Header("Buttons")]
    public Button restartButton;
    public Button quitButton;
    public Button shopButton; // 승리 시에만 활성화

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private LogicManager logicManager;

    void Start()
    {
        // logicManager = LogicManager.Instance;

        // // 버튼 이벤트 설정
        // if (restartButton != null)
        // {
        //     restartButton.onClick.AddListener(RestartGame);
        // }

        // if (quitButton != null)
        // {
        //     quitButton.onClick.AddListener(QuitGame);
        // }

        // if (shopButton != null)
        // {
        //     shopButton.onClick.AddListener(OpenShop);
        // }
    }

    public void UpdateView(Game gameData)
    {
        if (gameData == null) return;

        UpdateMenuContent(gameData);
        UpdatePanelVisibility(gameData);
        UpdateButtons(gameData);
    }

    private void UpdateMenuContent(Game gameData)
    {
        // 제목 텍스트
        if (titleText != null)
        {
            switch (gameData.CurrentState)
            {
                case GameState.GameOver:
                    titleText.text = "게임 오버";
                    break;
                case GameState.Victory:
                    titleText.text = "승리!";
                    break;
                default:
                    titleText.text = "Rogue Tetris";
                    break;
            }
        }

        // 결과 텍스트
        if (resultText != null)
        {
            switch (gameData.CurrentState)
            {
                case GameState.GameOver:
                    resultText.text = "목표 점수에 도달하지 못했습니다.";
                    break;
                case GameState.Victory:
                    resultText.text = "목표 점수를 달성했습니다!";
                    break;
                default:
                    resultText.text = "";
                    break;
            }
        }

        // 최종 점수
        if (finalScoreText != null)
        {
            finalScoreText.text = $"최종 점수: {gameData.CurrentScore:N0}";
        }

        // 목표 점수
        if (targetScoreText != null)
        {
            targetScoreText.text = $"목표 점수: {gameData.TargetScore:N0}";
        }

        // 게임 시간
        if (finalTimeText != null)
        {
            int minutes = Mathf.FloorToInt(gameData.GameTime / 60f);
            int seconds = Mathf.FloorToInt(gameData.GameTime % 60f);
            finalTimeText.text = $"플레이 시간: {minutes:00}:{seconds:00}";
        }
    }

    private void UpdatePanelVisibility(Game gameData)
    {
        // 게임 오버 패널
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(gameData.CurrentState == GameState.GameOver);
        }

        // 승리 패널
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(gameData.CurrentState == GameState.Victory);
        }
    }

    private void UpdateButtons(Game gameData)
    {
        // 상점 버튼은 승리 상태에서만 활성화
        if (shopButton != null)
        {
            shopButton.gameObject.SetActive(gameData.CurrentState == GameState.Victory);
        }

        // 다시 시작 버튼은 항상 활성화
        if (restartButton != null)
        {
            restartButton.interactable = true;
        }

        // 종료 버튼도 항상 활성화
        if (quitButton != null)
        {
            quitButton.interactable = true;
        }
    }

    private void RestartGame()
    {
        if (logicManager != null)
        {
            logicManager.RestartGame();
        }
    }

    private void OpenShop()
    {
        if (logicManager != null)
        {
            logicManager.OpenShop();
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
