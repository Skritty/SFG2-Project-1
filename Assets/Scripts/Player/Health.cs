using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Health : IDamageable
{
    public Action Death = delegate { };

    [SerializeField]
    int _maxHealth = 100;
    int _currentHealth;

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
        Death += Kill;
    }

    public override void TakeDamage(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
        HUD.PlayerGUI.UpdateGUI?.Invoke();
        GetComponent<PlayerController>().Oof();
        if (_currentHealth <= 0) Death?.Invoke();
    }

    public override void Kill()
    {
        HUD.PlayerGUI.ChangePlayer(null);
        GetComponent<PlayerController>().DoDie();
    }
}
