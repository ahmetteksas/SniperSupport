using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;


public class SoldierController : MonoBehaviour
{
    [SerializeField] IntVariable enemyCount;
    [SerializeField] IntVariable allyCount;

    [SerializeField] GameObjectCollection enemyList;
    [SerializeField] GameObjectCollection allyList;

    public Transform bulletSpawnPos;

    public float bulletForce;
    private float shootDelay = .5f;
    private float shootTime = 0f;

    private ProgressBarPro health;

    private Animator animBase;
    private bool animStart;

    private void Awake()
    {
        enemyCount.Value = 0;
        allyCount.Value = 0;
        enemyList.Clear();
        allyList.Clear();
    }
    void Start()
    {
        if (this.gameObject.tag == "Enemy")
        {
            enemyList.Add(this.gameObject);
            enemyCount.Value ++;
        }
        if (this.gameObject.tag == "Ally")
        {
            allyList.Add(this.gameObject);
            allyCount.Value++;
        }
        health = GetComponentInChildren<ProgressBarPro>();
        animBase = GetComponent<Animator>();
        //shootDelay = shootTime - Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.CompareTag("BulletSmall") || other.gameObject.CompareTag("BulletSmallPlayer"))&&this.gameObject.tag =="Ally")
        {
            health.Value -= .02f;
        }
        if (other.gameObject.CompareTag("BulletSmall") && this.gameObject.tag == "Enemy")
        {
            health.Value -= .01f;
        }
        if (other.gameObject.CompareTag("BulletSmallPlayer") &&this.gameObject.tag == "Enemy")
        {
            health.Value -= .1f;
        }
    }
    void Update()
    {
        shootTime += Time.deltaTime;
        if (shootTime > shootDelay)
        {
            Shoot();
        }
        if (health.Value ==0 && this.gameObject.tag =="Enemy")
        {
            enemyList.Remove(this.gameObject);
            enemyCount.Value--;
            if (!animStart)
            {
                StartCoroutine(DeathEvent());
                animStart = true;
            }
            
        }
        if (health.Value == 0 && this.gameObject.tag == "Ally")
        {
            allyList.Remove(this.gameObject);
            allyCount.Value--;
            if (!animStart)
            {
                StartCoroutine(DeathEvent());
                animStart = true;
            }
        }
    }
    public void Shoot()
    {
            GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmall", bulletSpawnPos.position, Quaternion.identity);
            _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
            shootTime = 0f;
    }
    IEnumerator DeathEvent()
    {
        animBase.SetTrigger("Death");
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
