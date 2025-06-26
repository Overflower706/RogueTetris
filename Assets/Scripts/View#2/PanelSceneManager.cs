using DG.Tweening;
using UnityEngine;

public enum SceneType
{
    None,
    Title,
    Game,
    Pack
}

public class PanelSceneManager : MonoSingleton<PanelSceneManager>
{
    private MiniSceneManager _currentSceneManager;

    [SerializeField] private MiniSceneManager TitleSceneManager;
    [SerializeField] private MiniSceneManager GameSceneManager;
    [SerializeField] private MiniSceneManager PackSceneManager;

    [field: SerializeField] public SceneType CurrentSceneType { get; private set; }

    private SceneType GetCurrentSceneType()
    {
        return _currentSceneManager switch
        {
            _ when _currentSceneManager == TitleSceneManager => SceneType.Title,
            _ when _currentSceneManager == GameSceneManager => SceneType.Game,
            _ when _currentSceneManager == PackSceneManager => SceneType.Pack,
            _ => SceneType.None
        };
    }

    private void Start()
    {
        // 일단 모든 SceneManager 비활성화
        TitleSceneManager.gameObject.SetActive(false);
        GameSceneManager.gameObject.SetActive(false);
        PackSceneManager.gameObject.SetActive(false);

        LoadTitleScene();
    }

    public void LoadTitleScene()
    {
        var sequence = DOTween.Sequence();
        if (_currentSceneManager != null)
        {
            // 현재 SceneManager가 있다면 UnloadScene 호출
            sequence.Append(_currentSceneManager.UnloadScene());
        }
        sequence.Append(TitleSceneManager.LoadScene());
        sequence.AppendCallback(() =>
        {
            _currentSceneManager = TitleSceneManager;
            CurrentSceneType = GetCurrentSceneType();
        });
    }

    public void LoadGameScene()
    {
        var sequence = DOTween.Sequence();
        if (_currentSceneManager != null)
        {
            // 현재 SceneManager가 있다면 UnloadScene 호출
            sequence.Append(_currentSceneManager.UnloadScene());
        }
        sequence.Append(GameSceneManager.LoadScene());
        sequence.AppendCallback(() =>
        {
            _currentSceneManager = GameSceneManager;
            CurrentSceneType = GetCurrentSceneType();
        });
    }

    public void LoadPackScene()
    {
        var sequence = DOTween.Sequence();
        if (_currentSceneManager != null)
        {
            // 현재 SceneManager가 있다면 UnloadScene 호출
            sequence.Append(_currentSceneManager.UnloadScene());
        }
        sequence.Append(PackSceneManager.LoadScene());
        sequence.AppendCallback(() =>
        {
            _currentSceneManager = PackSceneManager;
            CurrentSceneType = GetCurrentSceneType();
        });
    }
}