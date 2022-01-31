using System;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public event Action OnServerDie;
    public event Action<int, int> OnClientHealthUpdated; 

    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))] private int _currentHealth;

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
    }

    #endregion

    #region Client

    private void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        OnClientHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
}