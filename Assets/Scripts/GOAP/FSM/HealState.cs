using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : MonoBaseState
{
    public override event Action OnNeedsReplan;
    
    private float healInterval = .8f;
    private float next;

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
    }

    public override void Enter(IGoapState from, Dictionary<string, object> transitionParameters = null)
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        base.Enter(from, transitionParameters);
    }

    public override IGoapState ProcessInput()
    {
        if (_enemy.currentHp >= _enemy.maxHP)
        {
            // Has finished healing himself...
            GetComponent<SpriteRenderer>().color = Color.white;
            
            float distance = Vector2.Distance(transform.position, _enemy.player.transform.position);
        
            if (distance < _enemy.nearDistance)
            {
                if (!Transitions.ContainsKey("OnFleeState"))
                {
                    OnNeedsReplan?.Invoke();
                    return this;
                }
            
                return Transitions["OnFleeState"];
            }            
            
            if (distance > _enemy.attackDistance)
            {
                if (!Transitions.ContainsKey("OnChaseState"))
                {
                    OnNeedsReplan?.Invoke();
                    return this;        
                }

                return Transitions["OnChaseState"];    
            }
            else
            {
                if (!Transitions.ContainsKey("OnAttackState"))
                {
                    OnNeedsReplan?.Invoke();
                    return this;        
                }

                return Transitions["OnAttackState"];    
            }
        }

        return this;
    }


}
