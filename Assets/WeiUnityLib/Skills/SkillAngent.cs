using UnityEngine;
using System.Collections;


public class SkillAngent : MonoBehaviour {

    public enum SKILLTYPE {FireBall,IceBall,Flash,Laser,Heal};

    public SKILLTYPE[] skillTypes;
    ISkill[] skills; // this could be array or list or what ever.

    public string tagIgnore;

    [HideInInspector]
    public bool canUseNextSkill = true; //check if we can use a new skill. for some skills when we are OnSkillStay , we still can use another skills. Like Kayle's ultimate.

    [HideInInspector]
    public Transform eye;

    public bool useUIEvent = false;

    public delegate void SkillEventHander(object sender, MyEventSystem.SkillEventArgs e);
    public event SkillEventHander raiseSkillStay;
    public event SkillEventHander raiseSkillExit;

    // Use this for initialization
    void Awake () {
        skills = new ISkill[skillTypes.Length];
        for (int i = 0; i < skillTypes.Length; i++) {          
            skills[i] = InitializeSkills(skillTypes[i]);
            skills[i].SkillObjectSetUp(i);
        }
        eye = gameObject.transform.GetChild(0);
	}

    void Update() {
        for (int i = 0; i < skills.Length; i++) {
            skills[i].UpdateSkill();
        }
    }

	public void OnSkillEnter(int index) {
		skills[index].OnSkillEnter();
	}

    public void DisableForSecond(float t)
    {


    }

    public void OnSkillStay_UIEvent(MyEventSystem.SkillEventArgs e) {
        raiseSkillStay(this, e);
    }
    public void OnSkillExit_UIEvent(MyEventSystem.SkillEventArgs e) {
        raiseSkillExit(this, e);
    }

    ISkill InitializeSkills(SKILLTYPE type) {
        //if (type == SKILLTYPE.FireBall) {
        //    return new FireBall(this);
        //}
        //if (type == SKILLTYPE.IceBall) {
        //    return new IceBall(this);
        //}
        //if (type == SKILLTYPE.Flash) {
        //    return new Flash(this);
        //}
        //if (type == SKILLTYPE.Laser)
        //{
        //    return new MeteorSwarm(this);

        //}
        //if (type == SKILLTYPE.Heal)
        //{
        //    return new Heal(this);

        //}
        return null;
    }
}
