using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float damage;
    public GameObject impactParticle;
    public bool playerBullet;
    void Start()
    {
        StartCoroutine(DestroyObject());
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
        }
    }
    void Update()
    {

    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
