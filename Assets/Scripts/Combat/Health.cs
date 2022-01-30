using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public event Action OnServerDie;
    
    [SerializeField] private int maxHealth = 100;

    [SyncVar] private int _currentHealth;

    #region Server

    public override void OnStartServer()
    {
        _currentHealth = maxHealth;
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if(_currentHealth == 0) return;

        _currentHealth = Mathf.Max(0, _currentHealth - damageAmount);

        if (_currentHealth != 0) return;
        
        OnServerDie?.Invoke();
        
        Debug.Log("We Died!");
    }

    #endregion

    #region Client

    

    #endregion
}