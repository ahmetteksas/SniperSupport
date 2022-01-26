using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BulletController : MonoBehaviour
{
    public bool isRpg;

    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    public GameObject headShot;
    public float hsDelay = 1f;
    //public GameObject trail;

    public Transform target;

    bool isFirstPositionSetted;

    void Start()
    {
        //StartCoroutine(DestroyObject());
    }
    //private void OnEnable()
    //{
    //    //target.transform
    //    if (trail)
    //    {
    //        trail.SetActive(true);
    //    }
    //}
    //private void OnDisable()
    //{
    //    if (trail)
    //    {
    //        trail.SetActive(false);
    //    }
    //}

    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;//.DOMove(target.position, .5f);// = target.transform.position;
        }
        if (!isFirstPositionSetted)
        {
            if (transform.parent != null)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                isFirstPositionSetted = true;
                //if (isRpg)
                //{
                //    ObjectPool.instance.SpawnFromPool("AmmoTrail", transform.position, transform.rotation);
                //}
                if (!isRpg)
                {
                    ObjectPool.instance.SpawnFromPool("AmmoTrail", transform.position, transform.rotation);
                }
            }
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    gameObject.SetActive(false);
    //}
    //public void TrailOpen()
    //{
    //    if (trail)
    //    {
    //        trail.SetActive(true);
    //    }
    //}
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
                _soldierController.TakeHit(damage);
            }
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Enemy") && playerBullet)
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.TakeHit(damage);
            }
            Shooter _shooter = FindObjectOfType<Shooter>();
            _shooter.headShot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            _shooter.headShot.SetActive(true);
        }
        if (playerBullet && other.gameObject.CompareTag("Head"))
        {
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.TakeHit(damage * 20000f);
            }
            Shooter _shooter = FindObjectOfType<Shooter>();
            _shooter.headShot.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            _shooter.headShot.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    //IEnumerator CloseHs()
    //{
    //    yield return new WaitForSeconds(hsDelay);
    //    headShotImage.SetActive(false);
    //    headShot.SetActive(false);
    //}

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
