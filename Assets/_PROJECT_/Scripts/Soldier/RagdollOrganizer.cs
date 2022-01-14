using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RagdollOrganizer : MonoBehaviour
{
    public int targetLayerIndex;
    void Start()
    {
        
    }

    [Button]
    public void SetLayer()
    {
        foreach (Rigidbody _rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            _rigidbody.gameObject.layer = targetLayerIndex;
        }
    }
}
