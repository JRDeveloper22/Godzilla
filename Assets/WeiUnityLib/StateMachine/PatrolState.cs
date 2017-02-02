using UnityEngine;
using System.Collections;

public class PatrolState : IState {

    private readonly StateMachine stateMachine;
    Vector3 previousDest;
    Vector3 newPos;
    Vector3 gardPosition;
    


    public PatrolState(StateMachine _stateMachine) {
        stateMachine = _stateMachine;
        Vector2 move = Random.insideUnitCircle * (float)GaussianDist.GetRandomNumber(5, 5);
        gardPosition = stateMachine.guardPos[Random.Range(0,stateMachine.guardPos.Length-1)].position;
        newPos = gardPosition + new Vector3(move.x,0, move.y);
        previousDest = newPos;
    }
	
	public void UpdateState() {
        Patrol();
        ProcessInfo();   
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            stateMachine.isFindDanger = true;
        }
    }
  
    private void ProcessInfo()
    {
        if (stateMachine.isSeePlayer) {
            stateMachine.currentState = stateMachine.chaseState;
            StateTransitionOut();
            return;
        }

        if (stateMachine.isFindDanger) {
            stateMachine.currentState = stateMachine.alertState;
            StateTransitionOut();
            return;
        }
    }

    void Patrol() {
        //Debug.DrawLine(stateMachine.gameObject.transform.position,previousDest);
        CheckNewDestination();


        //if We need to set new destinaion
        if (newPos != previousDest)
        {
            stateMachine.navMeshAgent.SetDestination(newPos);
            previousDest = newPos;
        }
       
    }

    public void Ontransition()
    {
        stateMachine.navMeshAgent.SetDestination(previousDest);
        stateMachine.navMeshAgent.Resume();
        stateMachine.meshRendererFlag.material.color = Color.green;
    }

    public void StateTransitionOut()
    {
        stateMachine.previousState = this;
        stateMachine.LastpartolDestination = stateMachine.navMeshAgent.destination;
    }

    void CheckNewDestination() {
        if (stateMachine.navMeshAgent.remainingDistance <= stateMachine.navMeshAgent.stoppingDistance && !stateMachine.navMeshAgent.pathPending)
        {
            float dist;

            if (stateMachine.isGaussianDist)
                dist = (float)GaussianDist.GetRandomNumber(3, 3);
            else
                dist = Random.Range(3, 6);

            Vector2 move = (Random.insideUnitCircle).normalized * dist;

            newPos = stateMachine.guardPos[Random.Range(0, stateMachine.guardPos.Length - 1)].position + new Vector3(move.x, 0, move.y);
        }
    }
}
