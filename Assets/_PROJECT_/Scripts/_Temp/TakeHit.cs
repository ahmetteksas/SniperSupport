using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHit : MonoBehaviour
{
    public GameObject explosionBarrel;
    public GameObject vehicle;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ExplosionDamage(Vector3.forward*5f,.5f);
            //explosionBarrel.SetActive(true);
            //vehicle.SetActive(false);
            //gameObject.SetActive(false);
            //StartCoroutine(ExplosionStart());
        }
    }
    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            StartCoroutine(ExplosionStart());
        }
    }
    IEnumerator ExplosionStart()
    {
        explosionBarrel.SetActive(true);
        yield return new WaitForSeconds(2f);
        vehicle.SetActive(false);
        gameObject.SetActive(false);
        
    }
    void Update()
    {
        
    }
}
