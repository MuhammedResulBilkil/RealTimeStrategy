using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameOverHandler : NetworkBehaviour
{
    public static event Action OnServerGameOver;
    public static event Action<string> OnClientGameOver;

    private List<UnitBase> _unitBases = new List<UnitBase>();

    #region Server

    public override void OnStartServer()
    {
        UnitBase.OnServerBaseSpawned += ServerHandleBaseSpawned;
        UnitBase.OnServerBaseDeSpawned += ServerHandleBaseDespawned;
    }

    public override void OnStopServer()
    {
        UnitBase.OnServerBaseSpawned -= ServerHandleBaseSpawned;
        UnitBase.OnServerBaseDeSpawned -= ServerHandleBaseDespawned;
    }

    [Server]
    private void ServerHandleBaseSpawned(UnitBase unitBase)
    {
        _unitBases.Add(unitBase);
    }

    [Server]
    private void ServerHandleBaseDespawned(UnitBase unitBase)
    {
        _unitBases.Remove(unitBase);
        
        if(_unitBases.Count != 1) return;
        
        int playerID = _unitBases[0].connectionToClient.connectionId;
        
        RpcGameOver($"Player {playerID}");
        
        OnServerGameOver?.Invoke();
    }

    #endregion

    #region Client

    [ClientRpc]
    private void RpcGameOver(string winner)
    {
        OnClientGameOver?.Invoke(winner);
    }

    #endregion
}