using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelSliderController : MonoBehaviour
{
    [SerializeField] private GameObject currentLevelText;
    [SerializeField] private GameObject nextLevelText;
    [HideInInspector] public static float maxValue;
    [HideInInspector] public static float value;
    private Image slideFiller;
    private void Awake()
    {
        slideFiller = transform.GetChild(0).GetComponent<Image>();;
    }
    private void OnEnable()
    {
        int currentLevel = PlayerPrefs.GetInt("ElephantLevel");
        currentLevelText.GetComponent<TextMeshProUGUI>().text = currentLevel.ToString();
        nextLevelText.GetComponent<TextMeshProUGUI>().text = (currentLevel + 1).ToString();
        
    }
    void Update()
    {
        if (value == 0)
            slideFiller.fillAmount = 0;
        else if (value / maxValue <= 1)
            slideFiller.fillAmount = value / maxValue;
        else
            slideFiller.fillAmount = 1;
    }
    private void OnDisable()
    {
        if (value == 0)
            slideFiller.fillAmount = 0;
    }
}
