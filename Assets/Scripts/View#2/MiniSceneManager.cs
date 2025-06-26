using DG.Tweening;
using UnityEngine;

public abstract class MiniSceneManager : MonoBehaviour
{
    public abstract Tween LoadScene();
    public abstract Tween UnloadScene();
}