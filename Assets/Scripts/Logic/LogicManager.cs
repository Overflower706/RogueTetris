using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour
{
    [SerializeField] private Game gameData;
    // Logic 모듈들
    private TetrisLogic tetrisLogic;
    private ScoreLogic scoreLogic;
    private EffectLogic effectLogic;
    private ShopLogic shopLogic;

    // 싱글톤 패턴 (View에서 접근하기 위해)
    public static LogicManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameData != null && gameData.currentState == GameState.Playing)
        {
            gameData.gameTime += Time.deltaTime;
            effectLogic.UpdateEffects(Time.deltaTime);

            // 자동 낙하 처리
            tetrisLogic.UpdateAutoFall(Time.deltaTime);
        }
    }
    private void InitializeGame()
    {
        gameData = new Game();
        tetrisLogic = new TetrisLogic(gameData);
        scoreLogic = new ScoreLogic(gameData);
        effectLogic = new EffectLogic(gameData);
        shopLogic = new ShopLogic(gameData, scoreLogic, effectLogic);

        // 첫 번째 테트리미노 생성
        tetrisLogic.SpawnNewTetrimino();
    }

    // View에서 호출할 메서드들
    public void MoveTetrimino(Vector2Int direction)
    {
        if (gameData.currentState != GameState.Playing) return;
        tetrisLogic.MoveTetrimino(direction);
    }

    public void RotateTetrimino()
    {
        if (gameData.currentState != GameState.Playing) return;
        tetrisLogic.RotateTetrimino();
    }

    public void DropTetrimino()
    {
        if (gameData.currentState != GameState.Playing) return;
        tetrisLogic.HardDrop();
    }

    public void RestartGame()
    {
        InitializeGame();
    }

    public void OpenShop()
    {
        if (gameData.currentState == GameState.Victory)
        {
            gameData.currentState = GameState.Shop;
            gameData.isShopOpen = true;
        }
    }

    public void CloseShop()
    {
        if (gameData.currentState == GameState.Shop)
        {
            gameData.currentState = GameState.Playing;
            gameData.isShopOpen = false;
            // 새로운 라운드 시작
            gameData.targetScore = Mathf.RoundToInt(gameData.targetScore * 1.5f); // 목표 점수 증가
        }
    }

    // 상점 관련 메서드들
    public List<ShopItem> GetShopItems()
    {
        return shopLogic?.GenerateShopItems() ?? new List<ShopItem>();
    }

    public bool PurchaseShopItem(ShopItem item)
    {
        return shopLogic?.PurchaseItem(item) ?? false;
    }

    // View가 읽을 Game 데이터 접근자
    public Game GetGameData() => gameData;
}
