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
