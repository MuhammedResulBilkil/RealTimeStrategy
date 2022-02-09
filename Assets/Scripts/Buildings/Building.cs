using System;
using Mirror;
using UnityEngine;

public class Building : NetworkBehaviour
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _id = -1;
    [SerializeField] private int _price = 100;
    
    public static event Action<Building> OnServerBuildingSpawned; 
    public static event Action<Building> OnServerBuildingDespawned;
    public static event Action<Building> OnAuthorityBuildingSpawned;
    public static event Action<Building> OnAuthorityBuildingDespawned;

    #region Server

    public override void OnStartServer()
    {
        OnServerBuildingSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        OnServerBuildingDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        OnAuthorityBuildingSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if(!hasAuthority) return;
        
        OnAuthorityBuildingDespawned?.Invoke(this);
    }

    #endregion
    
    public Sprite GetIcon()
    {
        return _icon;
    }

    public int GetID()
    {
        return _id;
    }

    public int GetPrice()
    {
        return _price;
    }
}