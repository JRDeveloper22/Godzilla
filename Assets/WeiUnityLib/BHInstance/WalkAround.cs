using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WeiBHTLibrary;

public class WalkAround : Selector {
    TreeBase treeBase;

    bool getNewSearchPos = true;

    int positionIndex = 0;

    public WalkAround(TreeBase _treeBase) {
        treeBase = _treeBase;
        Add<Behavior>().Update = Patrol;
        Add<Behavior>().Update = Search;
        Add<Behavior>().Update = Fight;
        Add<Behavior>().Update = Hide;
        
    }

    Status Patrol()
    {
        if (treeBase.aiStatu == TreeBase.AiStatu.Partoal)
        {
            if (treeBase.navMeshAngent.remainingDistance <= treeBase.navMeshAngent.stoppingDistance && !treeBase.navMeshAngent.pathPending)
            {
                treeBase.navMeshAngent.SetDestination(treeBase.partrolPos[positionIndex%treeBase.partrolPos.Length].position);
                positionIndex++;
            }
            return Status.BhRunning;
        }
        else
        {
            return Status.BhFailure;
        }
    }

    Status Search() {
        if (treeBase.aiStatu == TreeBase.AiStatu.Search)
        {
            if (getNewSearchPos)
            {
                treeBase.navMeshAngent.SetDestination(treeBase.searchPos);
                getNewSearchPos = false;
            }
            return Status.BhRunning;
        }

        getNewSearchPos = true;
        return Status.BhFailure;
   }

    Status Fight()
    {
        if (treeBase.aiStatu == TreeBase.AiStatu.Fight)
        {
            if (treeBase.numberFrame % 30 == 0) {
                treeBase.navMeshAngent.SetDestination(treeBase.player.transform.position);
            }
            return Status.BhRunning;
        }
        else
            return Status.BhFailure;
    }

    Status Hide() {
        if (treeBase.aiStatu == TreeBase.AiStatu.Hide)
        {
            treeBase.navMeshAngent.SetDestination(treeBase.hidePos.position);
            return Status.BhRunning;
        }
        else {
            return Status.BhFailure;
        }
    }
}
