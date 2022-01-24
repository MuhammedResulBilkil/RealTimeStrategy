using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent _onSelected;
    [SerializeField] private UnityEvent _onDeselected;

    #region Client

    [Client]
    public void Select()
    {
        Debug.Log("Select");
        
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
}