using UnityEngine;
using System.Collections;

public interface ISkill {

    void SkillObjectSetUp(int _index);

    void OnSkillEnter();

    void OnSkillStay();

    void OnSkillExsit();

    void OnSkillSilence();

    void OnSkillCoolDown();

    void UpdateSkill();

}
