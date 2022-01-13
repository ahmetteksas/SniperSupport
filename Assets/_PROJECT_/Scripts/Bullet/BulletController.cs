using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    void Start()
    {
        StartCoroutine(DestroyObject());
    }

    void Update()
    {
        
    }
    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
