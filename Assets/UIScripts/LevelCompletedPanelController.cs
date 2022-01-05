using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelCompletedPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;

    private void OnEnable() {
        int currentLevel = PlayerPrefs.GetInt("ElephantLevel") - 1;
        levelText.GetComponent<TextMeshProUGUI>().text = "Level " + currentLevel;
    }
}
