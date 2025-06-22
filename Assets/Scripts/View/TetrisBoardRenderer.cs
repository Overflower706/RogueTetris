using UnityEngine;
using UnityEngine.UI;

public class TetrisBoardRenderer : MonoBehaviour
{
    [Header("Rendering Settings")]
    public GameObject blockPrefab;
    public float blockSize = 1f;
    public Color[] blockColors = new Color[8]; // 0=빈공간, 1-7=테트리미노 타입별 색상

    private GameObject[,] renderedBlocks;
    private int[,] lastRenderedGrid;

    void Start()
    {
        InitializeRenderer();
    }

    private void InitializeRenderer()
    {
        renderedBlocks = new GameObject[TetrisBoard.WIDTH, TetrisBoard.HEIGHT];
        lastRenderedGrid = new int[TetrisBoard.WIDTH, TetrisBoard.HEIGHT];

        // 기본 색상 설정 (없으면)
        if (blockColors.Length < 8)
        {
            blockColors = new Color[8];
            blockColors[0] = Color.clear;       // 빈 공간
            blockColors[1] = Color.cyan;        // I 블록
            blockColors[2] = Color.yellow;      // O 블록
            blockColors[3] = Color.magenta;     // T 블록
            blockColors[4] = Color.green;       // S 블록
            blockColors[5] = Color.red;         // Z 블록
            blockColors[6] = Color.blue;        // J 블록
            blockColors[7] = new Color(1f, 0.5f, 0f); // L 블록 (주황색)
        }
    }

    public void RenderBoard(TetrisBoard board)
    {
        if (board == null || board.grid == null) return;

        // 변경된 블록만 업데이트
        for (int x = 0; x < TetrisBoard.WIDTH; x++)
        {
            for (int y = 0; y < TetrisBoard.HEIGHT; y++)
            {
                int currentBlock = board.grid[x, y];

                // 블록이 변경되었으면 업데이트
                if (currentBlock != lastRenderedGrid[x, y])
                {
                    UpdateBlock(x, y, currentBlock);
                    lastRenderedGrid[x, y] = currentBlock;
                }
            }
        }
    }

    private void UpdateBlock(int x, int y, int blockType)
    {
        // 기존 블록 제거
        if (renderedBlocks[x, y] != null)
        {
            DestroyImmediate(renderedBlocks[x, y]);
            renderedBlocks[x, y] = null;
        }

        // 빈 공간이면 블록 생성하지 않음
        if (blockType == 0) return;

        // 새 블록 생성
        if (blockPrefab != null)
        {
            GameObject newBlock = Instantiate(blockPrefab, transform);
            Vector3 position = new Vector3(x * blockSize, y * blockSize, 0);
            newBlock.transform.localPosition = position;

            // 색상 설정
            Renderer renderer = newBlock.GetComponent<Renderer>();
            if (renderer != null && blockType < blockColors.Length)
            {
                renderer.material.color = blockColors[blockType];
            }

            // SpriteRenderer 사용하는 경우
            SpriteRenderer spriteRenderer = newBlock.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && blockType < blockColors.Length)
            {
                spriteRenderer.color = blockColors[blockType];
            }

            // Image 컴포넌트 사용하는 경우 (UI)
            Image image = newBlock.GetComponent<Image>();
            if (image != null && blockType < blockColors.Length)
            {
                image.color = blockColors[blockType];
            }

            renderedBlocks[x, y] = newBlock;
        }
    }

    public void ClearBoard()
    {
        for (int x = 0; x < TetrisBoard.WIDTH; x++)
        {
            for (int y = 0; y < TetrisBoard.HEIGHT; y++)
            {
                if (renderedBlocks[x, y] != null)
                {
                    DestroyImmediate(renderedBlocks[x, y]);
                    renderedBlocks[x, y] = null;
                }
                lastRenderedGrid[x, y] = 0;
            }
        }
    }

    void OnDestroy()
    {
        ClearBoard();
    }
}
