using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : MonoBaseState
{
    float healInterval = .8f;
    float next;
    float healedAmmount;
    float maxHeal;

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

    void Heal()
    {
        _enemy.hp += 1;
        healedAmmount += 1;
    }

    public override IGoapState ProcessInput()
    {
        if(healedAmmount >= maxHeal && Transitions.ContainsKey("OnChaseState"))
            return Transitions["OnChaseState"];

        return this;
    }


}
