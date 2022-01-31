using System;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private UnitMovement _unitMovement;
    [SerializeField] private Targeter _targeter;
    [SerializeField] private UnityEvent _onSelected;
    [SerializeField] private UnityEvent _onDeselected;

    public static event Action<Unit> OnServerUnitSpawned; 
    public static event Action<Unit> OnServerUnitDespawned;
    public static event Action<Unit> OnAuthorityUnitSpawned;
    public static event Action<Unit> OnAuthorityUnitDespawned;

    #region Server

    public override void OnStartServer()
    {
        _health.OnServerDie += ServerHandleDie;
        
        OnServerUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        _health.OnServerDie -= ServerHandleDie;
        
        OnServerUnitDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    #endregion
    
    #region Client

    public override void OnStartAuthority()
    {
        OnAuthorityUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if(!hasAuthority) return;
        
        OnAuthorityUnitDespawned?.Invoke(this);
    }

    [Client]
    public void Select()
    {
        if(!hasAuthority)
            return;
        
        _onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if(!hasAuthority)
            return;
        
        _onDeselected?.Invoke();
    }

    #endregion

    public UnitMovement GetUnitMovement()
    {
        return _unitMovement;
    }

    public Targeter GetTargeter()
    {
        return _targeter;
    }
}