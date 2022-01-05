using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;

public class CollectObjects : MonoBehaviour
{
    public List<GameObjectCollection> releatedColelctionList;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRight"))
        {
            foreach (var item in releatedColelctionList.FirstOrDefault())
            {

                item.GetComponent<StackCollectable>().Collect(other.gameObject);
            }

        }

    }
    void Update()
    {
        
    }
}
