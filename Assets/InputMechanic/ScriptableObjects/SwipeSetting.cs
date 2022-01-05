using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Swipe Setting", menuName = "OXO Games/Mechanics/Swipe Setting", order = 0)]

public class SwipeSetting : ScriptableObject
{
    [Header("Swipe Constants")]
    [SerializeField]
    public bool canMultiSwipe;
    [SerializeField]
    public float swipeDetectDistance;
    bool isSwipeDone;
    public int currentIndex;
    public float sensivity = 0.008f;
    
}
