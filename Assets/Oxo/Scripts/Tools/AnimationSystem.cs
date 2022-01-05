using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class AnimationSystem : MonoBehaviour
{
    public Animator _animator;

    [HideInInspector]
    public bool canDoAction;
    // change to true to levelmanager
    
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    /*
    public void NewValues()
    {
        print("New Values Set");
        _animator.SetFloat("AttackFloat",Random.Range(0f,1f));
        _animator.SetFloat("JumpFloat",Random.Range(0f, 1f));
        _animator.SetFloat("SlideFloat",Random.Range(0f,1f));
        canDoAction = true;
    }

    */
    void Update()
    
    {
        if (!canDoAction)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Attack");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _animator.SetTrigger("Jump");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _animator.SetTrigger("Slide");
        }
        
        
        if (transform.position.z > 50f)
        {
            transform.position = Vector3.zero;
        }
    }
}