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

    [SerializeField] float explosionRadius;



    private void Start()
    {
        transform.localScale = Vector3.zero;
    }
    private void FixedUpdate()
    {
        if (!target || isFirstPositionSetted)
            return;

        if (transform.parent != null)
        {
            isFirstPositionSetted = true;
            //target = target.GetComponentInChildren<SoldierController>().transform;
            Shoot();
        }
    }

    void Shoot()
    {

        float _random = Random.Range(1.25f, 1.55f);

        transform.LookAt(target.transform.position + Vector3.up * _random);


        if (!isRpg)
        {
            transform.DOMove(target.transform.position + Vector3.up * _random, .1f).SetEase(Ease.Linear);
        }
        else
        {
            transform.DOScale(Vector3.one * 2f, .1f);
            //transform.DOLookAt(target.transform.position + Vector3.up * _random, 0f);
            transform.DOMove(target.transform.position + Vector3.up * (/*_random - 1.24f*//*-.3f*/0.3f) /*+ Vector3.right * _random * 2f*/+ transform.forward * .3f, 1f).SetEase(Ease.Linear);
        }
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision other)
    {

        if (isRpg)
        {
            if (other.transform.CompareTag("Untagged"))
                return;

            bool firstHit = false;
            ObjectPool.instance.SpawnFromPool("RPGExplode", other.transform.position, Quaternion.identity);
            Collider[] hitColliders = Physics.OverlapSphere(other.transform.position, explosionRadius);
            foreach (var hitCollider in hitColliders)
            {
                SoldierController _soldierController = hitCollider.gameObject.GetComponentInParent<SoldierController>();
                if (_soldierController)
                {
                    if (!firstHit)
                    {
                        _soldierController.TakeHit(damage);
                        firstHit = true;
                    }
                }

                Vehicle _vehicleHit = hitCollider.gameObject.GetComponent<Vehicle>();

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

        if (isRpg)
            gameObject.SetActive(false);
    }
}