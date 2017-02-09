using System;
using UnityEngine;

public class LaserShoot : SkillObj {

    public float speed = 5;
    public GameObject fireBallObject;
    GameObject fireBallInstance;
    Vector3 moveDir;

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
        moveDir = skill.angentTransform.forward;
    }

    public override void OnSkillEnter()
    {
        if (fireBallInstance == null)
        {
            fireBallInstance = Instantiate(fireBallObject, transform.position+new Vector3(0, height, dest), transform.rotation);
            fiexdDel += UpdateFireBallPos;
            Invoke("OnSkillExsit", 20.0f);
        }
    }

    public override void OnSkillStay()
    {
        
    }

    public override void OnSkillSilence()
    {
        
    }

    public override void OnSkillCoolDown()
    {
        
    }

    public override void OnSkillExsit()
    {
        fiexdDel -= UpdateFireBallPos;
        pitch = 0;
        yaw = 180;
        height = 0.0f;
        currentRotation = Vector3.zero;
        dest = 3.0f;
        Destroy(fireBallInstance.gameObject);
    }

    #endregion
    float pitch;
    float yaw = 180;
    float height = 0.0f;
    Vector3 currentRotation;
    float dest = 3.0f;

    void UpdateFireBallPos()
    {
        //pitch += Time.fixedDeltaTime * 5;
        yaw += Time.fixedDeltaTime * 360;
        height += Time.fixedDeltaTime/2;
        fireBallInstance.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        fireBallInstance.transform.position = transform.position - fireBallInstance.transform.forward * dest + new Vector3(0, height, 0);
    }
}
