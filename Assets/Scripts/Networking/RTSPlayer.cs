using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class RTSPlayer : NetworkBehaviour
{
    private List<Unit> _myUnits = new List<Unit>();
    private List<Building> _myBuildings = new List<Building>();

    #region Server

    public override void OnStartServer()
    {
        Unit.OnServerUnitSpawned += OnServerHandleUnitSpawned;
        Unit.OnServerUnitDespawned += OnServerHandleUnitDespawned;

        Building.OnServerBuildingSpawned += OnServerHandleBuildingSpawned;
        Building.OnServerBuildingDespawned += OnServerHandleBuildingDespawned;
    }

    public override void OnStopServer()
    {
        Unit.OnServerUnitSpawned -= OnServerHandleUnitSpawned;
        Unit.OnServerUnitDespawned -= OnServerHandleUnitDespawned;
        
        Building.OnServerBuildingSpawned -= OnServerHandleBuildingSpawned;
        Building.OnServerBuildingDespawned -= OnServerHandleBuildingDespawned;
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
    
    private void OnServerHandleBuildingSpawned(Building building)
    {
        if(building.connectionToClient.identity != connectionToClient.identity) return;
        
        _myBuildings.Add(building);
    }
    
    private void OnServerHandleBuildingDespawned(Building building)
    {
        if(building.connectionToClient.identity != connectionToClient.identity) return;
        
        _myBuildings.Remove(building);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        if(NetworkServer.active) return;
        
        Unit.OnAuthorityUnitSpawned += OnAuthorityHandleUnitSpawned;
        Unit.OnAuthorityUnitDespawned += OnAuthorityHandleUnitDespawned;
        
        Building.OnAuthorityBuildingSpawned += OnAuthorityHandleBuildingSpawned;
        Building.OnAuthorityBuildingDespawned += OnAuthorityHandleBuildingDespawned;
    }

    public override void OnStopClient()
    {
        if(!isClientOnly || !hasAuthority) return;
        
        Unit.OnAuthorityUnitSpawned -= OnAuthorityHandleUnitSpawned;
        Unit.OnAuthorityUnitDespawned -= OnAuthorityHandleUnitDespawned;
        
        Building.OnAuthorityBuildingSpawned -= OnAuthorityHandleBuildingSpawned;
        Building.OnAuthorityBuildingDespawned -= OnAuthorityHandleBuildingDespawned;
    }
    
    private void OnAuthorityHandleUnitSpawned(Unit unit)
    {
        _myUnits.Add(unit);
    }
    
    private void OnAuthorityHandleUnitDespawned(Unit unit)
    {
        _myUnits.Remove(unit);
    }
    
    private void OnAuthorityHandleBuildingSpawned(Building building)
    {
        _myBuildings.Add(building);
    }
    
    private void OnAuthorityHandleBuildingDespawned(Building building)
    {
        _myBuildings.Remove(building);
    }

    #endregion

    public List<Unit> GetMyUnits()
    {
        return _myUnits;
    }
    
    public List<Building> GetMyBuildings()
    {
        return _myBuildings;
    }
}