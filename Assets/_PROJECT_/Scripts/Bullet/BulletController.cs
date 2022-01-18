using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    public GameObject headShot;
    public float hsDelay = 1f;
    public GameObject trail;

    void Start()
    {
        StartCoroutine(DestroyObject());
    }
    private void OnEnable()
    {
        if (trail)
        {
            TrailOpen();
        }
    }

    public void TrailOpen()
    {
        if (trail)
        {
            trail.SetActive(true);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (impactParticle)
        {
            impactParticle.SetActive(true);
            impactParticle.GetComponent<ParticleSystem>().Play();
        }

        if (playerBullet && other.gameObject.CompareTag("Head"))
        {
            damage *= 2;// hard to work.
            Debug.Log("HeadShot !");
            if (headShot)
            {
                headShot.SetActive(true);
                //StartCoroutine(CloseHs());
            }
        }
    }
    //IEnumerator CloseHs()
    //{
    //    yield return new WaitForSeconds(hsDelay);
    //    headShot.SetActive(false);
    //}

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
