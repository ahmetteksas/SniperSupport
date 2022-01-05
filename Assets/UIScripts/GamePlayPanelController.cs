using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GamePlayPanelController : MonoBehaviour
{
    private ButtonHandler buttonHandler;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject levelSlider;
    
    private GameObject collectable;
    private GameObject collectableText;
    public static int collectedCollectableInLevel;

    private void Start()
    {
        collectedCollectableInLevel = 0;
    }

    private void OnEnable()
    {
        collectable = transform.GetChild(0).GetChild(0).gameObject;
        collectableText = collectable.transform.GetChild(0).gameObject;
        buttonHandler = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<ButtonHandler>();
        Time.timeScale = 1;
        if (buttonHandler.activateCollectable)
        {
            collectable.SetActive(true);
            setCollectableText();
        }
        else
        {
            collectable.SetActive(false);
        }
        if (!buttonHandler.passLevel)
        {
            retryButton.SetActive(true);
        }
    }

    private void setCollectableText()
    {
        collectableText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt(buttonHandler.collectableNameInPlayerPrefs).ToString();
    }

    public void AdjustLevelSlider(bool open)
    {
        if (open)
            levelSlider.SetActive(true);
        else
            levelSlider.SetActive(false);
    }
}
