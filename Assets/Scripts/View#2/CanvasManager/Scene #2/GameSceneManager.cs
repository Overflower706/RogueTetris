using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameSceneManager : MiniSceneManager
{
    [Header("관리 캔버스(Always On)")]
    [SerializeField] private BannerCanvasManager CanvasManager_Banner;

    [Header("관리 캔버스(Only One)")]
    [SerializeField] private StageCanvasManager CanvasManager_Stage;
    [SerializeField] private GameCanvasManager CanvasManager_Game;
    [SerializeField] private ShopCanvasManager CanvasManager_Shop;

    private ICanvasManager _currentActiveCanvas;

    public override Tween LoadScene()
    {
        _currentActiveCanvas = null;

        // 먼저 GameObject 활성화
        gameObject.SetActive(true);

        CanvasManager_Banner.Init();
        CanvasManager_Stage.Init();
        CanvasManager_Game.Init();
        CanvasManager_Shop.Init();

        // 모든 캔버스 비활성화 후 Banner와 Stage 캔버스 순차 활성화
        Sequence sequence = DOTween.Sequence();

        return sequence.Append(CanvasManager_Banner.Show())
            .Append(ShowOnlyOneCanvas(CanvasManager_Stage));
    }

    public override Tween UnloadScene()
    {
        // 모든 캔버스 비활성화
        Sequence sequence = DOTween.Sequence();

        return sequence.Append(HideCurrentCanvas())
            .Append(CanvasManager_Banner.Hide())
            .AppendCallback(() => gameObject.SetActive(false))
            .AppendCallback(() =>
            {
                CanvasManager_Banner.Clear();
                CanvasManager_Stage.Clear();
                CanvasManager_Game.Clear();
                CanvasManager_Shop.Clear();
            });
    }

    public Tween ShowStageCanvas() => ShowOnlyOneCanvas(CanvasManager_Stage);
    public Tween ShowGameCanvas() => ShowOnlyOneCanvas(CanvasManager_Game);
    public Tween ShowShopCanvas() => ShowOnlyOneCanvas(CanvasManager_Shop);

    private Tween ShowOnlyOneCanvas(ICanvasManager targetCanvas)
    {
        var sequence = DOTween.Sequence();

        // 현재 활성 캔버스가 있으면 먼저 숨기기
        if (_currentActiveCanvas != null)
        {
            sequence.Append(_currentActiveCanvas.Hide())
                    .AppendInterval(0.2f);
        }

        // 새 캔버스 활성화
        sequence.Append(targetCanvas.Show())
            .AppendCallback(() => _currentActiveCanvas = targetCanvas);

        return sequence;
    }

    private Tween HideCurrentCanvas()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_currentActiveCanvas?.Hide())
            .AppendCallback(() => _currentActiveCanvas = null);
        return sequence;
    }
}