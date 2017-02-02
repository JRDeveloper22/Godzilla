using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class StateMachine:MonoBehaviour{

    public enum ActionStatus {normal,stun};

    [HideInInspector]
    public ActionStatus actionstatu;
    [HideInInspector]
    public float stunTimer = 0.0f;

    public float MoveSpeed = 3.0f;
    public float searchingTurnSpeed = 120f;
    public float searchingDuration = 4f;
    public float sightRange = 20f;
    public LayerMask targetMask;
    public Transform[] guardPos;
    public Transform eyes;
    public Vector3 offset = new Vector3(0, 0.5f, 0);
    public float fieldofViewAngle = 90;

    [HideInInspector]
    public MeshRenderer meshRendererFlag;

    public bool isGaussianDist = false;
    

    [HideInInspector]
    public Transform chaseTarget;

    [HideInInspector]
    public IState currentState;
    [HideInInspector]
    public IState previousState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public ChaseState chaseState;

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;


    [HideInInspector]
    public bool isSeePlayer = false;
    [HideInInspector]
    public bool isFindDanger = false;
    [HideInInspector]
    public int health = 100;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    [HideInInspector]
    public Transform visibleTarget;



    public float findVisibleFrequence = 0.5f;
    private float findVisibleTimer;

    public Vector3 LastpartolDestination;


    void Awake() {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this);

        meshRendererFlag = gameObject.GetComponent<MeshRenderer>();

    }

	void Start () {
        currentState = patrolState;
        findVisibleTimer = findVisibleFrequence;
        currentState.Ontransition();
	}
	
	void Update () {
        currentState.UpdateState();
	}

    void FixedUpdate() {
        findVisibleTimer -= Time.deltaTime;
        if (findVisibleTimer <= 0) {
            FindVisibleTargets();
            findVisibleTimer = findVisibleFrequence;
        }
    }

    public void FindVisibleTargets()
    {

       // Stopwatch sw = new Stopwatch();
        //sw.Start();

        if (isSeePlayer) {
            return; //for Enemy Ai, when we have a target, we do not need to get a new target.
        }

        //Get All Colider in OverlapSphere
        Collider[] targetsInViewRadius = Physics.OverlapSphere(eyes.transform.position, sightRange, targetMask); //gameobject with targetMask will be selected

        for (int i = 0; i < targetsInViewRadius.Length; i++) {

            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            //Check FieldOfView
            if(Vector3.Angle(transform.forward,dirToTarget) < fieldofViewAngle / 2)
            {
                RaycastHit hit;
                //float dstToTarget = Vector3.Distance(eyes.transform.position,target.position);

                UnityEngine.Debug.DrawRay(eyes.transform.position, dirToTarget * sightRange, Color.green);

                //Check if we can see it.
                if (Physics.Raycast(eyes.transform.position, dirToTarget, out hit, sightRange)) {

                    if (hit.collider.gameObject.tag == "Player")
                    {
                        visibleTarget = target; //over here we only need one target.
                        isSeePlayer = true;
                        //sw.Stop();
                        //print("FindVisibleTargets()" + sw.ElapsedMilliseconds + "ms");
                        return;
                    }
                }
            }
        }

    }

    IEnumerator FindTargetWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay); 
            FindVisibleTargets();
        }
    }

    public void MovementDisable(bool isDisable) {
        navMeshAgent.speed = isDisable ? 0.0f : MoveSpeed = 3.0f;
    }
}


enum SatetGroup {GAURD,SENDMASSAGE,FAMILITIME};