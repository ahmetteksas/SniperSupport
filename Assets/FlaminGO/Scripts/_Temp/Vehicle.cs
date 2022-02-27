using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vehicle : MonoBehaviour, IHitable
{
    public float health;
    public GameObject explodeCar;
    private bool explode;
    public GameObject destroyedState;
    public float radius;
    public float damage = .4f;


    void Update()
    {
        if (health == 0)
        {
            if (!explode)
            {
                explode = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator ExplosionDamage(Vector3 center, float radius)
    {
        yield return new WaitForSeconds(.65f);
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out IHitable _vehicleHit))
            {
                _vehicleHit.TakeDamage(1f);
            }

            SoldierController _soldierController = hitCollider.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController && hitCollider.gameObject.tag == "Head")
            {
                _soldierController.TakeHit(damage * 999f);
            }
        }
        StartCoroutine(ExplodeCar());
    }

    public void Explode()
    {
        ExplosionDamage(transform.position, radius);
        StartCoroutine(ExplodeCar());
    }

    public IEnumerator ExplodeCar()
    {
        yield return new WaitForSeconds(.4f);
        ObjectPool.instance.SpawnFromPool("CarExplode", transform.position, Quaternion.identity);
        explodeCar.SetActive(true);
        yield return new WaitForFixedUpdate();
        gameObject.SetActive(false);

        if (destroyedState)
        {
            destroyedState.SetActive(true);
            destroyedState.transform.parent = null;
        }
        yield return new WaitForSeconds(2.5f);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            ExplosionDamage(transform.position, 5f);
            health -= 50f;
        }
    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            StartCoroutine(ExplosionDamage(transform.position, radius * 1.3f));
            foreach (Weapon _weapon in GetComponentsInChildren<Weapon>())
            {
                _weapon.Throw(.65f);
            }
        }
    }

    public void TakeDamage()
    {

    }
}
