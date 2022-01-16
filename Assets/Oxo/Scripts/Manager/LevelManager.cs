using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using Sirenix.OdinInspector;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int level=1;
    public bool isGameRunning;

    public GameObject currentLevel;

    public List<GameObject> levelPrefabsList;
    public List<GameObject> currentLevelObjectsList;

    public GameObject finishPlatform;

    public GameObject blueFog;
    public GameObject yellowFog;
    private int index;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        index = Random.Range(0, 2);
        if (index == 0)
        {
            blueFog.SetActive(true);
        }
        else
        {
            yellowFog.SetActive(true);
        }
        level = PlayerPrefs.GetInt("Level");
        SetLevel();
    }

    public void GameStarted()
    {
        isGameRunning = true;
    }

    public void SetLevel()
    {
        ResetLevel();

        currentLevel = Instantiate(levelPrefabsList[level % levelPrefabsList.Count], new Vector3(-37.58931f, -4.614986f, 52.85594f) /*Vector3.zero*/, Quaternion.identity);
        //currentLevel.transform.position = new Vector3(-37.58931f, -4.614986f, 52.85594f);
        currentLevelObjectsList.Add(currentLevel);
    }

    public void ResetLevel()
    {
        //We are deleting the objects at the current level.
        ResetAndClearList(currentLevelObjectsList);

        CanvasManager.instance.levelText.text = $"LEVEL {level + 1}";
    }

    public void ResetAndClearList(List<GameObject> list)
    {
        list.ForEach(x => Destroy(x));
        list.Clear();
    }
}