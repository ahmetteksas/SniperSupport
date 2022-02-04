using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleHit : MonoBehaviour
{
    public float health;
    public GameObject explodeCar;
    private bool explode;
    public GameObject destroyedState;
    public float radius;
    public float damage = .4f;
    void Start()
    {

    }

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
    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            SoldierController _soldierController = hitCollider.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController && hitCollider.gameObject.tag == "Head")
            {
                _soldierController.TakeHit(damage * 999f);
            }
        }
        StartCoroutine(ExplodeCar());
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
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
        destroyedState.SetActive(true);
        destroyedState.transform.parent = null;
        yield return new WaitForSeconds(2.5f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            Debug.Log("Vechile Hitted");
            ExplosionDamage(transform.position, radius);
            health -= 50f;
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            Debug.Log("Vechile Hitted");
            ExplosionDamage(transform.position, 5f);
            health -= 50f;
        }
    }
}
