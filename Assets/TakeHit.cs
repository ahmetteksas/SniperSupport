using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHit : MonoBehaviour
{
    public GameObject explosionBarrel;

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
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
