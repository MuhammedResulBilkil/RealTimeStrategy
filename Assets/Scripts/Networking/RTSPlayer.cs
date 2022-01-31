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

    public override void OnStartAuthority()
    {
        if(NetworkServer.active) return;
        
        Unit.OnAuthorityUnitSpawned += OnAuthorityHandleUnitSpawned;
        Unit.OnAuthorityUnitDespawned += OnAuthorityHandleUnitDespawned;
    }

    public override void OnStopClient()
    {
        if(!isClientOnly || !hasAuthority) return;
        
        Unit.OnAuthorityUnitSpawned -= OnAuthorityHandleUnitSpawned;
        Unit.OnAuthorityUnitDespawned -= OnAuthorityHandleUnitDespawned;
    }
    
    private void OnAuthorityHandleUnitSpawned(Unit unit)
    {
        _myUnits.Add(unit);
    }
    
    private void OnAuthorityHandleUnitDespawned(Unit unit)
    {
        _myUnits.Remove(unit);
    }

    #endregion

    public List<Unit> GetMyUnits()
    {
        return _myUnits;
    }
}