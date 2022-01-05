using Dreamteck.Splines;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField]
    //PlayerSettings settings;
    //public FloatReference evolveCollectableCount;

    Rigidbody rigidbody;
    public Animator animator;

    private void Awake()
    {
        //if (settings == null)
        //    Debug.LogError("You need a settings for use this.");

        rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void OnEnable()
    {
        //transform.position = Vector3.zero;
    }

    public void StageChange(string _stage)
    {
        Debug.Log(_stage);
        if (_stage != "")
        {
            animator.SetTrigger(_stage);
        }
    }

    public void StartGame()
    {
        rigidbody.isKinematic = false;
        if (animator != null)
            animator.SetFloat("yDir", 1f);
    }
}
