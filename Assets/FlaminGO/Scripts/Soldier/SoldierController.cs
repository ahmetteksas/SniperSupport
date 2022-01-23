using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using RootMotion.Dynamics;

public class SoldierController : MonoBehaviour
{
    public int teamIndex;

    public float lookAtDelay;

    private List<SoldierController> allyList = new List<SoldierController>();
    private List<SoldierController> enemyList = new List<SoldierController>();

    [SerializeField] SoldierController targetEnemy;

    public bool isDead;

    public Transform bulletSpawnPos;

    public float bulletForce;
    public float deathForce = 1f;
    private float shootDelay = 1.5f;

    public float setPositionDelay = 2f;

    public Image healthBar;
    private Canvas canvas;

    private Animator animator;
    private bool animWalk;
    private bool animStart;

    //public ParticleSystem explosion;

    private Transform targetTransform;

    public GameObject healField;
    public float healFieldDelay = 2f;
    public bool rpgSoldier;

    private NavMeshAgent nMesh;
    private Collider colBase;

    //public ParticleSystem shootEffect;

    public float health;


    private void Awake()
    {
        //explosion = GetComponentInChildren<ParticleSystem>();
        //explosion.Stop();
    }
    public void AwakeGame()
    {
        targetTransform = transform.parent;
        enemyList.Clear();
        allyList.Clear();
        List<SoldierController> allSoldiers = FindObjectsOfType<SoldierController>().ToList();
        allyList = allSoldiers.Where(x => x.teamIndex == teamIndex).ToList();
        enemyList = allSoldiers.Where(x => x.teamIndex != teamIndex).ToList();
        StartCoroutine(SelectTargetV2());
        //SelectTarget();
        //healthBar = GetComponentInChildren<Image>();
        animator = GetComponentInChildren<Animator>();
        if (!animWalk)
        {
            animator.SetTrigger("Walk");
            animWalk = true;
        }
        StartCoroutine(AutoShoot());
        canvas = GetComponentInChildren<Canvas>();
    }

    public void StartGame()
    {
        nMesh = GetComponent<NavMeshAgent>();
        if (nMesh.enabled)
        {
            nMesh.destination = targetTransform.position;
        }
        colBase = GetComponent<Collider>();
        StartCoroutine(CanvasInd());
    }
    IEnumerator CanvasInd()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            canvas.transform.LookAt(Camera.main.transform);
            yield return null;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("BulletPlayer"))
        {
            TakeHit(0);
            //other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "BulletHeal")
        {
            StartCoroutine(HealField());
            healthBar.fillAmount += .2f;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeHit(0);
        }
    }
    IEnumerator HealField()
    {
        if (teamIndex == 0)
        {
            healField.SetActive(true);
            healField.GetComponent<ParticleSystem>().Play();
        }
        yield return new WaitForSeconds(healFieldDelay);
        if (teamIndex == 0)
        {
            healField.SetActive(false);
        }
    }
    private void Update()
    {
        //Debug.Log(enemyList.Count);
    }
    public void TakeHit(float _damage)
    {
        health -= _damage;
        healthBar.fillAmount = health;
        if (health <= 0 /*|| healthBar.fillAmount < 0*/)
        {
            isDead = true;
            if (!animStart)
            {
                DeathEvent();
                animStart = true;
            }
        }
    }

    IEnumerator SelectTargetV2()
    {
        while (!isDead)
        {
            targetEnemy = enemyList.Where(x => !x.isDead).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
            if (targetEnemy)
            {
                transform.DOLookAt(targetEnemy.transform.position, lookAtDelay);
                bulletSpawnPos.LookAt(targetEnemy.transform);
            }
            else
            {
                LevelManager.instance.isGameRunning = false;
            }
            yield return null;
        }
    }
    //public void OpenShoot()
    //{
    //    //shootParticle.SetActive(true);
    //}
    //void SelectTarget()
    //{
    //    targetEnemy = enemyList.Where(x => !x.isDead).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
    //    transform.DOLookAt(targetEnemy.transform.position, lookAtDelay);
    //}
    IEnumerator AutoShoot()
    {
        if (enemyList.Count != 0 || allyList.Count != 0)
        {
            yield return new WaitForSeconds(setPositionDelay);

            GameObject _smallBullet;
            Vector3 _offset = new Vector3(-.3f, 0, .5f);
            while (!isDead)
            {
                //shootEffect.Play();
                animator.SetTrigger("Aim");

                yield return new WaitForSeconds(shootDelay);
                //shootEffect.Stop();
                if (teamIndex == 0)
                {
                    if (!rpgSoldier)
                    {
                        _smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmall", bulletSpawnPos.position + _offset, Quaternion.identity);
                    }
                    else
                    {
                        _smallBullet = ObjectPool.instance.SpawnFromPool("BulletRocket", bulletSpawnPos.position + _offset, Quaternion.identity);
                    }

                }
                else
                {
                    if (!rpgSoldier)
                    {
                        _smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmallEnemy", bulletSpawnPos.position - _offset, Quaternion.identity);
                    }
                    else
                    {
                        _smallBullet = ObjectPool.instance.SpawnFromPool("BulletRocket", bulletSpawnPos.position - _offset, Quaternion.Euler(90, 90, 90));
                    }
                }
                if (targetEnemy)
                {
                    _smallBullet.transform.LookAt(targetTransform);
                    _smallBullet.gameObject.GetComponent<Rigidbody>().AddForce((targetEnemy.transform.position-transform.position).normalized * bulletForce);
                    //_smallBullet.transform.position = Vector3.MoveTowards(_smallBullet.transform.position, targetEnemy.transform.position, 40f * Time.deltaTime);
                }
            }
        }
    }

    void DeathEvent()
    {
        //colBase.enabled = false;
        if (nMesh != null)
        {
            nMesh.enabled = false;
        }
        animator.enabled = false;
        //explosion.Stop();
        GetComponentInChildren<Canvas>().enabled = false;
        PuppetMaster _puppetMaster = GetComponentInChildren<PuppetMaster>();
        _puppetMaster.state = PuppetMaster.State.Dead;
        //foreach (Rigidbody _rigidbody in GetComponentsInChildren<Rigidbody>())
        //{
        //    _rigidbody.isKinematic = false;
        //    _rigidbody.velocity = Vector3.zero;
        //}

        //GetComponent<Rigidbody>().isKinematic = true;

        //foreach (Rigidbody _rigidbody in GetComponentsInChildren<Rigidbody>())
        //{
        //    _rigidbody.AddForce(-transform.forward * deathForce);
        //}

        StartCoroutine(FinishGameEnum());
        //gameObject.SetActive(false);
    }

    IEnumerator FinishGameEnum()
    {
        Debug.Log(allyList.Where(x => !x.isDead).Count());

        yield return new WaitForSeconds(2f);
        //gameObject.SetActive(false);
        if (enemyList.Where(x => !x.isDead).Count() == 0)
        {
            //LevelManager.instance.isGameRunning = false;
            if (CanvasManager.instance.retryLevelButton != null)
            {
                Debug.Log("win the game");
                CanvasManager.instance.retryLevelButton.SetActive(true);
                Time.timeScale = 0;
                enemyList.Clear();
                allyList.Clear();
            }
            //nextLevel.SetActive(true);
        }
        if (allyList.Where(x => !x.isDead).Count() == 0)
        {
            //LevelManager.instance.isGameRunning = false;
            if (CanvasManager.instance.nextLevelButton != null)
            {
                Debug.Log("lost the game");
                CanvasManager.instance.nextLevelButton.SetActive(true);
                Time.timeScale = 0;
                enemyList.Clear();
                allyList.Clear();
            }
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