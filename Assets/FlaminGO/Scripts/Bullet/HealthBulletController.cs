using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthBulletController : MonoBehaviour
{
    Rigidbody rigidbody;
    public float force;
    public Transform target;
    public float healValue = .2f;

    bool shooted;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (target != null && !shooted)
        {
            //shooted = true;

            GoToTarget();
        }

    }

    public void GoToTarget()
    {
        Debug.Log("go to target");
        transform.DOMove(target.position, 1f);// = target.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
        Debug.Log(other.gameObject.name);
        if (_soldierController)
        {
            _soldierController.HealHit(healValue);
            GameObject shootParticle = ObjectPool.instance.SpawnFromPool("HealField", _soldierController.gameObject.transform.position, _soldierController.transform.rotation);
            shootParticle.SetActive(true);
            gameObject.SetActive(true);
            Debug.Log("BulletHealHitted");
        }
    }
}