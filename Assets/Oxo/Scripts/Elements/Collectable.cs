using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    CollectableSettings settings;


    public List<FloatVariable> releatedFloatList;
    public List<GameObjectCollection> releatedCollectionList;

    void Start()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");
    }

    void Update()
    {

    }
}
