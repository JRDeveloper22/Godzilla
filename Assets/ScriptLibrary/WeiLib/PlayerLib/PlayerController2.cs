using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


//This part of PlayerController only deal with the detail of animation
public partial class PlayerController
{

    [HideInInspector]
    public bool onAir;

    float privewY;
    float currentY;
    public bool swipingKeyDown;
    public bool punchKeyDown;
    public Transform hipTransform;

    public int animationBlockMask;

    public float jumpForceDelayTime = 0.4f;
    public float throwBuildingDelayTime = 1.05f;

    partial void UpdateAnimationSmooth()
    {
        //Set Animation information
        SetbasicMoveAnimation();
        SetSpecialAnimation();    
    }

    #region SetAnimationInfo

    //  We do not set any flag when we assign a value to animator
    //Instead we use animation state machine Enter and Exit to call back, and 
    //Set all flag values.
    void SetbasicMoveAnimation()
    {
        float animationSpeedPercent = ((isRunning) ? 1 : .5f) * moveInput.magnitude;
        if(!blockMovement)
            animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void SetSpecialAnimation()
    {
        if (jumpKeyDown & !blockJumpAnimation)
        {
            animator.SetBool("jump", jumpKeyDown);
        }
        if (swipingKeyDown & !blockAttackAnimation)
        {
            animator.SetBool("swiping", swipingKeyDown);
        }
        if(punchKeyDown & !blockAttackAnimation)
        {
            animator.SetBool("punch", punchKeyDown);
        }
      
    }

    #endregion

    /// <summary>
    ///     When we enter a animation state or exit a animation state we callback those 
    /// function to set or reset some specific values.
    /// </summary>
    #region AnimationCallBackFunction

    public bool throwBuilding = false;

    public enum CallBackFuncType
    {
        CallBackFunc1,
        CallBackFunc2,
        PunchGorge,
        JumpAnimationEnter,
        JumpAnimationExit,
        SwipingEnter,
        PunchEnter,
    }

    static bool initialCallbackMethod=false;
    static List<MethodInfo> methodInfos = new List<MethodInfo>();
    public static List<MethodInfo> CreateCallBackInstance()
    {
        if (!initialCallbackMethod)
        {
            List<CallBackFuncType> callBackFunctypes = GetListFromEnum<CallBackFuncType>();
            foreach (CallBackFuncType type in callBackFunctypes)
            {
                methodInfos.Add(typeof(PlayerController).GetMethod(type.ToString(), BindingFlags.Public | BindingFlags.Instance));
            }
            initialCallbackMethod = true;
            return methodInfos;
        }
        else {
            return methodInfos;
        }
    }
    //CallBack functions
    public void CallBackFunc1()
    {
        Debug.Log("Call Func 1");
    }
    public void CallBackFunc2()
    {
        Debug.Log("Call Func 2");
    }
    public void PunchGorge()
    {
        Debug.Log("PunchGorge get called");
    }
    public void JumpAnimationEnter()
    {
        Invoke("ApplayJumpForce", jumpForceDelayTime);
    }
    public void JumpAnimationExit()
    {

    }
    public void SwipingEnter()
    {
        Invoke("ThrowBuilding", throwBuildingDelayTime);
    }
    public void PunchEnter()
    {
        WeiAudioManager.instance.PlaySound2D("playerSound");
    }
    //Internal callBack sub Functions
    void ApplayJumpForce()
    {
        rg.AddForce(Vector3.up * rgJumpForce + transform.forward * rgJumpForce / 2);
    }
    void ThrowBuilding()
    {
        WeiAudioManager.instance.PlaySound2D("Impact");
        throwBuilding = true;
    }
    #endregion

    #region AnimationBlockMask

    bool blockAttackAnimation = false;
    bool blockJumpAnimation = false;
    public void ResetAnimationBlockMask(int mask)
    {
        blockMovement = (mask & (int)WeiASMB.MaskTypes.blockMovement) == 0 ? false : true;
        blockAttackAnimation = (mask & (int)WeiASMB.MaskTypes.blockAttack) == 0 ? false : true;
        blockJumpAnimation = (mask & (int)WeiASMB.MaskTypes.blockJump) == 0 ? false : true;
    }
    #endregion

    #region StaticHelperFuncs
    static List<T> GetListFromEnum<T>()
    {
        List<T> enumList = new List<T>();
        System.Array enums = System.Enum.GetValues(typeof(T));
        foreach (T e in enums)
        {
            enumList.Add(e);
        }
        return enumList;
    }
    #endregion
}