using UnityEngine;
using System.Text;

/// <summary>
/// 게임 상태를 시각화해서 로그에 출력하는 유틸리티 클래스
/// </summary>
public static class GameLogger
{
    /// <summary>
    /// Game 객체의 상태를 시각화해서 Debug.Log로 출력
    /// </summary>
    /// <param name="game">로그할 Game 객체</param>
    /// <param name="title">로그 제목 (선택사항)</param>
    public static void LogGame(Game game, string title = "Game State")
    {
        if (game == null)
        {
            Debug.Log($"[{title}] Game is null");
            return;
        }

        StringBuilder sb = new StringBuilder();

        // 헤더
        sb.AppendLine($"=== {title} ===");
        sb.AppendLine($"State: {game.currentState}");
        sb.AppendLine($"Score: {game.currentScore}/{game.targetScore}");
        sb.AppendLine($"Currency: {game.currency}");
        sb.AppendLine($"Time: {game.gameTime:F1}s");
        sb.AppendLine($"Shop Open: {game.isShopOpen}");
        sb.AppendLine($"Active Effects: {game.activeEffects?.Count ?? 0}");

        // 현재 테트리미노 정보
        if (game.currentTetrimino != null)
        {
            sb.AppendLine($"Current Tetrimino: {game.currentTetrimino.type} at ({game.currentTetrimino.position.x}, {game.currentTetrimino.position.y}) rotation={game.currentTetrimino.rotation}");
        }
        else
        {
            sb.AppendLine("Current Tetrimino: None");
        }

        if (game.nextTetrimino != null)
        {
            sb.AppendLine($"Next Tetrimino: {game.nextTetrimino.type}");
        }

        // 보드 시각화
        sb.AppendLine();
        sb.AppendLine("Board:");
        sb.AppendLine(VisualizeBoard(game.board, game.currentTetrimino));

        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// 테트리스 보드를 ASCII 문자로 시각화
    /// </summary>
    /// <param name="board">시각화할 보드</param>
    /// <param name="currentTetrimino">현재 테트리미노 (활성 블록 표시용)</param>
    /// <returns>시각화된 보드 문자열</returns>
    private static string VisualizeBoard(TetrisBoard board, Tetrimino currentTetrimino = null)
    {
        if (board == null) return "Board is null";

        StringBuilder sb = new StringBuilder();

        // 현재 테트리미노의 위치들을 미리 계산
        Vector2Int[] currentTetriminoPositions = null;
        if (currentTetrimino != null)
        {
            currentTetriminoPositions = currentTetrimino.GetWorldPositions();
        }        // 상단 경계선
        sb.Append("+");
        for (int x = 0; x < TetrisBoard.WIDTH; x++)
        {
            sb.Append("--");
        }
        sb.AppendLine("+");        // 보드를 위에서부터 아래로 그리기 (y가 큰 것부터)
        for (int y = TetrisBoard.HEIGHT - 1; y >= 0; y--)
        {
            sb.Append("|");

            for (int x = 0; x < TetrisBoard.WIDTH; x++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                bool isCurrentTetrimino = IsPositionInTetrimino(pos, currentTetriminoPositions);
                bool isBoardBlock = board.grid[x, y] != 0;

                if (isCurrentTetrimino && isBoardBlock)
                {
                    // 겹침 (충돌 상황)
                    sb.Append("XX");
                }
                else if (isCurrentTetrimino)
                {
                    // 현재 테트리미노
                    sb.Append("■");
                }
                else if (isBoardBlock)
                {
                    // 고정된 블록
                    sb.Append("▨");
                }
                else
                {
                    // 빈 공간
                    sb.Append("□");
                }
            }

            sb.Append("│");

            // 오른쪽에 y좌표 표시 (5칸마다)
            if (y % 5 == 0)
            {
                sb.Append($" {y:D2}");
            }

            sb.AppendLine();
        }

        // 하단 경계선
        sb.Append("└");
        for (int x = 0; x < TetrisBoard.WIDTH; x++)
        {
            sb.Append("--");
        }
        sb.AppendLine("┘");

        // x좌표 표시
        sb.Append(" ");
        for (int x = 0; x < TetrisBoard.WIDTH; x++)
        {
            sb.Append($"{x % 10} ");
        }
        sb.AppendLine();        // 범례
        sb.AppendLine();
        sb.AppendLine("Legend: ■=Current, ▨=Fixed, XX=Collision, □=Empty");

        return sb.ToString();
    }

    /// <summary>
    /// 주어진 위치가 테트리미노의 블록 위치인지 확인
    /// </summary>
    private static bool IsPositionInTetrimino(Vector2Int position, Vector2Int[] tetriminoPositions)
    {
        if (tetriminoPositions == null) return false;

        foreach (var tetriminoPos in tetriminoPositions)
        {
            if (tetriminoPos.x == position.x && tetriminoPos.y == position.y)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 간단한 보드 상태만 로그 (디버깅용)
    /// </summary>
    /// <param name="board">로그할 보드</param>
    /// <param name="title">로그 제목</param>
    public static void LogBoard(TetrisBoard board, string title = "Board State")
    {
        Debug.Log($"=== {title} ===\n{VisualizeBoard(board)}");
    }

    /// <summary>
    /// 테트리미노 정보만 로그
    /// </summary>
    /// <param name="tetrimino">로그할 테트리미노</param>
    /// <param name="title">로그 제목</param>
    public static void LogTetrimino(Tetrimino tetrimino, string title = "Tetrimino Info")
    {
        if (tetrimino == null)
        {
            Debug.Log($"[{title}] Tetrimino is null");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"=== {title} ===");
        sb.AppendLine($"Type: {tetrimino.type}");
        sb.AppendLine($"Position: ({tetrimino.position.x}, {tetrimino.position.y})");
        sb.AppendLine($"Rotation: {tetrimino.rotation}");

        sb.AppendLine("World Positions:");
        var worldPositions = tetrimino.GetWorldPositions();
        for (int i = 0; i < worldPositions.Length; i++)
        {
            sb.AppendLine($"  Block {i}: ({worldPositions[i].x}, {worldPositions[i].y})");
        }

        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// Game 객체의 상태를 문자열로 반환 (UI 표시용)
    /// </summary>
    /// <param name="game">상태를 가져올 Game 객체</param>
    /// <param name="title">상태 제목 (선택사항)</param>
    /// <returns>게임 상태를 나타내는 문자열</returns>
    public static string GetGameStateString(Game game, string title = "Game State")
    {
        if (game == null)
        {
            return $"[{title}] Game is null";
        }

        StringBuilder sb = new StringBuilder();

        // 헤더
        sb.AppendLine($"=== {title} ===");
        sb.AppendLine($"State: {game.currentState}");
        sb.AppendLine($"Score: {game.currentScore}/{game.targetScore}");
        sb.AppendLine($"Currency: {game.currency}");
        sb.AppendLine($"Time: {game.gameTime:F1}s");
        sb.AppendLine($"Shop Open: {game.isShopOpen}");
        sb.AppendLine($"Active Effects: {game.activeEffects?.Count ?? 0}");

        // 현재 테트리미노 정보
        if (game.currentTetrimino != null)
        {
            sb.AppendLine($"Current Tetrimino: {game.currentTetrimino.type} at ({game.currentTetrimino.position.x}, {game.currentTetrimino.position.y}) rotation={game.currentTetrimino.rotation}");
        }
        else
        {
            sb.AppendLine("Current Tetrimino: None");
        }

        if (game.nextTetrimino != null)
        {
            sb.AppendLine($"Next Tetrimino: {game.nextTetrimino.type}");
        }        // 보드 시각화
        sb.AppendLine();
        sb.AppendLine("Board:");
        sb.Append(VisualizeBoard(game.board, game.currentTetrimino));

        return sb.ToString();
    }

    /// <summary>
    /// 보드 상태만 문자열로 반환 (UI 표시용)
    /// </summary>
    /// <param name="board">시각화할 보드</param>
    /// <param name="currentTetrimino">현재 테트리미노 (활성 블록 표시용)</param>
    /// <returns>보드만 시각화된 문자열</returns>
    public static string GetBoardString(TetrisBoard board, Tetrimino currentTetrimino = null)
    {
        return VisualizeBoard(board, currentTetrimino);
    }
}
