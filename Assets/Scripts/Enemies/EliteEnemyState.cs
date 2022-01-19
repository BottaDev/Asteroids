using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemyState : MonoBehaviour
{
    public int maxHp = 12;
    public int currentHp;
    public float chaseSpeed = 2;
    public float fleeSpeed = 4;
    public float nearDistance = 2.5f;
    public float attackDistance = 7f;

    [HideInInspector] public PlayerModel player;
    
    private void Start()
    {
        currentHp = maxHp;
        
        player = FindObjectOfType<PlayerModel>();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nearDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
