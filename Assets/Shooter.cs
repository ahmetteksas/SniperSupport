using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform bulletSpawnPos;
    public float bulletForce;
    private float shootDelay = 1.5f;
    private float shootTime = 0f;
    void Start()
    {
        //shootDelay = shootTime - Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        shootTime += Time.deltaTime;
        if (shootTime>shootDelay)
        {
            Shoot();
        }  
    }
    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject _smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmall", bulletSpawnPos.position, Quaternion.identity);
            _smallBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            shootTime = 0f;
        }
    }
}
