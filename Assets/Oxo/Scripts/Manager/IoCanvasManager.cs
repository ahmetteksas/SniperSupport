using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MoreMountains.NiceVibrations;

public class IoCanvasManager : MonoBehaviour
{
    [HideInInspector]
    public static IoCanvasManager instance;

    public List<Sprite> flagsImageList;
    public List<string> nickList; // Random Nick List For IO

    public Dropdown countryDropDown;
    public GameObject mainMenuRect;
    public GameObject startRect;
    public GameObject inGameRect;
    public GameObject finishRect;

    public GameObject tapToPlayButton;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;

    public Image winnerPlayerFlag;
    public Slider playerSlider;
    public Slider enemySlider;
    public Slider enemySlider02;
    public Slider sensivitySlider;

    public Text levelText;
    public Text coinText;
    public Text nickInputFieldText;
    public Text finishInfoText;
    public Text waitText;
    public Text sensivitySliderText;
    public Text winnerName;

    bool nickWrited;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!nickWrited) // for game first start question nickname and country
        {
            startRect.SetActive(true);
            nickWrited = true;
        }
    }

    public void NextLevel()
    {
        //LevelManager.levelManager._level++;
        //PlayerPrefs.SetInt("Level", LevelManager.levelManager._level);
        //GameManager.instance.StartGame();
    }

    public void RestartGame()
    {
        //GameManager.instance.StartGame();
    }

    public void OpenFinishRect(bool isSuccess, string playerName, Image playerImage)
    {
        if(isSuccess)
        {
            retryLevelButton.SetActive(false);
            nextLevelButton.SetActive(true);
        }    
        else
        {
            retryLevelButton.SetActive(false);
            nextLevelButton.SetActive(true);
        }

        finishInfoText.text = "Winner";
        winnerName.text = playerName;
        winnerPlayerFlag.sprite = playerImage.sprite;

        inGameRect.SetActive(false);
        finishRect.GetComponent<CanvasGroup>().DOFade(1, 2f);

        finishRect.SetActive(true);
    }


    public IEnumerator WaitingForOppenent()
    {
        //GameManager.instance.waitIOPlayer = true; // For wait player input
        waitText.gameObject.SetActive(true);
        for (int i = 0; i < Random.Range(1, 5); i++)
        {
            yield return new WaitForFixedUpdate();
            waitText.text = "Waiting For Oppenent.";
            yield return new WaitForSeconds(.3f);
            waitText.text = "Waiting For Oppenent..";
            yield return new WaitForSeconds(.3f);
            waitText.text = "Waiting For Oppenent...";
            yield return new WaitForSeconds(.3f);

        }

        yield return StartCoroutine(TimeCounter());

        //GameManager.instance.waitIOPlayer = false;
        yield return new WaitForSeconds(.5f);
        waitText.gameObject.SetActive(false);
    }

    IEnumerator TimeCounter()
    {
        int counter = 3;

        while (counter > 0)
        {
            waitText.text = counter.ToString();
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            yield return new WaitForSeconds(.5f);
            counter--;

        }

        yield return new WaitForSeconds(.5f);

        waitText.text = "GO !";
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);

    }
}
