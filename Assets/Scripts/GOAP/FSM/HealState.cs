using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : MonoBaseState
{
    private float healInterval = .8f;
    private float next;
    private float healedAmmount;
    private float maxHeal;

    private EliteEnemy _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EliteEnemy>();
    }

    public override void UpdateLoop()
    {
        if (Time.time >= next)
        {
            next = Time.time + healInterval;
            Heal();
        }
    }

    private void Heal()
    {
        _enemy.currentHp += 1;
        healedAmmount += 1;
    }

    public override IGoapState ProcessInput()
    {
        if(healedAmmount >= maxHeal && Transitions.ContainsKey("OnChaseState"))
            return Transitions["OnChaseState"];

        return this;
    }


}
