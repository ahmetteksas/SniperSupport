using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    private Renderer myColor;
    void Start()
    {
       myColor = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerRight"))
        {
            myColor.material.color = Color.green;
        }
    }
    void Update()
    {
        
    }
}
