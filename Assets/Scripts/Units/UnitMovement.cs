using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    #region Server

    [ServerCallback]
    private void Update()
    {
        if(!_navMeshAgent.hasPath) return;
        if(_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
        
        _navMeshAgent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas ))
            return;

        _navMeshAgent.SetDestination(hit.position);
    }

    #endregion

}
