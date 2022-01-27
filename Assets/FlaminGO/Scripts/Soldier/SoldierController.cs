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
    private int shootCount;
    public int magSize;
    public float lookAtDelay;

    private List<SoldierController> allyList = new List<SoldierController>();
    private List<SoldierController> enemyList = new List<SoldierController>();

    [SerializeField] SoldierController targetEnemy;

    public bool isDead;

    public Transform bulletSpawnPos;

    public float bulletForce;
    public float deathForce = 1f;
    public float shootDelay = 2f;

    public float setPositionDelay = 2f;

    public Image healthBar;
    private Canvas canvas;

    private Animator animator;
    private bool animWalk;
    private bool animStart;
    bool isGameFinished;

    //public ParticleSystem explosion;

    private Transform targetTransform;

    public GameObject healField;
    public float healFieldDelay = 2f;
    public bool rpgSoldier;

    public float healBullet = .2f;

    private NavMeshAgent nMesh;
    private Collider colBase;

    //public ParticleSystem shootEffect;

    public float health = 1f;
    public float maxHealth = 1f;


    private void Awake()
    {
        nMesh = GetComponent<NavMeshAgent>();
        colBase = GetComponent<Collider>();
        targetTransform = transform.parent;
        enemyList.Clear();
        allyList.Clear();
        List<SoldierController> allSoldiers = FindObjectsOfType<SoldierController>().ToList();
        allyList = allSoldiers.Where(x => x.teamIndex == teamIndex).ToList();
        enemyList = allSoldiers.Where(x => x.teamIndex != teamIndex).ToList();
        animator = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
    }

    public void StartGame()
    {
        StartCoroutine(SelectTargetV2());
        //SelectTarget();
        //healthBar = GetComponentInChildren<Image>();
        if (!animWalk)
        {
            animator.SetTrigger("Walk");
            animWalk = true;
        }
        if (nMesh.enabled)
        {
            nMesh.destination = targetTransform.position;
        }
        StartCoroutine(CanvasInd());
        //ShootBullet();
        //StartCoroutine(AutoShoot());
    }

    IEnumerator CanvasInd()
    {
        yield return new WaitForSeconds(3f);
        while (true)
        {
            if (canvas)
                canvas.transform.LookAt(Camera.main.transform);
            if (isDead)
            {
                canvas.enabled = false;
            }
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
        //if (other.gameObject.tag == "BulletHeal")
        //{
        //    Debug.Log("HealBulletHitted");
        //    StartCoroutine(HealField());
        //    HealHit(0);
        //    //healthBar.fillAmount += .2f;
        //}
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

    public void HealHit(float _heal)
    {
        if (health + _heal > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += _heal;
        }

        healthBar.fillAmount = health / maxHealth;
    }

    public void TakeHit(float _damage)
    {
        health -= _damage;
        healthBar.fillAmount = health;
        if (health <= 0 /*|| healthBar.fillAmount < 0*/)
        {
            if (!isDead)
            {
                DeathEvent();
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

                //if (bulletSpawnPos)
                //    bulletSpawnPos.LookAt(targetEnemy.transform);
            }
            else
            {
                LevelManager.instance.isGameRunning = false;
            }
            yield return null;
        }
    }

    public void ShootBullet()
    {
        //if (enemyList.Count != 0 || allyList.Count != 0)
        //{
        if (!isDead) // look here
        {
            //shootEffect.Play();
            //animator.SetTrigger("Aim");
            SendBullet();
        }
        //}
        //shootParticle.SetActive(true);

    }

    public void AimStart()
    {
        if (!isDead)
        {
            animator.SetTrigger("Aim");
        }
    }

    public void ReloadBullet()
    {
        shootCount++;
        if (shootCount == magSize)
        {
            if (!isDead)
            {
                animator.SetTrigger("Reload");
                shootCount = 0;
            }
        }
    }

    //void SelectTarget()
    //{
    //    targetEnemy = enemyList.Where(x => !x.isDead).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
    //    transform.DOLookAt(targetEnemy.transform.position, lookAtDelay);
    //}
    //IEnumerator AutoShoot()
    //{
    //    if (enemyList.Count != 0 || allyList.Count != 0)
    //    {
    //        yield return new WaitForSeconds(setPositionDelay);
    //        while (!isDead)
    //        {
    //            //shootEffect.Play();
    //            animator.SetTrigger("Aim");

    //            yield return new WaitForSeconds(shootDelay);
    //            Shoot();
    //            //shootEffect.Stop();
    //            SendBullet();
    //        }
    //    }
    //}

    void SendBullet()
    {
        if (isDead)
        {
            return;
        }
        GameObject _smallBullet;

        if (!rpgSoldier)
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("AmmoTrail", transform.position, transform.rotation);
            _smallBullet.transform.SetParent(bulletSpawnPos);
            _smallBullet.transform.localPosition = Vector3.zero;
            _smallBullet.transform.localRotation = Quaternion.identity;
            //_smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmallEnemy", transform.up, bulletSpawnPos.rotation);
        }
        else
        {
            //_smallBullet = null;
            //_smallBullet = ObjectPool.instance.SpawnFromPool("BulletRocket", transform.up, bulletSpawnPos.rotation);
            _smallBullet = ObjectPool.instance.SpawnFromPool("RocketTrail", transform.position, transform.rotation);
            _smallBullet.transform.SetParent(bulletSpawnPos);
            _smallBullet.transform.localPosition = Vector3.zero;
            _smallBullet.transform.localRotation = Quaternion.identity;
        }

        if (targetEnemy)
        {
            //_smallBullet.transform.LookAt(targetEnemy.transform.position /*+ Vector3.up*/);
            //Transform _targetEnemy = targetEnemy.transform;
            //_targetEnemy = _smallBullet.GetComponent<BulletController>().target;
            //if (!rpgSoldier)
            //{
            //    _smallBullet.transform.LookAt(targetEnemy.transform.position + Vector3.up);
            //}
            //else
            //{
            //    _smallBullet.transform.LookAt(targetEnemy.transform.position + Vector3.up);
            //}
            //_smallBullet.transform.DOMove(targetEnemy.transform.position + Vector3.up, .1f).SetEase(Ease.Linear);

            //_smallBullet.transform.LookAt(_targetEnemy.position + Vector3.up);
            //_smallBullet.transform.DOMove(_targetEnemy.position + Vector3.up, 1f);
            //_smallBullet.gameObject.GetComponent<Rigidbody>().AddForce((targetEnemy.trEansform.position - transform.position).normalized * bulletForce);
            //_smallBullet.transform.position = Vector3.MoveTowards(_smallBullet.transform.position, targetEnemy.transform.position, 40f * Time.deltaTime);
        }
    }

    void DeathEvent()
    {
        isDead = true;
        //colBase.enabled = false;
        if (nMesh != null)
        {
            nMesh.enabled = false;
        }
        animator.enabled = false;
        //explosion.Stop();
        //GetComponentInChildren<Canvas>().enabled = false;
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
        //Debug.Log(allyList.Where(x => !x.isDead).Count());

        yield return new WaitForSeconds(2f);
        //gameObject.SetActive(false);
        if (enemyList.Where(x => !x.isDead).Count() == 0)
        {

            //LevelManager.instance.isGameRunning = false;
            if (CanvasManager.instance.retryLevelButton != null)
            {
                if (CanvasManager.instance.nextLevelButton.activeInHierarchy)
                    yield break;

                Debug.Log("win the game");
                CanvasManager.instance.retryLevelButton.SetActive(true);
                //Time.timeScale = 0;
                enemyList.Clear();
                allyList.Clear();
                //yield break;
            }
        }

        if (allyList.Where(x => !x.isDead).Count() == 0)
        {
            //LevelManager.instance.isGameRunning = false;
            if (CanvasManager.instance.nextLevelButton != null)
            {
                if (CanvasManager.instance.retryLevelButton.activeInHierarchy)
                    yield break;

                Debug.Log("lost the game");

                CanvasManager.instance.nextLevelButton.SetActive(true);
                //Time.timeScale = 0;
                enemyList.Clear();
                allyList.Clear();
                //yield break;
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