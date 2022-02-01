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
            animator.SetLayerWeight(1, 1);
            animWalk = true;
        }
        if (navMeshAgent.enabled)
        {
            navMeshAgent.destination = targetTransform.position;
        }
        StartCoroutine(CanvasInd());
        StartCoroutine(GoToNewPosition());
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

    public GameObject lastHittedBullet;
    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("BulletPlayer"))
        //{
        //    TakeHit(0);
        //}
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    TakeHit(0);
        //}
    }

    private void Update()
    {
        if (!LevelManager.instance.isGameRunning)
        {
            return;
        }

        if (navMeshAgent.enabled == true)
            if (navMeshAgent.isStopped)
            {
                animator.SetLayerWeight(1, 0);
                return;
            }

        if (navMeshAgent.enabled == true)
        {
            if (Vector3.Distance(navMeshAgent.destination, transform.position) < .2f)
            {
                Debug.Log("Stopped");
                navMeshAgent.isStopped = true;
            }
            else
            {
                Debug.Log(Vector3.Distance(navMeshAgent.destination, transform.position));
            }
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

            tempEnemySoldier = targetEnemy;

            yield return new WaitForSeconds(1f);
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
            _smallBullet = ObjectPool.instance.SpawnFromPool("AmmoTrail", bulletSpawnPos.transform.position, bulletSpawnPos.transform.rotation);

            if (_smallBullet.TryGetComponent(out BulletController bulletController))
                if (targetEnemy)
                    bulletController.target = targetEnemy.transform;

            _smallBullet.transform.SetParent(bulletSpawnPos);
        }
        else
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("RocketTrail", bulletSpawnPos.transform.position, bulletSpawnPos.transform.rotation);

            if (_smallBullet.TryGetComponent(out BulletController bulletController))
                if (targetEnemy)
                    bulletController.target = targetEnemy.transform;

            _smallBullet.transform.SetParent(bulletSpawnPos);
        }
        Debug.Log(targetEnemy);
    }

    [SerializeField] float deadForce;
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


        if (lastHittedBullet != null)
        {
            Debug.Log(name + " soldier dead." + (transform.position - lastHittedBullet.transform.position));
            Vector3 forceVector = (new Vector3((transform.position - lastHittedBullet.transform.position).x,
                (transform.position - lastHittedBullet.transform.position).y * .2f,
                (transform.position - lastHittedBullet.transform.position).z) * 10f/* + Vector3.up * 4.5f*/);

            foreach (Rigidbody _rigidbody in GetComponentsInChildren<Rigidbody>())
            {
                _rigidbody.AddForce(forceVector * deadForce / 5f);
            }
            GetComponentInChildren<Rigidbody>().AddForce(forceVector * deadForce);
        }
    }

    bool isWalk;
    IEnumerator GoToNewPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            Debug.Log("goNextPosition");

            animator.SetTrigger("Walk");

            Vector3 nextPosition = transform.position - (transform.position - targetEnemy.transform.position) / 5f;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(nextPosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    if (navMeshAgent.enabled == true)
                    {
                        NavMeshHit objectHit;
                        if (NavMesh.SamplePosition(nextPosition, out objectHit, 1.0f, NavMesh.AllAreas))
                        {
                            navMeshAgent.SetDestination(objectHit.position);
                        }

                        //navMeshAgent.SetDestination(objectHit.position);
                    }
                }
            }
            if (navMeshAgent.enabled == true)
                navMeshAgent.isStopped = false;
        }
    }
}