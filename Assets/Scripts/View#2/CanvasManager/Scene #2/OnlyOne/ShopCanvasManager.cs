using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopCanvasManager : MonoBehaviour, ICanvasManager
{
    [field: SerializeField]
    public MiniSceneManager SceneManager { get; set; }
    private GameSceneManager GameSceneManager => SceneManager as GameSceneManager;

    [Header("관리 Canvas")]
    [SerializeField] private Canvas Canvas_Shop;

    [Header("상점 패널")]
    [SerializeField] private GameObject Panel_Shop;
    [SerializeField] private Button Button_NextRound;
    [SerializeField] private Button Button_BackToTitle;

    private RectTransform _shopRectTransform;
    private Vector2 _shopOriginalPosition;

    public void Init()
    {
        Panel_Shop.SetActive(false);
        Canvas_Shop.gameObject.SetActive(false);

        _shopRectTransform = Panel_Shop.GetComponent<RectTransform>();
        _shopOriginalPosition = _shopRectTransform.anchoredPosition;

        Button_NextRound.onClick.AddListener(OnNextRoundButtonClicked);
        Button_BackToTitle.onClick.AddListener(() =>
        {
            PanelSceneManager.Instance.LoadTitleScene();
        });
    }

    public Tween Show()
    {
        // Canvas를 즉시 활성화
        Canvas_Shop.gameObject.SetActive(true);

        // Panel 초기 위치 설정 (화면 아래)
        _shopRectTransform.anchoredPosition = new Vector2(_shopOriginalPosition.x, _shopOriginalPosition.y - Screen.height);
        Panel_Shop.SetActive(true);
        Canvas_Shop.GetComponent<CanvasGroup>().interactable = false;

        // Panel을 원래 위치로 슬라이드 업
        return DOTween.To(() => _shopRectTransform.anchoredPosition,
                         x => _shopRectTransform.anchoredPosition = x,
                         _shopOriginalPosition, 0.5f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                Canvas_Shop.GetComponent<CanvasGroup>().interactable = true;
                Button_NextRound.interactable = true; // 버튼 활성화
            });
    }

    public Tween Hide()
    {
        Canvas_Shop.GetComponent<CanvasGroup>().interactable = false;

        // Panel을 화면 아래로 슬라이드 다운 후 Canvas 비활성화
        return DOTween.To(() => _shopRectTransform.anchoredPosition,
                         x => _shopRectTransform.anchoredPosition = x,
                         new Vector2(_shopOriginalPosition.x, _shopOriginalPosition.y - Screen.height), 0.5f)
            .SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                Panel_Shop.SetActive(false);
                Canvas_Shop.gameObject.SetActive(false);
            });
    }

    public void Clear()
    {
        _shopRectTransform.anchoredPosition = _shopOriginalPosition;
        Button_NextRound.onClick.RemoveAllListeners();
        Button_BackToTitle.onClick.RemoveAllListeners();
        Panel_Shop.SetActive(false);
        Canvas_Shop.gameObject.SetActive(false);
    }

    private void OnNextRoundButtonClicked()
    {
        GameSceneManager.ShowStageCanvas();
    }
}