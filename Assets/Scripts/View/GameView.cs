using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameView : MonoBehaviour
{
    [Header("Game Board")]
    public TetrisBoardRenderer boardRenderer;
    public TetriminoRenderer currentTetriminoRenderer;
    public TetriminoRenderer nextTetriminoRenderer;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetScoreText;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI gameTimeText;
    public Slider progressSlider;

    [Header("Effects UI")]
    public Transform effectsContainer;
    public GameObject effectItemPrefab;

    [Header("Game State UI")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI gameStateText;

    private System.Collections.Generic.List<GameObject> activeEffectItems = new System.Collections.Generic.List<GameObject>();

    public void UpdateView(Game gameData)
    {
        if (gameData == null) return;

        UpdateGameBoard(gameData);
        UpdateTetriminos(gameData);
        UpdateScoreUI(gameData);
        UpdateEffectsUI(gameData);
        UpdateGameStateUI(gameData);
    }

    private void UpdateGameBoard(Game gameData)
    {
        if (boardRenderer != null && gameData.Board != null)
        {
            boardRenderer.RenderBoard(gameData.Board);
        }
    }

    private void UpdateTetriminos(Game gameData)
    {
        // 현재 테트리미노 렌더링
        if (currentTetriminoRenderer != null && gameData.CurrentTetrimino != null)
        {
            currentTetriminoRenderer.RenderTetrimino(gameData.CurrentTetrimino);
        }

        // 다음 테트리미노 렌더링
        if (nextTetriminoRenderer != null && gameData.NextTetrimino != null)
        {
            nextTetriminoRenderer.RenderTetrimino(gameData.NextTetrimino);
        }
    }

    private void UpdateScoreUI(Game gameData)
    {
        // 점수 표시
        if (scoreText != null)
        {
            scoreText.text = $"점수: {gameData.CurrentScore:N0}";
        }

        // 목표 점수 표시
        if (targetScoreText != null)
        {
            targetScoreText.text = $"목표: {gameData.TargetScore:N0}";
        }

        // 화폐 표시
        if (currencyText != null)
        {
            currencyText.text = $"골드: {gameData.Currency}";
        }

        // 게임 시간 표시
        if (gameTimeText != null)
        {
            int minutes = Mathf.FloorToInt(gameData.GameTime / 60f);
            int seconds = Mathf.FloorToInt(gameData.GameTime % 60f);
            gameTimeText.text = $"시간: {minutes:00}:{seconds:00}";
        }

        // 진행도 슬라이더
        if (progressSlider != null)
        {
            float progress = gameData.TargetScore > 0 ? (float)gameData.CurrentScore / gameData.TargetScore : 0f;
            progressSlider.value = Mathf.Clamp01(progress);
        }
    }

    private void UpdateEffectsUI(Game gameData)
    {
        if (effectsContainer == null || effectItemPrefab == null) return;

        // 기존 효과 UI 제거
        foreach (GameObject item in activeEffectItems)
        {
            if (item != null) Destroy(item);
        }
        activeEffectItems.Clear();

        // 활성 효과들을 UI로 표시
        foreach (ActiveEffect effect in gameData.ActiveEffects)
        {
            GameObject effectItem = Instantiate(effectItemPrefab, effectsContainer);
            activeEffectItems.Add(effectItem);

            // 효과 정보 표시
            TextMeshProUGUI effectText = effectItem.GetComponentInChildren<TextMeshProUGUI>();
            if (effectText != null)
            {
                string effectDescription = GetEffectDescription(effect);
                effectText.text = $"{effectDescription} ({effect.timeRemaining:F1}초)";
            }

            // 시간에 따른 색상 변화
            Image effectImage = effectItem.GetComponent<Image>();
            if (effectImage != null)
            {
                float alpha = Mathf.Clamp01(effect.timeRemaining / effect.duration);
                Color color = effectImage.color;
                color.a = alpha;
                effectImage.color = color;
            }
        }
    }

    private void UpdateGameStateUI(Game gameData)
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

        // 게임 상태 텍스트
        if (gameStateText != null)
        {
            switch (gameData.CurrentState)
            {
                case GameState.Playing:
                    gameStateText.text = "";
                    break;
                case GameState.LineClearAnimation:
                    gameStateText.text = "라인 클리어!";
                    break;
                case GameState.GameOver:
                    gameStateText.text = "게임 오버";
                    break;
                case GameState.Victory:
                    gameStateText.text = "승리! ESC를 눌러 상점으로";
                    break;
                case GameState.Shop:
                    gameStateText.text = "";
                    break;
            }
        }
    }

    private string GetEffectDescription(ActiveEffect effect)
    {
        switch (effect.type)
        {
            case EffectType.ScoreMultiplier:
                return $"점수 {effect.value:F1}배";
            case EffectType.BonusPoints:
                return $"보너스 +{effect.value:F0}";
            case EffectType.LineClearBonus:
                return $"라인 보너스 +{effect.value:F0}";
            case EffectType.ComboBonus:
                return $"콤보 {effect.value:F1}배";
            default:
                return "특수 효과";
        }
    }
}
