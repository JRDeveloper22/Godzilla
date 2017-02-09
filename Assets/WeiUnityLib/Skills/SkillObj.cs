using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObj : MonoBehaviour{

    public abstract void Init(SkillHolder _skill);

    public abstract void OnSkillEnter();

    public abstract void OnSkillStay();

    public abstract void OnSkillExsit();

    public abstract void OnSkillSilence();

    public abstract void OnSkillCoolDown();
}
