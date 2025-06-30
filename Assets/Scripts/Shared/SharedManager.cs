using UnityEngine;

public class SharedManager : MonoSingleton<SharedManager>
{

    [Header("Logic Manager")]
    [SerializeField] private LogicManager _logic;

    public Game Game => _logic.GameData;
}