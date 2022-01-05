using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;
//using ElephantSDK;
public class ButtonHandler : MonoBehaviour
{
    private GameObject lastPanel;
    public static ButtonHandler instance = null;
    public static bool firstStart = true;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelCompletedPanel;
    //Add progression images to this array if you need to use progress slider in game.
    [SerializeField] private GameObject progressSlider;
    [SerializeField] private Sprite[] progressSliderImages;

    public bool dontDestroyOnLoad = true;
    public bool passLevel = true;
    public bool activateCollectable;
    public string collectableNameInPlayerPrefs;
    public string pointNameInPlayerPrefs;
    public bool showTapToStartAtEachLevel;
    public bool elephantAdded = false;
    public bool openLevelSlider = false;
    public bool openProgressSlider = false;



    /* Kılavuz:
     *
     * 1- Elephant eklendiyse yoruma alınmış olan elephant satırlarını kaldıralım (en üstte bulunan "using ElephantSDK" satırı unutulabilir) public olan elephantAdded variable'ini true yapalım.
     * 2- Vibration paketini mutlaka oyunlara kuralım. Kurulduktan sonra Start methodunda yer alan Vibration.init kodunun yorumunu kaldıralım ve aşağıda bulunan diğer vibration ile ilgili kısımların yorumlarını kaldıralım. 
     * 3- Oyun içinde bir progress varsa örneğin 3 levelda 1 yeni bir karakter geliyorsa bunların resimlerini progress slider images arrayi içine sürükleyip openProgressSlider değişkenini true yapalım.
     * 
     * 
     * Store'a yüklemeden önce kontrol edilecekler:
     * 1- Build Settingsten oyun portrait olarak ayarlandı mı?
     * 2- Facebook SDK kuruldu mu? Facebook id ayarlandı mı?
     * 3- Elephant SDK kuruldu mu? Elephant idler ayarlandı mı?
     * 4- Vibrationlar eklendi mi?
     * 
     */




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Application.targetFrameRate = 60;
        ApplySettings();
        InitializePlayerPrefs();
        if(passLevel)
        {
            int newSceneIndex = 0;

            if(elephantAdded)
                newSceneIndex = PlayerPrefs.GetInt("Level") % SceneManager.sceneCountInBuildSettings;
            else
                newSceneIndex = (PlayerPrefs.GetInt("Level") - 1) % SceneManager.sceneCountInBuildSettings;

            if (SceneManager.GetActiveScene().buildIndex != newSceneIndex)
            {
                SceneManager.LoadScene(newSceneIndex, LoadSceneMode.Single);
            }
        }
    }

    //Application Methods
    public void Start()
    {
        
        if (passLevel && (firstStart || showTapToStartAtEachLevel))
            startPanel.SetActive(true);
        else
            gamePlayPanel.SetActive(true);

        gamePlayPanel.GetComponent<GamePlayPanelController>().AdjustLevelSlider(openLevelSlider);
    }



    //REMOVE THESE COMMENTS IF VIBRATION MASTER PACKAGE ALREADY ADDED

    /*
    public void VibratePop()
    {
        if (PlayerPrefs.GetString("Vibration") == "Enabled")
            Vibration.VibratePop();
    }
    public void Vibrate()
    {
        if (PlayerPrefs.GetString("Vibration") == "Enabled")
            Vibration.Vibrate();
    }
    public void VibratePeek()
    {
        if (PlayerPrefs.GetString("Vibration") == "Enabled")
            Vibration.VibratePeek();
    }
    public void VibrateNope()
    {
        if (PlayerPrefs.GetString("Vibration") == "Enabled")
            Vibration.VibrateNope();
    }
    */



    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if(Time.timeScale != 0)
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if(!levelCompletedPanel.activeInHierarchy && !startPanel.activeInHierarchy)
        {
            gamePlayPanel.SetActive(false);
            pausePanel.SetActive(true);
        }

    }


    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }

    public void StartGame()
    {
        GamePlayPanelController.collectedCollectableInLevel = 0;
        if (passLevel)
        {
            if (firstStart)
            {
                firstStart = false;
            }
            else
            {

                int newSceneIndex = 0;

                if (elephantAdded)
                    newSceneIndex = PlayerPrefs.GetInt("Level") % SceneManager.sceneCountInBuildSettings;
                else
                    newSceneIndex = (PlayerPrefs.GetInt("Level") -1) % SceneManager.sceneCountInBuildSettings;


                if (SceneManager.GetActiveScene().buildIndex != newSceneIndex)
                {
                    SceneManager.LoadScene(newSceneIndex, LoadSceneMode.Single);
                }
            }

            //Elephant.LevelStarted(PlayerPrefs.GetInt("ElephantLevel"));
            gamePlayPanel.SetActive(true);
            startPanel.SetActive(false);
            levelCompletedPanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        GamePlayPanelController.collectedCollectableInLevel = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamePlayPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        lastPanel = EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject;
        lastPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void Back()
    {
        EventSystem.current.currentSelectedGameObject.transform.parent.transform.parent.gameObject.SetActive(false);
        lastPanel.SetActive(true);
    }

    public void OpenInstagramPage()
    {
        //Instagram Page
        Application.OpenURL("instagram://user?username=lostpandagames");
    }

    //Game over ve Level Finished Methodları
    public void LevelCompleted()
    {
        if (passLevel)
        {
            //Elephant.LevelCompleted(PlayerPrefs.GetInt("ElephantLevel"));
            LevelSliderController.value = 0;
            gamePlayPanel.SetActive(false);

            if (openProgressSlider)
                FillProgressSlider(3);
            else
                progressSlider.SetActive(false);
            
            if (elephantAdded)
            {
                if ((PlayerPrefs.GetInt("Level") + 1) % SceneManager.sceneCountInBuildSettings == 0)
                {
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 2);
                }
                else
                {
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                }
            }
            else
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            }

            PlayerPrefs.SetInt("ElephantLevel", PlayerPrefs.GetInt("ElephantLevel") + 1);
            PlayerPrefs.SetInt(collectableNameInPlayerPrefs, PlayerPrefs.GetInt(collectableNameInPlayerPrefs) + GamePlayPanelController.collectedCollectableInLevel);
            levelCompletedPanel.SetActive(true);
            PlayerPrefs.Save();
        }       
    }



    //Progress slider üzerine yerleştiren resimlerin doluşunu görselize etmek için kullanılan method
    //perLevel parametresi kaç levelda bir slider dolacağını gösterir

    public void FillProgressSlider(float perLevel)
    {
        int currentLevel = PlayerPrefs.GetInt("ElephantLevel");
        int spriteIndex = PlayerPrefs.GetInt("CurrentCharacter");

        float fillPercentPerLevel = 1f / perLevel;
        float fillAmount = (currentLevel % perLevel) * fillPercentPerLevel;

        if (fillAmount == 0)
            fillAmount = 1;

        if (spriteIndex >= progressSliderImages.Length)
        {
            progressSlider.SetActive(false);
        }
        else
        {
            progressSlider.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = fillAmount - fillPercentPerLevel;
            progressSlider.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = progressSliderImages[spriteIndex];

            if (fillAmount < 1)
                progressSlider.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "%" + ((int)(fillAmount * 100)).ToString();
            else
            {
                progressSlider.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "UNLOCKED";
                PlayerPrefs.SetInt("CurrentCharacter", PlayerPrefs.GetInt("CurrentCharacter") + 1);
            }

            StartCoroutine(FillImage(fillAmount));
        }
    }


    IEnumerator FillImage(float targetFill)
    {
        float currentFill = progressSlider.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount;
        while (currentFill < targetFill)
        {
            currentFill += 0.01f;
            progressSlider.transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = currentFill;
            yield return new WaitForSeconds(0.025f);
        }
    }

    public void GameOver()
    {
        
        //Elephant.LevelFailed(PlayerPrefs.GetInt("ElephantLevel"));
        LevelSliderController.value = 0;
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
    }

    //Helper Methods
    private void ApplySettings()
    {
        if(PlayerPrefs.GetString("Sound") == "Disabled")
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    private void InitializePlayerPrefs()
    {
        if (PlayerPrefs.GetInt("PlayerPrefsInitialized") == 0)
        {
            //PlayerPrefs
            PlayerPrefs.SetInt(collectableNameInPlayerPrefs, 0);
            PlayerPrefs.SetInt(pointNameInPlayerPrefs, 0);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("ElephantLevel", 1);
            PlayerPrefs.SetString("Vibration", "Enabled");
            PlayerPrefs.SetInt("HighestScore", 0);

            //PlayerPrefsCreated 1 means they were created
            PlayerPrefs.SetInt("PlayerPrefsInitialized", 1);
            PlayerPrefs.Save();
        }
    }


}
