using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvolveCollectable : MonoBehaviour
{
    [SerializeField]
    CollectableSettings settings;

    public float defaultValue;
    public List<FloatVariable> releatedFloatList;
    public List<GameObjectCollection> releatedCollectionList;

    void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");


        //foreach (GameObjectCollection releatedCollection in releatedCollectionList)
        //{
        //    releatedCollection.Clear();
        //}

        //foreach (FloatVariable releatedFloat in releatedFloatList)
        //{
        //    releatedFloat.Value =settings.in;
        //}
        //playerHealth.Value = settings.defaultHealth;
        //defaultValue

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObjectCollection releatedCollection in releatedCollectionList)
            {
                releatedCollection.Add(gameObject);
            }

            foreach (FloatVariable releatedFloat in releatedFloatList)
            {
                releatedFloat.Value += settings.increaseCount;
            }

            Pickup();
        }
    }

    void Pickup()
    {
        Instantiate(settings.pickupEffect, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
