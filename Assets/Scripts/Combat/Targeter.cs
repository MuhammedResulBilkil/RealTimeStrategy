using Mirror;
using UnityEngine;


public class Targeter : NetworkBehaviour
{
    private Targetable _target;

    #region Server

    public override void OnStartServer()
    {
        GameOverHandler.OnServerGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        GameOverHandler.OnServerGameOver -= ServerHandleGameOver;
    }

    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if(!targetGameObject.TryGetComponent(out Targetable target)) return;

        _target = target;
    }

    [Server]
    public void ClearTarget()
    {
        _target = null;
    }
    
    [Server]
    private void ServerHandleGameOver()
    {
        ClearTarget();
    }

    #endregion

    public Targetable GetTarget()
    {
        return _target;
    }
    
}