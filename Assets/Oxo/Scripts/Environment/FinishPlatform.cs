using DG.Tweening;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using UnityStandardAssets.Cameras;

public class FinishPlatform : MonoBehaviour
{
    [SerializeField]
    GameEvent complateGame = default(GameEvent);

    public bool isLinearEnding;
    public List<GameObjectCollection> stickmanList;
    public int finalScore;

    private void Start()
    {
        LevelManager.instance.finishPlatform = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerRight") || other.CompareTag("PlayerLeft"))
        {
            StartCoroutine(FinalAction()); 
        }
    }

    public IEnumerator FinalAction()
    {
        yield return new WaitForSeconds(0f);
        yield break;
        foreach (var stickman in stickmanList)
        {
            if (stickmanList.Contains(stickman))
            {
                //Camera.main.transform.root.GetComponent<AutoCam>().SetTarget(stickmanList.LastOrDefault().transform);
                yield return stickman.transform.DOMoveZ(stickman.transform.position.z + 2f, 0.2f).WaitForCompletion();
            }
           
        }
        //complateGame.Raise();
        Debug.Log("LevelComplate");
        //GameManager.instance.LevelComplete();
    }
}