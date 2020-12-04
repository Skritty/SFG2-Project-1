using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class HUD : MonoBehaviour
{
    public Action UpdateGUI = delegate { };
    public static HUD PlayerGUI;

    [SerializeField]
    PlayerController player;
    [SerializeField]
    RawImage hpBar;

    float hpPercent = 1;
    bool hidden = false;

    private void Awake()
    {
        if(PlayerGUI == null)
        {
            PlayerGUI = this;
            UpdateGUI += UpdateHealthBar;
        }
    }

    private void UpdateHealthBar()
    {
        if (player == null)
        {
            hpBar.rectTransform.localScale = Vector3.zero;
        }
        else
        {
            hpBar.rectTransform.localScale = new Vector3(player.GetComponent<Health>().GetCurrentHealth() / (float)player.GetComponent<Health>().GetMaxHealth(), 1, 1);
        }
        
    }

    public void ChangePlayer(PlayerController p)
    {
        player = p;
        UpdateGUI?.Invoke();
    }

}
