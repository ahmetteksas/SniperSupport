using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailBulletController : MonoBehaviour
{
    public float damage = .1f;
    public bool rocketParticle;
    void Start()
    {

    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("DamageHitted");
        if (other.gameObject.CompareTag("Enemy") && !rocketParticle)
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.TakeHit(damage);
               
            }
            //gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Enemy") && rocketParticle)
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.TakeHit(damage*3f);
            }
            //gameObject.SetActive(false);
        }

    }
    void Update()
    {

    }
}
