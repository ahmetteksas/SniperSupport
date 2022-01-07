using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform bulletSpawnPos;
    Transform character;
    Transform ai;
    public float bulletForce;
    private float shootDelay = 1.5f;
    private float shootTime = 0f;
    void Start()
    {
        if (this.gameObject.name =="Character")
        {
            character = transform;
        }
        else
        {
            ai = transform;
            ai.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, -character.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        //shootDelay = shootTime - Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        shootTime += Time.deltaTime;
        if (shootTime > shootDelay)
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (this.gameObject.name == "character")
            {
                GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
                _smallbullet.transform.rotation = character.rotation;
                _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce(character.forward * bulletForce);
                shootTime = 0f;
            }
            else
            {
                GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
                _smallbullet.transform.rotation = ai.rotation;
                _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce(ai.forward * bulletForce);
                shootTime = 0f;
            }
           
        }
    }
}
