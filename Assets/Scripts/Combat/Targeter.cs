using Mirror;
using UnityEngine;


public class Targeter : NetworkBehaviour
{
    private Targetable _target;

    #region Server

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

    #endregion

    public Targetable GetTarget()
    {
        return _target;
    }
    
}