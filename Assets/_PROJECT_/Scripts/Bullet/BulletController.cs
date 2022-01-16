using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public GameObject impact;
    void Start()
    {
        StartCoroutine(DestroyObject());
    }
    private void OnCollisionEnter(Collision other)
    {
        impact.SetActive(true);
        impact.GetComponent<ParticleSystem>().Play();
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
