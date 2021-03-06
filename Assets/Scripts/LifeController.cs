﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController
{

    private int currentLife;
    private int maxLife;
    private bool invincible;

    public event Action OnDead;
    public event Action<int, int> OnGetDamage; //int 0 = currentLife, int 1 = damage
    public event Action<int, int> OnGetHeal; //int 0 = currentLife, int 1 = healing

    public float InvincibilityTimeStart { get; private set; }
    private float invincibilityTime;

    public int CurrentLife
    {
        get => currentLife;

        set
        {
            currentLife = value;

            if (currentLife > maxLife)
            {
                currentLife = maxLife;
            }

            if (currentLife <= 0)
            {
                OnDead?.Invoke();
            }


        }
    }


    public LifeController(int maxlife)
    {
        this.maxLife = maxlife;
        CurrentLife = maxlife;
        InvincibilityTimeStart = 1.5f;
        invincibilityTime = InvincibilityTimeStart;
    }



    public void GetDamage (int damage)
    {
        if (invincibilityTime > InvincibilityTimeStart && !invincible)
        {
            CurrentLife -= damage;
            invincibilityTime = 0;
            OnGetDamage?.Invoke(CurrentLife, damage);
        }
    }
   
    public void ChangeInvincible (bool isInvincible)
    {
        invincible = isInvincible;
    }
    public void GetHeal (int heal)
    {
        CurrentLife += heal;
        OnGetHeal?.Invoke(currentLife, heal);
        //change anim
    }


    public void Update()
    {
        invincibilityTime += Time.deltaTime;
    }





}
