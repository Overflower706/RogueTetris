using UnityEngine;
using System.Collections.Generic;

public class TetrisLogic
{
    private Game game;
    private float fallTimer;
    private float fallInterval = 1.0f; // 1초마다 자동 낙하

    public TetrisLogic(Game gameData)
    {
        this.game = gameData;
        fallTimer = 0f;
    }
    public void SpawnNewTetrimino()
    {
        // 다음 테트리미노가 있으면 현재로 이동, 없으면 새로 생성
        if (game.nextTetrimino != null)
        {
            game.currentTetrimino = game.nextTetrimino;
        }
        else
        {
            game.currentTetrimino = GenerateRandomTetrimino();
        }

        // 테트리미노를 스폰 위치에 배치 (상단 중앙)
        game.currentTetrimino.position = new Vector2Int(TetrisBoard.WIDTH / 2 - 1, TetrisBoard.HEIGHT - 2);

        // 새로운 다음 테트리미노 생성
        game.nextTetrimino = GenerateRandomTetrimino();

        // 게임 오버 체크 - 스폰 위치에서 배치할 수 없으면 게임 오버
        if (!CanPlaceTetrimino(game.currentTetrimino))
        {
            game.currentState = GameState.GameOver;
        }
    }

    public void MoveTetrimino(Vector2Int direction)
    {
        if (game.currentTetrimino == null) return;

        Vector2Int newPosition = game.currentTetrimino.position + direction;
        Vector2Int oldPosition = game.currentTetrimino.position;

        game.currentTetrimino.position = newPosition;

        if (!CanPlaceTetrimino(game.currentTetrimino))
        {
            // 이동할 수 없으면 원래 위치로 복구
            game.currentTetrimino.position = oldPosition;

            // 아래로 이동이 불가능하면 테트리미노 고정
            if (direction.y < 0)
            {
                PlaceTetrimino();
            }
        }
    }

    public void RotateTetrimino()
    {
        if (game.currentTetrimino == null) return;

        int oldRotation = game.currentTetrimino.rotation;
        game.currentTetrimino.rotation = (game.currentTetrimino.rotation + 1) % 4;

        if (!CanPlaceTetrimino(game.currentTetrimino))
        {
            // 회전할 수 없으면 원래 회전 상태로 복구
            game.currentTetrimino.rotation = oldRotation;
        }
    }

    public void HardDrop()
    {
        if (game.currentTetrimino == null) return;

        while (CanPlaceTetrimino(game.currentTetrimino))
        {
            game.currentTetrimino.position += Vector2Int.down;
        }

        // 마지막 유효한 위치로 되돌리기
        game.currentTetrimino.position += Vector2Int.up;
        PlaceTetrimino();
    }

    public void SoftDrop()
    {
        if (game.currentTetrimino == null) return;

        // 한 칸 아래로 이동 시도
        MoveTetrimino(Vector2Int.down);

        // SoftDrop 사용 시 AutoFall 타이머 초기화
        fallTimer = 0f;
    }

    public void UpdateAutoFall(float deltaTime)
    {
        fallTimer += deltaTime;

        if (fallTimer >= fallInterval)
        {
            SoftDrop(); // MoveTetrimino 대신 SoftDrop 사용
        }
    }    private Tetrimino GenerateRandomTetrimino()
    {
        TetriminoType[] types = { TetriminoType.I, TetriminoType.O, TetriminoType.T,
                                 TetriminoType.S, TetriminoType.Z, TetriminoType.J, TetriminoType.L };
        TetriminoType randomType = types[Random.Range(0, types.Length)];
        int randomColor = Random.Range(1, 5); // 1~4 중 랜덤 색상
        return new Tetrimino(randomType, randomColor);
    }

    private bool CanPlaceTetrimino(Tetrimino tetrimino)
    {
        Vector2Int[] positions = tetrimino.GetWorldPositions();

        foreach (Vector2Int pos in positions)
        {
            if (!game.board.IsValidPosition(pos))
            {
                return false;
            }
        }

        return true;
    }    private void PlaceTetrimino()
    {
        if (game.currentTetrimino == null) return;

        Vector2Int[] positions = game.currentTetrimino.GetWorldPositions();

        // 보드에 테트리미노 블록 배치 (color 속성 사용)
        foreach (Vector2Int pos in positions)
        {
            game.board.PlaceBlock(pos, game.currentTetrimino.color);
        }

        // 라인 클리어 체크
        CheckLineClears();

        // 새로운 테트리미노 생성
        SpawnNewTetrimino();
    }
    private void CheckLineClears()
    {
        List<int> clearedLines = new List<int>();

        // 아래쪽부터 체크
        for (int y = 0; y < TetrisBoard.HEIGHT; y++)
        {
            if (game.board.IsLineFull(y))
            {
                clearedLines.Add(y);
            }
        }

        if (clearedLines.Count > 0)
        {
            // 점수 계산 (ScoreLogic에 위임)
            ScoreLogic scoreLogic = new ScoreLogic(game);
            scoreLogic.ProcessLineClears(clearedLines.Count, game.currentTetrimino);

            // 라인 클리어 실행 - 위쪽부터 역순으로 클리어 (인덱스 꼬임 방지)
            clearedLines.Sort((a, b) => b.CompareTo(a)); // 내림차순 정렬
            foreach (int line in clearedLines)
            {
                game.board.ClearLine(line);
            }

            // 승리 조건 체크
            if (game.currentScore >= game.targetScore)
            {
                game.currentState = GameState.Victory;
            }
        }
    }
}
