using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

[CreateAssetMenu(fileName = "GameEventManager", menuName = "Oxo/Managers/Create Game Event Manager")]
public class GameEventManager : ScriptableObject
{
    private static GameEventManager instance;

    public GameEvent startGame;
    public GameEvent complateGame;
    public GameEvent failGame;

    public static GameEventManager Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load("Oxo/GameEventManager") as GameEventManager;
            return instance;
        }
    }
}
