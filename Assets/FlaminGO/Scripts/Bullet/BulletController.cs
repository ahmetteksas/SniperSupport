using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Dreamteck.Splines;


public class BulletController : MonoBehaviour
{
    public bool isRpg;
    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    public float hsDelay = 1f;
    public Transform target;
    bool isFirstPositionSetted;


    private void FixedUpdate()
    {
        if (!target || isFirstPositionSetted)
            return;

        if (transform.parent != null)
        {
            isFirstPositionSetted = true;

            Shoot();
        }
    }

    void Shoot()
    {
        //return;
        transform.LookAt(target.transform.position);

        if (!isRpg)
        {
            transform.DOMove(target.transform.position + Vector3.up / 2, .2f).SetEase(Ease.Linear);
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(-(transform.position - target.transform.position - new Vector3(0, 1f, 0)).normalized * 2000f);
        }
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (impactParticle)
        {
            impactParticle.SetActive(true);
            impactParticle.GetComponent<ParticleSystem>().Play();
        }
        if (other.gameObject.CompareTag("Enemy") && !playerBullet)
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.lastHittedBullet = gameObject;
                _soldierController.TakeHit(damage);
            }
        }
        if (other.gameObject.CompareTag("Enemy") && playerBullet)
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.gameObject.transform.SetParent(null, true);
                _soldierController.lastHittedBullet = gameObject;
                _soldierController.TakeHit(damage);
            }
            Shooter _shooter = FindObjectOfType<Shooter>();
            if (_shooter.headShot)
            {
                _shooter.headShot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                _shooter.headShot.SetActive(true);
            }
        }
        if (playerBullet && other.gameObject.CompareTag("Head"))
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.gameObject.transform.SetParent(null, true);
                _soldierController.lastHittedBullet = gameObject;
                _soldierController.TakeHit(damage * 20000f);
            }
        }
        gameObject.SetActive(false);
    }
}