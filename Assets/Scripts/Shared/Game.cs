using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Game
{
    // 게임 상태
    public GameState currentState;
    public int currentScore;
    public int targetScore;
    public float gameTime;

    // 테트리스 보드
    public TetrisBoard board;

    // 현재 테트리미노
    public Tetrimino currentTetrimino;
    public Tetrimino nextTetrimino;

    // 파워업/효과
    public List<ActiveEffect> activeEffects;

    // 상점 관련
    public int currency;
    public bool isShopOpen;

    public Game()
    {
        board = new TetrisBoard();
        activeEffects = new List<ActiveEffect>();
        currentState = GameState.Playing;
        targetScore = 1000; // 기본 목표 점수
        currency = 0;
        isShopOpen = false;
    }
}

public enum GameState
{
    Playing,
    LineClearAnimation,
    GameOver,
    Victory,
    Shop
}
