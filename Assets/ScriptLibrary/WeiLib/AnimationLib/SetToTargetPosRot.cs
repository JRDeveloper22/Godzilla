using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToTargetPosRot : MonoBehaviour {

    public Transform targetTF;

    private void LateUpdate()
    {
        transform.position = targetTF.position;
        transform.rotation = targetTF.rotation;
    }
}
