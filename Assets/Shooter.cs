using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using UnityEngine.UI;
public class Shooter : MonoBehaviour
{
    public Transform bulletSpawnPos;
    public GameObjectCollection allyList;
    private Transform targetAlly;
    public float bulletForce;
    private float shootDelay = 1.5f;
    private float shootDelayAi = 1f;
    private float shootTime = 0f;
    private bool shoot;
    int counterBullet;
    int counterHeal;

    void Start()
    {

        //shootDelay = shootTime - Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.name == "AI")
        {
            targetAlly = allyList.LastOrDefault().transform;
        }

        shootTime += Time.deltaTime;
        if (shootTime > shootDelay && this.gameObject.name == "Character")
        {
            if (Input.GetMouseButtonUp(0) /*&& !shoot*/)
            {
                ShootBullet();
            }
            if (Input.GetMouseButtonUp(0) /*&& shoot*/)
            {
                ShootHeal();
            }
        }
        if (shootTime > shootDelayAi && this.gameObject.name == "AI")
        {
            AiShoot();
        }
    }
    public void ShootBullet()
    {
        counterBullet++;
        if (counterBullet ==1)
        {
            GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
            _smallbullet.transform.rotation = transform.rotation;
            _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            shootTime = 0f;
            //shoot = true;
        }
        
    }
    public void ShootHeal()
    {
        counterHeal++;
        if (counterHeal ==1)
        {
            GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletHeal", bulletSpawnPos.position, Quaternion.identity);
            _smallbullet.transform.rotation = transform.rotation;
            _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            shootTime = 0f;
            //shoot = false;
        }
        
    }
    public void AiShoot()
    {
        GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
        if (this.gameObject.name == "AI")
        {
            _smallbullet.transform.rotation = /*transform.rotation +*/ Quaternion.Euler(0, 0, 0);
        }
        _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce((targetAlly.position - transform.position) * bulletForce / 4);
        shootTime = 0f;
    }
}
