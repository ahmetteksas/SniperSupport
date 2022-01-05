using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDisplay : MonoBehaviour
{

    public float blendShapness;

    Animator _animator;

    [HideInInspector]
    public bool isIdle;
    [HideInInspector]
    public bool isWalk;
    [HideInInspector]
    public bool isRun;
    [HideInInspector]
    public bool isJump;
    [HideInInspector]
    public bool isAttack;
    [HideInInspector]
    public bool isSlide;
    [HideInInspector]
    public bool isDodge;
    [HideInInspector]
    public bool isBlock;
    [HideInInspector]
    public bool isFly;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        ForwardMovement(0);
    }
    public void ForwardMovement(float speed)
    {
        AllStateBoolFalse();

        _animator.SetTrigger("Run");
        _animator.SetFloat("yDir", speed);

        if (speed == 0)
            isIdle = true;
        else if (speed < 2)
            isWalk = true;
        else if (speed == 2)
            isRun = true;
    }
    public void StartJump(float i)
    {
        AllStateBoolFalse();

        isJump = true;

        _animator.SetTrigger("Jump");
        _animator.SetFloat("JumpFloat", i);
    }
    public void StartAttack(float i)
    {
        AllStateBoolFalse();

        isAttack = true;

        _animator.SetTrigger("Attack");
        _animator.SetFloat("AttackFloat", i);
    }
    public void StartSlide(float i)
    {
        AllStateBoolFalse();

        isSlide = true;

        _animator.SetTrigger("Slide");
        _animator.SetFloat("SlideFloat", i);
    }
    #region Summary
    /// <summary>
    /// (0 = RightDodge) (0.5 = LeftDodge) (1 = FrontDodge)
    /// </summary>
    #endregion

    public void StartDodge(float i)
    {
        AllStateBoolFalse();

        isDodge = true;
        print(i);
        _animator.SetTrigger("Dodge");
        _animator.SetFloat("DodgeFloat", i);
    }

    public void StartBlock(float i)
    {
        AllStateBoolFalse();

        isBlock = true;

        _animator.SetTrigger("Block");
        _animator.SetFloat("BlockFloat", i);
    }
    public void StartFly(float i)
    {
        AllStateBoolFalse();

        isFly = true;

        _animator.SetTrigger("Fly");
        _animator.SetFloat("FlyFloat", i);
    }
    public void StartState(string name, float blendValue)
    {
        AllStateBoolFalse();

        _animator.SetTrigger(name);
        _animator.SetFloat(name + "Float", blendValue);
    }
    void AllStateBoolFalse()
    {
        isIdle = false;
        isWalk = false;
        isRun = false;
        isJump = false;
        isAttack = false;
        isSlide = false;
        isDodge = false;
        isBlock = false;
        isFly = false;
    }
}
