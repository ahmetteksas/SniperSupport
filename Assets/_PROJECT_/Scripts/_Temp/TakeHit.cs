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
            ExplosionDamage(transform.position,2f);
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
            if (hitCollider.TryGetComponent(out SoldierController soldierController)) 
            {
                soldierController.healthBar.fillAmount -= .6f; 
            }
            //hitCollider.gameObject.GetComponent<SoldierController>().healthBar.fillAmount -= .2f;
            StartCoroutine(ExplosionStart());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2f);
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
