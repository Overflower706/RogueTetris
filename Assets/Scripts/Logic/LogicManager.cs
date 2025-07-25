using UnityEngine;
using System.Collections.Generic;

public class LogicManager : MonoBehaviour
{
    public Game GameData;
    // Logic 모듈들
    private TetrisLogic tetrisLogic;
    private ScoreLogic scoreLogic;
    private EffectLogic effectLogic;
    private ShopLogic shopLogic;

    public void Initialize()
    {
        GameData = new Game();
        tetrisLogic = new TetrisLogic(GameData);
        scoreLogic = new ScoreLogic(GameData);
        effectLogic = new EffectLogic(GameData);
        shopLogic = new ShopLogic(GameData, scoreLogic, effectLogic);

        // 게임 시작
        StartGame();
    }

    // View에서 호출할 메서드들
    public void MoveTetrimino(Vector2Int direction)
    {
        if (GameData.CurrentState != GameState.Playing) return;
        tetrisLogic.MoveTetrimino(direction);
    }

    public void RotateTetrimino()
    {
        if (GameData.CurrentState != GameState.Playing) return;
        tetrisLogic.RotateTetrimino();
    }
    public void SoftDrop()
    {
        if (GameData.CurrentState != GameState.Playing) return;
        tetrisLogic.SoftDrop();
    }

    public void DropTetrimino()
    {
        if (GameData.CurrentState != GameState.Playing) return;
        tetrisLogic.HardDrop();
    }
    public void RestartGame()
    {
        // Logic 모듈들 다시 초기화
        tetrisLogic = new TetrisLogic(GameData);
        scoreLogic = new ScoreLogic(GameData);
        effectLogic = new EffectLogic(GameData);
        shopLogic = new ShopLogic(GameData, scoreLogic, effectLogic);

        // 게임 재시작
        StartGame();
    }

    public void OpenShop()
    {
        if (GameData.CurrentState == GameState.Victory)
        {
            GameData.CurrentState = GameState.Shop;
            GameData.IsShopOpen = true;
        }
    }

    public void CloseShop()
    {
        if (GameData.CurrentState == GameState.Shop)
        {
            GameData.CurrentState = GameState.Playing;
            GameData.IsShopOpen = false;
            // 새로운 라운드 시작
            GameData.TargetScore = Mathf.RoundToInt(GameData.TargetScore * 1.5f); // 목표 점수 증가
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
    public Game GetGameData() => GameData;

    // View에서 호출할 Update 메서드 (매 프레임)
    public void UpdateAutoFall(float deltaTime)
    {
        if (GameData != null && GameData.CurrentState == GameState.Playing)
        {
            GameData.GameTime += deltaTime;
            effectLogic.UpdateEffects(deltaTime);

            // 자동 낙하 처리
            tetrisLogic.UpdateAutoFall(deltaTime);
        }
    }

    public void StartGame()
    {
        // 게임 초기 설정
        GameData.CurrentState = GameState.Playing;
        GameData.CurrentScore = 0;
        GameData.TargetScore = 1000; // 초기 목표 점수
        GameData.Currency = 0;
        GameData.GameTime = 0f;
        GameData.IsShopOpen = false;

        // 보드 초기화
        GameData.Board.Clear();

        // 첫 번째 테트리미노 생성
        if (tetrisLogic != null)
        {
            tetrisLogic.SpawnNewTetrimino();
        }

        Debug.Log("게임 시작! 목표 점수: " + GameData.TargetScore);
    }
}
