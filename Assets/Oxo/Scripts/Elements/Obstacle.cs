using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    ObstacleSettings settings;

    public List<FloatVariable> releatedFloatList;
    public List<GameObjectCollection> releatedCollectionList;

    //Collection<GameObject> 

    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            
        }
    }

    public void FailTheGame()
    {
        //settings.onFailEvent.Raise();
    }
}