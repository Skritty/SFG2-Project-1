using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent Death;

    [SerializeField]
    int _maxHealth = 100;
    int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void Damage(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - amount, 0, _maxHealth);
        if (_currentHealth == 0) Death?.Invoke();
    }
}
