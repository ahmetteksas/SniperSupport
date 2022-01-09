using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoMoveY : MonoBehaviour
{
    bool harmonic;
    void Start()
    {
        StartCoroutine(DanceMovement(gameObject));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("?arp??ma var !!");
        //harmonic = true;
    }
    private void OnTriggerExit(Collider other)
    {
        //harmonic = false;
    }
    void Update()
    {
        
    }
    IEnumerator DanceMovement(GameObject gameObject)
    {
        //while (!harmonic)
        //{
        //    yield return gameObject.transform.DOMoveY(4.6f, .4f).WaitForCompletion();
        //    yield return gameObject.transform.DOMoveY(4f, .4f).WaitForCompletion();
        //}
        while (!harmonic)
        {
            yield return gameObject.transform.DOMoveY(4.6f, .12f).WaitForCompletion();
            yield return gameObject.transform.DOMoveY(3.8f, .12f).WaitForCompletion();
        }

    }
   
}
