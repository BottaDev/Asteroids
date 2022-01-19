using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : MonoBaseState
{
    public override event Action OnNeedsReplan;
    
    private float healInterval = .8f;
    private float next;

    private EliteEnemyState _enemyState;

    private void Awake()
    {
        _enemyState = GetComponent<EliteEnemyState>();
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
        _enemyState.currentHp += 1;
    }

    public override void Enter(IGoapState from, Dictionary<string, object> transitionParameters = null)
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        base.Enter(from, transitionParameters);
    }

    public override IGoapState ProcessInput()
    {
        if (_enemyState.currentHp >= _enemyState.maxHp)
        {
            // Has finished healing himself...
            GetComponent<SpriteRenderer>().color = Color.white;
            
            float distance = Vector2.Distance(transform.position, _enemyState.player.transform.position);
        
            if (distance < _enemyState.nearDistance)
            {
                if (!Transitions.ContainsKey("OnFleeState"))
                {
                    OnNeedsReplan?.Invoke();
                    return this;
                }
            
                return Transitions["OnFleeState"];
            }            
            
            if (distance > _enemyState.attackDistance)
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
