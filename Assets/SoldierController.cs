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

    BoxCollider enemyBoss;
    BoxCollider allyBoss;

    private ParticleSystem explosion;

    public GameObject nextLevel;
    public GameObject retryLevel;
    private void Awake()
    {
        enemyCount.Value = 0;
        allyCount.Value = 0;
        enemyList.Clear();
        allyList.Clear();
    }
    void Start()
    {
        explosion = GetComponentInChildren<ParticleSystem>();
        if (this.gameObject.tag == "Enemy")
        {
            enemyBoss = GetComponent<BoxCollider>();
            if (enemyBoss)
            {
                enemyBoss.isTrigger = true;
            }
            
            enemyList.Add(this.gameObject);
            enemyCount.Value ++;
        }
        if (this.gameObject.tag == "Ally")
        {
            allyBoss = GetComponent<BoxCollider>();
            if (allyBoss)
            {
                allyBoss.isTrigger = true;
            }
            
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
        if (shootTime > shootDelay && this.gameObject != null)
        {
            Shoot();
        }
        if (health.Value ==0 && this.gameObject.tag =="Enemy")
        {
            enemyList.Remove(this.gameObject);
            
            if (!animStart)
            {
                StartCoroutine(DeathEvent());
                animStart = true;
                enemyCount.Value--;
            }
            
        }
        if (health.Value == 0 && this.gameObject.tag == "Ally")
        {
            allyList.Remove(this.gameObject);
            
            if (!animStart)
            {
                StartCoroutine(DeathEvent());
                animStart = true;
                allyCount.Value--;
            }
        }
        if (allyCount == 1)
        {
            if (allyBoss)
            {
                allyBoss.isTrigger = false;
            }
        }
        if (enemyCount == 1)
        {
            if (enemyBoss)
            {
                enemyBoss.isTrigger = false;
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
        if (animBase)
        {
            animBase.SetTrigger("Death");
        }
        explosion.Play();
        yield return new WaitForSeconds(2f);
            
        gameObject.SetActive(false);
        if (enemyCount.Value == 0)
        {
            nextLevel.SetActive(true);
        }
        if (allyCount.Value == 0)
        {
            retryLevel.SetActive(true);
        }
        //yield return new WaitForSeconds(5f);

    }
}
