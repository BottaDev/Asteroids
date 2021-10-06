using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareQuery : MonoBehaviour, IQuery 
{
    public SpatialGrid targetGrid;
    public float width = 15f;
    public float height = 30f;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();

    private void Start()
    {
        targetGrid = FindObjectOfType<SpatialGrid>();
    }

    public IEnumerable<IGridEntity> Query() 
    {
        var h = height * 0.5f;
        var w = width  * 0.5f;
        
        return targetGrid.Query(
                                transform.position + new Vector3(-w, -h, 0),
                                transform.position + new Vector3(w,  h, 0),
                                x => true);
    }

    private void OnDrawGizmos() 
    {
        if (targetGrid == null) 
            return;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}