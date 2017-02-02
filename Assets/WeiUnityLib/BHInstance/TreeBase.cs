using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeBase : MonoBehaviour {
    //===========================================================================================================================
    /// <summary>
    /// QLL : Queue,List,List
    /// Use QLL design because it is much more  Easy to manipulate
    /// Add delete new behavior at runtime.
    /// </summary>
    BehaviorMainTaskQueue BMTQ;                              //Main Task
    ParallelTaskList PTL;                                    //ParallelTasks which will be remove from the list when the behavior success.
    public List<WeiBHTLibrary.Behavior> parallelRepetendBH;  //Contain all the repetend Behavior which need to be Tick every frame. Normal it is a passitive action
    //===========================================================================================================================
    /// <summary>
    /// Other Angent: like Physic Detection, PathFiding Angent, Still Angent...
    /// </summary>
    public AiUtility.FieldOfView fieldOfView;
    [HideInInspector]
    public NavMeshAgent navMeshAngent;

    public Transform[] partrolPos;
    [HideInInspector]
    public System.UInt64 numberFrame = 0;
    public Transform hidePos;

    [HideInInspector]
    public Vector3 searchPos;

    [HideInInspector]
    public SkillAngent skillAngent;
    //===========================================================================================================================
    /// <summary>
    /// Flag Information for Behavior
    /// </summary>
    [HideInInspector]
    public List<string> viewTags;
    
    public enum AiStatu {Partoal,Fight,Sleep,Search,Hide}
    [HideInInspector]
    public AiStatu aiStatu; 

    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public bool isGetHurt = false;
    [HideInInspector]
    public bool isFindPlayer = false;
    //===========================================================================================================================
    void Start()
    {
        BMTQ = new BehaviorMainTaskQueue();
        PTL = new ParallelTaskList();

        parallelRepetendBH = new List<WeiBHTLibrary.Behavior>();

        fieldOfView = new AiUtility.FieldOfView(transform.GetChild(0), 10, 180);
        navMeshAngent = GetComponent<NavMeshAgent>();

        skillAngent = GetComponent<SkillAngent>();

        parallelRepetendBH.Add(new LookAround(this));
        parallelRepetendBH.Add(new WalkAround(this));
        parallelRepetendBH.Add(new SenseAndEmotion(this));

        BMTQ.Add(new ToKillPlayer(this));

        aiStatu = AiStatu.Partoal;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10);
       foreach (WeiBHTLibrary.Behavior b in parallelRepetendBH)
        {
            b.Tick();
        }

        PTL.ListTick();

        BMTQ.QueueTick();
        numberFrame++;
    }

    public void Fire() {
        skillAngent.OnSkillEnter(0);
    }

}
