using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blizzard : SkillObj {


    public GameObject Blizzardpartical;


    [HideInInspector]
    public SkillHolder skill;

    delegate void fixedUpDateDel();
    fixedUpDateDel fiexdDel;

    delegate void upDateDel();
    upDateDel del;

    private void Update()
    {
        if (del != null)
        {
            del();
        }
    }
    private void FixedUpdate()
    {
        if (fiexdDel != null)
        {
            fiexdDel();
        }
    }

    #region FrameWork

    public override void Init(SkillHolder _skill)
    {
        skill = _skill;
        
    }

    public override void OnSkillEnter()
    {
        
    }

    public override void OnSkillStay()
    {

    }

    public override void OnSkillSilence()
    {
        throw new NotImplementedException();
    }

    public override void OnSkillCoolDown()
    {

    }

    public override void OnSkillExsit()
    {
   
    }

    #endregion


}
