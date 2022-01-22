using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    public GameObject headShot;
    public GameObject headShotImage;
    public float hsDelay = 1f;
    public GameObject trail;
    void Start()
    {
        headShotImage = GameObject.FindGameObjectWithTag("Hs");
        //StartCoroutine(DestroyObject());
    }
    private void OnEnable()
    {
        if (trail)
        {
            trail.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if (trail)
        {
            trail.SetActive(false);
        }
    }
    //public void TrailOpen()
    //{
    //    if (trail)
    //    {
    //        trail.SetActive(true);
    //    }
    //}
    private void OnCollisionEnter(Collision other)
    {
        if (impactParticle)
        {
            impactParticle.SetActive(true);
            impactParticle.GetComponent<ParticleSystem>().Play();
        }

        if (playerBullet && other.gameObject.CompareTag("Head"))
        {
            damage = 1f;
            //damage = 1;// hard to work.
            Debug.Log("HeadShot !");
            Debug.Log(damage);
            if (headShotImage)
            {
                headShotImage.SetActive(true);
                Debug.Log("Image a��ld� !!");
            }
            if (headShot)
            {
                headShot.SetActive(true);
               

                //StartCoroutine(CloseHs());
                //StartCoroutine(DestroyObject());
            }
        }
        gameObject.SetActive(false);
    }
    //IEnumerator CloseHs()
    //{
    //    yield return new WaitForSeconds(hsDelay);
    //    headShotImage.SetActive(false);
    //    headShot.SetActive(false);
    //}

    //IEnumerator DestroyObject()
    //{
    //    yield return new WaitForSeconds(1.5f);
    //    gameObject.SetActive(false);
    //}
}
