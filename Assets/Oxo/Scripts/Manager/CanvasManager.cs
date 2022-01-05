using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    [HideInInspector] public static CanvasManager instance;

    public GameObject tapToPlayButton;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;
    public GameObject tutorialRect;
    public GameObject mainMenuRect;
    public GameObject inGameRect;
    public GameObject finishRect;

    public Slider sensitivitySlider;
    public Slider rotationSensitivitySlider;
    public Slider lerpIterationCountSlider;
    public Slider movementSpeedSlider;
    public Slider rotationValueSlider;
    public Toggle smoothStop;

    public Text levelText;
    public Text coinText;
    public Text finishInfoText;

    public Image FillImage;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        mainMenuRect.SetActive(true);
    }

    public void TapToPlayButtonClick()
    {
        ObjectPool.instance.isGameRunning = true;
        tutorialRect.SetActive(false);

        //GameManager.instance.StartGame();
    }
    //Calling UI Event (Next Level Button)
    public void NextLevel()
    {
        LevelManager.instance.level++;
        PlayerPrefs.SetInt("Level", LevelManager.instance.level);

        SceneManager.LoadScene(0);
        //LevelManager.instance.SetLevel();
    }
    //Calling UI Event (Restart Level Button (Opening Level Fail))
    public void RestartGame()
    {
        
        SceneManager.LoadScene(0);
        
    }

    public void ComplateGame()
    {
        OpenFinishRect();
        finishInfoText.text = "Congratulations";
        retryLevelButton.SetActive(false);
        nextLevelButton.SetActive(true);
    }

    public void FailGame()
    {
        OpenFinishRect();
        finishInfoText.text = "Fail";
        retryLevelButton.SetActive(true);
        nextLevelButton.SetActive(false);
    }

    public void OpenFinishRect()
    {
        inGameRect.SetActive(false);
        finishRect.GetComponent<CanvasGroup>().DOFade(1, 2f);

        finishRect.SetActive(true);
    }
}