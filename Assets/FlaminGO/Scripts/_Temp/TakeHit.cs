using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHit : MonoBehaviour, IHitable
{
    public float effectRadius;
    public float damage = .6f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            Debug.Log("Barrel Hitted");
            other.gameObject.SetActive(false);
        }
    }

    public void StartExplosion() => StartCoroutine(ExplosionStart());

    void ExplosionDamage(Vector3 center, float radius)
    {
        StartExplosion();

        Collider[] hitColliders = Physics.OverlapSphere(center, effectRadius);
        foreach (var hitCollider in hitColliders)
        {
            SoldierController _soldierController = hitCollider.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController && hitCollider.gameObject.tag == "Head")
            {
                _soldierController.TakeHit(damage * 999f);
            }

            VehicleHit _vehicleHit = hitCollider.gameObject.GetComponent<VehicleHit>();

            if (_vehicleHit)
            {
                _vehicleHit.Explode();
            }
            TakeHit _takeHit = hitCollider.gameObject.GetComponent<TakeHit>();

            if (_takeHit)
            {
                _takeHit.StartExplosion();
            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, effectRadius);

    IEnumerator ExplosionStart()
    {
        ObjectPool.instance.SpawnFromPool("CarExplode", transform.position, Quaternion.identity);
        //if (explosionBarrel)
        //{
        //    explosionBarrel.SetActive(true);
        //}
        //explosionOtherBarrel.SetActive(true);
        yield return new WaitForFixedUpdate();
        gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        //vehicle.SetActive(false);
        //otherBarrel.SetActive(false);
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float _damage)
    {
        ExplosionDamage(transform.position, 4f);
    }
}