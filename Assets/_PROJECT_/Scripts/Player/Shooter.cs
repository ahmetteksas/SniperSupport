using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
public class Shooter : MonoBehaviour
{
    Camera mainCamera;
    Animator animator;

    public bool isAI;
    public Transform bulletSpawnPos;
    public Transform bulletDir;
    public Transform character;
    public GameObjectCollection allyList;
    private Transform targetAlly;
    public float bulletForce;
    [SerializeField] float shootDelay = 2f;
    private float shootDelayAi = 1f;
    private float shootTime = 0f;
    int selectedBulletIndex;
    public float scopeZoom = 35;
    public float scopeOffset = 3f;
    private bool shooted;

    public float scopeZoomOutDelay = .7f;
    public GameObject cross;
    public GameObject headShot;


    //private bool scopeZ
    private Coroutine scopeZoomOut;

    public GameObject sniper;
    public Transform firstSniperPos;
    public Transform secondSniperPos;
    //private Animator animBase;
    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //animBase = GetComponent<Animator>();
        //shootDelay = shootTime - Time.deltaTime;
    }

    IEnumerator ScopeZoomIn()
    {
        if (shooted)
            yield break;

        Debug.Log("Zoom In");

        shooted = true;

        Camera.main.DOPause();
        Camera.main.transform.DOLocalMove(Vector3.forward * scopeOffset / 5f, .2f);

        sniper.transform.DOPause();
        sniper.transform.SetParent(secondSniperPos);
        sniper.transform.DOLocalRotate(Vector3.zero, .2f);
        yield return sniper.transform.DOLocalMove(Vector3.zero, .2f).WaitForCompletion();

        sniper.transform.DOPause();
        sniper.transform.SetParent(firstSniperPos);
        sniper.transform.DOLocalRotate(Vector3.zero, .3f);

        sniper.transform.DOLocalMove(Vector3.zero, .3f);

        cross.SetActive(true);

        //yield return new WaitForSeconds(.2f);

        Camera.main.DOPause();
        Camera.main.DOFieldOfView(scopeZoom, .2f);
        Camera.main.transform.DOLocalMove(Vector3.forward * scopeOffset, .4f);

    }

    IEnumerator ScopeZoomOut()
    {
        if (!cross.activeInHierarchy)
            yield break;
        Debug.Log("Zoom Out");

        shooted = false;

        Transform cam = Camera.main.transform;
        cam.DOShakePosition(.3f, .6f);


        sniper.transform.DOPause();
        sniper.transform.SetParent(firstSniperPos);
        sniper.transform.DOLocalRotate(Vector3.zero, scopeZoomOutDelay);

        yield return sniper.transform.DOLocalMove(Vector3.zero, scopeZoomOutDelay).WaitForCompletion();

        //yield return new WaitForSeconds(scopeZoomOutDelay);

        cross.SetActive(false);
        //headShot.SetActive(true);
        cam.DOPause();
        cam.transform.DOLocalMove(Vector3.zero, .1f);
        Camera.main.DOFieldOfView(80, .1f);

        animator.SetTrigger("Reload");
        scopeZoomOut = null;

        MainCameraDisplay mainCameraDislay = mainCamera.GetComponent<MainCameraDisplay>();

        mainCameraDislay.SetParentNull();

        yield return new WaitForSeconds(2f);

        mainCameraDislay.SetParentDefault();

    }
    //public bool Delay()
    //{
    //    return true;
    //}
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
                    //AiShoot();
                }
            }
        }
        else
        {
            if (shootTime > shootDelay)
            {
                if (Input.GetMouseButton(0) /*&& IsMouseOverUi()*//*&& !shoot*/)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (!cross.activeSelf)
                        {
                            if (LevelManager.instance.isGameRunning)
                            {
                                StartCoroutine(ScopeZoomIn());
                            }
                        }
                    }

                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (scopeZoomOut == null)
                {
                    scopeZoomOut = StartCoroutine(ScopeZoomOut());
                }
                if (cross.activeSelf)
                {
                    Shoot();
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
            Debug.Log("HealSeçildi !!");
        }
        //_smallBullet.gameObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * bulletForce);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (Screen.height / 2) + 2f, Camera.main.transform.position.z));

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Debug.Log(hit.transform.gameObject.name);
            _smallBullet.transform.position = Vector3.MoveTowards(_smallBullet.transform.position, objectHit.position, 400f);
        }
        shootTime = 0f;
        //}
    }

    //public void AiShoot()
    //{
    //    GameObject _smallbullet = ObjectPool.instance.SpawnFromPool("BulletSmallPlayer", bulletSpawnPos.position, Quaternion.identity);
    //    if (gameObject.name == "AI")
    //    {
    //        _smallbullet.transform.rotation = /*transform.rotation +*/ Quaternion.Euler(0, 0, 0);
    //    }
    //    _smallbullet.gameObject.GetComponent<Rigidbody>().AddForce((targetAlly.position - transform.position) * bulletForce / 4);
    //    shootTime = 0f;
    //}

}
