using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;
    
    [HideInInspector]
    public SpatialGrid targetGrid;

    [Header("Map Limit")]
    public float globalXLimit = 17.5f;
    public float globalYLimit = 9.5f;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        targetGrid = GetComponent<SpatialGrid>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector2(globalXLimit, globalYLimit));
    }
}
