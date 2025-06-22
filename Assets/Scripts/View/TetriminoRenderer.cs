using UnityEngine;
using UnityEngine.UI;

public class TetriminoRenderer : MonoBehaviour
{
    [Header("Rendering Settings")]
    public GameObject blockPrefab;
    public float blockSize = 1f;
    public bool isPreview = false; // 다음 테트리미노 미리보기용인지
    public float previewScale = 0.5f;

    private GameObject[] renderedBlocks;
    private Tetrimino lastRenderedTetrimino;

    // 테트리미노 타입별 색상
    private Color[] tetriminoColors = new Color[]
    {
        Color.cyan,        // I
        Color.yellow,      // O
        Color.magenta,     // T
        Color.green,       // S
        Color.red,         // Z
        Color.blue,        // J
        new Color(1f, 0.5f, 0f) // L (주황색)
    };

    public void RenderTetrimino(Tetrimino tetrimino)
    {
        if (tetrimino == null)
        {
            ClearRendering();
            return;
        }

        // 이전과 같은 테트리미노면 위치/회전 변경만 체크
        if (ShouldUpdateRendering(tetrimino))
        {
            UpdateRendering(tetrimino);
            lastRenderedTetrimino = tetrimino;
        }
    }

    private bool ShouldUpdateRendering(Tetrimino tetrimino)
    {
        if (lastRenderedTetrimino == null) return true;
        if (lastRenderedTetrimino.type != tetrimino.type) return true;
        if (lastRenderedTetrimino.position != tetrimino.position) return true;
        if (lastRenderedTetrimino.rotation != tetrimino.rotation) return true;
        if (lastRenderedTetrimino.effect?.isActive != tetrimino.effect?.isActive) return true;

        return false;
    }

    private void UpdateRendering(Tetrimino tetrimino)
    {
        ClearRendering();

        if (blockPrefab == null) return;

        Vector2Int[] positions = tetrimino.GetWorldPositions();
        renderedBlocks = new GameObject[positions.Length];

        Color blockColor = GetTetriminoColor(tetrimino);

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject block = Instantiate(blockPrefab, transform);

            Vector3 worldPos;
            if (isPreview)
            {
                // 미리보기는 로컬 좌표계 사용
                Vector2Int localPos = tetrimino.shape[i];
                worldPos = new Vector3(localPos.x * blockSize * previewScale, localPos.y * blockSize * previewScale, 0);
                block.transform.localScale = Vector3.one * previewScale;
            }
            else
            {
                // 실제 게임은 월드 좌표계 사용
                worldPos = new Vector3(positions[i].x * blockSize, positions[i].y * blockSize, 0);
            }

            block.transform.localPosition = worldPos;

            // 색상 설정
            SetBlockColor(block, blockColor);

            // 효과가 있는 경우 특수 표시
            if (tetrimino.effect != null && tetrimino.effect.isActive)
            {
                ApplyEffectVisual(block, tetrimino.effect);
            }

            renderedBlocks[i] = block;
        }
    }

    private Color GetTetriminoColor(Tetrimino tetrimino)
    {
        int typeIndex = (int)tetrimino.type;
        if (typeIndex >= 0 && typeIndex < tetriminoColors.Length)
        {
            return tetriminoColors[typeIndex];
        }
        return Color.white;
    }

    private void SetBlockColor(GameObject block, Color color)
    {
        // Renderer 컴포넌트
        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }

        // SpriteRenderer 컴포넌트
        SpriteRenderer spriteRenderer = block.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }

        // Image 컴포넌트 (UI)
        Image image = block.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }
    }

    private void ApplyEffectVisual(GameObject block, TetriminoEffect effect)
    {
        switch (effect.effectType)
        {
            case EffectType.ScoreMultiplier:
                // 금색 테두리 효과
                AddGlowEffect(block, Color.gold);
                break;
            case EffectType.BonusPoints:
                // 은색 테두리 효과
                AddGlowEffect(block, Color.white);
                break;
            case EffectType.LineClearBonus:
                // 초록색 테두리 효과
                AddGlowEffect(block, Color.green);
                break;
            case EffectType.ComboBonus:
                // 무지개 효과 (시간에 따라 색상 변화)
                AddRainbowEffect(block);
                break;
        }
    }

    private void AddGlowEffect(GameObject block, Color glowColor)
    {
        // 단순한 스케일 효과로 글로우 시뮬레이션
        block.transform.localScale *= 1.1f;

        // 색상을 약간 밝게
        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color currentColor = renderer.material.color;
            renderer.material.color = Color.Lerp(currentColor, glowColor, 0.3f);
        }
    }

    private void AddRainbowEffect(GameObject block)
    {
        // 시간에 따라 색상이 변하는 효과
        float hue = (Time.time * 0.5f) % 1f;
        Color rainbowColor = Color.HSVToRGB(hue, 0.8f, 1f);

        Renderer renderer = block.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = rainbowColor;
        }
    }

    public void ClearRendering()
    {
        if (renderedBlocks != null)
        {
            foreach (GameObject block in renderedBlocks)
            {
                if (block != null)
                {
                    DestroyImmediate(block);
                }
            }
            renderedBlocks = null;
        }
        lastRenderedTetrimino = null;
    }

    void OnDestroy()
    {
        ClearRendering();
    }
}
