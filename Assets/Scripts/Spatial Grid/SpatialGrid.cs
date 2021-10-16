using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpatialGrid : MonoBehaviour 
{
    public float x;
    public float y;
    public float cellWidth;
    public float cellHeight;
    [Tooltip("Number of columns (width of the grid)")] public int width;
    [Tooltip("Number of rows (height of the grid)")] public int height;
    
    public readonly Tuple<int, int> Outside = Tuple.Create(-1, -1);
    public readonly IGridEntity[] Empty = new IGridEntity[0];
    
    private Dictionary<IGridEntity, Tuple<int, int>> _lastPositions = new Dictionary<IGridEntity, Tuple<int, int>>();
    private HashSet<IGridEntity>[,] _buckets;

    private void Awake() 
    {
        _buckets = new HashSet<IGridEntity>[width, height];
        
        for (var i = 0; i < width; i++) 
        {
            for (var j = 0; j < height; j++) 
            {
                _buckets[i, j] = new HashSet<IGridEntity>();
            }
        }

        var ents = RecursiveWalker(transform)
                  .Select(n => n.GetComponent<IGridEntity>())
                  .Where(n => n != null);

        foreach (var e in ents) 
        {
            e.OnMove += UpdateEntity;
            UpdateEntity(e);
        }
    }

    public void Add(IGridEntity entity) 
    {
        entity.OnMove += UpdateEntity;
        UpdateEntity(entity);
    }

    public void Remove(IGridEntity entity) 
    {
        entity.OnMove -= UpdateEntity;
        
        UpdateEntity(entity);
        var currentPos = GetPositionInGrid(entity.Position);
        _buckets[currentPos.Item1, currentPos.Item2].Remove(entity);
        _lastPositions.Remove(entity);
    }

    public void UpdateEntity(IGridEntity entity) 
    {
        var lastPos= _lastPositions.ContainsKey(entity) ? _lastPositions[entity] : Outside;
        var currentPos= GetPositionInGrid(entity.Position);
        
        if (lastPos.Equals(currentPos))
            return;
        
        if (IsInsideGrid(lastPos))
            _buckets[lastPos.Item1, lastPos.Item2].Remove(entity);

        // We "put" it in the new cell, or we remove it if it left the grid 
        if (IsInsideGrid(currentPos)) 
        {
            _buckets[currentPos.Item1, currentPos.Item2].Add(entity);
            _lastPositions[entity] = currentPos;
        }
        else
            _lastPositions.Remove(entity);
    }

    public IEnumerable<IGridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition) 
    {
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), Mathf.Min(aabbFrom.y, aabbTo.y), 0);
        var to   = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), Mathf.Max(aabbFrom.y, aabbTo.y), 0);

        var fromCoord = GetPositionInGrid(from);
        var toCoord   = GetPositionInGrid(to);

        fromCoord = Tuple.Create(Utility.Clamp(fromCoord.Item1, 0, width), Utility.Clamp(fromCoord.Item2, 0, height));
        toCoord   = Tuple.Create(Utility.Clamp(toCoord.Item1,   0, width), Utility.Clamp(toCoord.Item2,   0, height));

        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return Empty;
        
        var cols = Utility.Generate(fromCoord.Item1, x => x + 1)
                       .TakeWhile(n => n < width && n <= toCoord.Item1);

        var rows = Utility.Generate(fromCoord.Item2, y => y + 1)
                       .TakeWhile(y => y < height && y <= toCoord.Item2);

        var cells = cols.SelectMany(col => rows.Select(row => Tuple.Create(col, row)));

        // Iterate those that are within the criteria
        return cells
              .SelectMany(cell => _buckets[cell.Item1, cell.Item2])
              .Where(e => 
                  from.x <= e.Position.x && e.Position.x <= to.x && 
                  from.y <= e.Position.y && e.Position.y <= to.y)
              .Where(n => filterByPosition(n.Position));
    }

    public Tuple<int, int> GetPositionInGrid(Vector3 pos) 
    {
        // Removes the difference, divide according to the cells, and floors it...
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth),
                            Mathf.FloorToInt((pos.y - y) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> position) 
    {
        // If it's less than 0 or greater than width or height, it's not in the grid...
        return 0 <= position.Item1 && position.Item1 < width &&
               0 <= position.Item2 && position.Item2 < height;
    }

    private void OnDestroy() 
    {
        var ents = RecursiveWalker(transform).Select(n => n.GetComponent<IGridEntity>())
                                             .Where(n => n != null);
        
        foreach (var e in ents) 
            e.OnMove -= UpdateEntity;
    }
    
    private static IEnumerable<Transform> RecursiveWalker(Transform parent) 
    {
        foreach (Transform child in parent) 
        {
            foreach (Transform grandchild in RecursiveWalker(child))
                yield return grandchild;
            
            yield return child;
        }
    }

    #region GRAPHIC REPRESENTATION

    public bool areGizmosShutDown;
    public bool activatedGrid;
    public bool showLogs = true;

    private void OnDrawGizmos() 
    {
        var rows = Utility.Generate(y, curr => curr + cellHeight)
                       .Select(row => Tuple.Create(new Vector3(x,                     row, 0),
                                                   new Vector3(x + cellWidth * width, row, 0)));

        var cols = Utility.Generate(x, curr => curr + cellWidth)
                       .Select(col => Tuple.Create(new Vector3(col, y, 0),
                                                   new Vector3(col, y + cellHeight * height, 0)));

        var allLines = rows.Take(width + 1).Concat(cols.Take(height + 1));

        foreach (var elem in allLines) 
        {
            Gizmos.DrawLine(elem.Item1, elem.Item2);
        }

        if (_buckets == null || areGizmosShutDown) 
            return;

        var originalCol = GUI.color;
        GUI.color = Color.red;
        
        if (!activatedGrid) 
        {
            var allElems = new List<IGridEntity>();
            foreach (var elem in _buckets)
                allElems = allElems.Concat(elem).ToList();

            int connections = 0;
            foreach (var entity in allElems) 
            {
                foreach (var neighbour in allElems.Where(x => x != entity)) 
                {
                    Gizmos.DrawLine(entity.Position, neighbour.Position);
                    connections++;
                }

                if (showLogs)
                    Debug.Log("Tengo " + connections + " conexiones por individuo");
                connections = 0;
            }
        }
        else 
        {
            int connections = 0;
            foreach (var elem in _buckets) 
            {
                foreach (var ent in elem) 
                {
                    foreach (var n in elem.Where(x => x != ent)) 
                    {
                        Gizmos.DrawLine(ent.Position, n.Position);
                        connections++;
                    }

                    if (showLogs)
                        Debug.Log("Tengo " + connections + " conexiones por individuo");
                    connections = 0;
                }
            }
        }

        GUI.color = originalCol;
        showLogs  = false;
    }

    #endregion
}
