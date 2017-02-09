using UnityEngine;
using System.Collections;


public class SkillAngent : MonoBehaviour {

    public SkillHolder[] skillHolders;

    public string tagIgnore;

    [HideInInspector]
    public bool skillBlocked = true; //check if we can use a new skill. for some skills when we are OnSkillStay , we still can use another skills. Like Kayle's ultimate.

    [HideInInspector]
    public Transform eye;

    public bool useUIEvent = false;

    public delegate void SkillEventHander(object sender, MyEventSystem.SkillEventArgs e);
    public event SkillEventHander raiseSkillStay;
    public event SkillEventHander raiseSkillExit;

    private void Start()
    {
        foreach (SkillHolder s in skillHolders)
        {
            s.Init(transform);
        }
    }

    void Update() {
        foreach (SkillHolder s in skillHolders)
        {
            if (Input.GetKeyDown(s.skillKey))
            {
                s.OnSkillEnter();
            }
        }
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
}

[System.Serializable]
public class SkillHolder
{

    public SkillObj skillObjPrefab;
    public KeyCode skillKey;
    SkillObj skillObjInstance;
    [HideInInspector]
    public Transform angentTransform;
    
    public void Init(Transform t)
    {
        angentTransform = t;
        skillObjInstance = GameObject.Instantiate(skillObjPrefab, angentTransform.position, angentTransform.rotation);
        skillObjInstance.transform.parent = angentTransform;
        skillObjInstance.Init(this);
    } 

    public void OnSkillEnter()
    {
         skillObjInstance.OnSkillEnter();
    }
  
}