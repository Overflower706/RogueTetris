using DG.Tweening;
using UnityEngine;

public class PackCanvasManager : MonoBehaviour, ICanvasManager
{
    [field: SerializeField]
    public MiniSceneManager SceneManager { get; set; }

    public void Init()
    {

    }

    public Tween Show()
    {
        return default;
    }

    public Tween Hide()
    {
        return default;
    }

    public void Clear()
    {

    }
}