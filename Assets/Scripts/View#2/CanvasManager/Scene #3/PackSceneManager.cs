using DG.Tweening;
using UnityEngine;

public class PackSceneManager : MiniSceneManager
{
    [Header("관리 캔버스")]
    [SerializeField] private PackCanvasManager CanvasManager_Pack;

    public override Tween LoadScene()
    {
        gameObject.SetActive(true);
        CanvasManager_Pack.Init();

        return CanvasManager_Pack.Show();
    }

    public override Tween UnloadScene()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(CanvasManager_Pack.Hide());
        sequence.AppendCallback(() => CanvasManager_Pack.Clear());
        sequence.AppendCallback(() => gameObject.SetActive(false));

        return sequence;
    }
}