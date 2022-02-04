using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BulletController : MonoBehaviour
{
    public bool isRpg;

    private ParticleSystem trail;
    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    //public GameObject headShot;
    public float hsDelay = 1f;
    //public GameObject trail;

    public Transform target;

    bool isFirstPositionSetted;
    private void Awake()
    {

    }
    void Start()
    {
        //StartCoroutine(DestroyObject());
        //if (TryGetComponent(out ParticleSystem particleSystem))
        //{
        //    trail = particleSystem;
        //    trail.Stop();
        //}
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
        //if (target != null)
        //{
        //    transform.position = target.position;//.DOMove(target.position, .5f);// = target.transform.position;
        //}
        if (!isFirstPositionSetted)
        {
            if (transform.parent != null)
            {
                isFirstPositionSetted = true;
                //return;
                //if (isRpg)
                //{
                //    //transform.localPosition = Vector3.forward * 2f;
                //}
                //else
                //{
                //    transform.localPosition = Vector3.zero;
                //}
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                if (target != null)
                {
                    transform.DOMove(target.position + Vector3.up, .2f).SetEase(Ease.Linear);
                }

                if (isRpg)
                {
                    //Debug.Log("bullet first position setted");
                }

                //trail.Play();

                if (isRpg)
                {
                    //target.position += Vector3.up * 4f;
                }
                else if (target)
                {
                    target.position += Vector3.up * 1.12f;
                }

                transform.SetParent(null);
                //transform.Translate(Vector3.right * 13f);
                //transform.Rotate(Vector3.up * 20f);
                //transform.LookAt(target);
            }
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.name);
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
                _soldierController.lastHittedBullet = gameObject;
                _soldierController.TakeHit(damage * 20000f);
            }
        }
        gameObject.SetActive(false);
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
