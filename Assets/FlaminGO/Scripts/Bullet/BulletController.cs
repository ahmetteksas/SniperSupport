using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BulletController : MonoBehaviour
{
    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    public GameObject headShot;
    public float hsDelay = 1f;
    //public GameObject trail;

    public Transform target;

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
            //damage = 1f;
            SoldierController _soldierController = other.gameObject.GetComponentInParent<SoldierController>();
            if (_soldierController)
            {
                _soldierController.TakeHit(damage * 20000f);
            }
            Shooter _shooter = FindObjectOfType<Shooter>();
            _shooter.headShot.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            _shooter.headShot.SetActive(true);
            //if (other.gameObject.TryGetComponent(out SoldierController _soldierController))
            //{
            //    //_soldierController.TakeHit(1f);
            //    _soldierController.TakeHit(damage);
            //}
            //damage = 1;// hard to work.
            //Debug.Log("HeadShot !");
            // Debug.Log(damage);
            //if (headShot)
            //{
            //    headShot.SetActive(true);
            //    //StartCoroutine(CloseHs());
            //    //StartCoroutine(DestroyObject());
            //}
        }
        //if (gameObject)
        //{
        //    StartCoroutine(DestroyObject());
        //}
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
