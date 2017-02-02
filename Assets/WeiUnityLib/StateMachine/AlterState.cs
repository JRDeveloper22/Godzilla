using UnityEngine;
using System.Collections;

public class AlertState : IState {
    private readonly StateMachine stateMachine;
    private float searchTime;

    public AlertState(StateMachine _stateMachine) {
        stateMachine = _stateMachine;
    }

    // Update is called once per frame
    public void UpdateState()
    {
        Search();
        ProcessInfo();
    }

    public void OnTriggerEnter(Collider other)
    {
        //what will happen when we search, something trigger?
    }

    public void OnTriggerStay(Collider other)
    {

    }

    private void ProcessInfo()
    {
        if (stateMachine.isSeePlayer)
        {
            stateMachine.currentState = stateMachine.chaseState;
            stateMachine.currentState.Ontransition();
            StateTransitionOut();
            return;
        }

        if (!stateMachine.isFindDanger) {
            stateMachine.currentState = stateMachine.patrolState;
            stateMachine.currentState.Ontransition();
            StateTransitionOut();
            return;
        }
    }

    private void Search() {
  
        stateMachine.navMeshAgent.Stop();
        stateMachine.transform.Rotate(0,stateMachine.searchingTurnSpeed * Time.deltaTime,0);
        searchTime += Time.deltaTime;

        if (searchTime >= stateMachine.searchingDuration) {
            stateMachine.isFindDanger = false;
            return;
        }
    }

    public void Ontransition()
    {
        stateMachine.meshRendererFlag.material.color = Color.yellow;
    }

    public void StateTransitionOut()
    {
        searchTime = 0.0f;
        stateMachine.isFindDanger = false;
        stateMachine.previousState = this;
    }
}
