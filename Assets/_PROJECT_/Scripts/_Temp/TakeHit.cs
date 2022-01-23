using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHit : MonoBehaviour
{
    public GameObject explosionBarrel;
    public GameObject vehicle;
    public float damage = .6f;
    //public GameObject explosionOtherBarrel;
    //public GameObject otherBarrel;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            other.gameObject.SetActive(false);
            ExplosionDamage(transform.position, 2f);
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
            SoldierController _soldierController = hitCollider.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.TakeHit(damage);
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
        ObjectPool.instance.SpawnFromPool("CarExplode", transform.position, Quaternion.identity);
        explosionBarrel.SetActive(true);
        //explosionOtherBarrel.SetActive(true);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        //vehicle.SetActive(false);
        //otherBarrel.SetActive(false);
    }
    void Update()
    {

    }
}
