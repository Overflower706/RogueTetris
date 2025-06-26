using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ICanvasManager
{
    MiniSceneManager SceneManager { get; }
    void Init();
    Tween Show();
    Tween Hide();
    void Clear();
}