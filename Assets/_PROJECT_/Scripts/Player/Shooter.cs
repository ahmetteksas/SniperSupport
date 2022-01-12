using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
public class Shooter : MonoBehaviour
{
    public bool isAI;
    public Transform bulletSpawnPos;
    public Transform bulletDir;
    public Transform character;
    public GameObjectCollection allyList;
    private Transform targetAlly;
    public float bulletForce;
    private float shootDelay = .5f;
    private float shootDelayAi = 1f;
    private float shootTime = 0f;
    int selectedBulletIndex;

    public GameObject cross;

    void Start()
    {
        //shootDelay = shootTime - Time.deltaTime;
    }

    void Update()
    {
        if (!LevelManager.instance.isGameRunning)
        {
            return;
        }

        shootTime += Time.deltaTime;

        if (isAI)
        {
            List<SoldierController> allSoldiers = FindObjectsOfType<SoldierController>().ToList();
            targetAlly = allSoldiers.LastOrDefault().transform;

            if (shootTime > shootDelayAi)
            {
                if (targetAlly)
                {
                    AiShoot();
                }
            }
        }
        else
        {
            if (shootTime > shootDelay)
            {
                if (Input.GetMouseButton(0) /*&& !shoot*/)
                {
                    if (!cross.activeSelf)
                    {
                        cross.SetActive(true);
                        Camera.main.DOPause();
                        Camera.main.DOFieldOfView(95, .2f);
                        Camera.main.transform.DOLocalMove(Vector3.forward * 2f, .4f);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (cross.activeSelf)
                {
                    cross.SetActive(false);
                    Camera.main.DOPause();
                    Camera.main.transform.DOLocalMove(Vector3.zero, .1f);
                    Camera.main.DOFieldOfView(80, .1f);
                    Shoot();
                    Animator _anim = GetComponent<Animator>();
                    _anim.SetTrigger("Shoot");
                }
            }
        }
    }
    public void SelectBullet(int index)
    {
        selectedBulletIndex = index;

    }

    public void Shoot()
    {
        GameObject _smallBullet;
        //var ray = /*GetComponentInChildren<Image>().transform.localPosition*/Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit, 100))
        //{
        //    Debug.DrawLine(character.position, hit.point);
        if (selectedBulletIndex == 0)
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
        }
        else
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("BulletHeal", bulletSpawnPos.position, Quaternion.identity);
        }
        //_smallBullet.transform.LookAt(hit.point);
        //Debug.Log("asd");
        //_smallBullet.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce);
        _smallBullet.gameObject.GetComponent<Rigidbody>().AddForce(bulletSpawnPos.transform.forward * bulletForce);
        shootTime = 0f;
        //}
    }

    public void AiShoot()
    {
        GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
        if (gameObject.name == "AI")
        {
            _smallbullet.transform.rotation = /*transform.rotation +*/ Quaternion.Euler(0, 0, 0);
        }
        _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce((targetAlly.position - transform.position) * bulletForce / 4);
        shootTime = 0f;
    }

}
