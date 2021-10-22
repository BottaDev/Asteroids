using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleQuery : MonoBehaviour, IQuery 
{
    public float radius = 5f;
    
    public IEnumerable<IGridEntity> Query() 
    {
        var halfSize = new Vector3(radius, radius, 0);

        var aabbFrom = transform.position - halfSize;
        var aabbTo = transform.position + halfSize;

        return LevelManager.instance.targetGrid.Query(aabbFrom, aabbTo, n => (transform.position - n).sqrMagnitude <= radius * radius);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}