using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private GameObject _healthBarParent;
    [SerializeField] private Image _healthBarImage;

    private void Awake()
    {
        _health.OnClientHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy()
    {
        _health.OnClientHealthUpdated -= HandleHealthUpdated;
    }

    private void OnMouseEnter()
    {
        _healthBarParent.SetActive(true);
    }

    private void OnMouseExit()
    {
        _healthBarParent.SetActive(false);
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        _healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}