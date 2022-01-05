using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private GameObject soundTextImageObject;
    [SerializeField] private GameObject vibrationTextImageObject;
    [SerializeField] private GameObject soundImageObject;
    [SerializeField] private GameObject vibrationImageObject;
    [SerializeField] private Sprite soundOnImage;
    [SerializeField] private Sprite soundOffImage;
    [SerializeField] private Sprite soundOnTextImage;
    [SerializeField] private Sprite soundOffTextImage;
    [SerializeField] private Sprite vibrationOnImage;
    [SerializeField] private Sprite vibrationOffImage;
    [SerializeField] private Sprite vibrationOnTextImage;
    [SerializeField] private Sprite vibrationOffTextImage;
    private Color toggleGreen = new Color(54 / 255f, 213 / 255f, 37 / 255f);
    private Color toggleGray = new Color(123 / 255f, 123 / 255f, 123 / 255f);

    private void OnEnable()
    {
        setToggleButtons();
    }

    private void setToggleButtons()
    {
        if (PlayerPrefs.GetString("Sound") == "Disabled")
        {
            soundTextImageObject.GetComponent<Image>().sprite = soundOffTextImage;
            soundImageObject.GetComponent<Image>().sprite = soundOffImage;
        }
        else
        {
            soundTextImageObject.GetComponent<Image>().sprite = soundOnTextImage;
            soundImageObject.GetComponent<Image>().sprite = soundOnImage;
        }

        if (PlayerPrefs.GetString("Vibration") == "Disabled")
        {
            vibrationTextImageObject.GetComponent<Image>().sprite = vibrationOffTextImage;
            vibrationImageObject.GetComponent<Image>().sprite = vibrationOffImage;
        }
        else
        {
            vibrationTextImageObject.GetComponent<Image>().sprite = vibrationOnTextImage;
            vibrationImageObject.GetComponent<Image>().sprite = vibrationOnImage;
        }
    }

    public void soundToggle()
    {
        if (PlayerPrefs.GetString("Sound") == "Disabled")
        {
            PlayerPrefs.SetString("Sound", "Enabled");
            soundTextImageObject.GetComponent<Image>().sprite = soundOnTextImage;
            soundImageObject.GetComponent<Image>().sprite = soundOnImage;
            AudioListener.volume = 1;
        }
        else
        {
            PlayerPrefs.SetString("Sound", "Disabled");
            soundTextImageObject.GetComponent<Image>().sprite = soundOffTextImage;
            soundImageObject.GetComponent<Image>().sprite = soundOffImage;
            AudioListener.volume = 0;
        }
        PlayerPrefs.Save();
    }

    public void vibrationToggle()
    {
        if (PlayerPrefs.GetString("Vibration") == "Disabled")
        {
            PlayerPrefs.SetString("Vibration", "Enabled");
            vibrationTextImageObject.GetComponent<Image>().sprite = vibrationOnTextImage;
            vibrationImageObject.GetComponent<Image>().sprite = vibrationOnImage;
        }
        else
        {
            PlayerPrefs.SetString("Vibration", "Disabled");
            vibrationTextImageObject.GetComponent<Image>().sprite = vibrationOffTextImage;
            vibrationImageObject.GetComponent<Image>().sprite = vibrationOffImage;
        }
        PlayerPrefs.Save();
    }
}
