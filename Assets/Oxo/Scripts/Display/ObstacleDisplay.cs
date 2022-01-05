using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDisplay : MonoBehaviour
{
    PlayerDisplay _playerDisplay;
    public GameObject spawnPositions;

    public List<GameObject> spawnObjectPrefabList;
    public List<GameObject> obstacleObjectList;

    public GameObject obstacleStartPos;
    public GameObject obstacleEndPos;
    public GameObject basePlatform;

    public float spawnDetectDistance;

    public bool directOpenObject;
    public bool randomSpawn;
    public bool allObjectOpen;

    void Start()
    {
        SpawnObject();
        // _playerDisplay = LevelManager.instance.player.GetComponent<PlayerDisplay>();
    }

    void Update()
    {

        if (_playerDisplay == null)
            return;

        if (!directOpenObject && Vector3.Distance(transform.position, _playerDisplay.transform.position) < spawnDetectDistance && !allObjectOpen)
        {
            OpenMyObjects();
        }

    }

    void SpawnObject()
    {
        if (spawnObjectPrefabList.Count == 0)
            return;


        if (randomSpawn)
            foreach (Transform item in spawnPositions.transform)
            {
                if (item.childCount == 0)
                {
                    GameObject go = Instantiate(spawnObjectPrefabList[Random.Range(0, spawnObjectPrefabList.Count)]);
                    go.transform.SetParent(item.transform);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.eulerAngles = item.eulerAngles;
                    obstacleObjectList.Add(go);
                    go.SetActive(true);
                }

            }
        else
        {
            for (int i = 0; i < spawnObjectPrefabList.Count; i++)
            {
                GameObject go = Instantiate(spawnObjectPrefabList[i]);
                go.transform.SetParent(spawnPositions.transform.GetChild(i));
                go.transform.localPosition = Vector3.zero;
                go.transform.eulerAngles = spawnPositions.transform.GetChild(i).eulerAngles;
                obstacleObjectList.Add(go);
                go.SetActive(true);
            }

        }

        if (!directOpenObject)
            CloseMyObjects();
    }

    void CloseMyObjects()
    {
        obstacleObjectList.ForEach(x => x.SetActive(false));
    }

    void OpenMyObjects()
    {
        allObjectOpen = true;
        obstacleObjectList.ForEach(x => x.SetActive(true));
    }
}
