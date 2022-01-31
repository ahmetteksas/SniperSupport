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

    Shooter shooter;

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

    private NavMeshAgent navMeshAgent;
    private Collider colBase;

    //public ParticleSystem shootEffect;

    public float health = 1f;
    public float maxHealth = 1f;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        colBase = GetComponent<Collider>();
        targetTransform = transform.parent;
        List<SoldierController> allSoldiers = FindObjectsOfType<SoldierController>().ToList();
        animator = GetComponentInChildren<Animator>();
        canvas = GetComponentInChildren<Canvas>();
    }

    public void StartGame()
    {
        shooter = FindObjectOfType<Shooter>();
        StartCoroutine(SelectTargetV2());
        if (!animWalk)
        {
            animator.SetTrigger("Walk");
            animWalk = true;
        }
        if (navMeshAgent.enabled)
        {
            navMeshAgent.destination = targetTransform.position;
        }
        StartCoroutine(CanvasInd());
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

    SoldierController tempEnemySoldier;
    IEnumerator SelectTargetV2()
    {
        while (!isDead)
        {
            tempEnemySoldier = targetEnemy;
            if (teamIndex == 0)
            {
                targetEnemy = shooter.enemyList.Where(x => !x.isDead).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
            }
            else
            {
                targetEnemy = shooter.allyList.Where(x => !x.isDead).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
            }


            if (tempEnemySoldier != targetEnemy)
            {
                shootCount = magSize - 1;
                ReloadBullet();
                transform.DOPause();

                if (targetEnemy)
                    yield return transform.DOLookAt(targetEnemy.transform.parent.position, lookAtDelay).WaitForCompletion();
            }
            yield return new WaitForSeconds(1f);

            //if (targetEnemy)
            //{
            //    transform.DOPause();
            //    transform.DOLookAt(targetEnemy.transform.position, lookAtDelay);

            //}
        }
    }

    public void ShootBullet()
    {
        if (!LevelManager.instance.isGameRunning)
            return;

        if (!isDead)
        {
            SendBullet();
        }

    }

    public void AimStart()
    {
        if (!isDead)
        {
            //navMeshAgent.enabled = false;
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

    void SendBullet()
    {
        GameObject _smallBullet;

        if (!rpgSoldier)
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("AmmoTrail", transform.position, transform.rotation);

            if (_smallBullet.TryGetComponent(out BulletController bulletController))
                bulletController.target = targetEnemy.transform;

            _smallBullet.transform.SetParent(bulletSpawnPos);
        }
        else
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("RocketTrail", transform.position, transform.rotation);

            if (_smallBullet.TryGetComponent(out BulletController bulletController))
                bulletController.target = targetEnemy.transform;

            _smallBullet.transform.SetParent(bulletSpawnPos);
        }
    }

    void DeathEvent()
    {
        isDead = true;
        //colBase.enabled = false;
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
        PuppetMaster _puppetMaster = GetComponentInChildren<PuppetMaster>();
        _puppetMaster.state = PuppetMaster.State.Dead;
    }
}