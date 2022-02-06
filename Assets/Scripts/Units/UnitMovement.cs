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
    [SerializeField] private Targeter _targeter;
    [SerializeField] private float _chaseRange = 10f;

    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.OnServerGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.OnServerGameOver += ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        Targetable target = _targeter.GetTarget();
        
        if (target != null)
        {
            float distance = (target.transform.position - transform.position).sqrMagnitude;

            if (distance > _chaseRange * _chaseRange)
            {
                _navMeshAgent.SetDestination(target.transform.position);
            }
            else if (_navMeshAgent.hasPath)
            {
                _navMeshAgent.ResetPath();
            }
            
            return;
        }
        
        if(!_navMeshAgent.hasPath) return;
        if(_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
        
        _navMeshAgent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        _targeter.ClearTarget();
        
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas ))
            return;

        _navMeshAgent.SetDestination(hit.position);
    }
    
    [Server]
    private void ServerHandleGameOver()
    { 
        _navMeshAgent.ResetPath();
    }

    #endregion

}
