using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memento<TSnapshot> 
{
    private List<TSnapshot> _snapshots = new List<TSnapshot>();

    public void Record(TSnapshot snapshot) 
    {
        _snapshots.Add(snapshot);
        
        // 5 seconds (0.1 * 50 = 5)
        if (_snapshots.Count > 50)
            _snapshots.RemoveAt(0);
    }

    public TSnapshot Remember() 
    {
        // Oldest moment
        var snapshot = _snapshots[0];
        _snapshots.Clear();

        return snapshot;
    }

    public bool CanRemember() 
    {
        return _snapshots.Count > 0;
    }
    
}
