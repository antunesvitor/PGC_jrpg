using Assets.scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    private PlayerController PlayerController;
    public int MaxHealth;
    public int CurrentHealth;

    public float _healthIncreasePerSec = 1f;
    public float _FloatcurrentHealth;

    public string name = "Herói";
    
    void Awake()
    {
        this.PlayerController = GetComponent<PlayerController>();
        this.MaxHealth = 500;
        this.CurrentHealth = 150;
        this._FloatcurrentHealth = this.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        this._FloatcurrentHealth += (_healthIncreasePerSec * Time.deltaTime);
        this.CurrentHealth = (int) Math.Truncate(this._FloatcurrentHealth);
    }
}
