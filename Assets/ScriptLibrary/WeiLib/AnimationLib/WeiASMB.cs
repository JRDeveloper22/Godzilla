using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public class WeiASMB : StateMachineBehaviour
{
    //For internal bool value change
    public BoolValues[] boolValus;

    public FloatValue[] enterFloatValus;
    public FloatValue[] exitFloatValus;

    /// <summary>
    /// Block mask did almost the same thing as CallBack function.
    /// </summary>
    [Header("Enter Mask")]
    public bool inverseEnter;
    public MaskTypes[] enterBlockMasks;
    int enterBlockMaskValue;

    [Header("Exit Mask")]
    public bool inverseExit;
    public MaskTypes[] exitBlockMasks;
    int exitBlockMaskValue;

    [Header("CallBack to PlayerController")]
    public PlayerController.CallBackFuncType[] enterCallbacks;
    public delegate void EnterDel(PlayerController _p);
    List<EnterDel> enterDels = new List<EnterDel>();

    public PlayerController.CallBackFuncType[] exitCallbacks;
    public delegate void ExitDel(PlayerController _p);
    List<ExitDel> exitDels = new List<ExitDel>();

    PlayerController p;

    private void OnEnable()
    {
        enterBlockMaskValue = 0x0000000;
        foreach (MaskTypes m in enterBlockMasks) { enterBlockMaskValue |= (int)m; }
        if (inverseEnter) enterBlockMaskValue ^= 0xfffffff;

        exitBlockMaskValue = 0x0000000;
        foreach (MaskTypes m in exitBlockMasks) { exitBlockMaskValue |= (int)m; }
        if (inverseExit) exitBlockMaskValue ^= 0xfffffff;

        //Get all callBack Method info for Enter
        foreach (PlayerController.CallBackFuncType type in enterCallbacks)
        {
            enterDels.Add((EnterDel)Delegate.CreateDelegate(typeof(EnterDel), null, typeof(PlayerController).GetMethod(type.ToString(), BindingFlags.Public | BindingFlags.Instance)));
        }
        //Get all callBack Method info for Exit
        foreach (PlayerController.CallBackFuncType type in exitCallbacks)
        {
            exitDels.Add((ExitDel)Delegate.CreateDelegate(typeof(ExitDel), null, typeof(PlayerController).GetMethod(type.ToString(), BindingFlags.Public | BindingFlags.Instance)));
        }
    }

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (p != null){
            p.ResetAnimationBlockMask(enterBlockMaskValue);
            foreach (EnterDel de in enterDels){de(p);}  
        }else{
            p = animator.transform.GetComponent<PlayerController>();
            if (!p){
                Debug.LogError("No PlayerController attach to this animator's parent GameObject");
            }else{
                p.ResetAnimationBlockMask(enterBlockMaskValue);
                foreach (EnterDel de in enterDels){de(p);}
            }
        }

        foreach (BoolValues b in boolValus){
            animator.SetBool(b.boolName, b.enterStatu);
        }
        foreach (FloatValue f in enterFloatValus)
        {
            animator.SetFloat(f.floatName, f.value);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (p != null){
            p.ResetAnimationBlockMask(exitBlockMaskValue);
            foreach (ExitDel de in exitDels){de(p);}
        }else{
            p = animator.transform.GetComponent<PlayerController>(); 
            if (!p){
                Debug.LogError("No PlayerController attach to this animator's parent GameObject");
            }else{
                p.ResetAnimationBlockMask(exitBlockMaskValue);
                foreach (ExitDel de in exitDels){de(p);}
            }
        }

        foreach (BoolValues b in boolValus)
        {
            if (b.resetOnExit) { animator.SetBool(b.boolName, !b.enterStatu); }
        }
        foreach (FloatValue f in exitFloatValus)
        {
            animator.SetFloat(f.floatName, f.value);
        }
    }

    [System.Serializable]
    public struct BoolValues
    {
        public string boolName;
        public bool enterStatu;
        public bool resetOnExit;
    }
    [System.Serializable]
    public struct FloatValue
    {
        public string floatName;
        public float value;
    }

    [Flags]
    public enum MaskTypes
    {
        blockJump = 0x1,
        blockMovement = 0x2,
        blockAttack = 0x4,
    }
}