using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderToText : MonoBehaviour
{
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {

    }

    public void WriteText(Slider _slider)
    {
        text.text = _slider.value.ToString();
    }
}
