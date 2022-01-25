using System;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement _unitMovement;
    [SerializeField] private UnityEvent _onSelected;
    [SerializeField] private UnityEvent _onDeselected;

    public static event Action<Unit> OnServerUnitSpawned; 
    public static event Action<Unit> OnServerUnitDespawned;
    public static event Action<Unit> OnAuthorityUnitSpawned;
    public static event Action<Unit> OnAuthorityUnitDespawned;

    #region Server

    public override void OnStartServer()
    {
        OnServerUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        OnServerUnitDespawned?.Invoke(this);
    }

    #endregion
    
    #region Client

    public override void OnStartClient()
    {
        if(!isClientOnly || !hasAuthority) return;
        
        OnAuthorityUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if(!isClientOnly || !hasAuthority) return;
        
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
}