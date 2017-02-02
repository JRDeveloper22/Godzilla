using UnityEngine;
using System.Collections;

public class ChaseState : IState {
    private readonly StateMachine stateMachine;

    public ChaseState(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    // Update is called once per frame
    public void UpdateState()
    {
        Chase();
        ProcessInfo();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerStay(Collider other) {

    }

    private void ProcessInfo()
    {
        if (stateMachine.isFindDanger)
        {
            stateMachine.currentState = stateMachine.alertState;
            stateMachine.currentState.Ontransition();
            StateTransitionOut();
            return;
        }
    }

    private void Chase() {
        
     
    
    }

    public void Ontransition()
    {
        stateMachine.meshRendererFlag.material.color = Color.red;
    }

    public void StateTransitionOut()
    {
        stateMachine.isSeePlayer = false;
        stateMachine.previousState = this;
    }
}
