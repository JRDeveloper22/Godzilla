using UnityEngine;
using System.Collections;

public interface IState{

    // Update is called once per frame
    void UpdateState();

    void OnTriggerEnter(Collider other);

    void Ontransition();

    void StateTransitionOut();

}
