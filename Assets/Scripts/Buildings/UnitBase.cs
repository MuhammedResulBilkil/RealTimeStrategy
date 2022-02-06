using System;
using Mirror;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{
    [SerializeField] private Health _health;

    public static event Action<int> OnServerPlayerDie; 
    public static event Action<UnitBase> OnServerBaseSpawned; 
    public static event Action<UnitBase> OnServerBaseDeSpawned; 

    #region Server

    public override void OnStartServer()
    {
        _health.OnServerDie += ServerHandleDie;

        OnServerBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        _health.OnServerDie -= ServerHandleDie;

        OnServerBaseDeSpawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        OnServerPlayerDie?.Invoke(connectionToClient.connectionId);
        
        NetworkServer.Destroy(gameObject);
    }

    #endregion


    #region Client

    

    #endregion
}