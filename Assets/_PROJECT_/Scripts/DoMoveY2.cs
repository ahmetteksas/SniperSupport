using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoMoveY2 : MonoBehaviour
{
    bool harmonic;
    public GameObject particle1;
    void Start()
    {
        StartCoroutine(DanceMovement(gameObject));
        
    }

    private void OnTriggerEnter(Collider other)
    {
        particle1.SetActive(true);
        StartCoroutine(StopParticle());
        //Debug.Log("?arp??ma var !!");
        //harmonic = true;
    }
    private void OnTriggerExit(Collider other)
    {
        //particle1.SetActive(false);
        //harmonic = false;
    }
    void Update()
    {

    }
    IEnumerator DanceMovement(GameObject gameObject)
    {
        //while (!harmonic)
        //{
        //    yield return gameObject.transform.DOMoveY(3.6f, .4f).WaitForCompletion();
        //    yield return gameObject.transform.DOMoveY(3f, .4f).WaitForCompletion();
        //}
        while (!harmonic)
        {
            yield return gameObject.transform.DOMoveY(4f, .12f).WaitForCompletion();
            yield return gameObject.transform.DOMoveY(3.4f, .12f).WaitForCompletion();
        }

    }
    IEnumerator StopParticle ()
    {
        yield return new WaitForSeconds(1.5f);
        particle1.SetActive(false);
    }
   
}
