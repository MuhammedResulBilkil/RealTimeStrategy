using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class RTSPlayer : NetworkBehaviour
{
    [SerializeField] private List<Unit> _myUnits = new List<Unit>();

    #region Server

    public override void OnStartServer()
    {
        Unit.OnServerUnitSpawned += OnServerHandleUnitSpawned;
        Unit.OnServerUnitDespawned += OnServerHandleUnitDespawned;
    }

    public override void OnStopServer()
    {
        Unit.OnServerUnitSpawned -= OnServerHandleUnitSpawned;
        Unit.OnServerUnitDespawned -= OnServerHandleUnitDespawned;
    }
    
    private void OnServerHandleUnitSpawned(Unit unit)
    {
        if(unit.connectionToClient.identity != connectionToClient.identity) return;
        
        _myUnits.Add(unit);
    }
    
    private void OnServerHandleUnitDespawned(Unit unit)
    {
        if(unit.connectionToClient.identity != connectionToClient.identity) return;
        
        _myUnits.Remove(unit);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        if(!isClientOnly) return;
        
        Unit.OnAuthorityUnitSpawned += OnAuthorityHandleUnitSpawned;
        Unit.OnAuthorityUnitDespawned += OnAuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if(!isClientOnly) return;
        
        Unit.OnAuthorityUnitSpawned -= OnAuthorityHandleUnitSpawned;
        Unit.OnAuthorityUnitDespawned -= OnAuthorityHandleUnitDespawned;
    }
    
    private void OnAuthorityHandleUnitSpawned(Unit unit)
    {
        if(!hasAuthority) return;
        
        _myUnits.Add(unit);
    }
    
    private void OnAuthorityHandleUnitDespawned(Unit unit)
    {
        if(!hasAuthority) return;
        
        _myUnits.Remove(unit);
    }

    #endregion
    
}