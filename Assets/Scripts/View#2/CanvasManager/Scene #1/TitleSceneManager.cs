using DG.Tweening;
using UnityEngine;

public class TitleSceneManager : MiniSceneManager
{
    [Header("관리 캔버스")]
    [SerializeField] private TitleCanvasManager CanvasManager_Title;

    public override Tween LoadScene()
    {
        gameObject.SetActive(true);
        CanvasManager_Title.Init();

        return CanvasManager_Title.Show();
    }

    public override Tween UnloadScene()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(CanvasManager_Title.Hide());
        sequence.AppendCallback(() => CanvasManager_Title.Clear());
        sequence.AppendCallback(() => gameObject.SetActive(false));

        return sequence;
    }
}