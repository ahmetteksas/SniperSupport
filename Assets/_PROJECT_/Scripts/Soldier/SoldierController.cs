using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;


public class SoldierController : MonoBehaviour
{
    private List<SoldierController> allyList = new List<SoldierController>();
    private List<SoldierController> enemyList = new List<SoldierController>();


    public int teamIndex;

    public Transform bulletSpawnPos;

    public float bulletForce;
    private float shootDelay = .5f;
    private float shootTime = 0f;

    private ProgressBarPro healthBar;

    private Animator animator;
    private bool animStart;

    private ParticleSystem explosion;


    bool isDead;
    private void Awake()
    {
        enemyList.Clear();
        allyList.Clear();
        List<SoldierController> allSoldiers = FindObjectsOfType<SoldierController>().ToList();
        allyList = allSoldiers.Where(x => x.teamIndex == teamIndex).ToList();
        enemyList = allSoldiers.Where(x => x.teamIndex != teamIndex).ToList();
    }
    void Start()
    {
        explosion = GetComponentInChildren<ParticleSystem>();
        healthBar = GetComponentInChildren<ProgressBarPro>();
        animator = GetComponent<Animator>();
        StartCoroutine(AutoShoot());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            healthBar.Value -= other.gameObject.GetComponent<BulletController>().damage;
            TakeHit();
        }
        if (other.gameObject.tag == "BulletHeal")
        {
            healthBar.Value += .1f;
        }
    }

    void TakeHit()
    {
        if (healthBar.Value == 0)
        {
            isDead = true;
            if (!animStart)
            {
                StartCoroutine(DeathEvent());
                animStart = true;
            }
        }
    }
    IEnumerator AutoShoot()
    {
        GameObject _smallBullet;
        while (!isDead)
        {
            yield return new WaitForSeconds(shootDelay);
            if (teamIndex == 0)
            {
                _smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmall", bulletSpawnPos.position, Quaternion.identity);
            }
            else
            {
                _smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmallEnemy", bulletSpawnPos.position, Quaternion.identity);
            }
            _smallBullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
        }
    }

    IEnumerator DeathEvent()
    {
        if (animator)
        {
            animator.SetTrigger("Death");
        }
        explosion.Play();
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        if (enemyList.Where(x => !x.isDead).Count() == 0)
        {
            //nextLevel.SetActive(true);
        }
        if (allyList.Where(x => !x.isDead).Count() == 0)
        {
            //retryLevel.SetActive(true);
        }
    }
}
/*
            shootTime = 0f;


        shootTime += Time.deltaTime;
        if (shootTime > shootDelay && gameObject != null && teamIndex ==0)
        {
            Shoot();
        }
        if (shootTime > shootDelay && gameObject != null && teamIndex == 1)
        {
            ShootEnemy();
        }
        if (gameObject.tag == "Enemy")
        {
            enemyBoss = GetComponent<BoxCollider>();
            if (enemyBoss)
            {
                enemyBoss.isTrigger = true;
            }
        }
        if (gameObject.tag == "Ally")
        {
            allyBoss = GetComponent<BoxCollider>();
            if (allyBoss)
            {
                allyBoss.isTrigger = true;
            }
            gameObject.transform.LookAt(enemyList.LastOrDefault().transform.position);
        }
 
        if (allyList.Where(x => !x.isDead).Count() == 1)
        {
            if (allyBoss)
            {
                allyBoss.isTrigger = false;
            }
        }
        if (enemyList.Where(x => !x.isDead).Count() == 1)
        {
            if (enemyBoss)
            {
                enemyBoss.isTrigger = false;
            }
        }
 
 
        if ((other.gameObject.CompareTag("BulletSmall") || other.gameObject.CompareTag("BulletSmallPlayer")) && gameObject.tag == "Ally")
        {
            healthBar.Value -= .02f;
        }
        if (other.gameObject.CompareTag("BulletSmall") && gameObject.tag == "Enemy")
        {
            healthBar.Value -= .01f;
        }
        if (other.gameObject.CompareTag("BulletSmallPlayer") && gameObject.tag == "Enemy")
        {
            healthBar.Value -= .1f;
        }
 
 
 
 
 
 */