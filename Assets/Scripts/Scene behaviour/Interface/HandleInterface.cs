﻿using UnityEngine;
using System.Collections;

public class HandleInterface : MonoBehaviour
{
    public GameObject playerObject;
    private PlayerControl player;

    public HealthBar healthBar;
    public ManaBar manaBar;
    public DamageBar damageBar;
    public ChargeBar chargeBar;
    public SpellBar spellBar;

    // Use this for initialization
    void Start()
    {
        player = playerObject.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void OnGUI()
    {
        manaBar.level = player.castComponent.mana;
        damageBar.level = player.healthComponent.totalDamage;
        if (player.sm.get() == player.sm.states[PlayerControl.States.Cast])
            chargeBar.GetCharge(player.castComponent.spellBook.Get());
        else
            chargeBar.Reset();
        int playerTicks = Mathf.CeilToInt(player.healthComponent.health / 10);
        if (healthBar.ticks != playerTicks)
            healthBar.ticks = playerTicks;
        spellBar.ReadSpells(player.castComponent);
    }
}
