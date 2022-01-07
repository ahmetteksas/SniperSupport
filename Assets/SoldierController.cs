using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SoldierController : MonoBehaviour
{
    public Transform bulletSpawnPos;
    public float bulletForce;
    private float shootDelay = .5f;
    private float shootTime = 0f;
    private ProgressBarPro health;
    void Start()
    {
        health = GetComponentInChildren<ProgressBarPro>();
        //shootDelay = shootTime - Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BulletSmall")&&this.gameObject.tag =="Ally")
        {
            health.Value -= .02f;
        }
        if (other.gameObject.CompareTag("BulletSmall") && this.gameObject.tag == "Enemy")
        {
            health.Value -= .01f;
        }
    }
    void Update()
    {
        shootTime += Time.deltaTime;
        if (shootTime > shootDelay)
        {
            Shoot();
        }
        if (health.Value ==0)
        {
            gameObject.SetActive(false);
        }
    }
    public void Shoot()
    {
        
            GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmall", bulletSpawnPos.position, Quaternion.identity);
            _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            shootTime = 0f;
        
    }
}
