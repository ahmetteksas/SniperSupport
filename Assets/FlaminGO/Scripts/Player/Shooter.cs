using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class Shooter : MonoBehaviour
{
    Camera mainCamera;
    Animator animator;

    public bool isAI;
    public Transform bulletSpawnPos;
    public Transform bulletDir;
    public Transform character;
    //public GameObjectCollection allyList;
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


    public List<SoldierController> allyList = new List<SoldierController>();
    public List<SoldierController> enemyList = new List<SoldierController>();

    //private Animator animBase;
    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        allyList = FindObjectsOfType<SoldierController>().Where(x => x.teamIndex == 0).ToList();
        enemyList = FindObjectsOfType<SoldierController>().Where(x => x.teamIndex == 1).ToList();
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsMouseOverUIWithIgnores()
    {
        PointerEventData _pointerEventData = new PointerEventData(EventSystem.current);
        _pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_pointerEventData, raycastResults);
        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<OnClickButton>() != null) 
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }

    public Coroutine swayingCoroutine;
    public float swayingDelay;

    IEnumerator Swaying()
    {
        while (true)
        {
            yield return mainCamera.transform.DOLocalRotate(Vector3.up * Random.Range(-.7f, .7f) + Vector3.right * Random.Range(-.5f, .5f), swayingDelay)
                /*.SetLoops(0, LoopType.Yoyo)*/.SetEase(Ease.Linear).WaitForCompletion();
        }
    }

    IEnumerator ScopeZoomIn()
    {
        if (shooted)
            yield break;
        foreach (SoldierController soldier in FindObjectsOfType<SoldierController>())
        {
            soldier.GetComponentInChildren<Canvas>().transform.DOScale(Vector3.one * 0.008f, .4f);
        }

        shooted = true;
        sniper.transform.DOPause();
        sniper.transform.SetParent(secondSniperPos);
        sniper.transform.DOLocalRotate(Vector3.zero, 0.2f);
        yield return sniper.transform.DOLocalMove(Vector3.zero, 0.3f).WaitForCompletion();
        mainCamera.transform.DOLocalMove(Vector3.forward * scopeOffset, 0.1f)/*.SetEase(Ease.Linear)*/;
        mainCamera.DOPause();
        mainCamera.DOFieldOfView(scopeZoom, 0.3f);
        cross.SetActive(true);
        if (swayingCoroutine == null)
            swayingCoroutine = StartCoroutine(Swaying());
    }

    IEnumerator ScopeZoomOut()
    {
        if (!cross.activeInHierarchy)
            yield break;
        sniper.transform.DOPause();
        sniper.transform.SetParent(firstSniperPos);
        sniper.transform.DOLocalRotate(Vector3.zero, 0f);
        sniper.transform.DOLocalMove(Vector3.zero, 0f);
        StopCoroutine(swayingCoroutine);
        mainCamera.transform.DOPause();
        mainCamera.transform.DOLocalMove(Vector3.zero, .5f);
        yield return mainCamera.transform.DOLocalRotate(Vector3.left * 2.4f, .21f).WaitForCompletion();
        mainCamera.transform.DOLocalRotate(Vector3.zero, 2f).WaitForCompletion();
        swayingCoroutine = null;
        foreach (SoldierController soldier in FindObjectsOfType<SoldierController>())
        {
            soldier.GetComponentInChildren<Canvas>().transform.DOScale(Vector3.one * 0.01f, .4f);
        }
        shooted = false;
        sniper.transform.DOPause();
        sniper.transform.SetParent(firstSniperPos);
        yield return sniper.transform.DOLocalMove(Vector3.zero, scopeZoomOutDelay).WaitForCompletion();
        cross.SetActive(false);
        mainCamera.transform.DOLocalMove(Vector3.zero, .1f);
        mainCamera.DOFieldOfView(80, .1f);

        animator.SetTrigger("Reload");
        scopeZoomOut = null;
        yield return new WaitForSeconds(2f);
    }
    bool stopped;
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
            #region GameEndCheck

            if (enemyList.Count != 0)
            {
                if (enemyList.Where(x => !x.isDead).Count() == 0)
                {
                    if (CanvasManager.instance.retryLevelButton != null)
                    {

                        Debug.Log("win the game");
                        GameEventManager.Instance.complateGame.Raise();
                    }
                }

                if (allyList.Where(x => !x.isDead).Count() == 0)
                {
                    if (CanvasManager.instance.nextLevelButton != null)
                    {
                        Debug.Log("lost the game");

                        GameEventManager.Instance.failGame.Raise();

                    }
                }
            }
            #endregion

            if (shootTime > shootDelay)
            {
                if (Input.GetMouseButton(0) && !IsMouseOverUIWithIgnores() /*&& IsMouseOverUi()*//*&& !shoot*/)
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
        GameObject _hsImpact;
        GameObject _groundImpact;
        if (selectedBulletIndex == 0)
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("PlayerBullet", bulletSpawnPos.position, Quaternion.identity);
        }
        else
        {
            _smallBullet = ObjectPool.instance.SpawnFromPool("BulletHeal", bulletSpawnPos.position, Quaternion.identity);
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, (Screen.height / 2) + 2f, Mathf.Infinity));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Transform objectHit = hit.transform;
            Debug.Log(objectHit.name);
            if (hit.collider.CompareTag("Head"))
            {
                _hsImpact = ObjectPool.instance.SpawnFromPool("HSImpact", hit.collider.gameObject.transform.position, Quaternion.identity);
                _hsImpact.SetActive(true);
                headShot.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                headShot.SetActive(true);
            }
            else if (hit.collider.name != "Ground")
            {
                headShot.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                headShot.SetActive(true);
            }
            else if (hit.collider.name == "Ground")
            {
                _groundImpact = ObjectPool.instance.SpawnFromPool("GroundImpact", /*hit.collider*/hit.transform.position, Quaternion.identity);
                _groundImpact.SetActive(true);
            }
            if (_smallBullet.TryGetComponent(out HealthBulletController healBullet))
            {
                healBullet.target = objectHit;
            }
            if (_smallBullet.TryGetComponent(out BulletController bullet))
            {
                bullet.target = objectHit;
            }
            _smallBullet.gameObject.GetComponent<Rigidbody>().AddForce((objectHit.transform.position - _smallBullet.transform.position).normalized * bulletForce);

        }
        shootTime = 0f;
    }


    public void FinishGame()
    {
        cross.SetActive(false);
    }


}
