using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBulletController : MonoBehaviour
{
    public float healValue = .2f;
    void Start()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
        if (_soldierController)
        {
            _soldierController.HealHit(healValue);
            Debug.Log("BulletHealHitted");
        }
        gameObject.SetActive(false);
    }

}
