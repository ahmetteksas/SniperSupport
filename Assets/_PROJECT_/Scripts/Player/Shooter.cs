using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using UnityEngine.UI;
public class Shooter : MonoBehaviour
{
    public Transform bulletSpawnPos;
    public Transform bulletDir;
    public Transform character;
    public GameObjectCollection allyList;
    private Transform targetAlly;
    public float bulletForce;
    private float shootDelay = 1.5f;
    private float shootDelayAi = 1f;
    private float shootTime = 0f;
    int selectedBulletIndex;

    void Start()
    {
        //shootDelay = shootTime - Time.deltaTime;
    }
    void Update()
    {
        if (this.gameObject.name == "AI")
        {
            List<SoldierController> allSoldiers = FindObjectsOfType<SoldierController>().ToList();
            targetAlly = allSoldiers.LastOrDefault().transform;
        }

        shootTime += Time.deltaTime;
        if (shootTime > shootDelay && this.gameObject.name == "Character")
        {
            if (Input.GetMouseButtonUp(0) /*&& !shoot*/)
            {
                Shoot();
                Animator _anim = GetComponent<Animator>();
                _anim.SetTrigger("Shoot");
            }

        }
        if (shootTime > shootDelayAi && this.gameObject.name == "AI")
        {
            if (targetAlly)
            {
                AiShoot();
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
        if (this.gameObject.name == "AI")
        {
            _smallbullet.transform.rotation = /*transform.rotation +*/ Quaternion.Euler(0, 0, 0);
        }
        _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce((targetAlly.position - transform.position) * bulletForce / 4);
        shootTime = 0f;
    }
}
